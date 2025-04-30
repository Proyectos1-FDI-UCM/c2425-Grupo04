//---------------------------------------------------------
// Busca botones en escena y si detecta movimiento de mando selecciona uno
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class ButtonSelector : MonoBehaviour
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
    GameObject LastSelected;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    void Update()
    {
        Vector2 tmp = InputManager.Instance.MovementVector;

        if (tmp != Vector2.zero || (LastSelected != null && EventSystem.current.currentSelectedGameObject != null))
        {
            SelectButton();
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
    public void SelectButton()
    {
        Button[] botones = FindObjectsOfType<Button>();
        Slider[] sliders = FindObjectsOfType<Slider>();
        Toggle[] toggles = FindObjectsOfType<Toggle>();
        TMP_Dropdown[] dropdowns = FindObjectsOfType<TMP_Dropdown>();

        if (botones.Length > 0 && EventSystem.current != null)
        {
            //Comprueba si hay ningún botón seleccionado
            int i = 0;
            bool enc = false;
            while (i < botones.Length && !enc)
            {
                if (EventSystem.current.currentSelectedGameObject == botones[i].gameObject)
                {
                    enc = true;
                }
                i++;
            }
            //Comprueba que no hay ningún slider seleccionado
            i = 0;
            while (i < sliders.Length && !enc)
            {
                if (EventSystem.current.currentSelectedGameObject == sliders[i].gameObject)
                {
                    enc = true;
                }
                i++;
            }
            //Comprueba que no hay ningún toggle seleccionado
            i = 0;
            while (i < toggles.Length && !enc)
            {
                if (EventSystem.current.currentSelectedGameObject == toggles[i].gameObject)
                {
                    enc = true;
                }
                i++;
            }
            //Comprueba que no hay ningún dropdown seleccionado
            i = 0;
            while (i < dropdowns.Length && !enc)
            {
                if (EventSystem.current.currentSelectedGameObject == dropdowns[i].gameObject)
                {
                    enc = true;
                }
                i++;
            }
            //Si no hay ninguno seleccionado selecciona el primero del array
            if (!enc)
            {
                EventSystem.current.SetSelectedGameObject(botones[0].gameObject);
                LastSelected = EventSystem.current.currentSelectedGameObject;
            }
        }
    }
    #endregion   

} // class ButtonSelector 
// namespace
