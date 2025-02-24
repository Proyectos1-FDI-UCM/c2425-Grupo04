//---------------------------------------------------------
// Permite el jugador hace Dash al click button derecho
// Letian Liye
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
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
    
    private CollisionDetecter colisionado;
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
        colisionado = GetComponent<CollisionDetecter>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
        LastDirection = GetComponent<PlayerMovement>().GetLastDirection();
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
    public bool dash()
    {
        return isDashing;
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
        if ((colisionado.GetCollisions()[0]&&LastDirection.y > 0 )|| (colisionado.GetCollisions()[1] && LastDirection.y < 0))
            { LastDirection.y = 0; }
        if ((colisionado.GetCollisions()[2] && LastDirection.x > 0) || (colisionado.GetCollisions()[3] && LastDirection.x < 0))
        { LastDirection.x = 0; }

        rb.velocity = new Vector3(LastDirection.x * DashSpeed* Time.fixedDeltaTime, LastDirection.y * DashSpeed* Time.fixedDeltaTime);
      
    }





    void EndDash()
    {
        isDashing = false;
        DashDuration = 0.2f; 
        rb.velocity = Vector3.zero;  
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing )
        {
            EndDash();
        }
    }
    #endregion   

} // class PlayerDash 
// namespace
