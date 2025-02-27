//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Ignacio Plaza
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
[Serializable]
public class CooldownGeneral
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    //#region Atributos del Inspector (serialized fields)
    //// Documentar cada atributo que aparece aquí.
    //// El convenio de nombres de Unity recomienda que los atributos
    //// públicos y de inspector se nombren en formato PascalCase
    //// (palabras con primera letra mayúscula, incluida la primera letra)
    //// Ejemplo: MaxHealthPoints

    //#endregion

    [SerializeField]
    private float cooldownTiempo = 3f;

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion

    private float ultimaAccion = -Mathf.Infinity;
    //Se necesita para iniciar el cooldown

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
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    public bool Empezando
    {
        get { return Time.time >= ultimaAccion + cooldownTiempo; }
    }
    public void Activar()
    {
        if (Empezando)
        {
            ultimaAccion = Time.time;
        }
    }
    public float ObtenerTiempoRestante()
    {
        float restante = (ultimaAccion + cooldownTiempo) - Time.time;
        return Mathf.Max(0, restante);
    }
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

   
} // class CooldownGeneral 
// namespace
