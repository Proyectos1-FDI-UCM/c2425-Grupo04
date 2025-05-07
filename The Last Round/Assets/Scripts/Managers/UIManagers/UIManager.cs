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
using System;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] GameObject gameOverUI;

    [SerializeField] private int NextScene;

    [SerializeField] Sprite OrionSprite;

    [SerializeField]
    private float TypeSpeed;

    [SerializeField]
    private AudioClip TypingSFX;

    [SerializeField]
    private float DisappearSpeed;

    [SerializeField]
    private TextMeshProUGUI dialogueBox, dialogueSkipBText, option1BText, option2BText, EmisorName,//DIALOGOS Y MONÓLOGOS

                    recompensa, material1, material2, material3, nombreBebida, servirBText;//BEBIDAS
    [SerializeField]
    private Image DrinkImage, material1Image, material2Image, material3Image, CharacterPortrait;
    [SerializeField]
    private Button dialogueSkipButton, ServirButton;

    [SerializeField]
    private Button option1Button, option2Button;


    [Header("SISTEMA DE CREACIÓN DE BEBIDAS")]
    [SerializeField]
    private Button regresarMats;

    [Header("MATERIALES DE LAS BEBIDAS")]
    [SerializeField]
    private GameObject[] Sources;

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

    [SerializeField] TextMeshProUGUI dineroTotalText;
    private float dineroTotal;

    [SerializeField] Animator Animator;

    [SerializeField]
    private AudioClip woodSfx, paperSfx, ClienteSfx1,CLienteSFX2,ServirSFX,NoServirSFX,regresar,sospechosoSfx,materialSfx;

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
    private int buttonUsing = 0;
    private float[] recursos;
    private int[] matsEnCesta = new int[8];    //0.JugoManzana   1.JugoUva   2.PielManzana   3.PielUva   4.SemillaManzana   5.SemillaUva   6.Levadura   7.Hielo
    private bool PickedBadChoice = false;
    private bool NoSkipSFX = true;
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
        AudioManager.Instance.PlaySFX(ClienteSfx1);
        dineroTotal = GameManager.Instance.GetDineros();
        dineroTotalText.text = dineroTotal.ToString();
        //Debug.Log(dineroTotal);
        GameManager.Instance.GiveUI(this);

        //Comprueba si el GameManager ha sumado 2 al sistema de sospecha antes de cambiar de escena
        //Si es así y el resultado es 8 o mayor
        GameManager.Instance.increaseSospechosos(0);

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

        for (int j = 0; j < matNums.Length; j++)
        {
            //Actualiza los textos
            matNums[(int)Sources[j].GetComponent<CastMaterial>().GetSourceName()].text = recursos[(int)Sources[j].GetComponent<CastMaterial>().GetSourceName()].ToString();
        }

        CharacterPortrait.gameObject.SetActive(false);
        DrinkImage.gameObject.SetActive(false);

        if (Animator != null)
        {
            Animator.Play($"ContSospecha{GameManager.Instance.GiveSospecha()}");
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        dineroTotal = GameManager.Instance.GetDineros();
        dineroTotalText.text = dineroTotal.ToString();


        if (dialogueSkipBText != null)
        {
            if (dialogue != null && (dialogue[DialogueLine].GoodText == dialogueBox.text || dialogue[DialogueLine].BadText == dialogueBox.text)) dialogueSkipBText.text = "Continuar";
            else dialogueSkipBText.text = "Saltar";
        }

        if (ClientDisappear)
        {
            if (!PickedBadChoice)
            {
               
                GameManager.Instance.increaseSospechosos(-1);
            }
            ClientC.r = Mathf.Clamp(ClientC.r - Time.deltaTime * DisappearSpeed, 0, 255);
            ClientC.g = Mathf.Clamp(ClientC.g - Time.deltaTime * DisappearSpeed, 0, 255);
            ClientC.b = Mathf.Clamp(ClientC.b - Time.deltaTime * DisappearSpeed, 0, 255);
            ClientC.a = Mathf.Clamp(ClientC.a - Time.deltaTime * DisappearSpeed, 0, 255);

            if (Client != null)
            {
                Client.color = ClientC / 255;
                //Debug.Log($"R: {ClientC.r}     G:{ClientC.g}     B:{ClientC.b}");
                //Debug.Log($"R: {Client.color.r}     G:{Client.color.g}     B:{Client.color.b}");
                if (Client.color.r == 0 && Client.color.g == 0 && Client.color.b == 0 && Client.color.a == 0)
                {
                    Destroy(Client.gameObject);

                    //Por si hay algo en la cesta se lo devuelve antes de irse de la escena
                    NoSkipSFX = false;
                    RegresarMats();

                    GameManager.Instance.ChangeScene(NextScene);
                }
            }
        }

        //Actualiza la cantidad de materiales
        for (int i = 0; i < recursos.Length; i++)
        {
            //Actualiza la cantidad de materiales al inventario que tienes
            matNums[i].text = recursos[i].ToString();

            //Actualiza la cantidad de materiales que hay en la cesta
            //matsNumsEnCesta[i].text = matsEnCesta[i].ToString();

            //En el caso de la cesta, si no hay ningún material simplemente no se verá el material
            if (matsEnCesta[i] > 0) matCestaImages[i].gameObject.SetActive(true);
            else matCestaImages[i].gameObject.SetActive(false);
        }

        //Contador con todos los materiales añadidos a la cesta
        int matsTotales = 0;
        for (int i = 0; i < matsEnCesta.Length; i++)
        {
            matsTotales += matsEnCesta[i];
        }

        //Si hay algún material en la cesta permite regresarlos
        //Si no hay ningún material en la cesta, no da esa opción
        if (matsTotales > 0) regresarMats.gameObject.SetActive(true);
        else regresarMats.gameObject.SetActive(false);

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
        CharacterPortrait.gameObject.SetActive(true);
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
        if(NoSkipSFX)
        { AudioManager.Instance.PlaySFX(paperSfx); }
        

        if (dialogue != null && (dialogue[DialogueLine].GoodText == dialogueBox.text || dialogue[DialogueLine].BadText == dialogueBox.text)) //Si el texto ha acabado queremos que pase al siguiente
        {
            
            if (DialogueLine < dialogue.GetLength(0) - 1) //Si queda texto pasa al siguiente
            {
                DialogueLine++;
                StartCoroutine(Write()); //Escribe texto
            }
            else //Si no queda texto no escribe nada y empieza a desaparecer
            {
                AudioManager.Instance.PlaySFX(CLienteSFX2);
                CharacterPortrait.gameObject.SetActive(false);
                EmisorName.gameObject.SetActive(false);
                dialogueSkipButton.gameObject.SetActive(false);
                dialogueBox.text = " ";
                ClientDisappear = true;
            }

        }
        else //Si el texto no ha acabado de escribirse queremos que se termine de escribir entero
        {
            SkipDialogue = true;
        }
        NoSkipSFX = true;
    }

    public void OptionL() //Cuando es pulsado el boton izquierdo
    {
        AudioManager.Instance.PlaySFX(paperSfx);
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
            AudioManager.Instance.PlaySFX (sospechosoSfx);
            GameManager.Instance.increaseSospechosos(1);
            way = 1;
            PickedBadChoice = true;
        }


        DialogueLine++;
        StartCoroutine(Write());
    }

    public void OptionR() //Cuando es pulsado el boton derecho
    {
        AudioManager.Instance.PlaySFX(paperSfx);
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
            AudioManager.Instance.PlaySFX(sospechosoSfx);
            GameManager.Instance.increaseSospechosos(1);
            way = 1;
            PickedBadChoice = true;
        }

        //Escribe la respuesta del jugador

        DialogueLine++;
        StartCoroutine(Write());
    }

    public Animator GiveAnimator()
    {
        return Animator;
    }

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

        else if (dialogue[DialogueLine].Estatus == Estado.Bebida)
        {
            string DialogueOnly = GoodBadDialogue();
            //Todo esto se cambia despues de terminar de pedir
            dialogueSkipButton.gameObject.SetActive(false); //Desactiva el botón de continuar/saltar
            ServirButton.gameObject.SetActive(true); //Activa el botón de servir

            //Actualiza el nombre de la bebida pedida
            nombreBebida.text = Convert.ToString(Drink.name).Replace("_", " ");

            //Actualiza la imagen de la bebida pedida
            DrinkImage.gameObject.SetActive(true);
            DrinkImage.sprite = Drink.GetComponent<SpriteRenderer>().sprite;
            DrinkImage.color = visible;

            //Actualiza la recompensa
            if (Client.gameObject.GetComponent<CastEnemy>() != null &&
                Client.gameObject.GetComponent<CastEnemy>().GetEnemyType() == EnemyType.Alcalde)
            {
                recompensa.text = $"{Drink.GetComponent<CastDrink>().GetDrinkReward() * 2} monedas";
            }
            else
            {
                recompensa.text = $"{Drink.GetComponent<CastDrink>().GetDrinkReward()} monedas";
            }

            //Actualiza el listado de materiales
            if (Drink.GetComponent<CastDrink>().GetDrinkMaterials().Length >= 3)
            {
                material3.text = $"x{Drink.GetComponent<CastDrink>().GetDrinkMaterials()[2].Amount}";
                material3Image.sprite = Drink.GetComponent<CastDrink>().GetDrinkMaterials()[2].Material.GetComponent<SpriteRenderer>().sprite;
                material3Image.color = visible;
            }

            if (Drink.GetComponent<CastDrink>().GetDrinkMaterials().Length >= 2)
            {
                material2.text = $"x{Drink.GetComponent<CastDrink>().GetDrinkMaterials()[1].Amount}";
                material2Image.sprite = Drink.GetComponent<CastDrink>().GetDrinkMaterials()[1].Material.GetComponent<SpriteRenderer>().sprite;
                material2Image.color = visible;
            }

            if (Drink.GetComponent<CastDrink>().GetDrinkMaterials().Length >= 1)
            {
                material1.text = $"x{Drink.GetComponent<CastDrink>().GetDrinkMaterials()[0].Amount}";
                material1Image.sprite = Drink.GetComponent<CastDrink>().GetDrinkMaterials()[0].Material.GetComponent<SpriteRenderer>().sprite;
                material1Image.color = visible;
            }
            ComproveBasket();
        }

        #endregion
    }

    /// <summary>
    /// Determina si los materiales necesarios para la creación de la bebida están o no en la cesta
    /// </summary>
    public void ComproveBasket()
    {
        matsReqEnCesta = true;

        if (Drink != null && Drink.GetComponent<CastDrink>() != null)
        {
            CastDrink drink = Drink.GetComponent<CastDrink>();

            //Busca en los materiales necesarios para la bebida
            //Siempre que tenga los materiales
            int i = 0;
            while (i < drink.GetDrinkMaterials().Length && matsReqEnCesta)
            {
                int j = 0;
                bool enc = false;
                //Busca el material en la cesta
                while (j < matCestaImages.Length && !enc)
                {
                    if (matCestaImages[j].sprite ==
                        drink.GetDrinkMaterials()[i].Material.GetComponent<SpriteRenderer>().sprite)
                    {
                        enc = true;
                        //Comprueba su cantidad
                        int amount;

                        try
                        {
                            amount = int.Parse(matCestaImages[j].GetComponentInChildren<TextMeshProUGUI>().text);
                        }
                        catch
                        {
                            amount = 0;
                        }


                        if (amount < drink.GetDrinkMaterials()[i].Amount)
                        {
                            //En el momento en el que encuentra uno de los materiales que no cumple los requisitos sale del bucle
                            matsReqEnCesta = false;
                        }
                    }
                    j++;
                }

                //Si directamente no encuentra el material en cesta entonces no están los materiales necesarios
                if (!enc)
                {
                    matsReqEnCesta = false;
                }

                i++;
            }
        }
    }

    public void SumarMaterial(Button material)
    {
        AudioManager.Instance.PlaySFX(materialSfx);
        //Antes de hacer nada busco el botón y le pregunto si tiene mas de 1 de material para proceder con el resto
        if (material.GetComponentInChildren<TextMeshProUGUI>().text != "0")
        {
            bool enc = false;
            int i = 0;

            //Coloca el sprite correspondiente en el primer botón posible si no hay otro botón ocupando el mismo material
            //Si no hay botón se ahorra la busqueda y simplemente añade la imagen
            if (buttonUsing != 0)
            {
                //Si hay alguno usandose hace una busqueda dentro de los usados para ver si el material ha sido ya usado
                //Solo en caso de no haber sido usado, pone su imagen en el siguiente botón e incrementa el valor de los botones usados
                while (i < buttonUsing && !enc)
                {
                    if (matCestaImages[i].sprite == material.GetComponent<Image>().sprite)
                        enc = true;
                    else i++;
                }
            }

            if (!enc)
            {
                matCestaImages[i].sprite = material.GetComponent<Image>().sprite;
                buttonUsing++;
            }

            //Suma 1 a la cantidad de la cesta
            matsEnCesta[i]++;

            //Actualizar texto
            matCestaImages[i].GetComponentInChildren<TextMeshProUGUI>().text = matsEnCesta[i].ToString();


            //Busca el material del botón que ha sido pulsado y resta 1 a su cantidad
            enc = false;
            i = 0;
            while (i < Sources.Length && !enc)
            {
                if (Sources[i].GetComponent<SpriteRenderer>().sprite == material.GetComponent<Image>().sprite)
                    enc = true;
                else i++;
            }
            recursos[(int)Sources[i].GetComponent<CastMaterial>().GetSourceName()]--;

            //Actualiza los textos
            matNums[(int)Sources[i].GetComponent<CastMaterial>().GetSourceName()].text = recursos[(int)Sources[i].GetComponent<CastMaterial>().GetSourceName()].ToString();
        }
        ComproveBasket();
    }


    public void RegresarMats()
    {
        if(NoSkipSFX)
        { AudioManager.Instance.PlaySFX(regresar); }
        
        //Hacen un recorrido por todos los botones que se están usando en la cesta (que tienen al menos un material)
        for (int i = 0; i < buttonUsing; i++)
        {
            // i es la imagen de la cesta sobre la que se posiciona

            //Busca el material de la imagen donde se encuentra en los botones y le pasa su cantidad de materiales
            int j = 0;
            bool enc = false;
            while (j < Sources.Length && !enc)
            {
                //Si encuentra el recurso al que equivale la imagen lo marca como encontrado
                if (matCestaImages[i].sprite == Sources[j].GetComponent<SpriteRenderer>().sprite)
                    enc = true;

                //Mientras tanto sigue buscando
                else j++;
            }
            //Una vez encontrado el recurso nos vamos a su posición en el array de recursos y le sumamos lo que había en la imagen
            recursos[(int)Sources[j].GetComponent<CastMaterial>().GetSourceName()] += matsEnCesta[i];
            //reiniciamos la cantidad de la imagen de la cesta
            matsEnCesta[i] = 0;
            //reiniciamos la imagen de la cesta
            matCestaImages[i].sprite = null;

            //Actualiza los textos
            matNums[(int)Sources[j].GetComponent<CastMaterial>().GetSourceName()].text = recursos[(int)Sources[j].GetComponent<CastMaterial>().GetSourceName()].ToString();
            matCestaImages[i].GetComponentInChildren<TextMeshProUGUI>().text = matsEnCesta[i].ToString();
        }
        buttonUsing = 0;
        ComproveBasket();
    }


    public void Servir()
    {
       NoSkipSFX = false;
            

            if (matsReqEnCesta)//Si están los materiales requeridos en el pedido, quita la cantidad del pedido de la cesta
            {
                for (int i = 0; i < matsEnCesta.Length; i++)
                {
                    if (matsEnCesta[i] > 0)
                    {
                        for (int j = 0; j < Drink.GetComponent<CastDrink>().GetDrinkMaterials().Length; j++)
                        {
                            if (matCestaImages[i].sprite == Drink.GetComponent<CastDrink>().GetDrinkMaterials()[j].Material.GetComponent<SpriteRenderer>().sprite)
                            {
                                matsEnCesta[i] -= Drink.GetComponent<CastDrink>().GetDrinkMaterials()[j].Amount;
                            }
                        }
                    }
                }
                    AudioManager.Instance.PlaySFX(ServirSFX);
                    //Como ha hecho el encargo pedido, el dialogo ira por good
                    way = 0;
                if (Client.gameObject.GetComponent<CastEnemy>() != null &&
                    Client.gameObject.GetComponent<CastEnemy>().GetEnemyType() == EnemyType.Alcalde)
                {
                    GameManager.Instance.increaseDinero(Drink.GetComponent<CastDrink>().GetDrinkReward() * 2);
                }
                else
                {
                    GameManager.Instance.increaseDinero(Drink.GetComponent<CastDrink>().GetDrinkReward());
                }

            }
            else
            {
            AudioManager.Instance.PlaySFX(NoServirSFX);
            //Si no ha hecho el encargo pedido, no se quitan ningun material de la cesta y el dialogo ira por bad
            way = 1;
            }
            //Devuelve resto al inventario
            RegresarMats();

            //Desactiva los botones y limpia el encargo y recompensas y activa los del dialogo
            DrinkImage.gameObject.SetActive(false);
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
    
    public void GameOverUI()
    {
        //Activa la UI de GameOver si el jugador pierde

        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
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

        //Cambia el nombre del emisor
        if (dialogue[DialogueLine].Emisor == Emisor.Jugador)
        {
            EmisorName.text = "Orión";
        }
        else if (dialogue[DialogueLine].Emisor == Emisor.Cliente)
        {
            if (Client.GetComponent<CastEnemy>() != null)
            {
                EmisorName.text = $"{Client.GetComponent<CastEnemy>().GetEnemyType()}";
            }
            else
            {
                EmisorName.text = "";
            }
        }
        else EmisorName.text = "";

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

} // class UIManager 
// namespace
