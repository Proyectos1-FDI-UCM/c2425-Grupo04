//---------------------------------------------------------
// Este sirve para destruir los recursos cuand oel jugador toca con los recursos
// Letian LIye
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Precursos : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion
   
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Rigidbody2D rb;
    
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
        rb = GetComponent<Rigidbody2D>();
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
    void OnCollisionEnter2D(Collision2D collision)
    {
        string objectName = collision.gameObject.name;
        if (collision.gameObject.CompareTag("Recursos"))
        {
            if (objectName == "Jugo de uva(Clone)")
            {
                GameManager.Instance.IncreaseResource(0);
            }
            else if (objectName == "Piel de uva negra(Clone)" || objectName == "Piel de uva roja(Clone)")
            {
                GameManager.Instance.IncreaseResource(1);
            }
            else if (objectName == "Semilla de uva(Clone)")
            {
                GameManager.Instance.IncreaseResource(2);
            }
            else if (objectName == "Jugo de manzana(Clone)")
            {
                GameManager.Instance.IncreaseResource(3);
            }
            else if (objectName == "Piel de manzana verde(Clone)" || objectName == "Piel de manzana roja(Clone)")
            {
                GameManager.Instance.IncreaseResource(4);
            }
            else if (objectName == "Semilla de manzana(Clone)")
            {
                GameManager.Instance.IncreaseResource(5);
            }
            Destroy(collision.gameObject); 
        }
    }

    #endregion

} // class NewBehaviourScript 
// namespace
