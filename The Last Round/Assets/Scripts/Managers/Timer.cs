//---------------------------------------------------------
// Maneja el temporizador de la escena de combate
// Víctor Castro Álvarez
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Timer : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] float time = 120f;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] private int NextScene;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private float timer;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    private void Start()
    {
        if (GameManager.Instance.GetCheats())
        {
            time = GameManager.Instance.GetTimerStart();
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (!gameOverUI.activeSelf)
        {
            timer -= Time.deltaTime;
        }
        
        if (timer <= 0)
        {
            //Se inicializa a 1 sin serialize porque se asume que siempre el temporizador baja 1 unidad cada 1 segundo
            timer = 1;
            
            if (time - timer < 0)
            {
                time = 0;
            }
            else
            {
                time -= timer;
            }

            if (time <= 0)
            {
                GameManager.Instance.ChangeScene(NextScene);
                GameManager.Instance.increaseSospechosos(2);
            }
            GameManager.Instance.GiveTimerToUIC(TimerText());
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public string TimerText()
    {
        return ChangeFormat();

    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    private string ChangeFormat()
    {
        int minutos = (int) time / 60;
        int segundos = (int) time % 60;
        string stringTime = "00:00";

        if (segundos < 10)
        {
            stringTime = $"{minutos}:0{segundos}";
        }
        else
        {
            stringTime = $"{minutos}:{segundos}";
        }

        return stringTime;
    }

    #endregion   

} // class Timer 
// namespace
