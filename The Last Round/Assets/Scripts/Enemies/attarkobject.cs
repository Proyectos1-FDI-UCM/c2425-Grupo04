//---------------------------------------------------------
// Un objeto que rodea al enemigo mirando hacia jugador, cuando toca al jugador hace dano
// Letian Liye
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class attarkobject : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    public GameObject enemy;
    public GameObject Player;
    public MoveToPlayer MoveToPlayer;
    public Transform rotationcenter;
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Vector3 PlayerDirection;
    
    
   
    private Rigidbody2D rb;
    
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

   
    void Update()
    {
        if (Player == null)
        {
            Player = GameManager.Instance.GetPlayer();
            Debug.Log("player encontrado");
        }
       
        //recoger la direction desde enemigo hasta jugador desde script de MoveToPlayer
        PlayerDirection = MoveToPlayer.UpdateVector(enemy);

        //Calcular el angulo objetivo (convertir a angulo 2D)
        float targetAngle = Mathf.Atan2(PlayerDirection.y, PlayerDirection.x) * Mathf.Rad2Deg;

        //Rotacion suave (utilice el angulo Lerp para evitar giros repentinos)
        float currentAngle = Mathf.LerpAngle( rotationcenter.eulerAngles.z,targetAngle, 1 * Time.deltaTime
        );
        //rotacion de objeto vacio , que esta en el centro del enemigo, y es elemento padre del objeto que tiene poder de atacar
        rotationcenter.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class attarkobject 
// namespace
