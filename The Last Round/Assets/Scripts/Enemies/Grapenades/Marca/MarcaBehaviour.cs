//---------------------------------------------------------
// La marca sigue al jugador hasta que se ha realizado el disparo
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class MarcaBehaviour : MonoBehaviour
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
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private bool follow = true;
    private PlaceMark emisor;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (follow)
        {
            GetComponent<MoveToPlayer>().Move(gameObject);
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
    public void UnfollowPlayer()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        follow = false;

        if (emisor != null)
        {
            emisor.MarkingStop();
        }

    }
    public void GrapenadeWasDestroy()
    {
        if (follow)
        {
            MarcaEliminada();
            Destroy(gameObject);
        }
    }
    public void MarcaEliminada()
    {
        if (emisor != null)
        {
            emisor.EliminarMarca();
        }
        else
        {
            //Si no existe una marca sin emisor no tiene dueño y por tanto queremos que se destruya
            Destroy(gameObject);
        }
    }
    public void SetEmisor(PlaceMark emisor)
    {
        this.emisor = emisor;
    }

    public bool CanShootBullet()
    {
        if (emisor != null)
        {
            return emisor.CanShoot();
        }
        else
        {
            //Si no existe una marca sin emisor no tiene dueño y por tanto queremos que se destruya
            Destroy(gameObject);
            return false;
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class MarcaBehaviour 
// namespace
