//---------------------------------------------------------
// Contiene el movimiento del enemigo "Manzariete"
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEditor.Experimental.GraphView;
using UnityEngine.PlayerLoop;

/// <summary>
/// Esta class es para el enemigo Manzariete
/// Sirve para el movimiento de este
/// Perseguirá al enemigo hasta un rango, entonces se parará y esperará un tiempo hasta moverse hacia él a gran velocidad
/// </summary>
public class ManzarieteMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    public float RangeAttack;
    public float ChargeTime;
    public float SprintSpeed;
    public float SprintTime;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private MoveToPlayer moveToplayer;
    private Vector3 EnemyPlayer, LastPlayerPosition;
    private float timer = -1;
    private float Stimer = -1, tmp;
    private bool IsCharging = false, IsSprinting = false, InRange = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        moveToplayer = GetComponent<MoveToPlayer>();
        tmp = SprintSpeed;
    }
    private void FixedUpdate()
    {
        EnemyPlayer = moveToplayer.UpdateVector(gameObject);


        InRange = EnemyPlayer.magnitude <= RangeAttack;
        
        if (InRange && !IsCharging && !IsSprinting)
        {
            IsCharging = true;
            timer = ChargeTime;
        }
        else if (IsCharging)
        {
            if (timer <= 0)
            {
                LastPlayerPosition = EnemyPlayer / Mathf.Sqrt(EnemyPlayer.x * EnemyPlayer.x +
                                  EnemyPlayer.y * EnemyPlayer.y);
                IsCharging = false;
                IsSprinting = true;
                Stimer = SprintTime;
            }
        }
        else if (IsSprinting)
        {
            

            if (Stimer > 0)
            {
                transform.position += LastPlayerPosition * SprintSpeed * Time.deltaTime;

                //Frenado final
                if (Stimer < SprintTime/3.5f && SprintSpeed > tmp/20)
                {
                    SprintSpeed -= tmp/20;
                }
            }
            else
            {
                IsSprinting = false;
                SprintSpeed = tmp;
            }
        }
        else
        {
            moveToplayer.Move(gameObject);
        }

        timer -= Time.deltaTime;
        Stimer -= Time.deltaTime;
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

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion

} // class ManzarieteMovement 
// namespace
