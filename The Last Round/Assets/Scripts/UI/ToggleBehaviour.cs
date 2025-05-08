//---------------------------------------------------------
// Contiene comportamientos de un toggle
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.UI;

/// <summary>
/// En esta clase se encuentran métodos para que los toggles tengan un comportamiento que no se puede conseguir desde el propio inspector
/// </summary>
public class ToggleBehaviour : MonoBehaviour
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
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Se encarga de desactivar un objeto cuando el toggle esté desactivado
    /// </summary>
    /// <param name="Object"></param>
    public void ActiveOn(GameObject Object)
    {
        bool on = GetComponent<Toggle>().isOn;
        Object.SetActive(on);
    }

    /// <summary>
    /// Se encarga de activar un objeto cuando el toggle se desactive
    /// </summary>
    /// <param name="Object"></param>
    public void DesactiveOn(GameObject Object)
    {
        bool on = GetComponent<Toggle>().isOn;
        Object.SetActive(!on);
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class ToggleBehaviour 
// namespace
