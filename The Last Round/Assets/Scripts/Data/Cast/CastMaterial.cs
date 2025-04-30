//---------------------------------------------------------
// Script responsable de configurar cada uno de los recursos
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class CastMaterial : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] private SourceName material;
    [SerializeField] private SourceType type;
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    
    //Devuelve el nombre del recurso
    public SourceName GetSourceName()
    {
        return material;
    }
    public SourceType GetSourceType()
    {
        return type;
    }
    #endregion
    
} // class CastMaterial 
// namespace
