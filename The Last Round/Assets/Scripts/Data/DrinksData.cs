//---------------------------------------------------------
// Recoge toda la información necesaria para configurar las bebidas
// Víctor Martínez Moreno
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

//Los tipos de bebidas son de manzana o de uva
//estos identificadores se usan para restringir un tipo de bebidas u otras cuando no queden enemigos
public enum DrinkType
{
    Manzana,
    Uva
}

/// <summary>
/// Nombres de las bebidas.
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

/// <summary>
/// Almacena la información de cuanta cantidad de un material se necesita.
/// Las bebidas tienen un array de este tipo para saber qué y cuantos materiales necesitan
/// </summary>
[System.Serializable]
public struct NeededMaterial
{
    public GameObject Material;
    public int Amount;
}

/// <summary>
/// Contienen toda la información de una bebida.
/// Su nombre, sus materiales, su precio o recompensa y su tipo
/// </summary>
[System.Serializable]
public struct Bebida
{
    public DrinkName Name;
    public NeededMaterial[] Materials;
    public int Reward;
    public DrinkType Type;
}
// namespace
