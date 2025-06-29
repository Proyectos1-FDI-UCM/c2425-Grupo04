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
    private Button[] buttons = new Button[6];
    [SerializeField]
    private TextMeshProUGUI[] descs = new TextMeshProUGUI[6];
    [SerializeField]
    private TextMeshProUGUI[] costBox = new TextMeshProUGUI[6];
    [SerializeField]
    private string[] descripciones = new string[4]; //sin distanceWpn ni dash
    //0 es distanceDmgCostBox
    //1 es meleeCostBox
    //2 es healthCostBox
    //3 es descuentos
    //4 es distanceWpn
    //5 es dash
    [SerializeField]
    private int costeUnicos, costeNormales; //Coste unicos son los que solo se pueden comprar una vez, coste normales son los que se compran mas de una vez
    [SerializeField]
    private Image[] coinImg = new Image[3]; //0 es arma distancia, 1 es dash, 2 es daño distancia
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
    private float dineroTotal;
    private float[] recuros;
    private enum desbloqueables {}
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

        //GameManager.Instance.increaseDinero(100); //Solo para testear

        // Tomamos los recuros del Gamemanager para la fabricación de bebidas curativas

        recuros = GameManager.Instance.GetRecursos();

        //Pone el precio
        costBox[1].text = "x" + costeNormales;
        costBox[2].text = "x" + costeNormales;
        costBox[3].text = "x" + costeNormales;

        //Darle al GameManager los porcentajes de mejora
        //GameManager.Instance.SetHealthPercent(HealthUpgradePercent / 100);
        GameManager.Instance.SetMeleeDamagePercent(MeleeDamageUpgradePercent / 100);
        GameManager.Instance.SetRangeDamagePercent(RangeDamageUpgradePercent / 100);

        //Cambia la descripcion de las mejoras normales (Vida y Melee y Sale)
        ChangeDesc(descs[1], GameManager.Instance.GetUpgradeLevel(1), 1);
        ChangeDesc(descs[2], GameManager.Instance.GetUpgradeLevel(2), 2);
        ChangeDesc(descs[3], GameManager.Instance.GetUpgradeLevel(3), 3);

        if (GameManager.Instance.GetBoolUpgrade(0))//Si el Arma a Distancia esta adquirida
        {
            ChangeDesc(descs[0], GameManager.Instance.GetUpgradeLevel(0), 0); //Cambia la descripcion de la mejora de Daño a distancia
            buttons[3].interactable = false; //Desactiva el boton de la mejora de Arma a distancia
            costBox[3].text = "Ya adquirida"; //Cambia el texto del coste y la descripcion del arma a distancia
            descs[3].text = "Ya adquirida";
            coinImg[0].enabled = false;
            coinImg[2].enabled = true;
        }
        else
        {
            buttons[0].interactable = false; //Desactiva el boton de mejora de Daño a distancia
            descs[0].text = "Se necesita el Arma a distancia para desbloquear"; //Cambia el texto del coste y la descripcion del Daño a distancia
            costBox[0].text = "Bloqueada";
            //Cambia el texto del coste y la descripcion del arma a distancia
            costBox[4].text = "x" + costeUnicos;
            descs[4].text = "Desbloquea poder usar el Arma a distancia";
            coinImg[0].enabled = true;
            coinImg[2].enabled = false;
        }

        if (GameManager.Instance.GetBoolUpgrade(1)) //si el dash esta adquirida
        {
            buttons[4].interactable = false; //Desactiva el boton de mejora y cambia sus descripciones
            costBox[4].text = "Ya adquirida";
            descs[4].text = "Ya adquirida";
            coinImg[1].enabled = false;
        }
        else //cambia sus descripciones a su valor
        {
            costBox[4].text = "x" + costeUnicos;
            descs[4].text = "Desbloquea la habilidad Dash";
            coinImg[1].enabled = true;
        }

        if (GameManager.Instance.GetBoolUpgrade(2))
        {
            buttons[5].interactable = false;
            costBox[5].text = "Ya adquirida";
            descs[5].text = "Ya adquirida";
            coinImg[3].enabled = false;
        }

        else
        {
            costBox[5].text = "x" + costeUnicos;
            descs[5].text = "Desbloquea el arma de barrido";
            coinImg[3].enabled = true;
        }
        int i  = 0;
        bool notEnoughtMaterials = false;
        while (notEnoughtMaterials && i < recuros.Length)
        {
            if (recuros[i] < costeCuración)
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
        drinksAmount.text = $"Necesarios x3 de cada material\nTienes: x{GameManager.Instance.HealDrinksNum}";

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        dineroTotal = GameManager.Instance.GetDineros();
        dineroTotalText.text = dineroTotal.ToString();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    
    public void UpgradeNormal(int element)
    {
        if (dineroTotal >= costeNormales)
        {
            AudioManager.Instance.PlaySFX(upgradeSfx);
            GameManager.Instance.DecreaseDinero(costeNormales); //Quita el coste del dinero total
            GameManager.Instance.IncreaseUpgradeLevel(element); //Sube el nivel de la mejora
            Debug.Log(element);
            ChangeDesc(descs[element], GameManager.Instance.GetUpgradeLevel(element), element); //Cambia su descripcion
        }
        else
        {
            AudioManager.Instance.PlaySFX(NoUpgradeSfx);
        }
    }

    public void UpgradeUnico(int element) //Solo para dash y arma a distancia
    {
        if (dineroTotal >= costeUnicos)
        {
            AudioManager.Instance.PlaySFX(upgradeSfx);
            GameManager.Instance.BoolUpgrade(element - 3); //Pone a true el bool la mejora, en gamemanager dash es 0 y arma a distancia es 1
            GameManager.Instance.DecreaseDinero(costeUnicos);
            buttons[element].interactable = false; //desactiva el boton y cambia su descripcion
            costBox[element].text = "Ya adquirida";
            descs[element].text = "Ya adquirida";
            coinImg[element - 3].enabled = false;
            if (element == 3) //si es el arma a distancia
            {
                buttons[0].interactable = true; //activa el boton de mejora de daño a distancia y cambia su descripcion y texto de coste
                costBox[0].text = "x" + costeNormales;
                ChangeDesc(descs[0], GameManager.Instance.GetUpgradeLevel(0), 0);
                coinImg[2].enabled = true;
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

        while (!notEnoughtMaterials && i < recuros.Length)
        {
            if (recuros[i] - costeCuración < 0) notEnoughtMaterials = true;
            i++;
        }

        if (!notEnoughtMaterials)
        {
            for (int j = 0; j < recuros.Length; j++)
            {
                recuros[j] -= costeCuración; //SUSTITUIR VARIABLES CON PRECIO
            }

            GameManager.Instance.HealDrinksNum++;
            Debug.Log(GameManager.Instance.HealDrinksNum);
        }
        else
        {
            makeDrinkButton.enabled = false;
            drinkText.text = "Insuficientes materiales";
        }

        drinksAmount.text = $"Necesarios x3 de cada material\nTienes: x{GameManager.Instance.HealDrinksNum}";
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
    private void ChangeDesc(TextMeshProUGUI textBox, int level, int element)
    {
        string upgrade = "";
        float amount = 0;
        float NextAmount = 0;

        if (element == 0 || element == 1)
        {
            upgrade = "Daño";

            if (element == 0)
            {
                amount = BaseRangeDamage + (int)((RangeDamageUpgradePercent / 100) * BaseRangeDamage * GameManager.Instance.GetUpgradeLevel(0));

                NextAmount = BaseRangeDamage + (int)((MeleeDamageUpgradePercent / 100) * BaseRangeDamage * (GameManager.Instance.GetUpgradeLevel(0) + 1));
            }
            else
            {
                amount = BaseMeleeDamage + (int)((MeleeDamageUpgradePercent / 100) * BaseMeleeDamage * GameManager.Instance.GetUpgradeLevel(1));

                NextAmount = BaseMeleeDamage + (int)((MeleeDamageUpgradePercent / 100) * BaseMeleeDamage * (GameManager.Instance.GetUpgradeLevel(1)+1));
            }
        }
        else if (element == 2)
        {
            upgrade = "Vida";

            amount = BaseHealth + (int)((HealthUpgradePercent / 100) * BaseHealth * GameManager.Instance.GetUpgradeLevel(2));

            NextAmount = BaseHealth + (int)((HealthUpgradePercent / 100) * BaseHealth * (GameManager.Instance.GetUpgradeLevel(2) + 1));
        }


        textBox.text = $"{descripciones[element]}\n" +
                       $"ACTUAL:\n" +
                       $"   {upgrade}: {amount} puntos\n" +
                       $"SIGUIENTE:\n" +
                       $"   Nivel: {level + 1}\n" +
                       $"   {upgrade}: {NextAmount} puntos\n";
    }
    #endregion

} // class UIManagerUpgrades 
// namespace
