//---------------------------------------------------------
// Encargado de manejar el sistema de intercambios de recursos con la clase ConvertSource
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using UnityEngine;

// Añadir aquí el resto de directivas using
using UnityEngine.UI;
using TMPro;

/// <summary>
/// La clase ConvertSource se encarga de identificar el recurso a convertir y el recurso al que se quiere convertir
/// Una vez hecho esto y pasado una serie de filtros el jugador podrá hacer el intercambio y se modifican los datos del inventario
/// </summary>
public class ConvertSource : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField]
    private TMP_Dropdown ToBeConvertedDropdown, ToConvertDropdown;

    [SerializeField]
    private Button ConvertButton;

    [SerializeField]
    private GameObject[] Sources;

    [SerializeField]
    private int AmountToConvert = 1, AmountToBeConverted = 2;

    [SerializeField]
    private TextMeshProUGUI valorToConvert, ValorToBeConverted, itemValorToConvert, itemValorToBeConverted;

    [SerializeField]
    private string MessageCanConvert, MessageCantConvert;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.

    private GameObject SourceToConvert;
    private GameObject SourceToBeConverted;
    private int LastIndex;
    private float[] InvSources;
    private bool CanConvert = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        valorToConvert.text = $"x{AmountToConvert}";
        ValorToBeConverted.text = $"x{AmountToBeConverted}";
        itemValorToConvert.text = $"x{AmountToConvert}";
        itemValorToBeConverted.text = $"x{AmountToBeConverted}";
        LastIndex = ToBeConvertedDropdown.value;
        ModifyUI();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Solo se modifica la interfaz si se ha cambiado la opción elegida
        //Esto para no tener tantos bucles en un update
        if (LastIndex != ToBeConvertedDropdown.value)
        {
            ModifyUI();
        }

        //En función de si se puede o no convertir se cambia el mensaje del botón de convertir y se permite o no hacerlo
        InvSources = GameManager.Instance.GetRecursos();

        if (SourceToBeConverted.GetComponent<CastMaterial>() != null &&
            InvSources[(int)SourceToBeConverted.GetComponent<CastMaterial>().GetSourceName()] >= AmountToBeConverted)
        {
            ConvertButton.GetComponentInChildren<TextMeshProUGUI>().text = MessageCanConvert;
            CanConvert = true;
        }
        else
        {
            ConvertButton.GetComponentInChildren<TextMeshProUGUI>().text = MessageCantConvert;
            CanConvert = false;
        }
        //Debug.Log(CanConvert);

        LastIndex = ToBeConvertedDropdown.value;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Él botón convert accede a él para modificar el array de recursos en función de las elecciones tomadas en la tabla de conversión
    /// </summary>
    public void Convert()
    {
        if (CanConvert)
        {
            //primero se identifica el material al que se va a convertir
            //Se identifica por el sprite
            //Se coge el sprite del objeto seleccionado para ser convertido
            Sprite SourceToConvertSprite;
            if (ToBeConvertedDropdown.captionImage != null)
            {
                SourceToConvertSprite = ToConvertDropdown.captionImage.sprite;

                //Se busca el recurso que es buscando el sprite recogido entre los sprites de los recursos
                int i = 0;
                bool enc = false;
                while (i < Sources.Length && !enc)
                {
                    SpriteRenderer SourceSprite = Sources[i].GetComponent<SpriteRenderer>();
                    if (SourceSprite != null &&
                        SourceSprite.sprite == SourceToConvertSprite)
                    {
                        enc = true;
                    }
                    else
                    {
                        ++i;
                    }
                }
                if (enc && Sources[i].GetComponent<CastMaterial>() != null)
                {
                    SourceToConvert = Sources[i];
                }
            }

            if (SourceToConvert.GetComponent<CastMaterial>() != null && SourceToBeConverted.GetComponent<CastMaterial>() != null)
            {
                SourceName SourceToConvertName = SourceToConvert.GetComponent<CastMaterial>().GetSourceName();
                SourceName SourceToBeConvertedName = SourceToBeConverted.GetComponent<CastMaterial>().GetSourceName();

                GameManager.Instance.IncreaseResource(SourceToBeConvertedName, -AmountToBeConverted);
                GameManager.Instance.IncreaseResource(SourceToConvertName, AmountToConvert);
            }
        }
    }

    /// <summary>
    /// Se encarga de cambiar la interfaz, las posibles opciones en función del primer material elegido y el dropdown resultado de la opción escogida
    /// </summary>
    public void ModifyUI()
    {
        //primero se identifica el material que se va a convertir
        //Se identifica por el sprite
        //Se coge el sprite del objeto seleccionado para ser convertido
        Sprite SourceToBeConvertedSprite;
        if (ToBeConvertedDropdown.captionImage != null)
        {
            SourceToBeConvertedSprite = ToBeConvertedDropdown.captionImage.sprite;

            //Se busca el recurso que es buscando el sprite recogido entre los sprites de los recursos
            int i = 0;
            bool enc = false;
            while (i < Sources.Length && !enc)
            {
                SpriteRenderer SourceSprite = Sources[i].GetComponent<SpriteRenderer>();
                if (SourceSprite != null &&
                    SourceSprite.sprite == SourceToBeConvertedSprite)
                {
                    enc = true;
                }
                else
                {
                    ++i;
                }
            }
            if (enc && Sources[i].GetComponent<CastMaterial>() != null)
            {
                SourceToBeConverted = Sources[i];
            }
        }

        //Una vez identificado el material que se quiere convertir vamos a configurar las posibles opciones para convertir
        //Si el material es de una manzana, las opciones son materiales de manzana menos él mismo
        //Si el mateiral es de una uva, las opciones son materiales de uva menos él mismo

        int j = 0;
        bool encA = false, encB = false;

        while (j < Sources.Length && !encB)
        {
            CastMaterial SourceCaster = Sources[j].GetComponent<CastMaterial>();
            CastMaterial SourceToBeConvertedCaster = SourceToBeConverted.GetComponent<CastMaterial>();

            //El objeto no puede ser él mismo
            if (SourceCaster != null && SourceToBeConvertedCaster != null &&
                SourceCaster.GetSourceName() != SourceToBeConvertedCaster.GetSourceName())
            {
                //Las siguientes comprovaciones se hacen en ese orden para asegurarme de que tras encontrar la primera opción siga al siguiente objeto y no recoja el mismo objeto como segunda opción al ser encA true.

                //Si he encontrado una primera opción y encuentro ya un objeto con el mismo tipo, he encontrado la segunda opción
                if (SourceCaster != null && SourceToBeConvertedCaster != null &&
                SourceCaster.GetSourceType() == SourceToBeConvertedCaster.GetSourceType()
                && encA)
                {
                    encB = true;
                    ToConvertDropdown.options[1].text = $"{SourceCaster.GetSourceName()}".Replace("_", " "); ;
                    ToConvertDropdown.options[1].image = Sources[j].GetComponent<SpriteRenderer>().sprite;
                }

                //Si es del mismo tipo, he encontrado la primera opción siempre que no la haya encontrado antes
                if (SourceCaster != null && SourceToBeConvertedCaster != null &&
                SourceCaster.GetSourceType() == SourceToBeConvertedCaster.GetSourceType()
                && !encA && !encB)
                {
                    encA = true;
                    ToConvertDropdown.options[0].text = $"{SourceCaster.GetSourceName()}".Replace("_", " "); ;
                    ToConvertDropdown.options[0].image = Sources[j].GetComponent<SpriteRenderer>().sprite;
                }
            }

            j++;
        }
        ToConvertDropdown.RefreshShownValue();
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion

} // class ConvertSource 
// namespace
