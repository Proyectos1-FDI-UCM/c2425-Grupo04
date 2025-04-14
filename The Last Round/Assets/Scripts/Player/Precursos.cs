//---------------------------------------------------------
// Este sirve para destruir los recursos cuand oel jugador toca con los recursos
// Letian LIye
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
public class Precursos : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private GameObject[] avisos;
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
            //Identifica el recurso y lo guarda en el inventario
            SourceName source = collision.gameObject.GetComponent<CastMaterial>().GetSourceName();
            GameManager.Instance.IncreaseResource(source);

            Destroy(collision.gameObject);

            //Se busca el aviso correspondiente con el recurso y se instancia
            int i = 0;
            bool enc = false;

            while (i < avisos.Length && !enc)
            {
                SourceName Material = avisos[i].GetComponent<CastMaterial>().GetSourceName();
                if (Material == source)
                {
                    enc = true;
                }
                else i++;
            }

            if (enc)
            {
                Instantiate(avisos[i], transform.position, Quaternion.identity);
            }
        }
    }

    #endregion

} // class NewBehaviourScript 
// namespace
