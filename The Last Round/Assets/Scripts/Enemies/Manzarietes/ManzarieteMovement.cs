//---------------------------------------------------------
// Contiene el movimiento del enemigo "Manzariete"
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;
// Añadir aquí el resto de directivas using


/// <summary>
/// Esta class es para el enemigo Manzariete
/// Sirve para el movimiento de este
/// Perseguirá al enemigo hasta un rango, entonces se parará y esperará un tiempo hasta moverse hacia él a gran velocidad
/// </summary>
public class ManzarieteMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    public float RangeAttack;
    public float ChargeTime;
    public float SprintSpeed;
    public float SprintTime;
    // ---- ATRIBUTOS PRIVADOS ----
    private MoveToPlayer moveToplayer;
    private Vector3 EnemyPlayer, LastPlayerPosition;
    private float timer = -1;
    private float Stimer = -1;
    private bool IsCharging = false, IsSprinting = false, InRange = false;
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    private void Start()
    {
        moveToplayer = GetComponent<MoveToPlayer>();
    }
    private void Update()
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
            }
            else IsSprinting = false;
        }
        else
        {
            moveToplayer.Move(gameObject);
        }

        timer -= Time.deltaTime;
        Stimer -= Time.deltaTime;
    }

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----


} // class ManzarieteMovement 
// namespace
