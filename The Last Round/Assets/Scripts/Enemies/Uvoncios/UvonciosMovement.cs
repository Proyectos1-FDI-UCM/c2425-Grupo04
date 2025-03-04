//---------------------------------------------------------
// Contiene el movimiento del enemigo "Uvoncio"
// Letian Liye
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class UvonciosMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] GameObject PielDeUva;
    [SerializeField] GameObject JugoDeUva;
    [SerializeField] GameObject SemillaDeUva;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private MoveToPlayer moveToplayer;
    private Vector3 EnemyPlayer;
    private GameObject recurso;
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
        moveToplayer = GetComponent<MoveToPlayer>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (moveToplayer != null)
        EnemyPlayer = moveToplayer.UpdateVector(gameObject);
        moveToplayer.Move(gameObject);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    public void GetDamage(float Pdamage)
    {
        int recursornd = Random.Range(0, 4);
        if (recursornd == 1)
        {
            recurso = PielDeUva;
            Debug.Log("Uvoncio: Piel de uva");
        }
        else if (recursornd == 2)
        {
            recurso = JugoDeUva;
            Debug.Log("Uvoncio: Jugo de uva");
        }
        else if (recursornd == 3)
        {
            recurso = SemillaDeUva;
            Debug.Log("Uvoncio: Semilla de uva");
        }
        else
        {
            recurso = null;
            Debug.Log("Uvoncio: Sin material");
        }
        if (recurso != null)
        {
            Instantiate(recurso, transform.position, Quaternion.identity);
           
        }
        GetComponent<EnemyLife>().getdamage(Pdamage);
    }


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    

    #endregion   

} // class UvonciosMovement 
// namespace
