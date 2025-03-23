//---------------------------------------------------------
// Este archivo es un detector de colisiones general que servirá a objetos para saber hacia donde pueden o no realizar movimientos
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

public enum Directions
{
    North,
    South,
    East,
    West,
}
/// <summary>
/// Se encarga de detectar colisiones con otros objetos, este es capaz de devolver un booleano por cada dirección
/// El booleano devuelto permite saber al objeto que lo pide si está colisionando o no en esa dirección
/// </summary>
public class CollisionDetector : MonoBehaviour
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
    private void LateUpdate()
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
    /// Devuelve true si está colisionando y false si no lo está
    /// toma como argumento un enum del tipo "Directions" y devuelve el booleano de esa dirección
    /// </summary>
    /// <returns></returns>
    public bool GetCollisions(Directions direction)
    {
        return collisions[(int)direction];
    }

    /// <summary>
    /// Refresca todas las colisiones
    /// </summary>
    public void Refresh()
    {
        ResetCollisions = true;
    }

    /// <summary>
    /// Reseta una layer en específico y toma como argumento el número de la capa
    /// </summary>
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
                collisions[(int)Directions.South] = true;
                objects[1] = collision.gameObject;
            }
            if (normal.y < -0.5f)
            {
                collisions[(int)Directions.North] = true;
                objects[0] = collision.gameObject;
            }

            if (normal.x > 0.5f)
            {
                collisions[(int)Directions.West] = true;
                objects[3] = collision.gameObject;
            }
            if (normal.x < -0.5f)
            {
                collisions[(int)Directions.East] = true;
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