//---------------------------------------------------------
// Controlador de la vida del jugador
// Víctor Castro Álvarez
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] int health = 50;
    [SerializeField] GameObject ContadorDaño;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    bool hasDied; 
    private TextMeshProUGUI text;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    private void Start()
    {
        text = ContadorDaño.GetComponentInChildren<TextMeshProUGUI>();
    
        hasDied = false;
    }

    private void Update()
    {
        //Reemplazar con lo que sea después
        if(health <= 0)
        {
            hasDied = true;
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void HitPlayer(int damage)
    {
        health -= damage;
        text.text = damage.ToString();
        Instantiate(ContadorDaño, gameObject.transform.position, Quaternion.identity);
    }

    public bool hasPlayerDied()
    {
        return hasDied;
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion   

} // class PlayerHealth 
// namespace
