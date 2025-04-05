//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;

/// <summary>
/// Contiene la cantidad total de un enemigo en una partida
/// </summary>
[System.Serializable]
struct NumEnemy
{
    public EnemyType Enemy;
    public int amount;
}
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
    [SerializeField] private int SourceTypes;

    [Header("Número de enemigos")]
    [SerializeField] private NumEnemy[] Enemies;

    [Header("Límites")]
    [SerializeField] private float MapWidth;
    [SerializeField] private float MapHeight;

    [Header("Clientes")]
    [SerializeField] private GameObject[] Clientes;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private ScenesManager ScenesManager;
    private UIManager UIManager;
    private UIManager_Combate UIManagerCombate;
    private UIManagerUpgrades UIManagerUpgrades;
    private static GameManager _instance;
    private GameObject Player;
    private float[] recursos;
    private int[] numEnemigos;
    private float NivelSospechosos = 0;
    private float Dineros = 0;
    private bool[,] DialoguesSaid;
    private float musicVolume = 100f, sfxVolume = 100f;
    private int[] upgradeLevel = new int[4]; //0 es daño a distancia, 1 es melee, 2 es vida, 3 es descuento
    private bool[] upgradeBool = new bool[2]; //0 es arma a distancia, 1 es dash
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

        numEnemigos = new int[Enemies.Length];
        ResetEnemyCounter();
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
        //Para ocupar lo necesario para que todos los dialogos quepan dentro del array
        //Se ve quien tiene más dialgos y se hace de ese tamaño
        int MaxDialogues = 0;
        for (int i = 0; i < Clientes.Length; i++)
        {
            if (Clientes[i].GetComponent<Dialogue>().GetDialogues().Length > MaxDialogues)
            {
                MaxDialogues = Clientes[i].GetComponent<Dialogue>().GetDialogues().Length;
            }
        }

        DialoguesSaid = new bool[Clientes.Length, MaxDialogues];
        recursos = new float[SourceTypes];
    }

    private void Update()
    {
        //podeis quitarlo tras comprobar que estos funciona - okey gracias cariño
        // Debug.Log(NivelSospechosos);
        //Debug.Log(Dineros);

        if (NivelSospechosos >= 8&&UIManager!=null)
        {
            GameManager.Instance.GetUI().GetComponent<UIManager>().GameOverUI();
        }
    }
    
    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    // --- RECOGIDA DE UIMANAGERS ---

    public ScenesManager GetScenesManager()
    {
        return ScenesManager;
    }
    public UIManager GetUI()
    {
        return UIManager;
    }

    public void GiveTimerToUIC(string time)
    {
        UIManagerCombate.Timer(time);
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

    public void GiveScenesManager(ScenesManager scenesManager)
    {
        this.ScenesManager = scenesManager;
    }
    // --- FIN RECOGIDA DE UIMANAGERS ---

    // --- GESTIÓN DE RECURSOS ---
    public void IncreaseResource(SourceName source)
    {
        recursos[(int)source] += 1;
    }

    public float[] GetRecursos()
    {
        return recursos;
    }
    // --- FIN GESTIÓN DE RECURSOS ---

    // --- GESTIÓN ECONÓMICA ---
    public void increaseDinero(int reward)
    {
        Dineros += reward;
    }

    public void DecreaseDinero(int cost) //Resta una cantidad al dinero total
    {
        if (cost > Dineros) Dineros = 0;
        else Dineros -= cost;
    }

    public float GetDineros()
    {
        return Dineros;
    }
    // --- FIN GESTIÓN ECONÓMICA ---

    // --- SISTEMA DE SOSPECHA ---
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
    // --- FIN SISTEMA DE SOSPECHA ---

    // --- LÍMITES MAPA ---
    public float GetMapHeight()
    {
        return MapHeight;
    }

    public float GetMapWidth()
    {
        return MapWidth;
    }
    // --- FIN LÍMITES MAPA ---

    // --- SISTEMA DE MEJORAS ---
    public int GetUpgradeLevel(int element) //Devuelve el nivel de la mejora correspondiente
    {
        return upgradeLevel[element];
    }

    public bool GetBoolUpgrade(int element) //Devuelve si esa mejora (dash o arma a distancia) esta adquirida
    {
        return upgradeBool[element];
    }

    public void IncreaseUpgradeLevel(int element) //Sube de nivel a la mejora
    {
        upgradeLevel[element]++;
    }

    public void BoolUpgrade(int element) //Adquiere esa mejora
    {
        upgradeBool[element] = true;
    }
    // --- FIN SISTEMA DE MEJORAS ---

    // ---CONTADOR DE ENEMIGOS---
    /// <summary>
    /// Reduce el número de enemigos en el contador y refresca las colisiones
    /// </summary>
    public void MataEnemigo(EnemyType enemy)
    {
        if (numEnemigos[(int)enemy] - 1 >= 0)
        {
            numEnemigos[(int)enemy]--;
        }

        if (numEnemigos[0] + numEnemigos[1] + numEnemigos[2] + numEnemigos[3] <= 0)
        {
            ScenesManager.CreditScenes();
        }
    }

    public void ResetEnemyCounter()
    {
        //Rellena el array numEnemigos en el orden del enum EnemyType con el número de ese enemigo en partida
        for (int i = 0; i < Enemies.Length; i++)
        {
            numEnemigos[(int)Enemies[i].Enemy] = Enemies[i].amount;
        }
    }

    /// <summary>
    /// 0 -> Uvoncio  |  1 -> Manzurria  |  2 -> Grapenade  |  3 -> Manzariete
    /// </summary>
    /// <returns> int[] </returns>
    public int[] GetEnemyCounter()
    {
        return numEnemigos;
    }
    // --- FIN CONTADOR DE ENEMIGOS ---

    // --- GESTIÓN DE DIÁLOGOS
    /// <summary>
    /// Toma como argumento el cliente y el número de dialogo y pone su DialogueSaid en true
    /// </summary>
    /// <param name="client"></param>
    /// <param name="dialogue"></param>
    public void SetSaid(int client, int dialogue)
    {
        DialoguesSaid[client, dialogue] = true;
    }

    /// <summary>
    /// Devuelve si se ha dicho o no el dialogo del cliente que se le pasa como argumento
    /// </summary>
    /// <param name="client"></param>
    /// <param name="dialogue"></param>
    /// <returns></returns>
    public bool HasSaid(int client, int dialogue)
    {
        bool tmp = DialoguesSaid[client, dialogue];
        return tmp;
    }
    // --- FIN GESTIÓN DIÁLOGOS
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

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = value;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSfxVolume()
    {
        return sfxVolume;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // De momento no hay nada que inicializar

        //Inicializacion de las mejoras
        upgradeBool[0] = false;
        upgradeBool[1] = false;
        upgradeLevel[0] = 0;
        upgradeLevel[1] = 0;
        upgradeLevel[2] = 0;
        upgradeLevel[3] = 0;
    }

    private void TransferSceneState()
    {
        // De momento no hay que transferir ningún estado
        // entre escenas
    }

    #endregion
} // class GameManager 
  // namespace