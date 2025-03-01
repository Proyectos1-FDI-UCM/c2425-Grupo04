//---------------------------------------------------------
// Contiene el movimiento del enemigo "Manzariete"
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEditor.Experimental.GraphView;
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
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private MoveToPlayer moveToplayer;
    private Vector3 EnemyPlayer, LastPlayerPosition;
    private float timer = -1;
    private float Stimer = -1, tmp;
    private bool IsCharging = false, IsSprinting = false, InRange = false, hit = false, OnCollision = false;
    private Rigidbody2D rb;
    private CollisionDetecter cD;
    private GameObject recurso;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveToplayer = GetComponent<MoveToPlayer>();
        tmp = SprintSpeed;
        cD = GetComponent<CollisionDetecter>();
    }
    private void FixedUpdate()
    {
        EnemyPlayer = moveToplayer.UpdateVector(gameObject);

        InRange = EnemyPlayer.magnitude <= RangeAttack;

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


        if (InRange && !IsCharging && !IsSprinting)
        {
            IsCharging = true;
            timer = ChargeTime;
        }
        else if (IsCharging)
        {
            if (timer <= 0)
            {
                if (EnemyPlayer.x != 0 && EnemyPlayer.y != 0 && EnemyPlayer.x * EnemyPlayer.x +
                                                                EnemyPlayer.y * EnemyPlayer.y != 0)
                    LastPlayerPosition = EnemyPlayer / Mathf.Sqrt(EnemyPlayer.x * EnemyPlayer.x +
                                              EnemyPlayer.y * EnemyPlayer.y);

                else if (EnemyPlayer.x != 0) LastPlayerPosition = EnemyPlayer / Mathf.Sqrt(EnemyPlayer.x * EnemyPlayer.x);

                else if (EnemyPlayer.y != 0) LastPlayerPosition = EnemyPlayer / Mathf.Sqrt(EnemyPlayer.y * EnemyPlayer.y);

                IsCharging = false;
                IsSprinting = true;
                Stimer = SprintTime;
            }
        }
        else if (IsSprinting)
        {
            if (Stimer > 0)
            {
                hit = false;

                if (OnCollision) ComproveCollision();

                if (hit)
                {
                    LastPlayerPosition *= -1;
                    hit = false;
                }

                rb.velocity = LastPlayerPosition * SprintSpeed * Time.fixedDeltaTime;

                //Frenado final
                if (Stimer < SprintTime / 3.5f && SprintSpeed > tmp / 20)
                {
                    SprintSpeed -= tmp / 20;
                }
            }
            else
            {
                rb.velocity = Vector3.zero;
                IsSprinting = false;
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
    public void Died()
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
        Destroy(gameObject);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollision = true;
    }

    private void ComproveCollision()
    {
        if ((cD.GetCollisions()[0] && LastPlayerPosition.y > 0) ||
            (cD.GetCollisions()[1] && LastPlayerPosition.y < 0) ||
            (cD.GetCollisions()[2] && LastPlayerPosition.x > 0) ||
            (cD.GetCollisions()[3] && LastPlayerPosition.x < 0)) hit = true;
    }
    #endregion

} // class ManzarieteMovement 
// namespace
