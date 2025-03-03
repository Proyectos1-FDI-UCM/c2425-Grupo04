//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Texts
{
    public enum Estado
    {
        monologo, dialogo, bebida
    }

    [System.Serializable]
    public struct Texto
    {
        public string GoodText;
        public string BadText;
        public Estado estatus;

        public Texto(string GoodText, string BadText, Estado estatus)
        {
            this.GoodText = GoodText;
            this.BadText = BadText;
            this.estatus = estatus;
        }
    }
    
} // class Texts 
// namespace
