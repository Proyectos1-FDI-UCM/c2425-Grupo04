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
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField]
    TextMeshProUGUI dialogueBox, dialogueSkipBText, option1BText, option2BText,//DIALOGOS Y MONÓLOGOS

                    recompensa, material1, material2, material3, nombreBebida, servirBText;//BEBIDAS
    [SerializeField]
    private Image DrinkImage, material1Image, material2Image, material3Image;
    [SerializeField]
    Button dialogueSkipButton, ServirButton;
    [SerializeField]
    private float TypeSpeed;
    [SerializeField]
    private Button option1Button, option2Button;

    [SerializeField]
    private float DisappearSpeed;

    [SerializeField]
    private Button regresarMats;

    [Header("ELEMENTOS BOTONES MATERIALES")]
    [SerializeField]
    private Button[] materials = new Button[8];

    [SerializeField]
    private TextMeshProUGUI[] matNums = new TextMeshProUGUI[8];


    [Header("ELEMENTOS MATERIALES EN CESTA")]
    [SerializeField]
    private Image[] matCestaImages = new Image[8];

    [SerializeField]
    private TextMeshProUGUI[] matsNumsEnCesta = new TextMeshProUGUI[8];



    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Texto[] dialogue;
    private int way; // 0 = good way , 1 = bad way
    private int DialogueLine = 0;
    private bool SkipDialogue = false, ClientDisappear = false, matsReqEnCesta = false;
    private SpriteRenderer Client;
    private Color ClientC, invisible, visible;
    private GameObject Drink;
    private bool mat1Yes = true, mat2Yes = true, mat3Yes = true;

    private float[] recursos;
    private int[] matsEnCesta = new int[8];    //0.JugoManzana   1.JugoUva   2.PielManzana   3.PielUva   4.SemillaManzana   5.SemillaUva   6.Levadura   7.Hielo
    private bool IfHavefalse = false;
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
        if (ServirButton != null) ServirButton.gameObject.SetActive(false);
        if (recompensa != null) recompensa.text = " ";
        if (nombreBebida != null) nombreBebida.text = " ";
        if (dialogueSkipButton != null) dialogueSkipButton.gameObject.SetActive(false);

        invisible.r = 255;
        invisible.g = 255;
        invisible.b = 255;
        invisible.a = 0;

        visible.r = 255;
        visible.g = 255;
        visible.b = 255;
        visible.a = 255;

        if (DrinkImage.gameObject != null)
        {
            DrinkImage.color = invisible;
            material1Image.color = invisible;
            material2Image.color = invisible;
            material3Image.color = invisible;
        }


        ClientC.r = 255;
        ClientC.g = 255;
        ClientC.b = 255;
        ClientC.a = 255;

        recursos = GameManager.Instance.GetRecursos();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (dialogueSkipBText != null)
        {
            if (dialogue != null && (dialogue[DialogueLine].goodText == dialogueBox.text || dialogue[DialogueLine].badText == dialogueBox.text)) dialogueSkipBText.text = "Continuar";
            else dialogueSkipBText.text = "Saltar";
        }


        if (ClientDisappear)
        {
            ClientC.r = Mathf.Clamp(ClientC.r - Time.deltaTime * DisappearSpeed, 0, 255);
            ClientC.g = Mathf.Clamp(ClientC.g - Time.deltaTime * DisappearSpeed, 0, 255);
            ClientC.b = Mathf.Clamp(ClientC.b - Time.deltaTime * DisappearSpeed, 0, 255);

            if (Client != null)
            {
                Client.color = ClientC / 255;
                Debug.Log($"R: {ClientC.r}     G:{ClientC.g}     B:{ClientC.b}");
                Debug.Log($"R: {Client.color.r}     G:{Client.color.g}     B:{Client.color.b}");
                if (Client.color.r == 0 && Client.color.g == 0 && Client.color.b == 0)
                {
                    Destroy(Client.gameObject);

                    if (!IfHavefalse)
                        GameManager.Instance.increaseSospechosos(-2);
                    ScenesManager.sceneManagerInstance.NextScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }

        for (int i = 0; i < recursos.Length; i++)
        {
            matNums[i].text = recursos[i].ToString();
            matsNumsEnCesta[i].text = matsEnCesta[i].ToString();

            if (matsEnCesta[i] > 0) matCestaImages[i].gameObject.SetActive(true);
            else matCestaImages[i].gameObject.SetActive(false);
        }
        int matsTotales = 0;
        for (int i = 0; i < matsEnCesta.Length; i++)
        {
            matsTotales += matsEnCesta[i];
        }

        if (matsTotales > 0) regresarMats.gameObject.SetActive(true);
        else regresarMats.gameObject.SetActive(false);

        if (Drink != null && Drink.GetComponent<CastDrink>().GetDrinkMaterials() != null)
        {
            //Mira si cada uno de los materiales estan en la cesta
            for (int i = 0; i < matsEnCesta.Length; i++)
            {
                if (Drink.GetComponent<CastDrink>().GetDrinkMaterials().Length >= 1 && matCestaImages[i].sprite == Drink.GetComponent<CastDrink>().GetDrinkMaterials()[0].material.GetComponent<SpriteRenderer>().sprite)
                {
                    if (matsEnCesta[i] >= Drink.GetComponent<CastDrink>().GetDrinkMaterials()[0].amount)
                    {
                        mat1Yes = true;
                    }
                    else
                    {
                        mat1Yes = false;
                    }
                }
                if (Drink.GetComponent<CastDrink>().GetDrinkMaterials().Length >= 2 && matCestaImages[i].sprite == Drink.GetComponent<CastDrink>().GetDrinkMaterials()[1].material.GetComponent<SpriteRenderer>().sprite)
                {
                    if (matsEnCesta[i] >= Drink.GetComponent<CastDrink>().GetDrinkMaterials()[1].amount)
                    {
                        mat2Yes = true;
                    }
                    else
                    {
                        mat2Yes = false;
                    }
                }
                if (Drink.GetComponent<CastDrink>().GetDrinkMaterials().Length >= 3 && matCestaImages[i].sprite == Drink.GetComponent<CastDrink>().GetDrinkMaterials()[2].material.GetComponent<SpriteRenderer>().sprite)
                {
                    if (matsEnCesta[i] >= Drink.GetComponent<CastDrink>().GetDrinkMaterials()[2].amount)
                    {
                        mat3Yes = true;
                    }
                    else
                    {
                        mat3Yes = false;
                    }
                }
            }
            //Si todos los materiales estan el la cesta
            if (mat1Yes && mat2Yes && mat3Yes)
            {
                matsReqEnCesta = true;
            }
            else
            {
                matsReqEnCesta = false;
            }
        }

        //Cambio de texto del boton de servir si estan los materiales pedidos en la cesta
        if (matsReqEnCesta)
        {
            servirBText.text = "Servir";
        }
        else
        {
            servirBText.text = "No servir";
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    #region Bartender

    //UIManager recoge al cliente y su velocidad de aparición, que utilizará para desaparecer.
    public void GetClientSprite(SpriteRenderer Client)
    {
        this.Client = Client;
    }

    //UIManager recoge el dialogo que se va a escribir ese dia
    public void GetDialogue(Texto[] dialogue)
    {
        dialogueSkipButton.gameObject.SetActive(true);

        this.dialogue = dialogue;
        DialogueLine = 0; //Inicializa el dialogo en el primer texto
        way = 0; //empieza en GoodWay
        StartCoroutine(Write()); //Escribe el primer texto
    }

    //UIManager recoge la bebida que va a pedir el cliente
    public void GetDrink(GameObject Drink)
    {
        this.Drink = Drink;
    }

    //Al pulsar continuar, empieza a escribir la siguiente frase
    public void SkipButton()
    {
        if (dialogue != null && (dialogue[DialogueLine].goodText == dialogueBox.text || dialogue[DialogueLine].badText == dialogueBox.text)) //Si el texto ha acabado queremos que pase al siguiente
        {
            if (DialogueLine < dialogue.GetLength(0) - 1) //Si queda texto pasa al siguiente
            {
                DialogueLine++;
                StartCoroutine(Write()); //Escribe texto
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
        if (option1BText.text == dialogue[DialogueLine].goodText)
        {
            IfHavefalse = false;
            way = 0;
        }

        else
        {
            GameManager.Instance.increaseSospechosos(1);
            way = 1;
            IfHavefalse = true;
        }


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
        if (option2BText.text == dialogue[DialogueLine].goodText)
        {
            way = 0;
            IfHavefalse = false;
        }

        else
        {
            GameManager.Instance.increaseSospechosos(1);
            way = 1;
            IfHavefalse = true;
        }

        //Escribe la respuesta del jugador

        DialogueLine++;
        StartCoroutine(Write());
    }

    public void DetectarEstatus()
    {
        #region Comportamiento del monólogo
        //Si es un monólogo no hace nada porque el texto ya se ha escrito
        #endregion

        #region Comportamiento del diálogo

        if (dialogue[DialogueLine].estatus == Estado.dialogo) //si es dialogo
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
                option1BText.text = dialogue[DialogueLine].goodText;
                option2BText.text = dialogue[DialogueLine].badText;
            }
            else
            {
                option1BText.text = dialogue[DialogueLine].badText;
                option2BText.text = dialogue[DialogueLine].goodText;
            }
        }
        #endregion

        #region Comportamiento de la bebida

        else if (dialogue[DialogueLine].estatus == Estado.bebida)
        {
            string DialogueOnly = GoodBadDialogue();
            //Todo esto se cambia despues de terminar de pedir
            dialogueSkipButton.gameObject.SetActive(false); //Desactiva el botón de continuar/saltar
            ServirButton.gameObject.SetActive(true); //Activa el botón de servir

            //Actualiza el nombre de la bebida pedida
            nombreBebida.text = Convert.ToString(Drink.name).Replace("_", " ");

            //Actualiza la imagen de la bebida pedida
            DrinkImage.sprite = Drink.GetComponent<SpriteRenderer>().sprite;
            DrinkImage.color = visible;

            //Actualiza la recompensa
            recompensa.text = $"{Drink.GetComponent<CastDrink>().GetDrinkReward()} monedas";

            //Actualiza el listado de materiales
            if (Drink.GetComponent<CastDrink>().GetDrinkMaterials().Length >= 3)
            {
                material3.text = $"x{Drink.GetComponent<CastDrink>().GetDrinkMaterials()[2].amount}";
                material3Image.sprite = Drink.GetComponent<CastDrink>().GetDrinkMaterials()[2].material.GetComponent<SpriteRenderer>().sprite;
                material3Image.color = visible;
                mat3Yes = false;
            }

            if (Drink.GetComponent<CastDrink>().GetDrinkMaterials().Length >= 2)
            {
                material2.text = $"x{Drink.GetComponent<CastDrink>().GetDrinkMaterials()[1].amount}";
                material2Image.sprite = Drink.GetComponent<CastDrink>().GetDrinkMaterials()[1].material.GetComponent<SpriteRenderer>().sprite;
                material2Image.color = visible;
                mat2Yes = false;
            }

            if (Drink.GetComponent<CastDrink>().GetDrinkMaterials().Length >= 1)
            {
                material1.text = $"x{Drink.GetComponent<CastDrink>().GetDrinkMaterials()[0].amount}";
                material1Image.sprite = Drink.GetComponent<CastDrink>().GetDrinkMaterials()[0].material.GetComponent<SpriteRenderer>().sprite;
                material1Image.color = visible;
                mat1Yes = false;
            }

        }

        #endregion

    }


    public void SumarMaterial(Button material)
    {
        int i = 0;
        bool selectedMatFound = false;

        while (i < materials.Length && !selectedMatFound)
        {
            if (material == materials[i]) selectedMatFound = true;
            else i++;
        }
        //Debug.Log(i);

        if (recursos[i] > 0)
        {
            matsEnCesta[i]++;
            recursos[i]--;
        }
        //Debug.Log(matsEnCesta[0] + " , " + matsEnCesta[1] + " , " + matsEnCesta[2] + " , " + matsEnCesta[3] + " , " + matsEnCesta[4] + " , " + matsEnCesta[5] + " , " + matsEnCesta[6] + " , " + matsEnCesta[7]);


    }


    public void RegresarMats()
    {
        for (int i = 0; i < matsEnCesta.Length; i++)
        {
            recursos[i] += matsEnCesta[i];
            matsEnCesta[i] = 0;
        }
    }


    public void Servir()
    {
        if (matsReqEnCesta)//Si están los materiales requeridos en el pedido, quita la cantidad del pedido de la cesta
        {
            for (int i = 0; i < matsEnCesta.Length; i++)
            {
                if (matsEnCesta[i] > 0)
                {
                    for (int j = 0; j < Drink.GetComponent<CastDrink>().GetDrinkMaterials().Length; j++)
                    {
                        if (matCestaImages[i].sprite == Drink.GetComponent<CastDrink>().GetDrinkMaterials()[j].material.GetComponent<SpriteRenderer>().sprite)
                        {
                            matsEnCesta[i] -= Drink.GetComponent<CastDrink>().GetDrinkMaterials()[j].amount;
                        }
                    }
                }
            }
            //Como ha hecho el encargo pedido, el dialogo ira por good
            way = 0;
            GameManager.Instance.increaseDinero(Drink.GetComponent<CastDrink>().GetDrinkReward());
        }
        else
        {
            //Si no ha hecho el encargo pedido, no se quitan ningun material de la cesta y el dialogo ira por bad
            way = 1;
        }
        //Devuelve resto al inventario
        RegresarMats();

        //Desactiva los botones y limpia el encargo y recompensas y activa los del dialogo
        ServirButton.gameObject.SetActive(false);
        dialogueSkipButton.gameObject.SetActive(true);
        recompensa.text = " ";
        nombreBebida.text = " ";
        material1.text = " ";
        material1Image.color = invisible;
        material2.text = " ";
        material2Image.color = invisible;
        material3.text = " ";
        material3Image.color = invisible;
        //Escribe siguiente dialogo
        SkipButton();
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
        dialogueSkipButton.gameObject.SetActive(true); // Activa el botón de continuar/saltar
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
            dialogueOnly = dialogue[DialogueLine].goodText;
        }
        else //bad text
        {
            dialogueOnly = dialogue[DialogueLine].badText;
        }

        return dialogueOnly;
    }

    #endregion

} // class UIManager 
// namespace
