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
    //-- CREAR DIALOGOS NECESARIOS --//
    [SerializeField]
    float AppearSpeed;
    [SerializeField]
    private DataContainer.Texto[] dialogue0, dialogue1;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    private UIManager uiManager;
    private DataContainer.Texto[] dialogue;
    private bool ClientAppear = false, DialogueGiven = false;
    private SpriteRenderer spriteRenderer;
    private Color color;
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

        //Aparece
        color.r = Mathf.Clamp(color.r + Time.deltaTime * AppearSpeed, 0, 255);
        color.g = Mathf.Clamp(color.g + Time.deltaTime * AppearSpeed, 0, 255);
        color.b = Mathf.Clamp(color.b + Time.deltaTime * AppearSpeed, 0, 255);
        spriteRenderer.color = color/255;
        Debug.Log(color.r);
        if (spriteRenderer.color.r == 1)
        ClientAppear = true;

        if (ClientAppear && !DialogueGiven)
        {
            if (uiManager != null)
            {
                uiManager.GetDialogue(dialogue);
                uiManager.GetClientSprite(spriteRenderer, AppearSpeed);
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
