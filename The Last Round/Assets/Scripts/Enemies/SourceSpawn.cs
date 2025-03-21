//---------------------------------------------------------
// Se encarga de crear los recursos que los enemigos dejan al ser destruidos
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
public class SourceSpawn : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private GameObject PielUvaNegra, PielUvaRoja, JugoUva, SemillaUva, PielManzanaRoja, PielManzanaVerde, JugoManzana, SemillaManzana;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private GameObject Piel, Jugo, Semilla, Recurso;
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
        if (GetComponent<CastEnemy>() != null)
        {
            EnemyType enemy = GetComponent<CastEnemy>().GetEnemyType();

            if (enemy == EnemyType.Manzurria || enemy == EnemyType.Manzariete)
            {
                if (enemy == EnemyType.Manzurria)
                {
                    Piel = PielManzanaVerde;
                }
                else
                {
                    Piel = PielManzanaRoja;
                }

                Jugo = JugoManzana;
                Semilla = SemillaManzana;
            }
            else
            {
                if (enemy == EnemyType.Uvoncio)
                {
                    Piel = PielUvaRoja;
                }
                else
                {
                    Piel = PielUvaNegra;
                }

                Jugo = JugoUva;
                Semilla = SemillaUva;
            }
        }
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

    public void Spawn()
    {
        int recursornd = Random.Range(0, 4);
        if (recursornd == 1)
        {
            Recurso = Piel;
            //Debug.Log("Uvoncio: Piel de uva");
        }
        else if (recursornd == 2)
        {
            Recurso = Jugo;
            //Debug.Log("Uvoncio: Jugo de uva");
        }
        else if (recursornd == 3)
        {
            Recurso = Semilla;
            //Debug.Log("Uvoncio: Semilla de uva");
        }
        else
        {
            Recurso = null;
            //Debug.Log("Uvoncio: Sin material");
        }

        if (Recurso != null)
        {
            Instantiate(Recurso, transform.position, Quaternion.identity);
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

} // class SourceSpawn 
// namespace
