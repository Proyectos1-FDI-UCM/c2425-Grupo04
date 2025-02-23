//---------------------------------------------------------
// Permite moverse al jugador a través de inputs de teclado
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.InputSystem;


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
    #region Atributos Privados (private fields)
    private Vector3 MoveDirection = new Vector3 (0, 0, 0);
    private Vector3 LastDirection;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    void Start()
    {
        GameManager.Instance.GivePlayer(gameObject);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Keyboard.current[Key.W].isPressed)
        {
            MoveDirection.y = 1;
        }
        else if (Keyboard.current[Key.S].isPressed)
        {
            MoveDirection.y = -1;
        }
        else MoveDirection.y = 0;

        if (Keyboard.current[Key.D].isPressed)
        {
            MoveDirection.x = 1;
        }
        else if (Keyboard.current[Key.A].isPressed)
        {
            MoveDirection.x = -1;
        }
        else MoveDirection.x = 0;

        if (MoveDirection != Vector3.zero)
        {
            LastDirection = MoveDirection;
        }

        MoveDirection = MoveDirection.normalized;

        transform.position += MoveDirection * MoveSpeed * Time.deltaTime;
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
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class PlayerMovement 
// namespace
