//---------------------------------------------------------
// Contiene el movimiento del enemigo "Manzariete"
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.PlayerLoop;

/// <summary>
/// Esta class es para el enemigo Manzariete
/// Sirve para el movimiento de este
/// Perseguirá al enemigo hasta un rango, entonces se parará y esperará un tiempo hasta moverse hacia él a gran velocidad
/// </summary>
public class ManzarieteMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    public float RangeAttack;
    public float ChargeTime;
    public float SprintSpeed;
    public float SprintTime;
    [SerializeField] GameObject PielDeManzana;
    [SerializeField] GameObject JugoDeManzana;
    [SerializeField] GameObject SemillaDeManzana;
    [SerializeField] GameObject AttackCube;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private MoveToPlayer moveToplayer;
    private Vector3 EnemyPlayer, LastPlayerPosition;
    private float timer = -1, Stimer = -1, tmp;
    private bool IsCharging = false,
                 IsSprinting = false,
                 InRange = false,
                 IsThereWall = false;
    private Rigidbody2D rb;
    private CollisionDetecter cD;
    private GameObject recurso;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        AttackCube.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        moveToplayer = GetComponent<MoveToPlayer>();
        tmp = SprintSpeed;
        cD = GetComponent<CollisionDetecter>();
    }
    private void FixedUpdate()
    {
        EnemyPlayer = moveToplayer.UpdateVector(gameObject);

        //Vector Jugador -> Enemigo sin retoques de colision
        Vector2 tmp1 = new Vector3(GameManager.Instance.GetPlayer().transform.position.x-transform.position.x,
                                  GameManager.Instance.GetPlayer().transform.position.y - transform.position.y);

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

        if (InRange && !IsCharging && !IsSprinting && !IsThereWall)
        {
            IsCharging = true;
            timer = ChargeTime;
        }
        else if (IsCharging)
        {
            if (timer <= 0)
            {
                if (tmp1.x != 0 && tmp1.y != 0 && tmp1.x * tmp1.x +
                                                                tmp1.y * tmp1.y != 0)
                    LastPlayerPosition = tmp1 / Mathf.Sqrt(tmp1.x * tmp1.x +
                                              tmp1.y * tmp1.y);

                else if (tmp1.x != 0) LastPlayerPosition = tmp1 / Mathf.Sqrt(tmp1.x * tmp1.x);

                else if (tmp1.y != 0) LastPlayerPosition = tmp1 / Mathf.Sqrt(tmp1.y * tmp1.y);

                IsCharging = false;
                AttackCube.SetActive(true);
                IsSprinting = true;
                Stimer = SprintTime;
            }
        }
        else if (IsSprinting)
        {
            if (Stimer > 0)
            {
                //Excluir las colisiones de Enemigos y Jugador (atravesarlos)
                rb.excludeLayers |= LayerMask.GetMask("Player", "Enemy");

                //Reseteamos las colisiones con las capas excluidas anteriormente
                GetComponent<CollisionDetecter>().ResetLayer(6 /*Player*/);
                GetComponent<CollisionDetecter>().ResetLayer(10 /*Enemy*/);

                // -Sensación de rebote- durante movimiento
                //Si colisiona en las zonas derecha o izquierda solo invierte solo el eje x
                if ((cD.GetCollisions()[2] && LastPlayerPosition.x > 0) || (cD.GetCollisions()[3] && LastPlayerPosition.x < 0))
                {
                    LastPlayerPosition.x *= -1;
                }

                //Si colisiona en las zonas encima o debajo solo invierte el eje y
                if ((cD.GetCollisions()[0] && LastPlayerPosition.y > 0) || (cD.GetCollisions()[1] && LastPlayerPosition.y < 0))
                {
                    LastPlayerPosition.y *= -1;
                }

                //movimiento
                rb.velocity = LastPlayerPosition * SprintSpeed;

                //Frenado final
                if (Stimer < SprintTime / 3.5f && SprintSpeed > tmp / 20)
                {
                    SprintSpeed -= tmp / 20;
                }
            }
            else
            {
                rb.excludeLayers &= ~LayerMask.GetMask("Player", "Enemy");
                rb.velocity = Vector3.zero;
                IsSprinting = false;
                AttackCube.SetActive(false);
                SprintSpeed = tmp;
            }
        }
        else
        {
            moveToplayer.Move(gameObject);
        }

        timer -= Time.deltaTime;
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
    public void GetDamage(float Pdamage)
    {
        GetComponent<EnemyLife>().getdamage(Pdamage);
    }
    public void InsRec()
    {
        int recursornd = Random.Range(0, 4);
        if (recursornd == 1)
        {
            recurso = PielDeManzana;
            Debug.Log("Manzariete: Piel de manzana");
        }
        else if (recursornd == 2)
        {
            recurso = JugoDeManzana;
            Debug.Log("Manzariete: Jugo de manzana");
        }
        else if (recursornd == 3)
        {
            recurso = SemillaDeManzana;
            Debug.Log("Manzariete: Semilla de manzana");
        }
        else
        {
            recurso = null;
            Debug.Log("Manzariete: Sin material");
        }
        if (recurso != null)
        {
            Instantiate(recurso, transform.position, Quaternion.identity);
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion

} // class ManzarieteMovement 
// namespace
