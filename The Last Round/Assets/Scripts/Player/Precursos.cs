//---------------------------------------------------------
// Este sirve para destruir los recursos cuand oel jugador toca con los recursos
// Letian LIye
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Precursos : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] public GameObject jm;
    [SerializeField] public GameObject pm;
    [SerializeField] public GameObject sm;
    [SerializeField] public GameObject ju;

    [SerializeField] public GameObject pu;
    [SerializeField] public GameObject su;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

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

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

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
    /// <summary>
    /// Detecta las colisiones y si es un recurso pregunta cuál es para añadirlo al inventario y destruirlo
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CastMaterial>() != null &&
            collision.gameObject.GetComponent<RecursoSpawner>() == null)
        {
            SourceName source = collision.gameObject.GetComponent<CastMaterial>().GetSourceName();
            GameManager.Instance.IncreaseResource(source);

            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "Jugo de manzana 1(Clone)")
            Instantiate(jm, gameObject.transform.position, Quaternion.identity);
        if (collision.gameObject.name == "Piel de manzana roja(Clone)" || collision.gameObject.name == "Piel de manzana verde(Clone)")
            Instantiate(pm, gameObject.transform.position, Quaternion.identity);
        if (collision.gameObject.name == "Semilla de manzana 1(Clone)")
            Instantiate(sm, gameObject.transform.position, Quaternion.identity);

        if (collision.gameObject.name == "Jugo de uva 1(Clone)")
            Instantiate(ju, gameObject.transform.position, Quaternion.identity);
        if (collision.gameObject.name == "Piel de uva negra(Clone)" || collision.gameObject.name == "Piel de uva roja(Clone)")
            Instantiate(pu, gameObject.transform.position, Quaternion.identity);
        if (collision.gameObject.name == "Semilla de uva 1(Clone)")
            Instantiate(su, gameObject.transform.position, Quaternion.identity);
    }

    #endregion

} // class NewBehaviourScript 
// namespace
