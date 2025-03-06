//---------------------------------------------------------
// Se genera el recurso natural cuando el jugador esta dentro de una distancia determinada con el spawner y se hace el click E
// Letian Liye
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.InputSystem;
using UnityEngine.Pool;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class RecursoSpawner : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField]
    private float detectdistancia = 1.2f, HoldingTime = 0;
    [SerializeField] 
    private float limitrecursos = 5;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private float distanciaconjugador;
    private GameObject player;
    private float timer = 0;
    private float recursosacado = 0;

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
        timer = HoldingTime;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (player == null) player = GameManager.Instance.GetPlayer();

        distanciaconjugador = Vector2.Distance(transform.position, player.transform.position);

        

        //Comprobamos que el jugador se encuentra en la distancia de recolección
        if (distanciaconjugador <= detectdistancia)
        {
            //Contador del hold
            if (InputManager.Instance.InteractIsPressed())//Si mantiene se va restando el timer
            {
                timer -= Time.deltaTime;
            }
            if (InputManager.Instance.InteractWasReleasedThisFrame()) //En el momento en el que se suelta el timer del hold se resetea
            {
                timer = HoldingTime;
            }

            //Si en algún momento se ha mantenido pulsado el Input el suficiente tiempo como para que el timer se haya acabado
            //Entonces se recoge el objeto
            if (timer <= 0)
            {
                timer = HoldingTime;
                if (gameObject.name == "Hielo spawner")
                {
                    GameManager.Instance.IncreaseResource(0, "Hielo");
                    recursosacado += 1;
                }
                else if (gameObject.name == "Levadura Spawner")
                {
                    GameManager.Instance.IncreaseResource(1, "Levadura");
                    recursosacado += 1;
                }

                if (recursosacado >= limitrecursos)
                    Destroy(gameObject);
            }
        }
        //En el momento en el que se salga del rango de recolección independientemente de cuanto tiempo haya holdeado el Input
        //El timer del hold se resetea
        else
        {
            timer = HoldingTime;
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

    // class RecursoSpawner 
    // namespace
}