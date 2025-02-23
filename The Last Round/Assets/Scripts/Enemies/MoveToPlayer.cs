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
   
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
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
        EnemyPlayer /= Mathf.Sqrt(EnemyPlayer.x * EnemyPlayer.x +
                                  EnemyPlayer.y * EnemyPlayer.y);
        //Mueve al objeto
        enemy.transform.position += EnemyPlayer * Speed * Time.deltaTime;
    }
    public Vector3 UpdateVector (GameObject enemy)
    {
        //Localiza el vector que une el jugador con el objeto
        EnemyPlayer = new Vector3(Player.transform.position.x - enemy.transform.position.x,
                                  Player.transform.position.y - enemy.transform.position.y,
                                  0);
        return EnemyPlayer;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detiene el movimiento si colisiona con algo
        if (collision.gameObject.CompareTag("wall"))
        {

            Vector2 normal = collision.contacts[0].normal;

            
            if (Mathf.Abs(normal.x) > 0.5f) 
            {
                EnemyPlayer.x = 0;
            }
            else if (Mathf.Abs(normal.y) > 0.5f) 
            {
                EnemyPlayer.y = 0;
            }
        }

       
    }
} // class MoveToPlayer 
// namespace
