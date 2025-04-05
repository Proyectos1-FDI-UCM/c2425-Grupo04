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

    #region relocateDetector
    /// <summary>
    /// Se encarga de detectar la primera vez que se encuentra en una colisión con otro objeto
    /// 
    /// Se usa para recolocar al objeto al colisionar y esto solo se hace con la primera colisión
    /// Si se recolocase en el Stay tendríamos los objetos pegados y no es lo esperado
    /// </summary>
    /// <param name="collision"></param>
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    for (int i = 0; i < collision.contacts.Length; i++)
    //    {
    //        Vector2 normal = collision.contacts[i].normal;

    //        if (normal.y > 0.5f)
    //        {
    //            //Debug.Log("South");
    //            Relocate(collision.gameObject, Directions.South);
    //        }
    //        if (normal.y < -0.5f)
    //        {
    //            //Debug.Log("North");
    //            Relocate(collision.gameObject, Directions.North);
    //        }

    //        if (normal.x > 0.5f)
    //        {
    //            //Debug.Log("West");
    //            Relocate(collision.gameObject, Directions.West);
    //        }
    //        if (normal.x < -0.5f)
    //        {
    //            //Debug.Log("East");
    //            Relocate(collision.gameObject, Directions.East);
    //        }
    //    }
    //}
    #endregion

    /// <summary>
    /// Se encarga de detectar de forma constante cuando se encuentra en una colisión con otro objeto
    /// 
    /// Esto se hace porque si dos objetos colisionan por el mismo lado y uno deja de colisionar
    /// activa el exit y anula la colisión hacia esa dirección.
    /// 
    /// Esto no es cierto puesto que sigue habiendo un objeto en esa dirección
    /// por lo tanto es necesario comprobar las colisiones en un Stay
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        #region basura
        //for (int i = 0; i < collision.contacts.Length; i++)
        //{
        //    Vector2 normal = collision.contacts[i].normal;

        //    if (normal.y > 0.5f)
        //    {
        //        collisions[(int)Directions.South] = true;
        //        objects[1] = collision.gameObject;
        //    }
        //    if (normal.y < -0.5f)
        //    {
        //        collisions[(int)Directions.North] = true;
        //        objects[0] = collision.gameObject;
        //    }

        //    if (normal.x > 0.5f)
        //    {
        //        collisions[(int)Directions.West] = true;
        //        objects[3] = collision.gameObject;
        //    }
        //    if (normal.x < -0.5f)
        //    {
        //        collisions[(int)Directions.East] = true;
        //        objects[2] = collision.gameObject;
        //    }
        //}
        #endregion

        //Busca el punto de colisión más cercano al objeto
        Vector3 tmp = collision.collider.ClosestPoint(transform.position);

        //Calcula la dirección entre el punto y el objeto
        tmp -= transform.position;

        //Calcula el ángulo de la dirección frente a la línea de tierra
        float ang = Mathf.Atan2(tmp.y, tmp.x);
        //Convierto los radianes a grados
        ang *= Mathf.Rad2Deg;
        //Debug.Log(ang);

        //En una colisión diagonal el movimiento no se anula
        //Solo se anula el movimiento entre estas diagonales
        if ((ang > 45 && ang < 135) || (ang > -315 && ang < -255))
        {
            collisions[(int)Directions.North] = true;
            objects[(int)Directions.North] = collision.gameObject;
            //Debug.Log("North");
        }//Colisión norte
        else if ((ang > 135 && ang < 225) || (ang > -255 && ang < -135))
        {
            collisions[(int)Directions.West] = true;
            objects[(int)Directions.West] = collision.gameObject;
            //Debug.Log("West");
        }//Colisión oeste
        else if ((ang > 225 && ang < 315) || (ang > -135 && ang < -45))
        {
            collisions[(int)Directions.South] = true;
            objects[(int)Directions.South] = collision.gameObject;
            //Debug.Log("South");
        }//Colisión sur
        else if ((ang > 315 && ang <= 360) || (ang >= 0 && ang < 45) || (ang > -45 && ang <= 0) || (ang >= -360 && ang < -315))
        {
            collisions[(int)Directions.East] = true;
            objects[(int)Directions.East] = collision.gameObject;
            //Debug.Log("East");
        }//Colisión este
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        for (int i = 0; i < collisions.Length; i++)
        {
            collisions[i] = false;
            objects[i] = null;
        }
    }

    #region relocate
    /// <summary>
    /// Se encarga de recolizar a un objeto fuera de otro cogiendo sus medidas,
    /// calculando la distancia que ha de moverse y la dirección
    /// </summary>
    /// <param name="collision"></param>
    /// <param name="direction"></param>
    //private void Relocate(GameObject collision, Directions direction)
    //{
    //    //Comprueba quién va más rápido y lo recoloca
    //    Rigidbody2D rb = GetComponent<Rigidbody2D>(), CollisionRb = collision.GetComponent<Rigidbody2D>();

    //    //Ambos objetos tienen este script y solo se recolocará el más rápido de ambos
    //    if (rb != null && CollisionRb != null && rb.velocity.magnitude >= CollisionRb.velocity.magnitude)
    //    {
    //        //Debug.Log(CollisionRb.velocity.magnitude);
    //        float Size = 0, CollisionSize = 0;

    //        //Si es una colisión horizontal coge las medidas x y si es vertical coge las medidas y
    //        if (direction == Directions.North || direction == Directions.South)
    //        {
    //            if (GetComponent<Collider2D>() != null)
    //            {
    //                Size = GetComponent<Collider2D>().bounds.size.y;
    //            }
    //            if (collision.GetComponent<Collider2D>() != null)
    //            {
    //                CollisionSize = collision.GetComponent<Collider2D>().bounds.size.y;
    //            }
    //        }
    //        else if (direction == Directions.East || direction == Directions.West)
    //        {
    //            if (GetComponent<Collider2D>() != null)
    //            {
    //                Size = GetComponent<Collider2D>().bounds.size.x;
    //            }
    //            if (collision.GetComponent<Collider2D>() != null)
    //            {
    //                CollisionSize = collision.GetComponent<Collider2D>().bounds.size.x;
    //            }
    //        }

    //        //Solo cogemos la mitad de la medida total (interesa la medida del centro del objeto a su extremo)
    //        Size /= 2;
    //        CollisionSize /= 2;

    //        //Se calcula la distancia a recolocar
    //        float distance = 0;
    //        if (direction == Directions.North || direction == Directions.South)
    //        {
    //            distance = (Size + CollisionSize) - Mathf.Abs(collision.transform.position.y - transform.position.y);
    //        }
    //        else if (direction == Directions.East || direction == Directions.West)
    //        {
    //            distance = Size + CollisionSize - Mathf.Abs(collision.transform.position.x - transform.position.x);
    //        }
    //        Debug.Log($"Size:{Size}," +
    //                  $" CollisionSize:{CollisionSize}," +
    //                  $" collisionPosition: ({collision.transform.position.x},{collision.transform.position.y})," +
    //                  $" Position: ({transform.position.x}, {transform.position.y})," +
    //                  $" Math.Abs:{Mathf.Abs(collision.transform.position.y - transform.position.y)}," +
    //                  $" SumSizes: {Size + CollisionSize}" +
    //                  $" SumDistanceY: {(Size + CollisionSize) - Mathf.Abs(collision.transform.position.y - transform.position.y)}");
    //        Debug.Log(distance);

    //        //En función de la dirección de la colisión se recoloca hacia una dirección u otra
    //        if (direction == Directions.North)
    //        {
    //            rb.position -= new Vector2(0, distance);
    //        }
    //        else if (direction == Directions.South)
    //        {
    //            rb.position += new Vector2(0, distance);
    //        }
    //        else if (direction == Directions.West)
    //        {
    //            rb.position += new Vector2(distance, 0);
    //        }
    //        else if (direction == Directions.East)
    //        {
    //            rb.position -= new Vector2(distance, 0);
    //        }
    //    }
    //}
    #endregion

    #endregion

} // class CollisionDetecter 
  // namespace