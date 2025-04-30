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
public class MeleeAttack : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    

    [SerializeField]
    public SpriteRenderer attackSprite;

    [SerializeField]
    public float duration;

    [SerializeField]
    public float PMdamage;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private bool Attack = false;
    private float timer = 0;
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
        GetComponent<Collider2D>().enabled = false;
        attackSprite = GetComponent<SpriteRenderer>();
        attackSprite.enabled = false;
    }
    private void Update()
    {
        if (Attack || timer > 0)
        {
            GetComponent<Collider2D>().enabled = true;
            attackSprite.enabled = true;
            Attack = false;

            if (timer <= 0) timer = duration;
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
            attackSprite.enabled = false;
        }
        
        timer -= Time.deltaTime;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// El objeto que tiene este script activa el ataque
    /// </summary>
    public void attack()
    {
        Attack = true;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class MeleeAttack 
// namespace
