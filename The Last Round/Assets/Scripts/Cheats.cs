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
    [SerializeField] private GameObject cheatMenu;
    [SerializeField] private UnityEngine.UI.Toggle invunerabilidad;
    [SerializeField]
    Slider numManzurrias, numUvoncios, numManzarietes, numGrapenades,
                            habMinManzariete, habMinGrapenade, tiempo, recursosIniciales,
                            dineroInicial, enemigosVez;
    [SerializeField]
    private AudioClip woodSfx;

    [SerializeField]
    [Header("Invulneraviblidad")]
    private bool invulneravilidad;

    [SerializeField]
    [Header("Cantidad de enemigos")]
    [Range(0, 100)]
    private int Manzurrias,
                Uvoncios,
                Manzarietes,
                Grapenades;

    [SerializeField]
    [Header("Habitantes para empezar a aparecer")]
    [Range(2, 401)]
    private int ManzarietesHab,
                GrapenadesHab;

    [SerializeField]
    [Header("Segundos hasta el amanecer")]
    [Range(1, 121)]
    private float segundos;

    [SerializeField]
    [Header("Recursos iniciales")]
    [Range(0, 99)]
    private int recursos;

    [SerializeField]
    [Header("Dinero inicial")]
    [Range(0, 999)]
    private int dinero;

    [SerializeField]
    [Header("Enemigos en combate a la vez")]
    [Range(1, 30)]
    private int MaxEnemiesAtOnce;

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
        if (cheatMenu != null)
        {
            cheatMenu.SetActive(false);
        }
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
            Invulnerabilidad(invunerabilidad.isOn);

            //numEnemigo v
            //Manejan los enemigos con los que empieza la partida
            NumEnemy(EnemyType.Manzurria, Mathf.RoundToInt(numManzurrias.value));
            NumEnemy(EnemyType.Manzariete, Mathf.RoundToInt(numManzarietes.value));
            NumEnemy(EnemyType.Grapenade, Mathf.RoundToInt(numGrapenades.value));
            NumEnemy(EnemyType.Uvoncio, Mathf.RoundToInt(numUvoncios.value));

            //numHab para Grap y Manzariete v
            //Controlan el numero necesario de habitantes para que spawneen
            MinNumManzariete(Mathf.RoundToInt(habMinManzariete.value));
            MinNumGrapenade(Mathf.RoundToInt(habMinGrapenade.value));

            //Tiempo v
            //Tiempo inicial de temporizador
            Temporizador(tiempo.value);

            //recursos iniciales
            //recursos con los que se empieza
            SourcesNum(recursosIniciales.value);

            //Dinero inicial v
            //Direno con que empieza
            NumDinero(Mathf.RoundToInt(dineroInicial.value));

            //enem a la vez v
            //numero de enemigos en combate que puede ver a la vez
            MaxEnemies(Mathf.RoundToInt(enemigosVez.value));
        }
        GameManager.Instance.SetCheats(applyCheats);
    }

    public void AplicarEscenasEspeciales()
    {
        //Invunerabilidad v
        //Si esta activa no baja de vida
        Invulnerabilidad(invulneravilidad);

        //numEnemigo v
        //Manejan los enemigos con los que empieza la partida
        NumEnemy(EnemyType.Manzurria, Mathf.RoundToInt(Manzurrias));
        NumEnemy(EnemyType.Manzariete, Mathf.RoundToInt(Manzarietes));
        NumEnemy(EnemyType.Grapenade, Mathf.RoundToInt(Grapenades));
        NumEnemy(EnemyType.Uvoncio, Mathf.RoundToInt(Uvoncios));

        //numHab para Grap y Manzariete v
        //Controlan el numero necesario de habitantes para que spawneen
        MinNumManzariete(Mathf.RoundToInt(ManzarietesHab));
        MinNumGrapenade(Mathf.RoundToInt(GrapenadesHab));

        //Tiempo v
        //Tiempo inicial de temporizador
        Temporizador(segundos);

        //recursos iniciales
        //recursos con los que se empieza
        SourcesNum(recursos);

        //Dinero inicial v
        //Direno con que empieza
        NumDinero(Mathf.RoundToInt(dinero));

        //enem a la vez v
        //numero de enemigos en combate que puede ver a la vez
        MaxEnemies(Mathf.RoundToInt(MaxEnemiesAtOnce));
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

    private void ApplyCheats()
    {
        GameManager.Instance.SetCheats(true);
    }

    private void Invulnerabilidad(bool On)
    {
        GameManager.Instance.SetInvunerabilidad(On);
    }

    private void NumEnemy(EnemyType enemy, int num)
    {
        GameManager.Instance.SetNumEnemies(enemy, Mathf.RoundToInt(num));
    }

    private void MinNumManzariete(int num)
    {
        GameManager.Instance.SetMinHabManzariete(num);
    }

    private void MinNumGrapenade(int num)
    {
        GameManager.Instance.SetMinHabGrapenade(num);
    }

    private void Temporizador(float time)
    {
        GameManager.Instance.SetTimerStart(time);
    }

    private void SourcesNum(float num)
    {
        GameManager.Instance.SetResources(num);
    }

    private void NumDinero(int num)
    {
        GameManager.Instance.increaseDinero(num);
    }

    private void MaxEnemies(int num)
    {
        GameManager.Instance.SetMaxEnemsScene(num);
    }

    #endregion

} // class Cheats 
// namespace
