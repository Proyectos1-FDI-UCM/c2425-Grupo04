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
    private Vector3 MoveDirection;
    private bool dashing;
    private CollisionDetector cD;
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
        cD = GetComponent<CollisionDetector>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        dashing = GetComponent<PlayerDash>().dash();

        MoveDirection = InputManager.Instance.MovementVector;

        if ((cD.GetCollisions(Directions.North) && MoveDirection.y > 0) || cD.GetCollisions(Directions.South) && MoveDirection.y < 0) MoveDirection.y = 0;
        if ((cD.GetCollisions(Directions.East) && MoveDirection.x > 0) || cD.GetCollisions(Directions.West) && MoveDirection.x < 0) MoveDirection.x = 0;

        

        if (MoveDirection != Vector3.zero)
        {
            LastDirection = MoveDirection; 
        }
        
        if (!dashing)
        rb.velocity = MoveDirection * MoveSpeed;
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
