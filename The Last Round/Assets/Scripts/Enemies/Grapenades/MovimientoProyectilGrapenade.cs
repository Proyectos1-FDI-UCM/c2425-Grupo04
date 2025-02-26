//---------------------------------------------------------
// El movimiento del proyectil del Grapenade, desde que spawnea hasta que se destruye en la marca.
// Víctor Castro Álvarez
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class MovimientoProyectilGrapenade : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] float velocidad;
    [SerializeField] float respawnAltura;
    [SerializeField] float tiempoPausa = 0.5f;
    [SerializeField] float spawnDelay;
    [SerializeField] float radioDaño;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Vector3 MarcaPosicion;
    private bool isReturning = false;

    #endregion
    
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
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, MarcaPosicion, velocidad * Time.deltaTime);
            if (Vector2.Distance(transform.position, MarcaPosicion) <= 0.1f)
            {

            }
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void LanzarProyectil(Vector3 posicionInicial, Vector3 marcaPos)
    {
        transform.position = posicionInicial;
        MarcaPosicion = marcaPos;
        StartCoroutine(LanzarConDelay());
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    private IEnumerator LanzarConDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(MovArribaYVuelve());
    }

    private IEnumerator MovArribaYVuelve()
    {
        //1. Subir y desaparecer cuando se sale de la pantalla
        while(transform.position.y < respawnAltura)
        {
            transform.position += new Vector3(0, velocidad * Time.deltaTime, 0);
            yield return null;
        }
        gameObject.SetActive(false);
        yield return new WaitForSeconds(tiempoPausa);

        //2. Volver abajo
        transform.position = new Vector3(MarcaPosicion.x, respawnAltura, 0);
        gameObject.SetActive(true);
        isReturning = true;
    }

    #endregion   

} // class MovimientoProyectilGrapenade 
// namespace
