//---------------------------------------------------------
// Sistema de diálogos, para que los clientes hablen con el jugador
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Dialogue : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Conversaciones posibles
    /// </summary>
    [SerializeField, TextArea(5, 6)] //Añadir el camino bueno y malo de las conversaciones que sean necesarias
    private string[] dialogue0_GoodWay, dialogue0_BadWay,
                     dialogue1_GoodWay, dialogue1_BadWay;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private UIManager uiManager;
    /// <summary>
    /// Conversaciones completas donde la segunda dimensión controla el camino de la cnoversación
    /// [0] es el buen camino y [1] es el mal camino
    /// </summary>
    private string[,] dialogue,
                      dialogue0, //IMPORTANTE: SI SE AÑADEN MÁS DIALOGOS HAY QUE AUMENTAR EL RANGO DEL RANDOM DE LA LÍNEA
                      dialogue1;
    private bool ClientAppear = false, DialogueGiven = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {

        dialogue0 = new string[dialogue0_GoodWay.Length, 2];
        dialogue1 = new string[dialogue1_GoodWay.Length, 2];

        for (int i = 0; i < dialogue0.GetLength(1); i++)
        {
            for (int j = 0; j < dialogue0.GetLength(0); j++)
            {
                if (i == 0)
                {
                    //Pone el GoodWay en la primera línea del array
                    dialogue0[j, i] = dialogue0_GoodWay[j];
                }
                else
                {
                    //Pone el BadWay en la segunda línea del array
                    dialogue0[j, i] = dialogue0_BadWay[j];
                }
            }
        }//Recoge dialogo 1

        for (int i = 0; i < dialogue1.GetLength(1); i++)
        {
            for (int j = 0; j < dialogue1.GetLength(0); j++)
            {
                if (i == 0)
                {
                    dialogue1[j, i] = dialogue1_GoodWay[j];
                }
                else
                {
                    dialogue1[j, i] = dialogue1_BadWay[j];
                }
            }
        }//Recoge dialogo 2

        int tmp = Random.Range(0,2);

        if (tmp == 0)
        {
            dialogue = dialogue0;
        }
        else if (tmp == 1)
        {
            dialogue = dialogue1;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (uiManager == null) uiManager = GameManager.Instance.GetUI();

        ClientAppear = true; //este booleano no hace nada ahora pero cuando se haga el cambio de rgb se pondrá en true solo cuando RGBvalue=255;

        if (ClientAppear && !DialogueGiven)
        {
            if (uiManager != null)
            uiManager.GetDialogue(dialogue);
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
