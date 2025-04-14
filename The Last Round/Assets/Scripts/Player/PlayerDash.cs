//---------------------------------------------------------
// Permite el jugador hace Dash al click button derecho
// Letian Liye
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
// Añadir aquí el resto de directivas using

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerDash : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private float ObjectSizeX, ObjectSizeY;
    [Header("DASH")]
    [SerializeField]
    private float DashSpeed = 10f,
                  DashDuration = 0.2f,
                  DashCooldown = 1f;
    
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private bool isDashing = false;
    private float cooldownTimer = 0f, minX, maxX, minY, maxY;
    private Vector3 LastDirection;
    private Rigidbody2D rb;
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

        maxX = GameManager.Instance.GetMapWidth() / 2;
        minX = -maxX;

        maxY = GameManager.Instance.GetMapHeight() / 2;
        minY = -maxY;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        LastDirection = GetComponent<PlayerMovement>().GetLastDirection();
        if (InputManager.Instance.DashWasPressedThisFrame() && CanDash())
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
                Vector3 posMinX, posMinY, posMaxX, posMaxY;

                posMinX = rb.position - new Vector2(ObjectSizeX / 2, 0);
                posMaxX = rb.position + new Vector2(ObjectSizeX / 2, 0);
                posMinY = rb.position - new Vector2(0, ObjectSizeY / 2);
                posMaxY = rb.position + new Vector2(0, ObjectSizeY / 2);

                if ((rb.velocity.y < 0 && posMinY.y <= minY) ||
                    (rb.velocity.y > 0 && posMaxY.y >= maxY))
                    rb.velocity = new Vector2(rb.velocity.x, 0);

                if ((rb.velocity.x < 0 && posMinX.x <= minX) ||
                    (rb.velocity.x > 0 && posMaxX.x >= maxX))
                    rb.velocity = new Vector2(0, rb.velocity.y);

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
        if (!GameManager.Instance.GetBoolUpgrade(1)) //si no tiene la mejora no puede hacer dash
        {
            return false;
        }
        else
        {
            return cooldownTimer <= 0 && !isDashing && LastDirection != Vector3.zero;
        }
    }
    void StartDash()
    {
        isDashing = true;
        cooldownTimer = DashCooldown;
        rb.velocity = new Vector3(LastDirection.x * DashSpeed, LastDirection.y * DashSpeed);
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
