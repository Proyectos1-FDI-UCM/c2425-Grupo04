//---------------------------------------------------------
// Permite moverse al jugador a través de inputs de teclado
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
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
    float MoveSpeed, ObjectSizeX, ObjectSizeY;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region   Atributos Privados (private fields)

    Rigidbody2D rb;
    private Vector3 LastDirection;
    private Vector3 MoveDirection;
    private bool dashing;
    private float minX, maxX, minY, maxY;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
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

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        MoveDirection = InputManager.Instance.MovementVector;

        Vector3 posMinX, posMinY, posMaxX, posMaxY;

        posMinX = rb.position - new Vector2(ObjectSizeX / 2, 0);
        posMaxX = rb.position + new Vector2(ObjectSizeX / 2, 0);
        posMinY = rb.position - new Vector2(0, ObjectSizeY / 2);
        posMaxY = rb.position + new Vector2(0, ObjectSizeY / 2);


        if ((MoveDirection.y < 0 && posMinY.y <= minY) ||
            (MoveDirection.y > 0 && posMaxY.y >= maxY))
            MoveDirection.y = 0;

        if ((MoveDirection.x < 0 && posMinX.x <= minX) ||
            (MoveDirection.x > 0 && posMaxX.x >= maxX)) MoveDirection.x = 0;



        if (MoveDirection != Vector3.zero)
        {
            LastDirection = MoveDirection;
        }
    }

    private void FixedUpdate()
    {
        dashing = GetComponent<PlayerDash>().dash();

        if (!dashing)
        {
            rb.velocity = MoveDirection * MoveSpeed;
        }

        animator.SetFloat("Horizontal", Mathf.Abs(MoveDirection.x));
        animator.SetFloat("Vertical", MoveDirection.y);
        animator.SetFloat("Speed", MoveDirection.sqrMagnitude);
        if(MoveDirection.x < 0)
        {
            //Si se mueve a la izquierda, flip al Sprite Renderer
            spriteRenderer.flipX = true;
        }
        else if(MoveDirection.x > 0)
        {
            //Si se mueve a la derecha, no hay flip al Sprite Renderer
            spriteRenderer.flipX = false;
        }
        //Hago dos ifs para que no haya un estado predeterminado y evitar problemas con el flip.
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<CambioEscenaAlcantarilla>() != null && !GameManager.Instance.IsFinalPhase())
        {
            GameManager.Instance.GetUIC().PressE();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<RecursoSpawner>() != null)
        {
            float timer, holdingTime;
            timer = collision.GetComponent<RecursoSpawner>().GetTimer();
            holdingTime = collision.GetComponent<RecursoSpawner>().GetHoldingTime();

            GameManager.Instance.GetUIC().HoldE(timer, holdingTime);
        }
        if (collision.GetComponent<CambioEscenaAlcantarilla>() != null && GameManager.Instance.IsFinalPhase())
        {
            GameManager.Instance.GetUIC().ClearMessage();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<CambioEscenaAlcantarilla>() != null || collision.GetComponent<RecursoSpawner>() != null)
        {
            GameManager.Instance.GetUIC().ClearMessage();
        }
    }
    #endregion

} // class PlayerMovement 
// namespace
