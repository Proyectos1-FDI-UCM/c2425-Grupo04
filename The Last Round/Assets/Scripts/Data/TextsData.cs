//---------------------------------------------------------
// El archivo contienen un struct que se encarga de contener toda la información de un texto
// Esto se usará en la escena de bartender y contendrá la información del estado y el emisor del texto y sus contenidos (bad way and good way)
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// El emisor es el responsable de decir el texto cuando se está escribiendo
/// Se utiliza para distinguir cuando el que habla es el jugador o el cliente y poder poner sus retratos cuando hablen
/// </summary>
public enum Emisor
{
    cliente, jugador
}

/// <summary>
/// Se almacena el estado del texto a escribir, puede distinguirse para saber cuál va a ser el comportamiento del UI
/// El monólogo es un texto a escribir
/// El diálogo es un texto del cual se espera una respuesta en forma de opciones
/// La bebida es un texto en el cual se pide una bebida
/// </summary>
public enum Estado
{
    monologo, dialogo, bebida
}

/// <summary>
/// Recoge toda la información de los textos.
/// La respuesta positiva, la respuesta negativa, el estado del texto y el emisor.
/// </summary>
[System.Serializable]
public struct Texto
{
    [TextArea(4, 6)]
    public string goodText;
    [TextArea(4, 6)]
    public string badText;
    public Estado estatus;
    public Emisor emisor;
}

/// <summary>
/// Es un array de Textos que se utilizará para poder añadir diferentes dialogos en un mismo cliente
/// Los arrays bidimensionales no se pueden serializar pero el array de un tipo conformado por otro array si que lo es
/// /// Además dos booleanos que indican si es un texto genérico (se puede decir más de una vez) y si se ha dicho en caso de que no sea genérico para no repetirlo
/// </summary>
[System.Serializable]
public struct dialogue
{
    public bool Generic;
    public Texto[] Lines;
    [HideInInspector]
    public bool WasSaid;
}
// namespace
