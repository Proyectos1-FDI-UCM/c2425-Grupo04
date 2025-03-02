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

/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField]
    TextMeshProUGUI dialogueBox;
    [SerializeField]
    private float TypeSpeed;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private string[,] dialogue;
    private int way = 1; // 1 = good way , 2 = bad way
    private int DialogueLine = 0;
    private bool SkipDialogue = false;
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
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && dialogue != null)
        {
            if (dialogue[DialogueLine, 0] == dialogueBox.text)
            {
                if (DialogueLine < dialogue.GetLength(0) - 1)
                {
                    DialogueLine++;
                    StartCoroutine(Write());
                }
                else
                {
                    dialogueBox.text = " ";
                }

            }
            else
            {
                SkipDialogue = true;
            }
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void GetDialogue(string[,] dialogue)
    {
        this.dialogue = dialogue;
        DialogueLine = 0;
        StartCoroutine(Write());
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    IEnumerator Write()
    {
        dialogueBox.text = string.Empty;

        for (int i = 0; i < dialogue[DialogueLine, 0].Length; i++)
        {
            char ch = dialogue[DialogueLine, 0][i];
            if (SkipDialogue)
            {
                dialogueBox.text = dialogue[DialogueLine, 0];
                i = dialogue[DialogueLine, 0].Length;
                SkipDialogue = false;
            }
            else dialogueBox.text += ch;
            yield return new WaitForSeconds(TypeSpeed);
        }
    }
    #endregion

} // class UIManager 
// namespace
