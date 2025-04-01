//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class CameraLogic : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private GameObject player;
    private float minX, maxX, minY, maxY;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    private void Start()
    {
        float CameraY = Camera.main.orthographicSize;
        float CameraX = CameraY * Camera.main.aspect;

        maxY = (GameManager.Instance.GetMapHeight() / 2) - CameraY;
        minY = -maxY;

        maxX = (GameManager.Instance.GetMapWidth() / 2) - CameraX;
        minX = -maxX;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void LateUpdate()
    {
        if (player == null)
        {
            player = GameManager.Instance.GetPlayer();
        }
        if (player != null)
        { 
            //Debug.Log($"({minX}, {minY}), ({maxX}, {maxY})");
            //Debug.Log($"({CameraX}, {CameraY})");

            transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, minX, maxX),
                                              Mathf.Clamp(player.transform.position.y, minY, maxY),
                                              transform.position.z);
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

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class CameraLogic 
// namespace
