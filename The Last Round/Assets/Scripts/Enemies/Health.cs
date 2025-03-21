//---------------------------------------------------------
// Controla la vida del enemigo
// Letian Liye
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Health : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private float Life;
    [SerializeField] GameObject ContadorDaño;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private TextMeshProUGUI text;
    bool PlayerhasDied = false;
    private EnemyType enemy;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start()
    {

       // Debug.Log(gameObject.name + " Enemigo tiene " + EnemigoLife);

        text = ContadorDaño.GetComponentInChildren<TextMeshProUGUI>();

        if (GetComponent<CastEnemy>() != null)
        enemy = GetComponent<CastEnemy>().GetEnemyType();
    }


    void Update()
    {
        if (Life <= 0)
        {
            if (GetComponent<SourceSpawn>() != null)
            {
                GetComponent<SourceSpawn>().Spawn();
            }

            if (GetComponent<PlaceMark>() != null) GetComponent<PlaceMark>().GrapenadeWasDestroy();

            if (GetComponent<CastEnemy>() != null)
            {
                GameManager.Instance.MataEnemigo(enemy);
                Destroy(gameObject);
            }
            else
            {
                PlayerhasDied = true;
            }
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void getdamage(float damage)
    {
        Life -= damage;
        text.text = damage.ToString();
        Instantiate(ContadorDaño, gameObject.transform.position, Quaternion.identity);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    public bool hasPlayerDied()
    {
        return PlayerhasDied;
    }
    #endregion

} // class EnemyLife 
// namespace
