//---------------------------------------------------------
// Permite moverse al jugador a través de inputs de teclado
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


/// <summary>
/// Recoge inputs y mueve al objeto que tenga el script en función de estos
/// Además contiene un método que devuelve el vector de movimiento
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    

    [SerializeField]
    float MoveSpeed;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region   Atributos Privados (private fields)

    Rigidbody2D rb;
    private Vector3 LastDirection;
    private Vector3 MoveDirection = new Vector3 (0, 0, 0);
    private CollisionDetecter cD;
    bool dashing;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    void Start()
    {
        GameManager.Instance.GivePlayer(gameObject);
        rb = GetComponent<Rigidbody2D>();
        cD = gameObject.GetComponent<CollisionDetecter>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        dashing = GetComponent<PlayerDash>().dash();

        MoveDirection = Vector3.zero;

        if (Keyboard.current[Key.W].isPressed && !cD.GetCollisions()[0])
        {
            MoveDirection.y = 1;
        }
        if (Keyboard.current[Key.S].isPressed && !cD.GetCollisions()[1])
        {
            MoveDirection.y = -1;
        }
        if (Keyboard.current[Key.D].isPressed && !cD.GetCollisions()[2])
        {
            MoveDirection.x = 1;
        }
        if (Keyboard.current[Key.A].isPressed && !cD.GetCollisions()[3])
        {
            MoveDirection.x = -1;
        }

        MoveDirection = MoveDirection.normalized;

        if (MoveDirection != Vector3.zero)
        {
            LastDirection = MoveDirection;
            Debug.Log($"LastDirection: {LastDirection}");
        }
        
        if (!dashing)
        rb.velocity = MoveDirection * MoveSpeed * Time.fixedDeltaTime;
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
    #endregion

} // class PlayerMovement 
// namespace
