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
    private Rigidbody2D rb;
    private bool floor = false;
    private bool ceiling = false;
    private bool LWall = false;
    private bool RWall = false;
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
        if (EnemyPlayer.x != 0 && EnemyPlayer.y != 0)
        EnemyPlayer /= Mathf.Sqrt(EnemyPlayer.x * EnemyPlayer.x +
                                  EnemyPlayer.y * EnemyPlayer.y);
        //Mueve al objeto
        enemy.transform.position += EnemyPlayer * Speed * Time.deltaTime;
    }
    public Vector3 UpdateVector (GameObject enemy)
    {
        if (enemy != null && Player != null)
        //Localiza el vector que une el jugador con el objeto
            EnemyPlayer = new Vector3(Player.transform.position.x - enemy.transform.position.x,
                                      Player.transform.position.y - enemy.transform.position.y,
                                      0);
        if ((floor && EnemyPlayer.y == -1) || (ceiling && EnemyPlayer.y == 1)) EnemyPlayer.y = 0;
        if ((LWall && EnemyPlayer.x == -1) || (RWall && EnemyPlayer.x == 1)) EnemyPlayer.x = 0;

        return EnemyPlayer;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detiene el movimiento si colisiona con algo

            Vector2 normal = collision.contacts[0].normal;

        ceiling = floor = LWall = RWall = false;

        if (normal.y > 0.5f) floor = true;
        else if (normal.y < -0.5f) ceiling = true;

        if (normal.x > 0.5f) LWall = true;
        else if (normal.x < -0.5) RWall = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        floor = ceiling = LWall = RWall = false;
    }
} // class MoveToPlayer 
// namespace
