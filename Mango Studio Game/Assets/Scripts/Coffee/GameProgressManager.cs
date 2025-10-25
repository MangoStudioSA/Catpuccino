using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    //Mecanicas activas
    public bool coffeeEnabled;
    public bool waterEnabled;
    public bool sugarEnabled;
    public bool iceEnabled;
    public bool typeOrderEnabled;
    public bool milkEnabled;
    public bool heatedMilkEnabled;
    public bool condensedMilkEnabled;
    public bool creamEnabled;
    public bool chocolateEnabled;
    public bool whiskeyEnabled;
    // Comida
    public bool cakesEnabled;
    public bool cookiesEnabled;
    public bool mufflinsEnabled;

    public void UpdateMechanicsForDay(int day)
    {
        switch (day)
        {
            case 1:
                coffeeEnabled = true;
                waterEnabled = true;
                sugarEnabled = true;
                iceEnabled = true;
                typeOrderEnabled = true;
                break;

            case 2:
                coffeeEnabled = true;
                waterEnabled = true;
                sugarEnabled = true;
                iceEnabled = true;
                typeOrderEnabled = true;
                milkEnabled = true;
                cakesEnabled = true;
                break;
            case 3:
                coffeeEnabled = true;
                waterEnabled = true;
                sugarEnabled = true;
                iceEnabled = true;
                typeOrderEnabled = true;
                milkEnabled = true;
                cakesEnabled = true;
                heatedMilkEnabled = true;
                break;

            case 4:
                coffeeEnabled = true;
                waterEnabled = true;
                sugarEnabled = true;
                iceEnabled = true;
                typeOrderEnabled = true;
                milkEnabled = true;
                cakesEnabled = true;
                heatedMilkEnabled = true;
                cookiesEnabled = true;
                break;

            case 5:
                coffeeEnabled = true;
                waterEnabled = true;
                sugarEnabled = true;
                iceEnabled = true;
                typeOrderEnabled = true;
                milkEnabled = true;
                cakesEnabled = true;
                heatedMilkEnabled = true;
                cookiesEnabled = true;
                condensedMilkEnabled = true;
                creamEnabled = true;
                break;

            case 6:
                coffeeEnabled = true;
                waterEnabled = true;
                sugarEnabled = true;
                iceEnabled = true;
                typeOrderEnabled = true;
                milkEnabled = true;
                cakesEnabled = true;
                heatedMilkEnabled = true;
                cookiesEnabled = true;
                condensedMilkEnabled = true;
                creamEnabled = true;
                mufflinsEnabled = true;
                break;

            case 7:
                coffeeEnabled = true;
                waterEnabled = true;
                sugarEnabled = true;
                iceEnabled = true;
                typeOrderEnabled = true;
                milkEnabled = true;
                cakesEnabled = true;
                heatedMilkEnabled = true;
                cookiesEnabled = true;
                condensedMilkEnabled = true;
                creamEnabled = true;
                mufflinsEnabled = true;
                chocolateEnabled = true;
                whiskeyEnabled = true;
                break;

            default:
                coffeeEnabled = waterEnabled = sugarEnabled = iceEnabled = typeOrderEnabled = true;
                break;
        }
    }
}
