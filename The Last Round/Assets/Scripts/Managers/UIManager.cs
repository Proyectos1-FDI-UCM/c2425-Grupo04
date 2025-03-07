//---------------------------------------------------------
// Este script se encargará de manejar todo lo referente a interfaz de usuario en la escena de Bartender
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using TMPro;
using System.Collections;
using System.Transactions;
using UnityEngine.InputSystem;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField]
    TextMeshProUGUI dialogueBox, dialogueSkipBText, option1BText, option2BText;
    [SerializeField]
    Button dialogueSkipButton;
    [SerializeField]
    private float TypeSpeed;
    [SerializeField]
    private Button option1Button, option2Button;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private DataContainer.Texto[] dialogue;
    private int way; // 0 = good way , 1 = bad way
    private int DialogueLine = 0;
    private bool SkipDialogue = false, ClientDisappear = false;
    private SpriteRenderer Client;
    private Color ClientC;
    private float DisappearSpeed;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        GameManager.Instance.GiveUI(this);

        if (dialogueSkipButton != null)
        dialogueSkipButton.gameObject.SetActive(true);

        ClientC.r = 255;
        ClientC.g = 255;
        ClientC.b = 255;
        ClientC.a = 255;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (dialogueSkipBText != null)
        {
            if (dialogue != null && (dialogue[DialogueLine].GoodText == dialogueBox.text || dialogue[DialogueLine].BadText == dialogueBox.text)) dialogueSkipBText.text = "Continuar";
            else dialogueSkipBText.text = "Saltar";
        }
        

        if (ClientDisappear)
        {
            ClientC.r = Mathf.Clamp(ClientC.r - Time.deltaTime * DisappearSpeed, 0, 255);
            ClientC.g = Mathf.Clamp(ClientC.g - Time.deltaTime * DisappearSpeed, 0, 255);
            ClientC.b = Mathf.Clamp(ClientC.b - Time.deltaTime * DisappearSpeed, 0, 255);
        }
        if (Client != null)
        {
            Client.color = ClientC / 255;
            if (Client.color.r == 0)
            {
                Destroy(Client.gameObject);
                ScenesManager.sceneManagerInstance.NextScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    #region Bartender

    //UIManager recoge al cliente y su velocidad de aparición, que utilizará para desaparecer.
    public void GetClientSprite(SpriteRenderer Client, float Speed)
    {
        this.Client = Client;
        DisappearSpeed = Speed;
    }

    //UIManager recoge el dialogo que se va a escribir ese dia
    public void GetDialogue(DataContainer.Texto[] dialogue)
    {
        this.dialogue = dialogue;
        DialogueLine = 0; //Inicializa el dialogo en el primer texto
        way = 0; //empieza en GoodWay
        DetectarEstatus(); //Detecta que estado tiene el primer texto
    }

    //Al pulsar continuar, empieza a escribir la siguiente frase
    public void SkipButton() 
    {
        if (dialogue != null && (dialogue[DialogueLine].GoodText == dialogueBox.text || dialogue[DialogueLine].BadText == dialogueBox.text)) //Si el texto ha acabado queremos que pase al siguiente
        {
            if (DialogueLine < dialogue.GetLength(0) - 1) //Si queda texto pasa al siguiente
            {
                DialogueLine++;
                DetectarEstatus();//Detecta lo que tiene que hacer en función del estado del texto
            }
            else //Si no queda texto no escribe nada y empieza a desaparecer
            {
                dialogueSkipButton.gameObject.SetActive(false);
                dialogueBox.text = " ";
                ClientDisappear = true;
            }

        }
        else //Si el texto no ha acabado de escribirse queremos que se termine de escribir entero
        {
            SkipDialogue = true;
        }
    }


    public void OptionL() //Cuando es pulsado el boton izquierdo
    {
        //desactiva las opciones y activa el boton de saltar dialogo
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);
        dialogueSkipButton.gameObject.SetActive(true);

        //Si el texto del botón corresponde con la opción buena, el dialogo sigue el buen camino.
        //Si el texto del botón corresponde con la opción mala, el diálogo sigue el mal camino.
        if (option1BText.text == dialogue[DialogueLine].GoodText) way = 0;
        else way = 1;

        //Escribe la respuesta del jugador
        DialogueLine++;
        StartCoroutine(Write());
    }
    public void OptionR() //Cuando es pulsado el boton derecho
    {
        //desactiva las opciones y activa el boton de saltar dialogo
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);
        dialogueSkipButton.gameObject.SetActive(true);

        //Si el texto del botón corresponde con la opción buena, el dialogo sigue el buen camino.
        //Si el texto del botón corresponde con la opción mala, el diálogo sigue el mal camino.
        if (option2BText.text == dialogue[DialogueLine].GoodText) way = 0;
        else way = 1;

        //Escribe la respuesta del jugador
        DialogueLine++;
        StartCoroutine(Write());
    }
    public void DetectarEstatus()
    {
        #region Comportamiento del monólogo

        if (dialogue[DialogueLine].estatus == DataContainer.Estado.monologo) //si es monologo, escribe
        {
            dialogueSkipButton.gameObject.SetActive(true);
            StartCoroutine(Write()); //escribe el resto
        }

        #endregion

        #region Comportamiento del diálogo

        else if (dialogue[DialogueLine].estatus == DataContainer.Estado.dialogo) //si es dialogo
        {
            //oculta el boton de saltar o continuar y activa las opciones
            dialogueSkipButton.gameObject.SetActive(false);
            option1Button.gameObject.SetActive(true);
            option2Button.gameObject.SetActive(true);

            //Escribe el diálogo o pregunta del cliente
            StartCoroutine(Write());

            //Pone el texto de dialogo siguiente en el texto de las opciones
            //Siendo el bueno una opción y el malo otra opción
            //Esto de forma aleatoria para que los botones no tengan un patrón de respuesta buena o mala
            DialogueLine++;
            int tmp = Random.Range(0, 2);
            if (tmp == 0)
            {
                option1BText.text = dialogue[DialogueLine].GoodText;
                option2BText.text = dialogue[DialogueLine].BadText;
            }
            else
            {
                option1BText.text = dialogue[DialogueLine].BadText;
                option2BText.text = dialogue[DialogueLine].GoodText;
            }
        }
        #endregion

        #region Comportamiento de la bebida

        #endregion

    }
    #endregion //termina región de bartender

    #endregion //Termina región métodos públicos

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    //Corrutina que se encarga de escribir el texto que tiene que escribir char a char
    IEnumerator Write()
    {
        dialogueBox.text = string.Empty;

        //Elige el recorrido de la conversación a escribir
        string dialogueOnly = GoodBadDialogue();

        //Recorre el tamaño del texto que tiene que escribir y se va escribiendo char por char
        for (int i = 0; i < dialogueOnly.Length; i++)
        {
            char ch = dialogueOnly[i];
            if (SkipDialogue)
            {
                dialogueBox.text = dialogueOnly;
                i = dialogueOnly.Length;
                SkipDialogue = false;
            }
            else dialogueBox.text += ch;
            yield return new WaitForSeconds(TypeSpeed);
        }
    }

    private string GoodBadDialogue() //string que mira si esta en Good Way o Bad Way y lo devuelve para escribirlo
    {
        //Si se encuentra en el buen camino escribe el texto del buen camino
        //Lo mismo al revés
        string dialogueOnly;
        if (way == 0) //good way
        {
            dialogueOnly = dialogue[DialogueLine].GoodText;
        }
        else //bad text
        {
            dialogueOnly = dialogue[DialogueLine].BadText;
        }

        return dialogueOnly;
    }

    #endregion

} // class UIManager 
// namespace
