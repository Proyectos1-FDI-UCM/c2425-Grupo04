//---------------------------------------------------------
// Script que maneja todos los cheats para testing
// Víctor Castro Álvarez
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Cheats : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private GameObject cheatMenu;
    [SerializeField] private Toggle invunerabilidad;
    [SerializeField]
    Slider numManzurrias, numUvoncios, numManzarietes, numGrapenades,
                            habMinManzariete, habMinGrapenade, tiempo, recursosIniciales,
                            dineroInicial, enemigosVez;
    [SerializeField]
    private AudioClip woodSfx;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private bool applyCheats = false;

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
        cheatMenu.SetActive(false);

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Aplica los cheats, si se activa
    /// </summary>
    public void Aplicar()
    {
        applyCheats = cheatMenu.activeSelf;

        if (applyCheats)
        {
            //Funcionalidad de los cheats aquí

            //Invunerabilidad v
            //Si esta activa no baja de vida
            GameManager.Instance.SetInvunerabilidad(invunerabilidad.isOn);

            //numEnemigo v
            //Manejan los enemigos con los que empieza la partida
            GameManager.Instance.SetNumEnemies(EnemyType.Manzurria, Mathf.RoundToInt(numManzurrias.value));
            GameManager.Instance.SetNumEnemies(EnemyType.Manzariete, Mathf.RoundToInt(numManzarietes.value));
            GameManager.Instance.SetNumEnemies(EnemyType.Grapenade, Mathf.RoundToInt(numGrapenades.value));
            GameManager.Instance.SetNumEnemies(EnemyType.Uvoncio, Mathf.RoundToInt(numUvoncios.value));

            //numHab para Grap y Manzariete v
            //Controlan el numero necesario de habitantes para que spawneen
            GameManager.Instance.SetMinHabManzariete(Mathf.RoundToInt(habMinManzariete.value));
            GameManager.Instance.SetMinHabGrapenade(Mathf.RoundToInt(habMinGrapenade.value));

            //Tiempo v
            //Tiempo inicial de temporizador
            GameManager.Instance.SetTimerStart(tiempo.value);

            //recursos iniciales
            //recursos con los que se empieza
            GameManager.Instance.SetResources(recursosIniciales.value);

            //Dinero inicial v
            //Direno con que empieza
            GameManager.Instance.increaseDinero(Mathf.RoundToInt(dineroInicial.value));

            //enem a la vez v
            //numero de enemigos en combate que puede ver a la vez
            GameManager.Instance.SetMaxEnemsScene(Mathf.RoundToInt(enemigosVez.value));
        }
        GameManager.Instance.SetCheats(applyCheats);
    }

    public void OpenCloseMenu(GameObject MenuToClose)
    {
        AudioManager.Instance.PlaySFX(woodSfx);
        
        if (!MenuToClose.activeSelf)
        {
            MenuToClose.SetActive(true);
        }
        else
        {
            MenuToClose.SetActive(false);
        }
    }

    public void PlaySoundEffect()
    {
        AudioManager.Instance.PlaySFX(woodSfx);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Cheats 
// namespace
