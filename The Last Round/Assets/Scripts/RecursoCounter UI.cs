//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class RecursoCounterUI : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    public float floatSpeed = 1f;      
    public float duration = 1.5f;        
    public float fadeDuration = 1.0f;   
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private float timer = 0f;
    private RectTransform rectTransform;
    private Image image;
    private Color startColor;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        if (image != null)
        {
            startColor = image.color;
        }
    }

    
    void Update()
    {
        timer += Time.deltaTime;

     
        rectTransform.anchoredPosition += new Vector2(0, floatSpeed * Time.deltaTime);

        
        if (image != null)
        {
            float alpha = Mathf.Lerp(startColor.a, 0, timer / fadeDuration);
            image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
        }

        
        if (timer >= duration)
        {
            Destroy(gameObject);
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

} // class RecursoCounterUI 
// namespace
