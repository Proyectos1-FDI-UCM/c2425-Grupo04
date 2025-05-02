//---------------------------------------------------------
// Contiene el movimiento del enemigo "Manzariete"
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;
// Añadir aquí el resto de directivas using

/// <summary>
/// Esta class es para el enemigo Manzariete
/// Sirve para el movimiento de este
/// Perseguirá al enemigo hasta un rango, entonces se parará y esperará un tiempo hasta moverse hacia él a gran velocidad
/// </summary>
public class ManzarieteMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    //Rango de sprint, tiempo de carga del sprint, velocidad del sprint, tiempo que se hace el sprint
    //, velocidad de frenado y tiempo de frenado
    [SerializeField] private float RangeAttack, ChargeTime,
                                   SprintSpeed, SprintTime,
                                   BreakSpeed, BreakTime,
                                   ObjectSizeX, ObjectSizeY;
    [SerializeField]
    private AudioClip dashManzariete;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    //Recogida de componente para mover al objeto
    private MoveToPlayer moveToplayer;

    //Localizar la posición del jugador para hacer el sprint
    private Vector3 EnemyPlayer, LastPlayerPosition;

    //Timer de carga y sprint y variable para guardar la velocidad base (para el frenado)
    private float Ctimer = -1, Stimer = -1, MaxSpeed;

    //Booleanos de estado y condiciones de sprint (estar en rango y no haber una pared de por medio)
    private bool IsCharging = false,
                 IsSprinting = false,
                 InRange = false,
                 IsThereWall = false;

    //El cuerpo se utiliza para mover al objeto
    private Rigidbody2D rb;

    //Detector que gestiona las colisiones

    //Se ajusta el collider para saber si hay un objeto entre el jugador y el enemigo
    private Collider2D ObjectCollider;

    //Límites de movimiento
    private float minX, maxX, minY, maxY;

    private bool playSfx = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveToplayer = GetComponent<MoveToPlayer>();
        MaxSpeed = SprintSpeed;
        ObjectCollider = GetComponent<Collider2D>();

        maxX = GameManager.Instance.GetMapWidth() / 2;
        minX = -maxX;
        maxY = GameManager.Instance.GetMapHeight() / 2;
        minY = -maxY;
    }
    private void FixedUpdate()
    {
        EnemyPlayer = moveToplayer.UpdateVector(gameObject);

        IsThereWall = GetComponentInChildren<FollowPlayer>().GetWall(); //Se comprueba si hay o no una paredentre el jugador y el enemigo

        InRange = EnemyPlayer.magnitude <= RangeAttack;
        if (IsThereWall && IsCharging)//Si está cargando y recibe una pared cancela la carga
        {
            IsCharging = false;
        }
        //Si está cargando hace freeze x e y para que no se mueva con la gravedad 0
        if (IsCharging)
        {
            rb.constraints |= RigidbodyConstraints2D.FreezePosition;
            rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints &= ~RigidbodyConstraints2D.FreezePosition;
            rb.constraints &= RigidbodyConstraints2D.FreezeRotation;
        }

        //Ajustar el CapsuleCollider para que siga al jugador, mas adelante en el OnTrigger se comprobará qué hay en ese camino hasta el jugador

        //Si está en rango, no está cargando ni sprintando y no hay una pared de por medio, empiezaa cargar
        if (InRange && !IsCharging && !IsSprinting && !IsThereWall)
        {
            IsCharging = true;
            Ctimer = ChargeTime;
            playSfx = true;
        }

        //Si está cargando realiza el movimiento de carga
        if (IsCharging)
        {
            Charge();
        }
        //En cambio si está sprintando realiza el movimiento de sprint
        else if (IsSprinting)
        {
            Sprint();
        }
        //En caso contrario simplemente anda hacia el jugador
        else
        {
            moveToplayer.Move(gameObject);
        }

        Ctimer -= Time.deltaTime;
        Stimer -= Time.deltaTime;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    //Pasar si está o no sprintando a FollowPlayer
    public bool Sprinting()
    {
        return IsSprinting;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Método encargado de realizar el sprint
    /// </summary>
    private void Sprint()
    {
        if (playSfx)
        {
            AudioManager.Instance.PlaySFX(dashManzariete);
            playSfx = false;
        }
        if (Stimer > 0)
        {
            // -Sensación de rebote- durante movimiento
            //Si colisiona en las zonas derecha o izquierda solo invierte solo el eje x
            Vector3 posMinX, posMinY, posMaxX, posMaxY;

            posMinX = rb.position - new Vector2(ObjectSizeX / 2, 0);
            posMaxX = rb.position + new Vector2(ObjectSizeX / 2, 0);
            posMinY = rb.position - new Vector2(0, ObjectSizeY / 2);
            posMaxY = rb.position + new Vector2(0, ObjectSizeY / 2);

            if ((LastPlayerPosition.x < 0 && posMinX.x <= minX) ||
                (LastPlayerPosition.x > 0 && posMaxX.x >= maxX))
            {
                LastPlayerPosition.x *= -1;
            }

            //Si colisiona en las zonas encima o debajo solo invierte el eje y
            if ((LastPlayerPosition.y < 0 && posMinY.y <= minY) ||
                (LastPlayerPosition.y > 0 && posMaxY.y >= maxY))
            {
                LastPlayerPosition.y *= -1;
            }

            //movimiento
            rb.velocity = LastPlayerPosition * SprintSpeed;

            //Frenado final
            if (Stimer <= BreakTime && SprintSpeed > MaxSpeed / BreakSpeed)
            {
                SprintSpeed -= MaxSpeed / BreakSpeed;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            IsSprinting = false;
            SprintSpeed = MaxSpeed;
        }
    }//Sprint

    /// <summary>
    /// Método encargado de cargar el sprint
    /// </summary>
    private void Charge()
    {
        //Vector (Jugador -> Enemigo) sin retoques de colision
        Vector2 tmp1 = new Vector3(GameManager.Instance.GetPlayer().transform.position.x - transform.position.x,
                                  GameManager.Instance.GetPlayer().transform.position.y - transform.position.y);

        if (Ctimer <= 0)
        {
            if (tmp1.x != 0 && tmp1.y != 0 && tmp1.x * tmp1.x +
                                                            tmp1.y * tmp1.y != 0)
                LastPlayerPosition = tmp1 / Mathf.Sqrt(tmp1.x * tmp1.x +
                                          tmp1.y * tmp1.y);

            else if (tmp1.x != 0) LastPlayerPosition = tmp1 / Mathf.Sqrt(tmp1.x * tmp1.x);

            else if (tmp1.y != 0) LastPlayerPosition = tmp1 / Mathf.Sqrt(tmp1.y * tmp1.y);

            IsCharging = false;
            IsSprinting = true;
            Stimer = SprintTime;
            playSfx = true;
        }
    }//Charge
    #endregion

    /// <summary>
    /// Encargado de detectar colisiones con otros objetos
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si detecta la colisión con algún objeto mientras hace el sprint da el efecto de rebote
        //En realidad esto se aplica a todos los objetos a excepción de la capa "Player" (jugadores)
        //Esto es porque el collider "no trigger" que se encarga de la reacción de frenado, excluye a esta capa
        //Los objetos en la capa "Player" activan otro collider trigger por lo que con ellos esta función no se llama.
        if (IsSprinting)
        {
            Vector2 tmp = collision.contacts[0].normal;

            if (tmp.y > 0.5f && LastPlayerPosition.y < 0|| tmp.y < -0.5 && LastPlayerPosition.y > 0)
            {
                LastPlayerPosition.y *= -1;
            }
            
            if (tmp.x > 0.5f && LastPlayerPosition.x < 0 || tmp.x < -0.5 && LastPlayerPosition.x > 0)
            {
                LastPlayerPosition.x *= -1;
            }

        }
    }
} // class ManzarieteMovement 
// namespace
