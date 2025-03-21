//---------------------------------------------------------
// Recoge toda la información necesaria para configurar las bebidas
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

public enum DrinkType
{
    manzana,
    uva
}

/// <summary>
/// Nombres de las bebidas, únicos en mayusculas debido a que son nombres propios
/// Estos se escribirán en el juego más adelante
/// </summary>
public enum DrinkName
{
    //Bebidas Manzanas
    Sidra,
    Applejack,
    Calvado,
    Licor_de_manzana,
    Eau_de_vie,
    Brandy_de_manzana,
    Pommeau,

    //Bebidas Uvas
    Vino,
    Armagak,
    Raki,
    Vermú,
    Pisco,
    Grappa,
    Singani,
}

[System.Serializable]
public struct NeededMaterial
{
    public GameObject material;
    public int amount;
}

[System.Serializable]
public struct Bebida
{
    public DrinkName name;
    public NeededMaterial[] materials;
    public int reward;
    public DrinkType type;
}
// namespace
