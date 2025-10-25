using UnityEngine;

[System.Serializable]
public class FoodOrder
{
    public FoodCategory category;
    public CakeType cakeType;
    public CookieType cookieType;
    public MufflinType mufflinType;

    public FoodOrder(FoodCategory category)
    {
        this.category = category;
        cakeType = CakeType.ninguno;
        cookieType = CookieType.ninguno;
        mufflinType = MufflinType.ninguno;

        switch (category)
        {
            case FoodCategory.bizcocho:
                cakeType = (CakeType)Random.Range(1, System.Enum.GetValues(typeof(CakeType)).Length);
                break;
            case FoodCategory.galleta:
                cookieType = (CookieType)Random.Range(1, System.Enum.GetValues(typeof(CookieType)).Length);
                break;
            case FoodCategory.mufflin:
                mufflinType = (MufflinType)Random.Range(1, System.Enum.GetValues(typeof(MufflinType)).Length);
                break;
        }
    }

    public string GetFoodDescription()
    {
        if (category == FoodCategory.no)
        {
            return "No quiero comida";
        }

        string tipo = "";
        string nombre = "";

        switch (category)
        {
            case FoodCategory.bizcocho:
                tipo = "un bizcocho";
                nombre = cakeType.ToString();
                break;
            case FoodCategory.galleta:
                tipo = "una galleta";
                nombre = cookieType.ToString();
                break;
            case FoodCategory.mufflin:
                tipo = "un mufflin";
                nombre = mufflinType.ToString();
                break;
        }

        if (string.IsNullOrEmpty(nombre) || nombre == "no")
            return "No quiero comida.";

        // Se corrigen nombres espaciados
        nombre = nombre.Replace("RedVelvet", "Red Velvet")
                        .Replace("dulceLeche", "dulce de leche");

        return $" También, quiero {tipo} de {nombre.ToLower()}";
    }
}

public enum FoodCategory { no, bizcocho, galleta, mufflin }
public enum CakeType { ninguno, chocolate, mantequilla, zanahoria, RedVelvet }
public enum CookieType { ninguno, chocolate, blanco, mantequilla }
public enum MufflinType { ninguno, pistacho, arandanos, cereza, dulceLeche }
