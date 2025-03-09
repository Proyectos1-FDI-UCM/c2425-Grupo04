//---------------------------------------------------------
// Controla la vida del enemigo
// Letian Liye
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
public class EnemyLife : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private float EnemigoLife = 100;
    [SerializeField] GameObject ContadorDaño;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private TextMeshProUGUI text;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    void Start()
    {
        if (gameObject.GetComponent<UvonciosMovement>())
            EnemigoLife = 150f;
        else if (gameObject.GetComponent<ManzarieteMovement>())
            EnemigoLife = 100f;
        else if (gameObject.GetComponent<ManzurriaMovement>())
            EnemigoLife = 150f;
        else if (gameObject.GetComponent<GrapenadeMovement>())
            EnemigoLife = 50f;

        Debug.Log(gameObject.name+" Enemigo tiene " + EnemigoLife);

        text = ContadorDaño.GetComponentInChildren<TextMeshProUGUI>();
    }

  
    void Update()
    {
        if(EnemigoLife <= 0)
        {
            if (GetComponent<UvonciosMovement>() != null) GetComponent<UvonciosMovement>().InsRec();
            else if (GetComponent<ManzarieteMovement>() != null) GetComponent<ManzarieteMovement>().InsRec();
            else if (GetComponent<GrapenadeMovement>() != null) GetComponent<GrapenadeMovement>().InsRec();
            else if (GetComponent<ManzurriaMovement>() != null) GetComponent<ManzurriaMovement>().InsRec();

            if (GetComponent<PlaceMark>() != null) GetComponent<PlaceMark>().GrapenadeWasDestroy();

            Destroy(gameObject);
        }    
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void getdamage(float damage)
    {
        EnemigoLife -= damage;
        text.text = damage.ToString();
        Instantiate(ContadorDaño, gameObject.transform.position , Quaternion.identity);
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class EnemyLife 
// namespace
