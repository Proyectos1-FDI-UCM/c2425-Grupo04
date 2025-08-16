//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// The Last Round
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using

public enum UpgradeType
{
    Bool,
    Int
}

public enum BoolUpgradeType
{
    Ataque_a_distancia,
    Arma_de_barrido,
    Dash
}
public enum IntUpgradeType
{
    Vida,
    Daño_a_distancia,
    Daño_a_melee
}

[System.Serializable]
public struct BoolUpgrade
{
    public BoolUpgradeType type;
    public int price;
    public string description;

    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI name;
    public TextMeshProUGUI priceText;
    public Image CoinImage;
}

[System.Serializable]
public struct IntUpgrade
{
    public IntUpgradeType type;
    public int price;
    public string description;

    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI name;
    public TextMeshProUGUI priceText;
    public Image CoinImage;
}
// namespace
