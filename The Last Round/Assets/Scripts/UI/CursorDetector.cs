//---------------------------------------------------------
// Script que indicará a un elemento de UI si el cursor ha pasado o no por encima para acto seguido activar una imagen, se utilizará en los botones de mejoras para que al posicionar el ratón encima del botón se active una imagen con una información más detallada

// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

// Añadir aquí el resto de directivas using
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


/// <summary>
/// Clase encargada de detectar el cursor y activar la imagen
/// </summary>
public class CursorDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] private GameObject Description;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private UIManagerUpgrades uimanager;
    private bool detected = false;
    #endregion

    // ---- MÉTODOS DE IPOINTERENTERHANDLER E IPOINTERENTERHANDLER ----
    #region Métodos de IPointerEnterHandler e IPointerExitHandler

    /// <summary>
    /// Cuando el botón esté encima de el botón recibirá una llamada con dicho evento
    /// En este caso el evento no nos importa, solo activamos las imagenes
    /// </summary>
    public void OnPointerEnter(PointerEventData evento)
    {
        uimanager = GameManager.Instance.GetUIU();

        if (uimanager != null && Description != null)
        {
            uimanager.ActiveImage(Description);
        }

        detected = true;
    }

    /// <summary>
    /// Cuando el botón salga del botón recibirá una llamada con dicho evento
    /// En este caso el evento no nos importa, solo desactivamos las imagenes
    /// </summary>
    public void OnPointerExit(PointerEventData evento)
    {
        //No recojo aquí el UIManager porque para que el cursor salga, ha tenido que entrar anteriormente
        //Por lo que teóricamente el uimanager ya ha debido de ser cacheado.

        if (uimanager != null && Description != null)
        {
            uimanager.DesactiveImage(Description);
        }
        detected = false;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    public bool IsMouseOnButton()
    {
        return detected;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class CursorDetector 
// namespace