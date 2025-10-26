using UnityEngine;

[System.Serializable]
public class FoodOrder
{
    public FoodCategory category;
    public CakeType cakeType;
    public CookieType cookieType;
    public MufflinType mufflinType;

    public FoodCategory foodTargetCategory;
    public int foodTargetType;

    public FoodCategory foodPrecisionCategory;
    public int foodPrecisionType;

    public CookState targetCookState;
    public CookState precisionCookState;

    public FoodOrder(FoodCategory category)
    {
        this.category = category;
        foodTargetCategory = category;
        foodPrecisionCategory = FoodCategory.no;
        foodPrecisionType = -1;

        targetCookState = CookState.horneado;
        precisionCookState = CookState.no;

        switch (category)
        {
            case FoodCategory.bizcocho:
                foodTargetType = Random.Range(1, System.Enum.GetValues(typeof(CakeType)).Length);
                cakeType = (CakeType)foodTargetType;
                break;
            case FoodCategory.galleta:
                foodTargetType = Random.Range(1, System.Enum.GetValues(typeof(CookieType)).Length);
                cookieType = (CookieType)foodTargetType;
                break;
            case FoodCategory.mufflin:
                foodTargetType = Random.Range(1, System.Enum.GetValues(typeof(MufflinType)).Length);
                mufflinType = (MufflinType)foodTargetType;
                break;
            default:
                foodTargetType = -1;
                break;
        }
    }

    public void SetFoodPrecision(FoodCategory category, int type)
    {
        foodPrecisionCategory = category;
        foodPrecisionType = type;
        Debug.Log($"La precisionCategory es: {category} y la precisionType es: {type}");
    }

    public void SetCookStatePrecision(CookState state)
    {
        precisionCookState = state;
        Debug.Log($"Estado de coccion asignado: {state}");
    }

    public string GetFoodDescription()
    {
        if (category == FoodCategory.no)
        {
            return "No quiero comida.";
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
            return "No comida.";

        // Se corrigen nombres espaciados
        nombre = nombre.Replace("RedVelvet", "Red Velvet")
                        .Replace("dulceLeche", "dulce de leche");

        return $" También, quiero {tipo} de {nombre.ToLower()}.";
    }
}

public enum FoodCategory { no, bizcocho, galleta, mufflin }
public enum CakeType { ninguno, chocolate, mantequilla, zanahoria, RedVelvet }
public enum CookieType { ninguno, chocolate, blanco, mantequilla }
public enum MufflinType { ninguno, pistacho, arandanos, cereza, dulceLeche }
public enum CookState { no, crudo, horneado, quemado }
