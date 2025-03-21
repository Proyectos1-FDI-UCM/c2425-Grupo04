//---------------------------------------------------------
// El archivo contienen un struct que se encarga de contener toda la información de un texto
// Esto se usará en la escena de bartender y contendrá la información del estado y el emisor del texto y sus contenidos (bad way and good way)
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

public enum Emisor
{
    cliente, jugador
}
public enum Estado
{
    monologo, dialogo, bebida
}

[System.Serializable]
public struct Texto
{
    public string goodText;
    public string badText;
    public Estado estatus;
    public Emisor emisor;
}

[System.Serializable]
public struct dialogue
{ 
    public Texto[] Lines;
}
// namespace
