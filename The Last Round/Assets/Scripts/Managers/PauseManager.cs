//---------------------------------------------------------
// Se encarga de manejar los menus de pausa
// Aryan Guerrero Iruela
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PauseManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField]
    private TextMeshProUGUI PauseTitle;
    [SerializeField]
    private Slider musicSlider, sfxSlider;
    [SerializeField]
    private GameObject PauseMenu, SubMenu;
    [SerializeField]
    private Button MenuButton;
    [SerializeField]
    private AudioClip woodSfx, paperSfx;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private Button SelectButton;
    private GameObject LastButtonInUse;
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
        GameManager.Instance.SetPauseMenu(PauseMenu);
        //Pone el valor de los sliders al volumen que está en el gameManager
        if (musicSlider != null)
        {
            musicSlider.value = GameManager.Instance.GetMusicVolume();
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = GameManager.Instance.GetSfxVolume();
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        if (InputManager.Instance.PauseWasPressedThisFrame())
        {
            if (SubMenu != null && SubMenu.activeSelf)
            {
                ClosePauseMenu(SubMenu);
            }
            else if (PauseMenu != null && PauseMenu.activeSelf)
            {
                ClosePauseMenu(PauseMenu);
            }
            else if (PauseMenu != null)
            {
                SetSelectButton(MenuButton);
                OpenPauseMenu(PauseMenu);
                Time.timeScale = 0f;
            }
        }
        MusicVolume();
        SfxVolume();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    public void SetSelectButton(Button SelectButton)
    {
        this.SelectButton = SelectButton;
    }
    //Se abre el menu que se asigna desde el boton que llama al metodo
    public void OpenPauseMenu(GameObject menuToOpen)
    {
        PlaySoundEffect(woodSfx);
        if (menuToOpen != null)
        {
            menuToOpen.SetActive(true);
        }
        if (SelectButton != null)
        {
            if (menuToOpen == PauseMenu)
            {
                //Guarda el último botón seleccionado antes de activar la pausa (solo la pausa)
                LastButtonInUse = EventSystem.current.currentSelectedGameObject;
                //Debug.Log(LastButtonInUse.name);
            }
            EventSystem.current.SetSelectedGameObject(SelectButton.gameObject);
        }
    }

    //Se cierra el menu que se asigna desde el boton que llama al metodo y si es el de pausa pone el tiempo en marcha
    public void ClosePauseMenu(GameObject menuToClose)
    {
        //PlaySoundEffect(paperSfx);
        if (menuToClose == PauseMenu)
        {
            Time.timeScale = 1f;
            //Si había un botón seleccionado antes de activar la pausa y se va a cerrar el menú, se selecciona de vuelta dicho botón
            if (LastButtonInUse != null)
            {
                EventSystem.current.SetSelectedGameObject(LastButtonInUse.gameObject);
            }
        }
        if (menuToClose != null)
        {
            menuToClose.SetActive(false);
        } 
    }

    //Le da el volumen al gameManager
    public void MusicVolume()
    {
        //AudioManager.Instance.PlaySFX(woodSfx);
        if (musicSlider != null)
        {
            GameManager.Instance.SetMusicVolume(musicSlider.value);
        }
    }

    public void SfxVolume()
    {
        //AudioManager.Instance.PlaySFX(woodSfx);
        if (sfxSlider != null)
        {
            GameManager.Instance.SetSfxVolume(sfxSlider.value);
        }
    }

    public void PlaySoundEffect(AudioClip sfx)
    {
        AudioManager.Instance.PlaySFX(sfx);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class PauseManager 
// namespace
