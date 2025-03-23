//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor;
using UnityEngine;


/// <summary>
/// Componente responsable de la gestión global del juego. Es un singleton
/// que orquesta el funcionamiento general de la aplicación,
/// sirviendo de comunicación entre las escenas.
///
/// El GameManager ha de sobrevivir entre escenas por lo que hace uso del
/// DontDestroyOnLoad. En caso de usarlo, cada escena debería tener su propio
/// GameManager para evitar problemas al usarlo. Además, se debería producir
/// un intercambio de información entre los GameManager de distintas escenas.
/// Generalmente, esta información debería estar en un LevelManager o similar.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)
    [Header("Número de enemigos")]
    [SerializeField] int Uvoncio;
    [SerializeField] int Manzurria;
    [SerializeField] int Grapenade;
    [SerializeField] int Manzariete;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private UIManager UIManager;
    private UIManager_Combate UIManagerCombate;
    private UIManagerUpgrades UIManagerUpgrades;
    private static GameManager _instance;
    private GameObject Player;
    private float[] recursos = new float[8];
    private int[] numEnemigos = new int[4];

    //0 = Jugo de Uva       //3 = Jugo de manzana
    //1 = Piel de Uva       //4 = Piel de manzana
    //2 = Semilla de Uva    //5 = Semilla manzana
    //6 = Hielo             //7 = Levadura

    //0 = Uvoncio           //1 = Manzurria
    //2 = Grapenade         //3 = Manzariete

    private float NivelSospechosos = 0;
    private float Dineros = 0;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // GameManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Si es necesario, transferimos la configuración que es
            // dependiente de la escena. Esto permitirá al GameManager
            // real mantener su estado interno pero acceder a los elementos
            // de la escena particulares o bien olvidar los de la escena
            // previa de la que venimos para que sean efectivamente liberados.
            TransferSceneState();

            // Y ahora nos destruímos del todo. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer GameManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        } // if-else somos instancia nueva o no.
    }

    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal
    }
    private void Start()
    {
        ResetEnemyCounter();
    }

    private void Update()
    {
        //podeis quitarlo tras comprobar que estos funciona - okey gracias cariño
       // Debug.Log(NivelSospechosos);
        //Debug.Log(Dineros);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    // --- RECOGIDA DE UIMANAGERS ---
    public UIManager GetUI()
    {
        return UIManager;
    }
    public UIManager_Combate GetUIC()
    {
        return UIManagerCombate;
    }
    public UIManagerUpgrades GetUIU()
    {
        return UIManagerUpgrades;
    }

    public void GiveUI(UIManager UIManager)
    {
        this.UIManager = UIManager;
    }
    public void GiveUIC(UIManager_Combate UIManagerCombate)
    {
        this.UIManagerCombate = UIManagerCombate;
    }
    public void GiveUIU(UIManagerUpgrades UIManagerUpgrades)
    {
        this.UIManagerUpgrades = UIManagerUpgrades;
    }
    // --- FIN RECOGIDA DE UIMANAGERS ---

    public void IncreaseResource(SourceName source)
    {
        recursos[(int)source] += 1;
    }

    public void increaseDinero(int reward)
    {
        Dineros += reward;
    }

    public float increaseSospechosos(int i)
    {
        if (NivelSospechosos < 8 && NivelSospechosos >= 0)
        {
            if (NivelSospechosos == 1 && i == -2 || NivelSospechosos == 0 && i == -2)
                NivelSospechosos = 0;
            else
                NivelSospechosos += i;

        }
        return NivelSospechosos;
    }

    // ---CONTADOR DE ENEMIGOS---

    /// <summary>
    /// Reduce el número de enemigos en el contador
    /// </summary>
    public void MataEnemigo(EnemyType enemy)
    {
        numEnemigos[(int)enemy] -= 1;
    }

    public void ResetEnemyCounter()
    {
        numEnemigos[0] = Uvoncio;
        numEnemigos[1] = Manzurria;
        numEnemigos[2] = Grapenade;
        numEnemigos[3] = Manzariete;
    }


    /// <summary>
    /// 0 -> Uvoncio  |  1 -> Manzurria  |  2 -> Grapenade  |  3 -> Manzariete
    /// </summary>
    /// <returns> int[] </returns>
    public int[] GetEnemyCounter()
    {
        return numEnemigos;
    }
    // ---CONTADOR DE ENEMIGOS---

    public static GameManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }
    public void GivePlayer(GameObject player)
    {
        Player = player;
    }

    public GameObject GetPlayer()
    {
        return Player;
    }

    public float[] GetRecursos()
    {
        return recursos;
    } 

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Método que cambia la escena actual por la indicada en el parámetro.
    /// </summary>
    /// <param name="index">Índice de la escena (en el build settings)
    /// que se cargará.</param>
    public void ChangeScene(int index)
    {
        // Antes y después de la carga fuerza la recolección de basura, por eficiencia,
        // dado que se espera que la carga tarde un tiempo, y dado que tenemos al
        // usuario esperando podemos aprovechar para hacer limpieza y ahorrarnos algún
        // tirón en otro momento.
        // De Unity Configuration Tips: Memory, Audio, and Textures
        // https://software.intel.com/en-us/blogs/2015/02/05/fix-memory-audio-texture-issues-in-unity
        //
        // "Since Unity's Auto Garbage Collection is usually only called when the heap is full
        // or there is not a large enough freeblock, consider calling (System.GC..Collect) before
        // and after loading a level (or put it on a timer) or otherwise cleanup at transition times."
        //
        // En realidad... todo esto es algo antiguo por lo que lo mismo ya está resuelto)
        System.GC.Collect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        System.GC.Collect();
    } // ChangeScene

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // De momento no hay nada que inicializar
    }

    private void TransferSceneState()
    {
        // De momento no hay que transferir ningún estado
        // entre escenas
    }

    #endregion
} // class GameManager 
  // namespace