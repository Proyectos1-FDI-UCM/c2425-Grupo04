//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Cambia el valor de la scrollbar vertical en función de la posición del elemento UI seleccionado dentro del ScrollView
/// El ScrollView se recoge como atributo privado serializado
/// </summary>
public class AutoScroll : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField]
    private ScrollRect scrollRect;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private RectTransform content;

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
        content = scrollRect.content;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Se recoge el botón seleccionado
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        //Se comprueba que haya seleccionado y si lo hay si forma parte del contenido del scrollbar
        if (selected != null /*&& selected.transform.IsChildOf(content.transform)*/)
        {
            //Se recogen la posición del botón seleccionado y del viewport que maneja el contentido
            Vector2 viewportLocalPosition = scrollRect.viewport.localPosition;
            //El único elemento UI que no es hijo en el panel de el objeto perteneciente al panel es el toggle de invulnerabilidad
            //Por tanto al toggle se le recoge la posición local y al resto la posición local del padre
            Vector2 selectedPosition;
            if (selected.GetComponent<Toggle>() != null)
            {
                selectedPosition = selected.GetComponent<RectTransform>().localPosition;
            }
            else
            {
                //Debug.Log(selected.transform.parent.GetComponentInParent<RectTransform>().gameObject.name);
                selectedPosition = selected.transform.parent.GetComponentInParent<RectTransform>().localPosition;
            }
            //Debug.Log(viewportLocalPosition + "       " + selectedPosition);

            //Se calcula la nueva posición del scrollview
            float normalizedPosition = Mathf.Clamp01((-selectedPosition.y - (scrollRect.viewport.rect.height / 2)) / (content.rect.height - scrollRect.viewport.rect.height));

            //Se asigna al scrollrect del scrollview la nueva posición
            scrollRect.verticalScrollbar.value = 1 - normalizedPosition;
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

} // class AutoScroll 
// namespace
