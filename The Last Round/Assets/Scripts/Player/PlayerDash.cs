//---------------------------------------------------------
// Permite el jugador hace Dash al click button derecho
// Letian Liye
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerDash : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [Header("DASH")]
    public float DashSpeed = 10f;
    public float DashDuration = 0.2f;
    public float DashCooldown = 1f;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private bool isDashing = false;
    private float cooldownTimer = 0f;
    private Vector3 LastDirection;
    private Rigidbody2D rb;
    private Vector3 MoveDirection = new Vector3(0, 0, 0);
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
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        if (Keyboard.current[Key.W].isPressed)
        {
            MoveDirection.y = 1;
            LastDirection.y = MoveDirection.y;
        }
        else if ((Keyboard.current[Key.S].isPressed && MoveDirection.x == -1) )
        {
            Debug.Log("下");
            MoveDirection.y = -1;
            LastDirection.y = MoveDirection.y;
            LastDirection.x = -1;
        }
        else if (Keyboard.current[Key.S].isPressed)
        {
            MoveDirection.y = -1;
            LastDirection.y = MoveDirection.y;
        }
        else if ((Keyboard.current[Key.W].isPressed && MoveDirection.x == -1))
        {
            Debug.Log("上");
            MoveDirection.y = 1;
            LastDirection.y = MoveDirection.y;
            LastDirection.x = -1;
        }

        else MoveDirection.y = 0;
        
        if (Keyboard.current[Key.D].isPressed)
        {
            MoveDirection.x = 1;
            LastDirection.x = MoveDirection.x;
        }
       
        else if (Keyboard.current[Key.A].isPressed)
        {
            MoveDirection.x = -1;
             LastDirection.x = MoveDirection.x;
        }
        

        else MoveDirection.x = 0;

        
        

        MoveDirection = MoveDirection.normalized;



        if (Mouse.current.rightButton.wasPressedThisFrame && CanDash())
        {
            StartDash();
        }
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }
    void FixedUpdate()
    {

        if (isDashing)
        {
            if (DashDuration > 0)
            {
                DashDuration -= Time.fixedDeltaTime;
            }
            else
            {
                EndDash();
            }
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public Vector3 GetLastDirection()
    {
        return LastDirection;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    bool CanDash()
    {
        return cooldownTimer <= 0 && !isDashing && LastDirection != Vector3.zero;
    }
    void StartDash()
    {
        isDashing = true;
        cooldownTimer = DashCooldown;
        rb.velocity = new Vector3(LastDirection.x * DashSpeed, LastDirection.y * DashSpeed, 0);

    }
    void EndDash()
    {
        rb.velocity = Vector3.zero;
        isDashing = false;
        DashDuration = 0.2f;
       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && collision.gameObject.CompareTag("wall"))
        {
            EndDash();
        }
    }
    #endregion   

} // class PlayerDash 
// namespace
