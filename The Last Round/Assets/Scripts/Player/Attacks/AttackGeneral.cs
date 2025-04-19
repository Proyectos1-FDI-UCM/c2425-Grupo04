//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Óscar Daniel Fernández Cabana
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using JetBrains.Annotations;
using UnityEditor.Search;
using UnityEngine;
// Añadir aquí el resto de directivas using
using UnityEngine.InputSystem;


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
    private Transform customCursor;



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
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Vector3 mousePos;
    private Vector2 originRotation;
    private float timer = 0;
    private bool weaponType; //TEMPORAL TRUE = DISPARO      FALSE = MELEE
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

        //Debug.Log(mousePos);
        customCursor.position = new Vector3(mousePos.x, mousePos.y, 0);
        Vector3 rotationOrigin = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotationOrigin.y, rotationOrigin.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0, 0, rotZ - 90);


        if (InputManager.Instance.ChangeWeaponWasPressedThisFrame())
        {
            if (weaponType || !GameManager.Instance.GetBoolUpgrade(0)) weaponType = false;
            else if (!weaponType) weaponType = true;
        }

        GameManager.Instance.WeaponSwitch(weaponType);

        if (InputManager.Instance.FireWasPressedThisFrame() && timer <= 0 &&
            !GameManager.Instance.IsPauseActive())
        {
            if (weaponType) Shoot();
            else Melee();
            timer = AttackCooldown;
        }
        timer -= Time.deltaTime;

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
    }

    private void Melee()
    {
        meleeObject.GetComponent<MeleeAttack>().attack();
    }
    #endregion   

} // class Proyectile 
// namespace
