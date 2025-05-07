//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Óscar Daniel Fernández Cabana
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------



using UnityEngine;
// Añadir aquí el resto de directivas using
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class AttackGeneral : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField]
    private float AttackCooldown;

    [SerializeField]
    private Button detector;

    [SerializeField]
    private Transform pivot; //centro de rotación
    [SerializeField]
    private Transform origin; //Desde donde se instancia la bala

    [SerializeField]
    private Camera cameraMain;



    [SerializeField]
    private BulletMovement bulletPrefab;

    [SerializeField]
    private GameObject meleeObject;

    [SerializeField] AudioClip MeleeSFX;
    [SerializeField] AudioClip DistanceThrowSFX;
    [SerializeField] AudioClip WeaponSwitch;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Vector3 mousePos;
    private Vector2 originRotation;
    private float timer = 0;
    private bool weaponType; //TEMPORAL TRUE = DISPARO      FALSE = MELEE
    private bool UsingJoystick = false;
    private Vector2 LastMousePosition;
    private float timerCanRotate = 0;
    private float randomPitch;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //customCursor.position = new Vector2 (Mouse.current.position.x.value /mouseReduction , Mouse.current.position.y.value/mouseReduction);
        mousePos = cameraMain.ScreenToWorldPoint(new Vector3(Mouse.current.position.x.value, Mouse.current.position.y.value, 0));
        Vector2 mousePosScreen = new Vector2(Mouse.current.position.x.value, Mouse.current.position.y.value);

        if (!UsingJoystick && InputManager.Instance.AimVector != Vector2.zero)
        {
            UsingJoystick = true;
        }
        else if (UsingJoystick && mousePosScreen != LastMousePosition)
        {
            UsingJoystick = false;
        }
        LastMousePosition = mousePosScreen;

        Vector3 rotationOrigin;
        if (!UsingJoystick)
        {
            rotationOrigin = mousePos - transform.position;
        }
        else
        {
            rotationOrigin = InputManager.Instance.AimVector;
        }

        if (timerCanRotate <= 0)
        {
            float rotZ = Mathf.Atan2(rotationOrigin.y, rotationOrigin.x) * Mathf.Rad2Deg;
            pivot.transform.rotation = Quaternion.Euler(0, 0, rotZ - 90);
        }

        if (InputManager.Instance.ChangeWeaponWasPressedThisFrame() &&
            !GameManager.Instance.IsPauseActive())
        {
            if (weaponType)
            {
                weaponType = false;
                AudioManager.Instance.PlaySFX(WeaponSwitch);
            }
            else if (!weaponType && GameManager.Instance.GetBoolUpgrade(0))
            {
                weaponType = true;
                AudioManager.Instance.PlaySFX(WeaponSwitch);
            }
        }

        if (GameManager.Instance.GetUIC() != null)
        {
            GameManager.Instance.GetUIC().SwitchWeaponDisplay(weaponType);
        }

        bool CanFire = InputManager.Instance.FireWasPressedThisFrame() &&
                       timer <= 0 &&
                       !GameManager.Instance.IsPauseActive();

        if (detector != null && detector.gameObject.activeSelf && detector.GetComponent<CursorDetector>() != null)
        {
            CanFire = CanFire && !detector.GetComponent<CursorDetector>().IsMouseOnButton();
        }

        if (CanFire)
        {
            if (weaponType) Shoot();
            else Melee();
            timer = AttackCooldown;
        }

        if (!GameManager.Instance.IsPauseActive())
        {
            timer -= Time.deltaTime;
        }

        if (timerCanRotate >= 0)
        {
            timerCanRotate -= Time.deltaTime;
        }
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
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    private void Shoot()
    {
        Instantiate(bulletPrefab, origin.transform.position, pivot.transform.rotation);
        randomPitch = Random.Range(0.8f, 1.2f);
        randomPitch = Mathf.Round(randomPitch * 100) / 100; //Para que tenga 2 decimales
        AudioManager.Instance.ChangePitchSFX(randomPitch);
        AudioManager.Instance.PlaySFX(DistanceThrowSFX);
        StartCoroutine(RestorePitch(0.2f));
    }

    private void Melee()
    {
        timerCanRotate = meleeObject.GetComponent<MeleeAttack>().GetDuration();
        Debug.Log(timerCanRotate);
        meleeObject.GetComponent<MeleeAttack>().attack();
        randomPitch = Random.Range(0.8f, 1.2f);
        randomPitch = Mathf.Round(randomPitch * 100) / 100; //Para que tenga 2 decimales
        AudioManager.Instance.ChangePitchSFX(randomPitch);
        AudioManager.Instance.PlaySFX(MeleeSFX);
        StartCoroutine(RestorePitch(0.2f));
    }

    private IEnumerator RestorePitch(float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.ChangePitchSFX(1);
    }
    #endregion   

} // class Proyectile 
// namespace
