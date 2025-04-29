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
using System;

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

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.

    private GameObject SourceToConvert;
    private GameObject SourceToBeConverted;
    private int LastIndex;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        LastIndex = ToBeConvertedDropdown.value;
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
        LastIndex = ToBeConvertedDropdown.value;
        //Tras tener las posibles opciones hay que configurar qué hacer cuando le de al botón de convertir con la opción seleccionada.
        //Se hará en un método público llamado "Convert"
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Él botón convert accede a él para modificar el array de recursos en función de las elecciones tomadas en la tabla de conversión
    /// </summary>
    public void Convert()
    {

    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Se encarga de cambiar la interfaz, las posibles opciones en función del primer material elegido y el dropdown resultado de la opción escogida
    /// </summary>
    private void ModifyUI()
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

} // class ConvertSource 
// namespace
