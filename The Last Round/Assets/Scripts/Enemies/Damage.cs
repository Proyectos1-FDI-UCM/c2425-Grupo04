//---------------------------------------------------------
// Script encargado de hacer daño a todo lo que tenga vida siempre que no sea un aliado
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
public class Damage : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private int Basedamage;
    [SerializeField] private float cooldown;
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

    private void Update()
    {
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        Hurt(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hurt(collision.gameObject);
    }

    private void Hurt(GameObject collision)
    {
        bool IsEnemy = collision.GetComponent<CastEnemy>() != null;
        bool ImEnemy = GetComponent<CastEnemy>() != null;
        Health health = collision.GetComponent<Health>();
        //Hago daño si colisiono contra algo que tiene vida
        //Si soy un enemigo y colisiono contra algo que no es un enemigo
        //Si no soy un enemigo y colisiono contra un enemigo
        //Si el cooldown ha pasado

        if (health != null && timer <= 0 &&
            ((!ImEnemy && IsEnemy) || (ImEnemy && !IsEnemy)))
        {
            float mejoraDmg = 0;
            if (GetComponent<MeleeAttack>() != null) //si es el cuerpo a cuerpo
            {
                mejoraDmg = 0.1f * Basedamage * GameManager.Instance.GetUpgradeLevel(1); //mejora un 10% el daño por cada nivel de mejora
            }
            else if (GetComponent<BulletMovement>() != null) //si es la bala
            {
                mejoraDmg = 0.1f * Basedamage * GameManager.Instance.GetUpgradeLevel(0);
            }

            health.GetDamage(Basedamage + mejoraDmg);
            timer = cooldown;
        }
    }

    #endregion   

} // class DamagePlayer 
// namespace
