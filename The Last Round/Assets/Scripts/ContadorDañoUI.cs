//---------------------------------------------------------
// Script para configurar el comportamiento de los textos que aparecen cuando se produce daño a una entidad
// Víctor Castro Álvarez
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class ContadorDañoUI : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] float velocidad = 1;
    [SerializeField] float FadeVelocidad = 1;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private TextMeshProUGUI contadorText;
    private Color color;

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
        contadorText = GetComponent<TextMeshProUGUI>();
        color = contadorText.color;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        color.a -= Time.deltaTime * FadeVelocidad;
        contadorText.color = color;

        transform.position += new Vector3(0, Time.deltaTime*velocidad, 0);

       if (color.a <= 0f) Destroy(this.transform.parent.transform.parent.gameObject);//NO VSE PUEDE DESTRUIR EL TRANSFORM. SE HA DE DESTRUIR EL OBJETO. Elimina objeto de texto al hacerse completamente invisible

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


    #endregion   

} // class ContadorDañoUI 
// namespace
