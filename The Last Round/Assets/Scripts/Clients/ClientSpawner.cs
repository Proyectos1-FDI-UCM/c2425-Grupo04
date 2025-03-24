//---------------------------------------------------------
// Instancia a los clientes dependiendo de su probabilidad de aparecer
// Aryan Guerrero Iruela
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class ClientSpawner : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// 0 es alcalde, 1 y 3 manzanas, 2 y 4 uvas
    /// </summary>
    [SerializeField]
    private GameObject[] clients;

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
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        Spawn();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    public void Spawn()
    {
        int rnd; //0 es alcalde, 1 y 3 es manzana y 2 y 4 uvas

        int[] contador = GameManager.Instance.GetEnemyCounter();

        do
        {
            rnd = UnityEngine.Random.Range(0, 9); //genera un numero al azar entre el 0 y el 8
            if (rnd != 0) //Si es 0 es el alcalde, y si no es 0 entonces divide el numero entre 2 y lo redondea hacia arriba
            {
                rnd = Mathf.CeilToInt(rnd / 2f);
            }
            Debug.Log(clients[rnd].name);
        }
        while (rnd == 1 && contador[1] <= 0 || rnd == 3 && contador[3] <= 0 ||
               rnd == 2 && contador[2] <= 0 || rnd == 0 && contador[0] <= 0 );
        
        Instantiate(clients[rnd], transform.position, Quaternion.identity);
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class ClientSpawner 
// namespace
