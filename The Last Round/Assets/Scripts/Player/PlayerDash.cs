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
    private Vector2 LastDirection = Vector2.up;
    private Rigidbody2D rb;
    private Vector2 MoveDirection;
    bool hasDiagonal =false;
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
        // 计算MoveDirection
        MoveDirection.y = 0;
        if (Keyboard.current[Key.W].isPressed)
        {
            MoveDirection.y = 1;
        }
        else if (Keyboard.current[Key.S].isPressed)
        {
            MoveDirection.y = -1;
        }

        MoveDirection.x = 0;
        if (Keyboard.current[Key.D].isPressed)
        {
            MoveDirection.x = 1;
        }
        else if (Keyboard.current[Key.A].isPressed)
        {
            MoveDirection.x = -1;
        }

        bool isDiagonal = MoveDirection.x != 0 && MoveDirection.y != 0;
        if (MoveDirection.x == 0 || MoveDirection.y == 0)
            hasDiagonal = false;
        if (isDiagonal)
        {
            // 处理斜方向
            if (MoveDirection.x == 1 && MoveDirection.y == 1)
            {
                LastDirection = new Vector2(1, 1);
                Debug.Log("右上");
            }
            else if (MoveDirection.x == 1 && MoveDirection.y == -1)
            {
                LastDirection = new Vector2(1, -1);
                Debug.Log("右下");
            }
            else if (MoveDirection.x == -1 && MoveDirection.y == -1)
            {
                LastDirection = new Vector2(-1, -1);
                Debug.Log("左下");
            }
            else if (MoveDirection.x == -1 && MoveDirection.y == 1)
            {
                LastDirection = new Vector2(-1, 1);
                Debug.Log("左上");
            }
            

            hasDiagonal = true;
        }
        else
        {
            if (!hasDiagonal)
            {
                // 处理单一方向
                if (MoveDirection.y == 1)
                {
                    LastDirection = new Vector2(0, 1);
                    Debug.Log("上");
                }
                else if (MoveDirection.y == -1)
                {
                    LastDirection = new Vector2(0, -1);
                    Debug.Log("下");
                }
                else if (MoveDirection.x == 1)
                {
                    LastDirection = new Vector2(1, 0);
                    Debug.Log("右");
                }
                else if (MoveDirection.x == -1)
                {
                    LastDirection = new Vector2(-1, 0);
                    Debug.Log("左");
                }
            }

            // 重置斜方向标记当停止移动时
            if (MoveDirection.x == 0 && MoveDirection.y == 0)
            {
                hasDiagonal = false;
            }
        }

        MoveDirection = MoveDirection.normalized;
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            StartDash();
        }

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
        return cooldownTimer <= 0 && !isDashing && LastDirection != Vector2.zero;
    }
    void StartDash()
    {
        isDashing = true;
        cooldownTimer = DashCooldown;
        rb.velocity = new Vector3(LastDirection.x * DashSpeed, LastDirection.y * DashSpeed);

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
