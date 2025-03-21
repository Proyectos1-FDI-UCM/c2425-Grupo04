//---------------------------------------------------------
// Sistema de diálogos, para que los clientes hablen con el jugador
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using System;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Dialogue : MonoBehaviour
{

    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    //-- CREAR DIALOGOS NECESARIOS --//
    [SerializeField]
    float AppearSpeed;
    [SerializeField]
    private Texto[] dialogue0, dialogue1;
    [SerializeField]
    private GameObject[] BebidasPosibles;
    [SerializeField]
    private bool ElAlcalde;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    private UIManager uiManager;
    private Texto[] dialogue;
    private bool ClientAppear = false, DialogueGiven = false;
    private SpriteRenderer spriteRenderer;
    private Color color;
    private GameObject BebidaPedida;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        //Oscurecer personaje
        spriteRenderer = GetComponent<SpriteRenderer>();
        color.r = 0;
        color.g = 0;
        color.b = 0;
        color.a = 255;
        spriteRenderer.color = color;

        //Elige el diálogo que contar
        int tmp = UnityEngine.Random.Range(0, 2);

        if (tmp == 0)
        {
            dialogue = dialogue0;
        }

        else if (tmp == 1)
        {
            dialogue = dialogue1;
        }


        int tmp1;
        int[] contador = GameManager.Instance.GetEnemyCounter();
        float[] recursos = GameManager.Instance.GetRecursos();
        bool tmp2 = true;

        //FILTRO, QUEDAN ESE TIPO DE CIUDADANOS AÚN?
        //SI NO QUEDAN TIENES MATERIALES PARA HACER LA BEBIDA?

        do
        {
            tmp1 = UnityEngine.Random.Range(0, BebidasPosibles.Length);

            //TENGO MATERIALES PARA HACER ESA BEBIDA?
            int j = 0;
            NeededMaterial[] sources = BebidasPosibles[tmp1].GetComponent<CastDrink>().GetDrinkMaterials();
            while (j < sources.Length && tmp2)
            {
                if (sources[j].amount > recursos[(int)sources[j].material.GetComponent<CastMaterial>().GetSourceName()])
                {
                    tmp2 = false;
                }
                j++;
            }
            Debug.Log(BebidasPosibles[tmp1].name);
        }
        while (BebidasPosibles[tmp1].GetComponent<CastDrink>().GetDrinkType() == DrinkType.manzana && contador[1] + contador[3] <= 0  && !tmp2||
               BebidasPosibles[tmp1].GetComponent<CastDrink>().GetDrinkType() == DrinkType.uva && contador[0] + contador[2] <= 0 && !tmp2);


        BebidaPedida = BebidasPosibles[tmp1];

        //Hace un replace de la palabra "(bebida)" en el diálogo por el nombre de la bebida pedida
        int i = 0;
        bool enc = false;

        while (i < dialogue.Length && !enc)
        {
            if (dialogue[i].estatus == Estado.bebida)
            {
                enc = true;

                //Hace el replace en ambos caminos
                //además sustituye los "_" por espacios
                //y el masculino en femenino en caso de ser una sidra

                //Filtro 1 y 2
                dialogue[i].goodText = dialogue[i].goodText.Replace("(bebida)", $"{Convert.ToString(BebidaPedida.name).Replace("_", " ")}");
                dialogue[i].badText = dialogue[i].badText.Replace("(bebida)", $"{Convert.ToString(BebidaPedida.name).Replace("_", " ")}");

                //Filtro 3
                if (BebidaPedida.GetComponent<CastDrink>().GetDrinkName() == DrinkName.Sidra)
                {
                    dialogue[i].goodText = dialogue[i].goodText.Replace("ese", "esa");
                    dialogue[i].goodText = dialogue[i].goodText.Replace("este", "esta");
                    dialogue[i].goodText = dialogue[i].goodText.Replace("aquel", "aquella");
                    dialogue[i].goodText = dialogue[i].goodText.Replace("un", "una");
                    dialogue[i].goodText = dialogue[i].goodText.Replace("el", "la");
                }
            }
            i++;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (uiManager == null) uiManager = GameManager.Instance.GetUI();

        //Aparece
        color.r = Mathf.Clamp(color.r + Time.deltaTime * AppearSpeed, 0, 255);
        color.g = Mathf.Clamp(color.g + Time.deltaTime * AppearSpeed, 0, 255);
        color.b = Mathf.Clamp(color.b + Time.deltaTime * AppearSpeed, 0, 255);
        spriteRenderer.color = color / 255;
        //Debug.Log(color.r);
        if (spriteRenderer.color.r == 1)
            ClientAppear = true;

        if (ClientAppear && !DialogueGiven)
        {
            if (uiManager != null)
            {
                // Si el cliente es el alcalde, duplica la recompensa por dos al valor de la bebida
                if (ElAlcalde)
                {
                    ///////////////////////////////////////////////////////////////////////////////////
                    //                                                                               //
                    //                                                                               //
                    // ESTA PARTE HAY QUE CAMBIARLA                                                  //
                    // NO SE PUEDE EDITAR EL PRECIO DE LA BEBIDA                                     //
                    // EL ALCALDE TIENE QUE TENER UN IDENTIFICADOR QUE PUEDA DETECTAR EL UIMANAGER   //
                    // Y GAME MANAGER, QUIENES SE ENCARGAN DE ESCRIBIR EL PRECIO Y DAR LA RECOMPENSA //
                    //                                                                               //
                    //                                                                               //
                    ///////////////////////////////////////////////////////////////////////////////////
                    
                    int price = BebidaPedida.GetComponent<CastDrink>().GetDrinkReward();
                    price = 2 * BebidaPedida.GetComponent<CastDrink>().GetDrinkReward();
                }
                uiManager.GetClientSprite(spriteRenderer);
                uiManager.GetDrink(BebidaPedida);
                uiManager.GetDialogue(dialogue);
            }

            DialogueGiven = true;
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Dialogue 
// namespace
