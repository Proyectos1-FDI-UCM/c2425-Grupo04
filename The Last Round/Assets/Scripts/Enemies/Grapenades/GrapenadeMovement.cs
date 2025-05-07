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
    [SerializeField]
    private float ObjectSizeX, ObjectSizeY;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Rigidbody2D rb;
    private MoveToPlayer moveToPlayer;
    private Vector3 EnemyPlayer;
    private GameObject recurso;
    private float minX, maxX, minY, maxY;
    private Animator grapenadeAnim;
    private bool moveBool, canShoot;
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
        canShoot = false;
        moveBool = true;
        rb = GetComponent<Rigidbody2D>();
        moveToPlayer = GetComponent<MoveToPlayer>();

        maxX = GameManager.Instance.GetMapWidth() / 2;
        minX = -maxX;

        maxY = GameManager.Instance.GetMapHeight() / 2;
        minY = -maxY;

        grapenadeAnim = GetComponent<Animator>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        EnemyPlayer = GameManager.Instance.GetPlayer().transform.position - transform.position;
        Vector3 velocidad = rb.velocity;
        if (velocidad == Vector3.zero)
        {
            grapenadeAnim.SetTrigger("mark2");
        }
        //si puede andar
        if(moveBool)
        {
            //si el jugador esta en rango, pone marca, si no hay
            if (Mathf.Floor(EnemyPlayer.magnitude * 10) / 10 == range)
            {
                if (!GetComponent<PlaceMark>().MarcaInstanciada())
                {
                    canShoot = false;
                    //Debug.Log("1");
                    //se para y inicia la animacion de coger la bomba
                    rb.velocity = Vector3.zero;
                    grapenadeAnim.SetBool("Marking", true);
                    Debug.Log(grapenadeAnim.GetBool("Marking"));
                    GetComponent<PlaceMark>().MarcarJugador();
                }
            }
            //se aleja del jugador si esta muy cerca
            else if (Mathf.Floor(EnemyPlayer.magnitude * 10) / 10 < range)
            {
                Vector3 posMinX, posMinY, posMaxX, posMaxY;

                posMinX = rb.position - new Vector2(ObjectSizeX / 2, 0);
                posMaxX = rb.position + new Vector2(ObjectSizeX / 2, 0);
                posMinY = rb.position - new Vector2(0, ObjectSizeY / 2);
                posMaxY = rb.position + new Vector2(0, ObjectSizeY / 2);

                if ((-EnemyPlayer.y < 0 && posMinY.y <= minY) ||
                    (-EnemyPlayer.y > 0 && posMaxY.y >= maxY))
                    EnemyPlayer.y = 0;

                if ((-EnemyPlayer.x < 0 && posMinX.x <= minX) ||
                    (-EnemyPlayer.x > 0 && posMaxX.x >= maxX))
                    EnemyPlayer.x = 0;

                rb.velocity = -EnemyPlayer.normalized * marchaAtrasSpeed;

                if (transform.position.y > range) transform.position = new Vector2(transform.position.x, range);
                if (transform.position.x > range) transform.position = new Vector2(range, transform.position.y);
            }
            //le persigue si esta lejos
            else
            {
                moveToPlayer.Move(gameObject);
            }
        }
        //si no esta parado
        else
        {
            rb.velocity = Vector3.zero;
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
    
    //activa la animacion de lanzar la bomba
    public void StopMarking()
    {
        grapenadeAnim.SetBool("Marking", false);
        
    }
    //activa o desactiva el movimiento, se usa en Animation events en las animaciones coger y en lanzar la boma
    public void CanMove(string canMove)
    {
        moveBool = bool.Parse(canMove);
        grapenadeAnim.SetBool("canMove", moveBool);
    }

    public bool CheckShoot()
    {
        return canShoot;
    }

    public void Shooting()
    {
        canShoot = true;
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
