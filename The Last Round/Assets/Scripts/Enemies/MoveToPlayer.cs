//---------------------------------------------------------
// Este script localiza y mueve a un objeto hacia el jugador (los enemigos lo llamarán)
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
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
    float Speed, ObjectSizeX, ObjectSizeY;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Vector3 EnemyPlayer;
    private GameObject Player;
    private Rigidbody2D rb;
    //Los inicializo a -1 para saber cuando no están definidos
    private float minX, maxX, minY, maxY;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        //Animación de movimiento
        if (animator != null && spriteRenderer != null)
        {
            animator.SetFloat("Horizontal", Mathf.Abs(rb.velocity.x));
            animator.SetFloat("Vertical", rb.velocity.y);
            animator.SetFloat("Speed", rb.velocity.magnitude);

            if (Mathf.Abs(rb.velocity.y) < 0.2f)
            {
                if (rb.velocity.x < 0)
                {
                    //Si se mueve a la izquierda, flip al Sprite Renderer
                    spriteRenderer.flipX = true;
                }
                else if (rb.velocity.x > 0)
                {
                    //Si se mueve a la derecha, no hay flip al Sprite Renderer
                    spriteRenderer.flipX = false;
                }
                //Hago dos ifs para que no haya un estado predeterminado y evitar problemas con el flip.
            }
        }
    }

    public Vector3 UpdateVector(GameObject enemy)
    {
        Vector3 posMinX, posMinY, posMaxX, posMaxY;

        posMinX = rb.position - new Vector2(ObjectSizeX / 2, 0);
        posMaxX = rb.position + new Vector2(ObjectSizeX / 2, 0);
        posMinY = rb.position - new Vector2(0, ObjectSizeY / 2);
        posMaxY = rb.position + new Vector2(0, ObjectSizeY / 2);

        if (Player != null && enemy != null)
        {
            
            Vector2 PlayerPos, EnemyPos;
            PlayerPos = Player.transform.position;
            EnemyPos = enemy.transform.position;
            //Si el jugador tiene collider
            //la posición del jugador será la del centro del collider
            if (Player.GetComponent<Collider2D>() != null)
            {
                Collider2D tmp = Player.GetComponent<Collider2D>();
                PlayerPos += tmp.offset;
            }
            if (Player.GetComponent<Collider2D>() != null)
            {
                Collider2D tmp = enemy.GetComponent<Collider2D>();
                EnemyPos += tmp.offset;
            }

            //Localiza el vector que une el jugador con el objeto
            EnemyPlayer = new Vector3(PlayerPos.x - EnemyPos.x,
                                      PlayerPos.y - EnemyPos.y,
                                      0);
        }
            
            

        if ((EnemyPlayer.y < 0.1 && EnemyPlayer.y > -0.1) ||
            (EnemyPlayer.y < 0 && posMinY.y <= minY) ||
            (EnemyPlayer.y > 0 && posMaxY.y >= maxY)) EnemyPlayer.y = 0;

        if ((EnemyPlayer.x < 0.1 && EnemyPlayer.x > -0.1) ||
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
