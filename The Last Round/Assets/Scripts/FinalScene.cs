//---------------------------------------------------------
// Script encargado de manejar todo lo que sucede en la escena final del juego
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
// Añadir aquí el resto de directivas using


/// <summary>
/// La clase FinalScene se encarga de manejar todo lo que sucede en la escena final del juego
/// Esto sucede desde que el alcalde aparece en escena al quedar 1 solo enemigo, él.
/// </summary>
public class FinalScene : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.

    [SerializeField] private float StartSceneDistance;
    [SerializeField] private float PlayerSpeed;
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.

    private Vector3 AlcaldePlayer, Destiny;
    private Vector2 Direction;
    private GameObject Player;
    private bool DisablePlayer = false, StartDialogue = false;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
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
        Player = GameManager.Instance.GetPlayer();

        rb = Player.GetComponent<Rigidbody2D>();

        Destiny = new Vector3(transform.position.x, transform.position.y - Player.transform.localScale.y/2 - transform.localScale.y, transform.position.z);

        animator = Player.GetComponent<Animator>();

        spriteRenderer = Player.GetComponent<SpriteRenderer>(); 
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Guarda la distancia del alcalde al jugador
        AlcaldePlayer = Player.transform.position;

        //Cuando esté a la distancia determinada el jugador pierde el control del personaje
        if (AlcaldePlayer.magnitude <= StartSceneDistance && !DisablePlayer)
        {
            PlayerMovement movimiento = Player.GetComponent<PlayerMovement>();
            PlayerDash dash = Player.GetComponent<PlayerDash>();
            AttackGeneral ataque = Player.GetComponentInChildren<AttackGeneral>();

            if(movimiento != null)
            {
                movimiento.enabled = false;
            }
            if(dash != null)
            {
                dash.enabled = false;
            }
            if (ataque != null)
            {
                ataque.enabled = false;
            }

            DisablePlayer = true;
        }

        //Inicia conversación
        //Esto va antes que la recolocación para asegurarme de que entra aquí en la siguiente vuelta de update y en el update que termina el jugador se ha recolocado bien antes de poner su velocidad a 0.
        if (StartDialogue)
        {
            //Para al jugador
            rb.velocity = Vector2.zero;

            //Spawnea conversación
        }

        //El personaje se recoloca debajo del alcalde

        if (DisablePlayer && !StartDialogue)
        {
            if (rb != null)
            {
                Direction = (Destiny - Player.transform.position);

                if (Direction.magnitude < 0.1f)
                {
                    rb.velocity = new Vector2(0, 0.01f);
                    StartDialogue = true;
                }
                else if (Mathf.Abs(Direction.y) < 0.1f)
                {
                    Direction = new Vector2(Direction.x, 0);
                    rb.velocity = Direction.normalized * PlayerSpeed;
                }
                else if (Mathf.Abs(Direction.x) < 0.1f)
                {
                    Direction = new Vector2(0, Direction.y);
                    rb.velocity = Direction.normalized * PlayerSpeed;
                }
                else
                {
                    rb.velocity = Direction.normalized * PlayerSpeed;
                }  
            }
        }
    }

    private void FixedUpdate()
    {
        if (animator != null && DisablePlayer && spriteRenderer != null)
        {
            animator.SetFloat("Horizontal", Mathf.Abs(Direction.x));
            animator.SetFloat("Vertical", Direction.y);
            animator.SetFloat("Speed", Direction.sqrMagnitude);
            if (Direction.x < 0)
            {
                //Si se mueve a la izquierda, flip al Sprite Renderer
                spriteRenderer.flipX = true;
            }
            else if (Direction.x > 0)
            {
                //Si se mueve a la derecha, no hay flip al Sprite Renderer
                spriteRenderer.flipX = false;
            }
            //Hago dos ifs para que no haya un estado predeterminado y evitar problemas con el flip.
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

} // class FinalScene 
// namespace
