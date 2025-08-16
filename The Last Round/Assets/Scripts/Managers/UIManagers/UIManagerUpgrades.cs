//---------------------------------------------------------
// Se encarga de manejar todos los elementos UI de la escena de mejoras
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
//using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;

public class UIManagerUpgrades : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField]
    private TextMeshProUGUI dineroTotalText;
    [SerializeField]
    private float HealthUpgradePercent,
                  MeleeDamageUpgradePercent,
                  RangeDamageUpgradePercent,
                  BaseHealth = 0,
                  BaseMeleeDamage = 0,
                  BaseRangeDamage = 0;
    [SerializeField]
    private AudioClip upgradeSfx, NoUpgradeSfx;

    [SerializeField]
    private Button makeDrinkButton;

    [SerializeField]
    private TextMeshProUGUI drinkText;

    [SerializeField]
    private TextMeshProUGUI drinksAmount;

    [SerializeField]
    private int costeCuración;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private float[] recursos;
    private CastUpgrade[] upgrades;
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
        GameManager.Instance.GiveUIU(this);

        dineroTotalText.text = GameManager.Instance.GetDineros().ToString();

        //Darle al GameManager los porcentajes de mejora
        GameManager.Instance.SetMeleeDamagePercent(MeleeDamageUpgradePercent / 100);
        GameManager.Instance.SetRangeDamagePercent(RangeDamageUpgradePercent / 100);
        GameManager.Instance.SetHealthPercent(HealthUpgradePercent / 100);

        //Buscamos las mejoras disponibles
        upgrades = FindObjectsOfType<CastUpgrade>();

        // Tomamos los recuros del Gamemanager para la fabricación de bebidas curativas
        recursos = GameManager.Instance.GetRecursos();


        //Pone el precio
        //Pone el nombre

        for (int j = 0; j < upgrades.Length; j++)
        {
            if (upgrades[j].GetUpgradeType() == UpgradeType.Bool)
            {
                CastBoolUpgrade boolUpgrade = upgrades[j].gameObject.GetComponent<CastBoolUpgrade>();

                if (boolUpgrade != null)
                {
                    boolUpgrade.GetUpgradePriceText().text = $"x{boolUpgrade.GetUpgradePrice()}";
                    boolUpgrade.GetUpgradeText().text = boolUpgrade.GetUpgradeType().ToString().Replace('_', ' ');



                    boolUpgrade.gameObject.GetComponent<Button>().interactable = !GameManager.Instance.GetBoolUpgrade((int)boolUpgrade.GetUpgradeType());
                    boolUpgrade.GetUpgradeCoinImage().gameObject.SetActive(!GameManager.Instance.GetBoolUpgrade((int)boolUpgrade.GetUpgradeType()));

                    if (GameManager.Instance.GetBoolUpgrade((int)boolUpgrade.GetUpgradeType()))
                    {
                        boolUpgrade.GetUpgradeDescriptionText().text = "Ya adquirida";
                        boolUpgrade.GetUpgradePriceText().text = "";
                    }
                    else
                    {
                        boolUpgrade.GetUpgradeDescriptionText().text = $"Desbloquea poder usar el {boolUpgrade.GetUpgradeText().text}";
                    }

                }
            }
            else
            {
                CastIntUpgrade intUpgrade = upgrades[j].gameObject.GetComponent<CastIntUpgrade>();

                if (intUpgrade != null)
                {
                    intUpgrade.GetUpgradePriceText().text = $"x{intUpgrade.GetUpgradePrice()}";
                    intUpgrade.GetUpgradeText().text = intUpgrade.GetUpgradeType().ToString().Replace('_', ' ');
                    ChangeDesc(intUpgrade,
                               GameManager.Instance.GetUpgradeLevel((int)intUpgrade.GetUpgradeType()),
                               (int)intUpgrade.GetUpgradeType());

                    if (intUpgrade.GetUpgradeType() == IntUpgradeType.Daño_a_distancia)
                    {
                        intUpgrade.gameObject.GetComponent<Button>().interactable = GameManager.Instance.GetBoolUpgrade((int)BoolUpgradeType.Ataque_a_distancia);
                        intUpgrade.GetUpgradeCoinImage().gameObject.SetActive(GameManager.Instance.GetBoolUpgrade((int)BoolUpgradeType.Ataque_a_distancia));

                        if (!GameManager.Instance.GetBoolUpgrade((int)BoolUpgradeType.Ataque_a_distancia))
                        {
                            intUpgrade.GetUpgradeDescriptionText().text = "Se necesita el arma a distancia para desbloquear";
                            intUpgrade.GetUpgradeText().text = "Bloqueada";
                            intUpgrade.GetUpgradePriceText().text = "";
                        }
                    }
                }
            }
        }

        int i = 0;
        bool notEnoughtMaterials = false;
        while (notEnoughtMaterials && i < recursos.Length)
        {
            if (recursos[i] < costeCuración)
            {
                notEnoughtMaterials = true;
            }
            i++;
        }

        if (notEnoughtMaterials)
        {
            makeDrinkButton.enabled = false;
            drinkText.text = "Insuficientes materiales";
        }
        else
        {
            makeDrinkButton.enabled = true;
            drinkText.text = "Hacer bebida";
        }
        drinksAmount.text = $"Necesarios x3 de cada material\nTienes: x{GameManager.Instance.GetHealDrinksNum()}";

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public void UpgradeNormal(CastIntUpgrade upgrade)
    {
        if (GameManager.Instance.GetDineros() >= upgrade.GetUpgradePrice())
        {
            AudioManager.Instance.PlaySFX(upgradeSfx);
            GameManager.Instance.DecreaseDinero(upgrade.GetUpgradePrice()); //Quita el coste del dinero total
            dineroTotalText.text = GameManager.Instance.GetDineros().ToString();
            GameManager.Instance.IncreaseUpgradeLevel((int)upgrade.GetUpgradeType()); //Sube el nivel de la mejora
            ChangeDesc(upgrade, GameManager.Instance.GetUpgradeLevel((int)upgrade.GetUpgradeType()), (int)upgrade.GetUpgradeType()); //Cambia su descripcion
            
        }
        else
        {
            AudioManager.Instance.PlaySFX(NoUpgradeSfx);
        }
    }

    public void UpgradeUnico(CastBoolUpgrade upgrade) //Solo para dash y arma a distancia
    {

        if (GameManager.Instance.GetDineros() >= upgrade.GetUpgradePrice())
        {
            AudioManager.Instance.PlaySFX(upgradeSfx);
            GameManager.Instance.BoolUpgrade((int)upgrade.GetUpgradeType());
            GameManager.Instance.DecreaseDinero(upgrade.GetUpgradePrice());
            dineroTotalText.text = GameManager.Instance.GetDineros().ToString();

            upgrade.gameObject.GetComponent<Button>().interactable = false; //desactiva el boton y cambia su descripcion

            upgrade.GetUpgradeDescriptionText().text = "Ya adquirida";
            upgrade.GetUpgradePriceText().text = "";
            upgrade.GetUpgradeCoinImage().gameObject.SetActive(false);

            if (upgrade.GetUpgradeType() == BoolUpgradeType.Ataque_a_distancia) //si es el arma a distancia
            {
                //Encontrar mejora
                CastIntUpgrade[] Intupgrades = FindObjectsOfType<CastIntUpgrade>();
                int j = 0;
                bool enc = false;

                while (j < upgrades.Length && !enc)
                {
                    if (Intupgrades[j].GetUpgradeType() == IntUpgradeType.Daño_a_distancia) enc = true;
                    else j++;
                }

                if (enc)
                {
                    Intupgrades[j].gameObject.GetComponent<Button>().interactable = true;
                    Intupgrades[j].GetUpgradePriceText().text = $"x{Intupgrades[j].GetUpgradePrice()}";
                    ChangeDesc(Intupgrades[j], GameManager.Instance.GetUpgradeLevel((int)Intupgrades[j].GetUpgradeType()), (int)Intupgrades[j].GetUpgradeType());
                    Intupgrades[j].GetUpgradeText().text = Intupgrades[j].GetUpgradeType().ToString().Replace('_', ' ');
                    Intupgrades[j].GetUpgradeCoinImage().gameObject.SetActive(true);
                }
            }
        }
        else
        {
            AudioManager.Instance.PlaySFX(NoUpgradeSfx);
        }
    }

    public void CraftHealingDrink()
    {
        bool notEnoughtMaterials = false;
        int i = 0;

        while (!notEnoughtMaterials && i < recursos.Length)
        {
            if (recursos[i] - costeCuración < 0) notEnoughtMaterials = true;
            i++;
        }

        if (!notEnoughtMaterials)
        {
            for (int j = 0; j < recursos.Length; j++)
            {
                recursos[j] -= costeCuración; //SUSTITUIR VARIABLES CON PRECIO
            }

            GameManager.Instance.IncreaseHealDrinksNum(1);
           // Debug.Log(GameManager.Instance.GetHealDrinksNum());
        }
        else
        {
            makeDrinkButton.enabled = false;
            drinkText.text = "Insuficientes materiales";
        }

        drinksAmount.text = $"Necesarios x{costeCuración} de cada material\nTienes: x{GameManager.Instance.GetHealDrinksNum()}";
    }


    public void ActiveImage(GameObject image)
    {
        image.SetActive(true);
    }
    public void DesactiveImage(GameObject image)
    {
        image.SetActive(false);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void ChangeDesc(CastIntUpgrade textBox, int level, int element)
    {
        string upgrade = "";
        float amount = 0;
        float NextAmount = 0;

        if (element == (int)IntUpgradeType.Daño_a_melee || element == (int)IntUpgradeType.Daño_a_distancia)
        {
            upgrade = "Daño";

            if (element == (int)IntUpgradeType.Daño_a_distancia)
            {
                amount = BaseRangeDamage + (int)((RangeDamageUpgradePercent / 100) * BaseRangeDamage * GameManager.Instance.GetUpgradeLevel((int)IntUpgradeType.Daño_a_distancia));

                NextAmount = BaseRangeDamage + (int)((RangeDamageUpgradePercent / 100) * BaseRangeDamage * (GameManager.Instance.GetUpgradeLevel((int)IntUpgradeType.Daño_a_distancia) + 1));
            }
            else
            {
                amount = BaseMeleeDamage + (int)((MeleeDamageUpgradePercent / 100) * BaseMeleeDamage * GameManager.Instance.GetUpgradeLevel((int)IntUpgradeType.Daño_a_melee));

                NextAmount = BaseMeleeDamage + (int)((MeleeDamageUpgradePercent / 100) * BaseMeleeDamage * (GameManager.Instance.GetUpgradeLevel((int)IntUpgradeType.Daño_a_melee) + 1));
            }
        }
        else if (element == (int)IntUpgradeType.Vida)
        {
            upgrade = "Vida";

            amount = BaseHealth + (int)((HealthUpgradePercent / 100) * BaseHealth * GameManager.Instance.GetUpgradeLevel((int)IntUpgradeType.Vida));

            NextAmount = BaseHealth + (int)((HealthUpgradePercent / 100) * BaseHealth * (GameManager.Instance.GetUpgradeLevel((int)IntUpgradeType.Vida) + 1));
        }


        textBox.GetUpgradeDescriptionText().text = $"{textBox.GetUpgradeDescription()}\n" +
                                                   $"ACTUAL:\n" +
                                                   $"   {upgrade}: {amount} puntos\n" +
                                                   $"SIGUIENTE:\n" +
                                                   $"   Nivel: {level + 1}\n" +
                                                   $"   {upgrade}: {NextAmount} puntos\n";
    }
    #endregion

} // class UIManagerUpgrades 
// namespace
