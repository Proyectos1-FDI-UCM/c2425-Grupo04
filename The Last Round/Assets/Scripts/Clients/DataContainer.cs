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
public class DataContainer
{
    public enum Emisor
    {
        Cliente, Jugador
    }
    public enum Estado
    {
        monologo, dialogo, bebida
    }
    
    public enum DrinkType
    {
        Manzana,
        Uva
    }
    [System.Serializable]
    public struct Texto
    {
        public string GoodText;
        public string BadText;
        public Estado estatus;
        public Emisor emisor;

        public Texto(string GoodText, string BadText, Estado estatus, Emisor emisor)
        {
            this.GoodText = GoodText;
            this.BadText = BadText;
            this.estatus = estatus;
            this.emisor = emisor;
        }
    }

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

    public enum MaterialName
    {
        //Materiales Uva
        PielDeUva,
        JugoDeUva,
        SemillaDeUva,

        //Materiales Manzana
        Piel_de_manzana,
        JugoDeManzana,
        SemillaDeManzana,

        //Recursos naturales
        Hielo,
        levadura
    }

    [System.Serializable]
    public struct materials
    {
        public MaterialName name;
        public int amount;
        public Sprite materialImage;
    }

    [System.Serializable]
    public struct Bebida
    {
        public DrinkName name;
        public materials[] materials;
        public Sprite image;
        public int reward;
        public DrinkType type;
        public Bebida(DrinkName name, materials[] materials, Sprite image, int reward, DrinkType type)
        {
            this.name = name;
            this.materials = materials;
            this.image = image;
            this.reward = reward;
            this.type = type;
        }
    }
    
} // class Texts 
// namespace
