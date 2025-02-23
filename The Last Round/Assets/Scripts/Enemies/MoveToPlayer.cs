//---------------------------------------------------------
// Este script localiza y mueve a un objeto hacia el jugador (los enemigos lo llamarán)
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Esta clase se encargará de tener un método que toma la posición.
/// Dicho método crea un vector de movimiento y mueve al objeto hacia él.
/// El método será llamado desde los enemigos que se pasarán como objetos al método para ser movidos
/// </summary>
public class MoveToPlayer : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField]
    float Speed;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Vector3 EnemyPlayer;
    private GameObject Player;
    private bool Floor = false, Ceiling = false, RWall = false, LWall = false;
    private Rigidbody2D rb;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Player == null)
            Player = GameManager.Instance.GetPlayer();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void Move(GameObject enemy)
    {
        EnemyPlayer = UpdateVector(enemy);
        //Convierte este vector en un vector unitario
        //Al hacer que todos los vectores midan 1u no hace falta normalizar el vector
        if (EnemyPlayer.x != 0 && EnemyPlayer.y != 0 && EnemyPlayer.x * EnemyPlayer.x +
          /*QUE HUECO MAS FEO QUE HAY AQUÍ, LO RELLENO*/EnemyPlayer.y * EnemyPlayer.y != 0)
            EnemyPlayer /= Mathf.Sqrt(EnemyPlayer.x * EnemyPlayer.x +
              /*Y AQUÍ IGUAL MACHO*/  EnemyPlayer.y * EnemyPlayer.y);
        else if (EnemyPlayer.x != 0) EnemyPlayer /= Mathf.Sqrt(EnemyPlayer.x * EnemyPlayer.x);
        else if (EnemyPlayer.y != 0) EnemyPlayer /= Mathf.Sqrt(EnemyPlayer.y * EnemyPlayer.y);

        //Mueve al objeto
        rb.velocity = EnemyPlayer * Speed * Time.deltaTime;
    }
    public Vector3 UpdateVector(GameObject enemy)
    {
        if (Player != null && enemy != null)
        //Localiza el vector que une el jugador con el objeto
        EnemyPlayer = new Vector3(Player.transform.position.x - enemy.transform.position.x,
        /*   []  . .  []    UN  */Player.transform.position.y - enemy.transform.position.y,
        /*  \___________/  SAPO */0);
        if (Floor && EnemyPlayer.y < 0 || Ceiling && EnemyPlayer.y > 0) EnemyPlayer = Vector3.zero;
        if (LWall && EnemyPlayer.x > 0 || RWall && EnemyPlayer.x < 0) EnemyPlayer = Vector3.zero;

        return EnemyPlayer;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;


        if (normal.y > 0.5f)
        {
            Floor = true;
        }
        if (normal.y < -0.5)
        {
            Ceiling = true;
        }

        if (normal.x < -0.5f)
        {
            LWall = true;
        }
        if (normal.x > 0.5f)
        {
            RWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("AAAAAA");
        Floor = Ceiling = LWall = RWall = false;
    }
    #endregion
} // class MoveToPlayer 
// namespace
