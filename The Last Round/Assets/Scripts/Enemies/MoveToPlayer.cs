//---------------------------------------------------------
// Este script (Serializado) localiza y mueve a un objeto hacia el jugador (los enemigos lo llamarán)
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
//[SerializeField]
public class MoveToPlayer : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    [SerializeField]
    float Speed;
    [SerializeField]
    GameObject Player;

    // ---- ATRIBUTOS PRIVADOS ----
    private Vector3 EnemyPlayer;
    
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
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    public void Move(GameObject enemy)
    {
        //Localiza el vector que une el jugador con el objeto
        EnemyPlayer = new Vector3(Player.transform.position.x - enemy.transform.position.x,
                                  Player.transform.position.y - enemy.transform.position.y,
                                  0);

        //Convierte este vector en un vector unitario
        //Al hacer que todos los vectores midan 1u no hace falta normalizar el vector
        EnemyPlayer /= Mathf.Sqrt(EnemyPlayer.x*EnemyPlayer.x +
                                  EnemyPlayer.y*EnemyPlayer.y);

        //Mueve al objeto
        enemy.transform.position += EnemyPlayer * Speed * Time.deltaTime;
    }

    // ---- MÉTODOS PRIVADOS ----
    //Testeo del script
    private void FixedUpdate()
    {
        Move(gameObject);
    }

} // class MoveToPlayer 
// namespace
