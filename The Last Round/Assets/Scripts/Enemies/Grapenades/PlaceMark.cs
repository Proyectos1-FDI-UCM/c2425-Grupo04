//---------------------------------------------------------
// Crea una marca a los pies del jugador cuando el objeto referenciado se queda quieto
// Víctor Castro Álvarez
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Runtime.CompilerServices;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlaceMark : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private GameObject MarcaPrefab;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private GameObject marcaInstanciada;
    private GameObject Player;
    private bool marcaInstanciadaBool = false;
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
        if (Player == null)
            Player = GameManager.Instance.GetPlayer();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void MarcarJugador()
    {
        if (Player != null)
        {
            marcaInstanciada = Instantiate(MarcaPrefab, Player.transform.position, Quaternion.identity);
            marcaInstanciada.GetComponent<MarcaBehaviour>().SetEmisor(this);
        }


        marcaInstanciadaBool = true;

        if (marcaInstanciada != null && marcaInstanciada.GetComponent<Shoot>() != null)
            marcaInstanciada.GetComponent<Shoot>().Shooting();
    }

    public void EliminarMarca()
    {
        marcaInstanciadaBool = false;
    }

    public bool MarcaInstanciada()
    {
        return marcaInstanciadaBool;
    }

    public void GrapenadeWasDestroy()
    {
        if (marcaInstanciada != null) marcaInstanciada.GetComponent<MarcaBehaviour>().GrapenadeWasDestroy();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    //Si la marca se choca con el proyectil del Grapenade (llega a su destino), la marca se destruye

    #endregion   

} // class PlaceMark 
// namespace
