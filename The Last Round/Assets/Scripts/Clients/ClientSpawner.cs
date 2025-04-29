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
    /// Clientes que pueden aparecer
    /// </summary>
    [SerializeField]
    private GameObject[] clients;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
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
    private void Spawn()
    {
        int rnd;
        //Primero se comprueba si entre los clientes está el alcalde
        int i = 0;
        bool ThereIsAlcalde = false;

        //Busca al alcalde entre los clientes
        while (i < clients.Length && !ThereIsAlcalde)
        {
            GameObject client = clients[i];
            if (client != null &&
                client.GetComponent<CastEnemy>() != null &&
                client.GetComponent<CastEnemy>().GetEnemyType() == EnemyType.Alcalde)
            {
                ThereIsAlcalde = true;
            }
            else i++;
        }

        //Si no está se spawnea un cliente habitual
        //Si está se elegirá si spawneará él o un cliente habitual
        if (ThereIsAlcalde)
        {
            //Si el alcalde es el único objeto a spawnear entonces lo spawnea sin rodeos
            if (clients.Length > 1)
            {
                rnd = Random.Range(0, 4);
                if (rnd == 3)
                {
                    rnd = i;
                }
                else
                {
                    rnd = SpawnClient();
                }
            }
            else
            {
                rnd = i;
            }
            //Hay un 50% de probabilidad menos de que el elegido sea un alcalde que un cliente habitual
            //Primero se elige si el elegido es el alcalde o no lo es

        }
        else
        {
            rnd = SpawnClient();
        }

        if (rnd >= 0)
        {
            Instantiate(clients[rnd], transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Método encargado de buscar el cliente habitual a spawnear
    /// </summary>
    /// <returns></returns>
    private int SpawnClient()
    {
        int[] contador = GameManager.Instance.GetEnemyCounter();
        bool repeat = false;
        int rnd = -1;
        int GeneralCounter = 0;

        //Primero se comprueba que haya enemigos que spawnear (sin contar al alcalde)
        for (int i = 0; i < contador.Length; i++)
        {
            if (i != (int)EnemyType.Alcalde)
            {
                GeneralCounter += contador[i];
            }
        }
        if (GeneralCounter > 0)
        {
            do
            {
                rnd = Random.Range(0, clients.Length);

                //Vuelve a pedir si no hay nada que spawnear
                repeat = clients[rnd] == null;

                if (clients[rnd].GetComponent<CastEnemy>() != null)
                {
                    EnemyType client = clients[rnd].GetComponent<CastEnemy>().GetEnemyType();

                    //Vuelve a pedir si es el alcalde
                    repeat = repeat || client == EnemyType.Alcalde;
                    //Vuelve a pedir si no quedan especímenes del habitante elegido vivos
                    repeat = repeat || contador[(int)client] <= 0;
                }
            }
            while (repeat);
        }


        return rnd;
    }
    #endregion

} // class ClientSpawner 
// namespace
