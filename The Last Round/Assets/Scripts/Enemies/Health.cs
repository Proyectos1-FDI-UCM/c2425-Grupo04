//---------------------------------------------------------
// Se encarga de gestionar la vida de los objetos
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
    private EnemyType enemy;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start()
    {

       // Debug.Log(gameObject.name + " Enemigo tiene " + EnemigoLife);

        text = ContadorDaño.GetComponentInChildren<TextMeshProUGUI>();

        if (GetComponent<CastEnemy>() != null) enemy = GetComponent<CastEnemy>().GetEnemyType();
        else if (GetComponent<CastEnemy>() == null)
        {
            Life = Life + 0.1f * Life * GameManager.Instance.GetUpgradeLevel(2); //Sube la vida un 10% por cada nivel de la mejora
        }
    }
    

    void Update()
    {
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Recibe el daño a recibir y se lo resta a la vida del objeto que tenga este script, 
    /// </summary>
    public void GetDamage(float damage)
    {
        Life -= damage;
        text.text = damage.ToString();
        Instantiate(ContadorDaño, gameObject.transform.position, Quaternion.identity);

        if (Life <= 0)
        {
            Kill(); //Si su vida es 0 o menor el objeto muere
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Se encarga de manejar la muerte del objeto
    /// </summary>
    private void Kill()
    {
        //Si puede spawnear algo al morir se spawnea
        if (GetComponent<SourceSpawn>() != null)
        {
            GetComponent<SourceSpawn>().Spawn();
        }

        //Si es un Grapenade con el componente PlaceMark se le avisa de que el Grapenade ha sido eliminado
        if (GetComponent<PlaceMark>() != null) GetComponent<PlaceMark>().GrapenadeWasDestroy();

        //Si es un enemigo se resta del contador de enemigos y se destruye
        if (GetComponent<CastEnemy>() != null)
        {
            GameManager.Instance.MataEnemigo(enemy);
            Destroy(gameObject);
        }
        //Si es el jugador se activa el proceso de menú de muerte
        else
        {
            GameManager.Instance.GetUIC().GetComponent<UIManager_Combate>().GameOverUI();
        }
    }
    #endregion

} // class EnemyLife 
// namespace
