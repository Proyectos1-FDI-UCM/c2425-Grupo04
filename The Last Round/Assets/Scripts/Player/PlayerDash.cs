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

    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        colisionado = GetComponent<CollisionDetecter>();
    }

   
    void Update()
    {
        
        LastDirection = GetComponent<PlayerMovement>().GetLastDirection();
        // al tocar boton derecho se empieza dash.
        if (Mouse.current.rightButton.wasPressedThisFrame && CanDash())
        {
           
          StartDash();
        }
        // al empezarse el dash, tambien activa timer de cooldown
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        //si esta dashing, duracion de dash empieza a disminuir hasta 0, cuando es 0, enddash
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
        //para poder decir a PlayerMovement que esta haciendo dash, para que desactiva movimiento normal cuando esta en dash
        return isDashing;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    bool CanDash()
    {
        //candash depende si no hay cooldowntime, no esta en dashing y el vector no es igual al cero, en nuestro caso, vector nunca va ser zero, excepto se empieza el juego y no ha tocado nada
        return cooldownTimer <= 0 && !isDashing && LastDirection != Vector3.zero;
    }
    void StartDash()
    {
        //se empieza dash y se renova cooldowntime
        isDashing = true;
        cooldownTimer = DashCooldown;
        // al colisionarse con algo que no es enemigo,se deja de dash en ese direccion
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
