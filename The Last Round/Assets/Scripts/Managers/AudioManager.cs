//---------------------------------------------------------
// Manager a cargo de las distintas músicas del juego
// Óscar Daniel Fernández Cabana
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

/// <summary>
/// Enum que contiene todos los momentos con distinta música en el juego
/// </summary>
public enum Scenes
{
    Inicio,
    MenuInicio,
    Combate,
    Bartender,
    Mejora,
    Creditos,
    GameOver
}

//Contiene un momento con música y la música
[System.Serializable]
public struct Music
{
    public Scenes Escena;
    public AudioClip SceneMusic;
}

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource SFXSource;

    [SerializeField]
    private Music[] MusicTrack;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    #endregion

    private static AudioManager instance;
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    /// 


    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
        }

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        GameManager.Instance.GiveMusicManager(this);
        //musicSource.clip = GameMusic[0];
        //musicSource.Play();

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        musicSource.volume = GameManager.Instance.GetMusicVolume() / 100;
        SFXSource.volume = GameManager.Instance.GetSfxVolume() / 100;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    public void PlaySceneMusic(Scenes scene)
    {
        int i = 0;
        bool enc = false;

        while (i < MusicTrack.Length && !enc)
        {
            if (MusicTrack[i].Escena == scene)
            {
                enc = true;
            }
            else
            {
                i++;
            }
        }
        if (enc)
        {
            musicSource.clip = MusicTrack[i].SceneMusic;
            musicSource.Play();
        }
    }
    public void PlaySFX(AudioClip sfx)
    {
        if (SFXSource != null)
        {
            SFXSource.clip = sfx;
            SFXSource.Play();
        }
    }
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class MusicManager 
// namespace
