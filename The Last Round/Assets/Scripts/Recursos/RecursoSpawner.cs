//---------------------------------------------------------
// Se genera el recurso natural cuando el jugador esta dentro de una distancia determinada con el spawner y se hace el click E
// Letian Liye
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
// Añadir aquí el resto de directivas using

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class RecursoSpawner : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField]
    private float HoldingTime = 0;
    [SerializeField]
    private float limitrecursos = 5;
    [SerializeField] GameObject Contadorrecurso;
    [SerializeField]
    private AudioClip recursoSFX;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private float timer = 0;
    private float recursosacado = 0;
    private SourceName source;
    private bool touchingPlayer = false;
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
        source = GetComponent<CastMaterial>().GetSourceName();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Comprobamos que el jugador se encuentra en la distancia de recolección
        if (touchingPlayer)
        {
            //Contador del hold
            if (InputManager.Instance.InteractIsPressed())//Si mantiene se va restando el timer
            {
                timer -= Time.deltaTime;
            }
            //En el momento en el que se suelta el timer del hold se resetea si no está en pausa
            if (InputManager.Instance.InteractWasReleasedThisFrame() &&
                !GameManager.Instance.IsPauseActive())
            {
                timer = HoldingTime;
            }

            //Si en algún momento se ha mantenido pulsado el Input el suficiente tiempo como para que el timer se haya acabado
            //Entonces se recoge el objeto
            if (timer <= 0)
            {
                timer = HoldingTime;

                AudioManager.Instance.PlaySFX(recursoSFX);
                GameManager.Instance.IncreaseResource(source, 1);
                Instantiate(Contadorrecurso, gameObject.transform.position, Quaternion.identity);
                recursosacado += 1;

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
    public float GetHoldingTime()
    {
        return HoldingTime;
    }
    public float GetTimer()
    {
        return timer;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            touchingPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            touchingPlayer = false;
        }
    }
    #endregion

    // class RecursoSpawner 
    // namespace
}