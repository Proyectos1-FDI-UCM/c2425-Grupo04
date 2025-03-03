//---------------------------------------------------------
// Sistema de diálogos, para que los clientes hablen con el jugador
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Dialogue : MonoBehaviour
{

    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)



    //-- CREAR DIALOGOS NECESARIOS --//
    [SerializeField]
    private DataContainer.Texto[] dialogue0, dialogue1;


    private UIManager uiManager;
    private DataContainer.Texto[] dialogue;
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

        int tmp = Random.Range(0, 2);

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
