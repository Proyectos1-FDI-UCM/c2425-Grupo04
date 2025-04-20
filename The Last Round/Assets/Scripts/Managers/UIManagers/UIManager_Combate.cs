//---------------------------------------------------------
// UIManager de la escena de combate
// Víctor Castro Álvarez
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using TMPro;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


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
    [SerializeField] private Image currentWeapon;
    [SerializeField] private Sprite weaponDistanceImage;
    [SerializeField] private Sprite weaponMeleeImage;
    [SerializeField] private float TimerBeatIntensity;
    
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Health playerHealth;
    private string time;
    private Timer timerScript;
    private int populationNum = 0;
    private bool fewTime = false;
    private float TimerMaxSize;
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
        GameManager.Instance.GiveUIC(this);

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
    }

    private void Update()
    {
        if (playerHealth == null)
            playerHealth = GameManager.Instance.GetPlayer().GetComponent<Health>();

        if (fewTime)
        {
            timer.fontSize -= TimerBeatIntensity;
            if (timer.fontSize < 0)
            {
                timer.fontSize = 0;
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

    public void Timer(string time)
    {
        if (time == "0:30")
        {
            timer.color = Color.red;
            fewTime = true;
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
        population.text = $"Población: {populationNum}";
    }

    public void PressE()
    {
        InteractMessage.text = "PRESIONE [E]";
    }
    public void HoldE(float timer, float holdingTime)
    {
        InteractMessage.text = $"MANTÉN [E]  {Mathf.Round((holdingTime-timer)*100)/100} / {holdingTime}";
    }
    public void ClearMessage()
    {
        InteractMessage.text = "";
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
