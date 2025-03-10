//---------------------------------------------------------
// Este archivo es un detector de colisiones general que servirá a objetos para saber hacia donde pueden o no realizar movimientos
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Contiene un método que devuelve un array de booleanos
/// True = está en colisión / False = no está en colisión
/// Array: 
///   [0] = N
///   [1] = S
///   [2] = E
///   [3] = W
/// </summary>
public class CollisionDetecter : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private bool[] collisions = new bool[4];
    private GameObject[] objects = new GameObject[4];
    private bool ResetCollisions = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Update()
    {
        if (ResetCollisions)
        {
            for (int i = 0; i < collisions.Length; i++)
            {
                collisions[i] = false;
                objects[i] = null;
            }
            ResetCollisions = false;
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Array: 
    ///   [0] = N or UP,  
    ///   [1] = S or DOWN,  
    ///   [2] = E or RIGHT,  
    ///   [3] = W or LEFT.
    /// </summary>
    /// <returns></returns>
    public bool[] GetCollisions()
    {
        return collisions;
    }
    public void Reset()
    {
        ResetCollisions = true;
    }

    //Función que resetea una Layer en especifico
    public void ResetLayer(int Layer)
    {
        for (int i = 0; i < collisions.Length; i++)
        {
            if (objects[i] != null && objects[i].gameObject.layer == Layer)
            {
                collisions[i] = false;
                Debug.Log("E U P");
            }

            if (objects[i] != null)
            Debug.Log("Ole Ole " + objects[i].name + " " + objects[i].layer);
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    private void OnCollisionStay2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            Vector2 normal = collision.contacts[i].normal;

            if (normal.y > 0.5f)
            {
                collisions[1] = true;
                objects[1] = collision.gameObject;
            }
            if (normal.y < -0.5f)
            {
                collisions[0] = true;
                objects[0] = collision.gameObject;
            }

            if (normal.x > 0.5f)
            {
                collisions[3] = true;
                objects[3] = collision.gameObject;
            }
            if (normal.x < -0.5f)
            {
                collisions[2] = true;
                objects[2] = collision.gameObject;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        for (int i = 0; i < collisions.Length; i++)
        {
            collisions[i] = false;
            objects[i] = null;
        }
    }
    #endregion   

} // class CollisionDetecter 
  // namespace