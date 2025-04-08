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

        //Pone el precio
        costBox[1].text = "x" + costeNormales;
        costBox[2].text = "x" + costeNormales;
        costBox[3].text = "x" + costeNormales;
        //Cambia la descripcion de las mejoras normales (Vida y Melee)
        ChangeDesc(descs[1], GameManager.Instance.GetUpgradeLevel(1), 1);
        ChangeDesc(descs[2], GameManager.Instance.GetUpgradeLevel(2), 2);
        ChangeDesc(descs[3], GameManager.Instance.GetUpgradeLevel(3), 3);

        if (GameManager.Instance.GetBoolUpgrade(0))//Si el Arma a Distancia esta adquirida
        {
            ChangeDesc(descs[0], GameManager.Instance.GetUpgradeLevel(0), 0); //Cambia la descripcion de la mejora de Daño a distancia
            buttons[4].interactable = false; //Desactiva el boton de la mejora de Arma a distancia
            costBox[4].text = "Ya adquirida"; //Cambia el texto del coste y la descripcion del arma a distancia
            descs[4].text = "Ya adquirida";
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
            buttons[5].interactable = false; //Desactiva el boton de mejora y cambia sus descripciones
            costBox[5].text = "Ya adquirida";
            descs[5].text = "Ya adquirida";
            coinImg[1].enabled = false;
        }
        else //cambia sus descripciones a su valor
        {
            costBox[5].text = "x" + costeUnicos;
            descs[5].text = "Desbloquea la habilidad Dash";
            coinImg[1].enabled = true;
        }
        
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
            GameManager.Instance.DecreaseDinero(costeNormales); //Quita el coste del dinero total
            GameManager.Instance.IncreaseUpgradeLevel(element); //Sube el nivel de la mejora
            Debug.Log(element);
            ChangeDesc(descs[element], GameManager.Instance.GetUpgradeLevel(element), element); //Cambia su descripcion
        }
    }

    public void UpgradeUnico(int element) //Solo para dash y arma a distancia
    {
        if (dineroTotal >= costeUnicos)
        {
            GameManager.Instance.BoolUpgrade(element - 4); //Pone a true el bool la mejora, en gamemanager dash es 0 y arma a distancia es 1
            GameManager.Instance.DecreaseDinero(costeUnicos);
            buttons[element].interactable = false; //desactiva el boton y cambia su descripcion
            costBox[element].text = "Ya adquirida";
            descs[element].text = "Ya adquirida";
            coinImg[element - 4].enabled = false;
            if (element == 4) //si es el arma a distancia
            {
                buttons[0].interactable = true; //activa el boton de mejora de daño a distancia y cambia su descripcion y texto de coste
                costBox[0].text = "x" + costeNormales;
                ChangeDesc(descs[0], GameManager.Instance.GetUpgradeLevel(0), 0);
                coinImg[2].enabled = true;
            }
        }
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
        textBox.text = "Nivel actual:       " + level + "\nSiguiente nivel:    " + (level + 1) + "\n" + descripciones[element];
    }
    #endregion

} // class UIManagerUpgrades 
// namespace
