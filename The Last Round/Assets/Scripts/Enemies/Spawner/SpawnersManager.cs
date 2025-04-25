//---------------------------------------------------------
// Contiene la clase SpawnersManager
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Se encarga de gestionar el funcionamiento de los spawners en escena
/// </summary>
public class SpawnersManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private float cooldown;
    [SerializeField] private GameObject[] SpawnObjects;
    [SerializeField] int MaxEnemiesInScene, HowEnemiesToManzariete, HowEnemiesToGrapenade;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private float timer;
    private int[] EnemiesAmount;
    private SpawnerBehaviour[] Spawners;
    private GameObject ChosenObject;
    private SpawnerBehaviour ChosenSpawner;

    //Se usa de listado para que los randoms sepan que han pasado por todos los objetos
    private GameObject[] tmp;
    private SpawnerBehaviour[] tmp1;
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
        HowEnemiesToManzariete = GameManager.Instance.GetMinHabManzariete();
        HowEnemiesToGrapenade = GameManager.Instance.GetMinHabGrapenade();
        MaxEnemiesInScene = GameManager.Instance.GetMaxEnemsScene();

        EnemiesAmount = new int[GameManager.Instance.GetEnemyCounter().Length];
        for (int k = 0; k < EnemiesAmount.Length; k++)
        {
            EnemiesAmount[k] = GameManager.Instance.GetEnemyCounter()[k];
        }

        //Busca los spawners
        Spawners = FindObjectsOfType<SpawnerBehaviour>();
        timer = cooldown;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (timer <= 0)
        {
            int EnemiesInScene = FindObjectsOfType<CastEnemy>().Length;
            int EnemyCount = 0;

            for (int i = 0; i < GameManager.Instance.GetEnemyCounter().Length; i++)
            {
                EnemyCount += GameManager.Instance.GetEnemyCounter()[i];
            }

            if (EnemiesInScene < MaxEnemiesInScene)
            {
                //Atributo encargado de llevar una lista sobre los objetos que el random ya ha elegido para que este no se repita
                tmp = new GameObject[SpawnObjects.Length];
                //posición en el array de objetos elegidos
                int j = 0;

                // -- ELIGE EL OBJETO A INSTANCIAR --
                do
                {
                    //Escoge un objeto
                    ChosenObject = SpawnObjects[UnityEngine.Random.Range(0, SpawnObjects.Length)];

                    //Busca el objeto elegido en los objetos ya elegidos para no repetir la elección
                    bool enc = false;
                    int i = 0;
                    while (i < tmp.Length && !enc)
                    {
                        if (tmp[i] != null && ChosenObject == tmp[i]) enc = true;
                        i++;
                    }
                    if (!enc)
                    {
                        tmp[j] = ChosenObject;
                        j++;
                    }
                }
                // Las condiciones para pedir otro objeto son:
                // * Que no sea un enemigo o que sea un enemigo que no se pueda spawnear
                // * Que no haya probado ya con todos los objetos spawneables

                while ( j<tmp.Length &&
                       (
                        ChosenObject.GetComponent<CastEnemy>() == null ||
                        (
                         ChosenObject.GetComponent<CastEnemy>() != null &&
                          (
                           EnemiesAmount[(int)ChosenObject.GetComponent<CastEnemy>().GetEnemyType()] <= 0 ||
                           (ChosenObject.GetComponent<CastEnemy>().GetEnemyType() == EnemyType.Manzariete && EnemyCount > HowEnemiesToManzariete) ||
                           (ChosenObject.GetComponent<CastEnemy>().GetEnemyType() == EnemyType.Grapenade && EnemyCount > HowEnemiesToGrapenade)
                          )
                        )
                       )
                      );

                //Si ha llegado a pasar por todos los spawners y este no cumple las condiciones no elige ninguno
                if ((ChosenObject.GetComponent<CastEnemy>() == null ||
                (ChosenObject.GetComponent<CastEnemy>() != null &&
                EnemiesAmount[(int)ChosenObject.GetComponent<CastEnemy>().GetEnemyType()] <= 0)))
                {
                    ChosenObject = null;
                }

                // -- ElIGE SPAWNER --
                //Reutiliza el array para los objetos ya elegidos
                tmp1 = new SpawnerBehaviour[Spawners.Length];
                j = 0;

                do
                {
                    //Escoge un spawner
                    ChosenSpawner = Spawners[UnityEngine.Random.Range(0, Spawners.Length)];

                    //Busca el spawner elegido entre los ya elegidos para no repetir la elección
                    bool enc = false;
                    int i = 0;
                    while (i < tmp1.Length && !enc)
                    {
                        if (tmp1[i] != null && ChosenSpawner == tmp1[i])
                        {
                            enc = true;
                        }

                        i++;
                    }
                    if (!enc)
                    {
                        tmp1[j] = ChosenSpawner;
                        j++;
                    }
                }
                // Las condiciones para volver a pedir otro spawner son:
                // * Que el spawner esté dentro de la cámara
                // * Que no haya probado ya con todas los objetos spawneables
                while (ChosenSpawner.IsVisible() &&
                       j < tmp1.Length);

                //Si ha llegado a pasar por todos los spawners y este no cumple las condiciones no elige ninguno
                if (ChosenSpawner.IsVisible())
                {
                    ChosenSpawner = null;
                }

                //Spawnea el objeto
                if (ChosenSpawner != null && ChosenObject != null)
                {
                    ChosenSpawner.GetComponent<SpawnerBehaviour>().Spawn(ChosenObject);
                    // Resta al array de enemigos que quedan por spawnear el enemigo spawneado
                    EnemiesAmount[(int)ChosenObject.GetComponent<CastEnemy>().GetEnemyType()]--;
                }

                //Reinicia el contador
                timer = cooldown;
            }
        }
        timer -= Time.deltaTime;
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

} // class SpawnersManager 
// namespace
