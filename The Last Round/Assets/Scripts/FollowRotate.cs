//---------------------------------------------------------
// Se asigna este script a un objeto con un pivote en otro objeto distinto,
// y rota siguiendo a un objeto (que será normalmente el ratón)
// Víctor Castro Álvarez
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class FollowRotate : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] GameObject FollowObject;
    [SerializeField] GameObject PivotObject;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Vector3 ObjectPos;
    #endregion
    bool moveornot=true;   
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
        GetObjectVector();
        float rotation = Mathf.Atan2(ObjectPos.y, ObjectPos.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0, 0, rotation); 
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void tocado(bool tocado)
    {
       if(tocado==true)moveornot = false;
        if (tocado ==false) moveornot = true;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    //Versión modificada de UpdateVector (de MoveToPlayer)
    private void GetObjectVector()
    {
        if (FollowObject != null && PivotObject != null&&moveornot ==true)
            ObjectPos = new Vector3(FollowObject.transform.position.x - PivotObject.transform.position.x,
                              FollowObject.transform.position.y - PivotObject.transform.position.y,
                              0);



        #endregion
    }
  
   

} // class FollowRotate 
// namespace
