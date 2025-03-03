//---------------------------------------------------------
// Este script se encargará de manejar todo lo referente a interfaz de usuario en la escena de Bartender
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using
using TMPro;
using System.Collections;
using System.Transactions;
using UnityEngine.InputSystem;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField]
    TextMeshProUGUI dialogueBox, Button, option1Text, option2Text;
    [SerializeField]
    Button ButtonBox;
    [SerializeField]
    private float TypeSpeed;
    [SerializeField]
    private Button option1Box, option2Box;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private DataContainer.Texto[] dialogue;
    private int way = 1; // 1 = good way , 2 = bad way
    private int DialogueLine = 0;
    private bool SkipDialogue = false, 
                 hayOpciones = false; //
    private int correcta; //
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
        GameManager.Instance.GiveUI(this);
        ButtonBox.gameObject.SetActive(true);
        option1Box.gameObject.SetActive(false);//
        option2Box.gameObject.SetActive(false);//
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (dialogue != null && (dialogue[DialogueLine].GoodText == dialogueBox.text || dialogue[DialogueLine].BadText == dialogueBox.text)) Button.text = "Continuar";
        else Button.text = "Saltar";
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public void GetDialogue(DataContainer.Texto[] dialogue)
    {
        this.dialogue = dialogue;
        DialogueLine = 0;
        StartCoroutine(Write());
    }

    public void SkipButton()
    {
        if (dialogue != null && (dialogue[DialogueLine].GoodText == dialogueBox.text || dialogue[DialogueLine].BadText == dialogueBox.text))
        {
            if (DialogueLine < dialogue.GetLength(0) - 1)
            {
                DialogueLine++;
                StartCoroutine(Write());
            }
            else
            {
                HayOpciones();//
                ButtonBox.gameObject.SetActive(false);
                dialogueBox.text = " ";
            }

        }
        else
        {
            SkipDialogue = true;
        }
    }

    public void SetChoice(string option1, string option2, string dialogue1, string dialogue2, int correcta) //
    {
        option1Text.text = option1;
        option1Box.gameObject.SetActive(true);
        option2Text.text = option2;
        option2Box.gameObject.SetActive(true);
        //dialogue[0, 0] = dialogue1;
        //dialogue[1, 0] = dialogue2;
        this.correcta = correcta;
    }

    public void OptionL() //
    {
        option1Box.gameObject.SetActive(false);
        option2Box.gameObject.SetActive(false);
        DialogueLine = 0;
        way = 0;
        StartCoroutine(Write());

        if (correcta != 0)
        {
            way = 1;
        }
        else
        {
            way = 0;
        }
    }
    public void OptionR() //
    {
        option1Box.gameObject.SetActive(false);
        option2Box.gameObject.SetActive(false);
        DialogueLine = 1;
        way = 0;
        StartCoroutine(Write());

        if (correcta != 1)
        {
            way = 1;
        }
        else
        {
            way = 0;
        }
    }
    public void HayOpciones() //Decide al azar si hay opciones o no //
    {
        int rnd = Random.Range(0, 2);
        if (rnd == 0)
        {
            hayOpciones = false;
        }
        else
        {
            hayOpciones = true;
        }
    }

    public bool chequeoOpciones()//
    {
        return hayOpciones;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    IEnumerator Write()
    {
        dialogueBox.text = string.Empty;
        //Recorre el tamaño del texto que tiene que escribir y se va escribiendo char por char
        for (int i = 0; i < dialogue[DialogueLine].GoodText.Length; i++)//aqui he puesto .GoodText pero esto es mentira, hay que modificarlo a Good o Bad en función de la elección anterior
        {
            char ch = dialogue[DialogueLine].GoodText[i];
            if (SkipDialogue)
            {
                dialogueBox.text = dialogue[DialogueLine].GoodText;
                i = dialogue[DialogueLine].GoodText.Length;
                SkipDialogue = false;
            }
            else dialogueBox.text += ch;
            yield return new WaitForSeconds(TypeSpeed);
        }
    }

    #endregion

} // class UIManager 
// namespace
