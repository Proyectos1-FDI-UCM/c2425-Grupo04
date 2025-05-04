//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using TMPro;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class TutorialDialoguesUIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] Sprite OrionSprite;
    [SerializeField] private Image Box;
    [SerializeField]
    private float TypeSpeed;
    [SerializeField]
    private AudioClip TypingSFX;
    [SerializeField]
    private FinalScene ScriptFinalScene;
    [SerializeField]
    private TextMeshProUGUI dialogueBox, dialogueSkipBText, option1BText, option2BText;
    [SerializeField]
    private Image CharacterPortrait;
    [SerializeField]
    private Button dialogueSkipButton;
    [SerializeField]
    private Button option1Button, option2Button;
    [SerializeField]
    private bool ChangeSceneAtEnd;
    [SerializeField]
    private int NextScene;
    [SerializeField]
    private AudioClip paperSFX;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Texto[] dialogue;
    private int way; // 0 = good way , 1 = bad way
    private int DialogueLine = 0;
    private bool SkipDialogue = false, ClientDisappear = false;
    private SpriteRenderer Client;
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
        GameManager.Instance.SetTDUI(this);

        if (dialogueSkipButton != null)
        {
            dialogueSkipButton.gameObject.SetActive(false);
        }

        CharacterPortrait.gameObject.SetActive(false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (dialogueSkipBText != null)
        {
            if (dialogue != null &&
                (dialogue[DialogueLine].GoodText == dialogueBox.text ||
                dialogue[DialogueLine].BadText == dialogueBox.text))
            {
                dialogueSkipBText.text = "Continuar";
            }
            else
            {
                dialogueSkipBText.text = "Saltar";
            }
        }

        if (ClientDisappear)
        {
            Box.gameObject.SetActive(false);
            if (ChangeSceneAtEnd)
            {
                GameManager.Instance.ChangeScene(NextScene);
            }
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    //UIManager recoge el sprite de quien contiene los diálogos
    public void GetClientSprite(SpriteRenderer Client)
    {
        this.Client = Client;
    }

    //UIManager recoge el dialogo que se va a escribir
    public void GetDialogue(Texto[] dialogue)
    {
        dialogueSkipButton.gameObject.SetActive(true);

        this.dialogue = dialogue;
        DialogueLine = 0; //Inicializa el dialogo en el primer texto
        way = 0; //empieza en GoodWay
        CharacterPortrait.gameObject.SetActive(true);
        StartCoroutine(Write()); //Escribe el primer texto
    }

    //Al pulsar continuar, empieza a escribir la siguiente frase
    public void SkipButton()
    {
        AudioManager.Instance.PlaySFX(paperSFX);

        if (dialogue != null && (dialogue[DialogueLine].GoodText == dialogueBox.text || dialogue[DialogueLine].BadText == dialogueBox.text)) //Si el texto ha acabado queremos que pase al siguiente
        {
            if (DialogueLine < dialogue.GetLength(0) - 1) //Si queda texto pasa al siguiente
            {
                DialogueLine++;
                StartCoroutine(Write()); //Escribe texto
            }
            else //Si no queda texto no escribe nada y empieza a desaparecer
            {
                CharacterPortrait.gameObject.SetActive(false);
                dialogueSkipButton.gameObject.SetActive(false);
                dialogueBox.text = " ";
                ClientDisappear = true;

                if (ScriptFinalScene != null)
                {
                    ScriptFinalScene.StartAttack();
                }
            }

        }
        else //Si el texto no ha acabado de escribirse queremos que se termine de escribir entero
        {
            SkipDialogue = true;
        }
    }

    public void OptionL() //Cuando es pulsado el boton izquierdo
    {
        AudioManager.Instance.PlaySFX(paperSFX);

        //desactiva las opciones y activa el boton de saltar dialogo
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);
        dialogueSkipButton.gameObject.SetActive(true);

        //Si el texto del botón corresponde con la opción buena, el dialogo sigue el buen camino.
        //Si el texto del botón corresponde con la opción mala, el diálogo sigue el mal camino.
        if (option1BText.text == dialogue[DialogueLine].GoodText)
        {
            way = 0;
        }

        else
        {
            GameManager.Instance.increaseSospechosos(1);
            way = 1;
        }


        DialogueLine++;
        StartCoroutine(Write());
    }

    public void OptionR() //Cuando es pulsado el boton derecho
    {
        AudioManager.Instance.PlaySFX(paperSFX);

        //desactiva las opciones y activa el boton de saltar dialogo
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);
        dialogueSkipButton.gameObject.SetActive(true);

        //Si el texto del botón corresponde con la opción buena, el dialogo sigue el buen camino.
        //Si el texto del botón corresponde con la opción mala, el diálogo sigue el mal camino.
        if (option2BText.text == dialogue[DialogueLine].GoodText)
        {
            way = 0;
        }

        else
        {
            GameManager.Instance.increaseSospechosos(1);
            way = 1;
        }

        //Escribe la respuesta del jugador

        DialogueLine++;
        StartCoroutine(Write());
    }

    /// <summary>
    /// Se encarga de detectar el estado del texto a escribir y actuar conforme a este
    /// </summary>
    public void DetectarEstatus()
    {
        #region Comportamiento del monólogo
        //Si es un monólogo no hace nada porque el texto ya se ha escrito
        #endregion

        #region Comportamiento del diálogo

        if (dialogue[DialogueLine].Estatus == Estado.Dialogo) //si es dialogo
        {
            //oculta el boton de saltar o continuar y activa las opciones
            dialogueSkipButton.gameObject.SetActive(false);
            option1Button.gameObject.SetActive(true);
            option2Button.gameObject.SetActive(true);

            //Pone el texto de dialogo siguiente en el texto de las opciones
            //Siendo el bueno una opción y el malo otra opción
            //Esto de forma aleatoria para que los botones no tengan un patrón de respuesta buena o mala
            DialogueLine++;
            int tmp = UnityEngine.Random.Range(0, 2);
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

    #endregion //Termina región métodos públicos

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    //Corrutina que se encarga de escribir el texto que tiene que escribir char a char
    IEnumerator Write()
    {
        dialogueBox.text = string.Empty;

        //Elige el recorrido de la conversación a escribir
        string dialogueOnly = GoodBadDialogue();
        dialogueSkipButton.gameObject.SetActive(true); // Activa el botón de continuar/saltar
        //Cambia la imagen del emisor
        if (dialogue[DialogueLine].Emisor == Emisor.Jugador)
        {
            CharacterPortrait.sprite = OrionSprite;
        }
        else if (dialogue[DialogueLine].Emisor == Emisor.Cliente)
        {
            CharacterPortrait.sprite = Client.sprite;
        }
        else CharacterPortrait.sprite = null;

        //Recorre el tamaño del texto que tiene que escribir y se va escribiendo char por char
        for (int i = 0; i < dialogueOnly.Length; i++)
        {
            AudioManager.Instance.PlaySFX(TypingSFX);
            char ch = dialogueOnly[i];
            if (SkipDialogue)
            {
                dialogueBox.text = dialogueOnly;
                i = dialogueOnly.Length;
                SkipDialogue = false;
            }
            else dialogueBox.text += ch;

            //Si acaba de hablar realiza la acción que deba realizar
            if (dialogueBox.text == dialogueOnly)
            {
                DetectarEstatus();
            }
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

} // class TutorialDialoguesUIManager 
// namespace
