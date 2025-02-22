//---------------------------------------------------------
// Permite moverse al jugador a través de inputs de teclado
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


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
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        MoveDirection = new Vector3(0, 0, 0);

        if (Keyboard.current[Key.W].isPressed)
        {
            if (Keyboard.current[Key.D].isPressed) MoveDirection = new Vector3 (1, 1, 0);
            else if (Keyboard.current[Key.A].isPressed) MoveDirection = new Vector3(-1, 1, 0);
            else MoveDirection = new Vector3(0, 1, 0);
        }
        else if (Keyboard.current[Key.S].isPressed)
        {
            if (Keyboard.current[Key.D].isPressed) MoveDirection = new Vector3(1, -1, 0);
            else if (Keyboard.current[Key.A].isPressed) MoveDirection = new Vector3(-1, -1, 0);
            else MoveDirection = new Vector3(0, -1, 0);
        }
        else if (Keyboard.current[Key.D].isPressed)
        {
            MoveDirection = new Vector3(1, 0, 0);
        }
        else if (Keyboard.current[Key.A].isPressed)
        {
            MoveDirection = new Vector3(-1, 0, 0);
        }

        if (MoveDirection != Vector3.zero) LastDirection = MoveDirection;

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
