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
    private CollisionDetector cD;
    private Collider2D ObjectCollider;
    //Los inicializo a -1 para saber cuando no están definidos
    private float minX, maxX, minY, maxY;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cD = GetComponent<CollisionDetector>();
        ObjectCollider = GetComponent<Collider2D>();

        maxX = GameManager.Instance.GetMapWidth() / 2;
        minX = -maxX;

        maxY = GameManager.Instance.GetMapHeight() / 2;
        minY = -maxY;
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
        EnemyPlayer = UpdateVector(enemy).normalized;

        ////Mueve al objeto
        rb.velocity = EnemyPlayer * Speed;
    }

    public Vector3 UpdateVector(GameObject enemy)
    {
        Vector3 posMinX, posMinY, posMaxX, posMaxY;

        posMinX = rb.position - new Vector2(ObjectCollider.bounds.size.x / 2, 0);
        posMaxX = rb.position + new Vector2(ObjectCollider.bounds.size.x / 2, 0);
        posMinY = rb.position - new Vector2(0, ObjectCollider.bounds.size.y / 2);
        posMaxY = rb.position + new Vector2(0, ObjectCollider.bounds.size.y / 2);

        if (Player != null && enemy != null)
            //Localiza el vector que une el jugador con el objeto
            EnemyPlayer = new Vector3(Player.transform.position.x - enemy.transform.position.x,
                                      Player.transform.position.y - enemy.transform.position.y,
                                      0);

        if ((cD.GetCollisions(Directions.North) && EnemyPlayer.y > 0) ||
            (cD.GetCollisions(Directions.South) && EnemyPlayer.y < 0) ||
            (EnemyPlayer.y < 0.1 && EnemyPlayer.y > -0.1) ||
            (EnemyPlayer.y < 0 && posMinY.y <= minY) ||
            (EnemyPlayer.y > 0 && posMaxY.y >= maxY)) EnemyPlayer.y = 0;

        if ((cD.GetCollisions(Directions.East) && EnemyPlayer.x > 0) ||
            (cD.GetCollisions(Directions.West) && EnemyPlayer.x < 0) ||
            (EnemyPlayer.x < 0.1 && EnemyPlayer.x > -0.1) ||
            (EnemyPlayer.x < 0 && posMinX.x <= minX) ||
            (EnemyPlayer.x > 0 && posMaxX.x >= maxX)) EnemyPlayer.x = 0;

        return EnemyPlayer;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion
} // class MoveToPlayer 
// namespace
