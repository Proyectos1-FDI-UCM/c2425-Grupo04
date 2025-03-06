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

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField]
    TextMeshProUGUI dialogueBox, dialogueSkipBText, option1BText, option2BText, inventario;
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
    private float[] recursos, Nrecursos;
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
        if (dialogue != null && (dialogue[DialogueLine].GoodText == dialogueBox.text || dialogue[DialogueLine].BadText == dialogueBox.text)) dialogueSkipBText.text = "Continuar";
        else dialogueSkipBText.text = "Saltar";

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
                ScenesManager.sceneManagerInstance.NextScene();
            }
        }
        // --- INVENTARIO PROVISIONAL ---
        recursos = GameManager.Instance.GetResources();
        Nrecursos = GameManager.Instance.GetNResources();
        if (inventario != null)
        inventario.text = $"Jugo de uva: {recursos[0]}, Piel de uva: {recursos[1]}, Semilla de uva: {recursos[2]}, Jugo de manzana: {recursos[3]}, Piel de manzana: {recursos[4]}, Semilla de manzana: {recursos[5]}, Hielo: {Nrecursos[0]}, Levadura: {Nrecursos[1]}";
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos


    public void GetClientSprite(SpriteRenderer Client, float Speed)
    {
        this.Client = Client;
        DisappearSpeed = Speed;
    }
    public void GetDialogue(DataContainer.Texto[] dialogue) //consigue el dialogo
    {
        this.dialogue = dialogue;
        DialogueLine = 0;
        way = 0; //empieza en GoodWay
        DetectarEstatus();
    }

    public void SkipButton() //al pulsar continuar, empieza a escribir la siguiente frase
    {
        if (dialogue != null && (dialogue[DialogueLine].GoodText == dialogueBox.text || dialogue[DialogueLine].BadText == dialogueBox.text))
        {
            if (DialogueLine < dialogue.GetLength(0) - 1)
            {
                DialogueLine++;
                DetectarEstatus();
            }
            else
            {
                dialogueSkipButton.gameObject.SetActive(false);
                dialogueBox.text = " ";
                ClientDisappear = true;
            }

        }
        else
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
        if (option1BText.text == dialogue[DialogueLine].GoodText) way = 0; //TEMPORAL Es respuesta correcta
        else way = 1;
        //Escribe la respuesta del jugador
        DialogueLine++;
        StartCoroutine(Write());
    }
    public void OptionR() //Cuando es pulsado el boton derecho
    {
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);
        dialogueSkipButton.gameObject.SetActive(true);
        if (option2BText.text == dialogue[DialogueLine].GoodText) way = 0; //TEMPORAL Es respuesta correcta
        else way = 1;
        DialogueLine++;
        StartCoroutine(Write());
    }
    public void DetectarEstatus()
    {
        if(dialogue[DialogueLine].estatus == DataContainer.Estado.monologo) //si es monologo, escribe
        {
            dialogueSkipButton.gameObject.SetActive(true);
            StartCoroutine(Write()); //escribe el resto
        }
        else if(dialogue[DialogueLine].estatus == DataContainer.Estado.dialogo) //si es dialogo
        {
            //oculta el boton de saltar, y activa las opciones
            dialogueSkipButton.gameObject.SetActive(false);
            option1Button.gameObject.SetActive(true);
            option2Button.gameObject.SetActive(true);
            //Escribe
            StartCoroutine(Write());
            //Pone el texto de dialogo siguiente en el texto de las opciones
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
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    IEnumerator Write()
    {
        dialogueBox.text = string.Empty;
        string dialogueOnly = GoodBadDialogue();

        //Recorre el tamaño del texto que tiene que escribir y se va escribiendo char por char
        for (int i = 0; i < dialogueOnly.Length; i++)//aqui he puesto .GoodText pero esto es mentira, hay que modificarlo a Good o Bad en función de la elección anterior
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
