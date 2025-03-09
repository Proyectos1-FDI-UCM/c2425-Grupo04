//---------------------------------------------------------
// Contiene el movimiento del enemigo "Grapenade"
// Aryan Guerrero Iruela
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Esta clase se encarga del movimiento del enemigo Grapenade
/// Perseguirá al jugador hasta una cierta distancia, donde se quedará quieto. 
/// Y si esta distancia es reducida, se moverá en sentido contrario hasta que vuelva a estar en esa distancia
/// </summary>
public class GrapenadeMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField]
    private float range;
    [SerializeField]
    private float marchaAtrasSpeed;
    [SerializeField] GameObject PielDeUva;
    [SerializeField] GameObject JugoDeUva;
    [SerializeField] GameObject SemillaDeUva;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Rigidbody2D rb;
    private CollisionDetecter cD;
    private MoveToPlayer moveToPlayer;
    private Vector3 EnemyPlayer;
    private GameObject recurso;
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
        rb = GetComponent<Rigidbody2D>();
        moveToPlayer = GetComponent<MoveToPlayer>();
        cD = GetComponent<CollisionDetecter>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        EnemyPlayer = GameManager.Instance.GetPlayer().transform.position - transform.position;

        if (Mathf.Floor(EnemyPlayer.magnitude * 10) / 10 == range)
        {
            rb.velocity = Vector3.zero;
        }
        else if (Mathf.Floor(EnemyPlayer.magnitude * 10) / 10 < range)
        {
            if ((cD.GetCollisions()[0] && -EnemyPlayer.y > 0) || (cD.GetCollisions()[1] && -EnemyPlayer.y < 0)) EnemyPlayer.y = 0;
            if ((cD.GetCollisions()[2] && -EnemyPlayer.x > 0) || (cD.GetCollisions()[3] && -EnemyPlayer.x < 0)) EnemyPlayer.x = 0;
            rb.velocity = -EnemyPlayer.normalized * marchaAtrasSpeed * Time.deltaTime;
        }
        else
        {
            moveToPlayer.Move(gameObject);
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    public void GetDamage(float Pdamage)
    {
        GetComponent<EnemyLife>().getdamage(Pdamage);
    }
    public void InsRec()
    {
        int recursornd = Random.Range(0, 4);
        if (recursornd == 1)
        {
            recurso = PielDeUva;
            Debug.Log("Grapenade: Piel de uva");
        }
        else if (recursornd == 2)
        {
            recurso = JugoDeUva;
            Debug.Log("Grapenade: Jugo de uva");
        }
        else if (recursornd == 3)
        {
            recurso = SemillaDeUva;
            Debug.Log("Grapenade: Semilla de uva");

        }
        else
        {
            recurso = null;
            Debug.Log("Grapenade: Sin material");
        }

        if (recurso != null)
        {
            Instantiate(recurso, transform.position, Quaternion.identity);

        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Grapenade 
// namespace
