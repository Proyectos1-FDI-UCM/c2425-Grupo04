//---------------------------------------------------------
// Script para configurar el comportamiento de los textos que aparecen cuando se produce daño a una entidad
// Víctor Castro Álvarez
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;
// Añadir aquí el resto de directivas using
using UnityEngine.UI;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class DamageCounterUI : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] float velocidad = 1;
    [SerializeField] float FadeVelocidad = 1;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Color color, contadorTextColor;

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
        if (GetComponent<TextMeshProUGUI>() != null)
        {
            contadorTextColor = GetComponent<TextMeshProUGUI>().color;
        }
        else if (GetComponent<Image>() != null)
        {
            contadorTextColor = GetComponent<Image>().color;
        }
        color = contadorTextColor;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        color.a -= Time.deltaTime * FadeVelocidad;

        if (GetComponent<TextMeshProUGUI>() != null)
        {
            GetComponent<TextMeshProUGUI>().color = color;
        }
        else if (GetComponent<Image>() != null)
        {
            GetComponent<Image>().color = color;
        }

        transform.position += new Vector3(0, Time.deltaTime*velocidad, 0);
        //Elimina objeto de texto al hacerse completamente invisible
        if (color.a <= 0f)
        {
            Destroy(this.transform.parent.transform.parent.gameObject);
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


    #endregion   

} // class ContadorDañoUI 
// namespace
