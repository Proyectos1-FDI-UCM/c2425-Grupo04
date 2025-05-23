//---------------------------------------------------------
// UIManager de la escena de combate
// Víctor Castro Álvarez
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class UIManager_Combate : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private GameObject gameOverUI, PlayerLife;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI population;
    [SerializeField] private TextMeshProUGUI InteractMessage;
    [SerializeField] private Image currentWeapon, DashCharge, InteractMessageImage;
    [SerializeField] private Sprite weaponDistanceImage;
    [SerializeField] private Sprite weaponMeleeImage;
    [SerializeField] private float TimerBeatIntensity;
    [SerializeField] private Image DashFillBar;
    [SerializeField] private float SecondsToStartBeating = 30;
    [SerializeField] private Image Fade;
    [SerializeField] private float FadeSpeed;
    [SerializeField] private Slider InteractMessageSlider;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Health playerHealth;
    private string time;
    private Timer timerScript;
    private int populationNum = 0;
    private bool fewTime = false, fadeIn = false, fadeOut = false;
    private float TimerMaxSize;
    private float DashCooldown, DashCooldownTimer;
    private PlayerDash Playerdash;
    private UnityEngine.Color FadeColor;
    private bool Alcalde = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    private void Awake()
    {
        //Cuando inicia la escena de Combate comprueba si hay enemigos (puede que no)
        GameManager.Instance.GiveUIC(this);
        GameManager.Instance.CompruebaEnemies();
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        if (!GameManager.Instance.GetBoolUpgrade(1))
        {
            DashCharge.gameObject.SetActive(false);
        }

        for (int i = 0; i < GameManager.Instance.GetEnemyCounter().Length; i++)
        {
            populationNum += GameManager.Instance.GetEnemyCounter()[i];
        }
        if (population != null)
        {
            population.text = $"Población: {populationNum}";
        }

        if (timer != null)
        {
            TimerMaxSize = timer.fontSize;
        }

        if (GameManager.Instance.IsFinalPhase())
        {
            if (population != null)
            {
                population.color = UnityEngine.Color.red;
                population.text = $"¡Acaba con todos!";
            }
        }

        FadeColor.r = 0;
        FadeColor.g = 0;
        FadeColor.b = 0;
        FadeColor.a = 0;
    }

    private void Update()
    {
        if (playerHealth == null)
            playerHealth = GameManager.Instance.GetPlayer().GetComponent<Health>();

        if (fewTime && !GameManager.Instance.IsPauseActive())
        {
            timer.fontSize -= TimerBeatIntensity;
            if (timer.fontSize < 0)
            {
                timer.fontSize = 0;
            }
        }

        if (GameManager.Instance.GetPlayer() != null &&
            GameManager.Instance.GetPlayer().GetComponent<PlayerDash>() != null &&
            Playerdash == null)
        {
            Playerdash = GameManager.Instance.GetPlayer().GetComponent<PlayerDash>();
            DashCooldown = Playerdash.GetDashCooldown();
        }

        //Debug.Log(GameManager.Instance.GetBoolUpgrade(1) + " A");
        //Debug.Log((DashFillBar != null) + " B");
        //Debug.Log((Playerdash != null) + " C");
        //Debug.Log((DashCooldown >= 0) + " D");

        if (GameManager.Instance.GetBoolUpgrade(1) && (DashFillBar != null) &&
            (Playerdash != null) && (DashCooldown >= 0) && !GameManager.Instance.IsPauseActive())
        {
            DashCooldownTimer = Playerdash.GetcooldownTimer();
            //Debug.Log(DashFillBar.fillAmount + " PRE");
            //Debug.Log(DashCooldownTimer / DashCooldown);
            DashFillBar.fillAmount = (DashCooldownTimer / DashCooldown);
            //Debug.Log(DashFillBar.fillAmount + " POST");
            //DashFillBar.fillAmount -= (0.1f / DashCooldown) * Time.deltaTime;
        }

        if (GameManager.Instance.IsFinalPhase() && timer != null && timer.gameObject.activeSelf)
        {
            timer.gameObject.SetActive(false);
        }

        if (Fade != null)
        {
            Fade.color = FadeColor / 255;
        }

        if (fadeIn)
        {
            FadeColor.a = Mathf.Clamp(FadeColor.a + Time.deltaTime * FadeSpeed, 0, 255);

            if (Fade.color.a == 1)
            {
                fadeIn = false;
                if (Alcalde)
                {
                    GameManager.Instance.SpawnAlcalde();
                }
                fadeOut = true;
            }
        }
        if (fadeOut)
        {
            FadeColor.a = Mathf.Clamp(FadeColor.a - Time.deltaTime * FadeSpeed, 0, 255);

            if (Fade.color.a == 0)
            {
                fadeOut = false;
                Fade.gameObject.SetActive(false);
            }
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void GameOverUI()
    {
        //Activa la UI de GameOver si el jugador pierde
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        if (timer != null)
        {
            timer.gameObject.SetActive(false);
        }
        PlayerLife.SetActive(false);
        currentWeapon.gameObject.SetActive(false);
        population.text = "";

        Time.timeScale = 0f;
    }

    public bool IsGameOver()
    {
        return gameOverUI.activeSelf;
    }

    public void Timer(string time, float timeNum)
    {
        if (timeNum <= SecondsToStartBeating)
        {
            timer.color = UnityEngine.Color.red;
            fewTime = true;
            AudioManager.Instance.ChangePitchMusic(1.1f);
        }

        if (fewTime)
        {
            timer.fontSize = TimerMaxSize;
        }
        timer.text = time;
    }

    public void SwitchWeaponDisplay(bool weaponPlayer)
    {
        if (weaponPlayer)
        {
            currentWeapon.sprite = weaponDistanceImage;
        }
        else
        {
            currentWeapon.sprite = weaponMeleeImage;
        }
    }

    public void ChangePopulation()
    {
        populationNum--;

        if (population != null)
        {
            if (GameManager.Instance.IsFinalPhase())
            {
                population.color = UnityEngine.Color.red;
                population.text = $"¡Acaba con todos!";
            }
            else
            {
                population.text = $"Población: {populationNum}";
            }
        }

    }

    public void PressE()
    {
        InteractMessage.text = "PRESIONE [E] /";

        if (InteractMessageImage != null)
        {
            InteractMessageImage.gameObject.SetActive(true);
        }
    }
    public void HoldE(float timer, float holdingTime)
    {
        InteractMessage.text = $"MANTÉN  [E]  /";

        if (InteractMessageImage != null)
        {
            InteractMessageImage.gameObject.SetActive(true);
        }
        if (InteractMessageSlider != null)
        {
            InteractMessageSlider.gameObject.SetActive(true);
            InteractMessageSlider.value = 1 - (timer / holdingTime);
        }
    }
    public void ClearMessage()
    {
        InteractMessage.text = "";

        if (InteractMessageImage != null)
        {
            InteractMessageImage.gameObject.SetActive(false);
        }
        if (InteractMessageSlider != null)
        {
            InteractMessageSlider.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Usado para la escena final, el método deshabilita toda la interfaz innecesaria como habilidades, vida y contador
    /// </summary>
    public void DisableUI()
    {
        PlayerLife.gameObject.SetActive(false);
        currentWeapon.transform.parent.gameObject.SetActive(false);
        DashCharge.gameObject.SetActive(false);
        DashFillBar.gameObject.SetActive(false);
        population.gameObject.SetActive(false);
    }

    /// <summary>
    /// Inicia un FadeIn - FadeOut en la pantalla de combate
    /// </summary>
    public void StartFade(bool Alcalde)
    {
        fadeIn = true;
        this.Alcalde = Alcalde;
        Fade.gameObject.SetActive(true);
    }

    /// <summary>
    /// Devuelve si está haciendo el fadeOut por si se quiere tomar alguna opción justo al empezar a hacer fadeOut
    /// Es decir entre el FadeIn y el FadeOut, ya sea instanciar cualquier objeto o cambiar algo en la escena.
    /// </summary>
    /// <returns></returns>
    public bool IsFadingOut()
    {
        return fadeOut;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class UIManager_Combate 
// namespace
