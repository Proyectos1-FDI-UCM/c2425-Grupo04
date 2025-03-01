//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class ManzurriaMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] GameObject PielDeManzana;
    [SerializeField] GameObject JugoDeManzana;
    [SerializeField] GameObject SemillaDeManzana;

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
    public void Died()
    {
        
        int recursornd = Random.Range(0, 4);
        if (recursornd == 1)
        {
            recurso = PielDeManzana;
            Debug.Log("Manzurria: Piel de manzana");
        }
        else if (recursornd == 2)
        {
            recurso = JugoDeManzana;
            Debug.Log("Manzurria: Jugo de manzana");
        }
        else if (recursornd == 3)
        {
            recurso = SemillaDeManzana;
            Debug.Log("Manzurria: Semilla de manzana");
        }
        else
        {
            recurso = null;
            Debug.Log("Manzurria: Sin material");
        }
        
        Instantiate(recurso, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class ManzurriaMovement 
// namespace
