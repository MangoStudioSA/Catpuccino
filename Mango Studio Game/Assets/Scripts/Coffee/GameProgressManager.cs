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
    public bool milkEnabled;
    public bool heatedMilkEnabled;
    public bool sugarEnabled;
    public bool iceEnabled;
    public bool typeOrderEnabled;

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
                break;
            case 3:
                coffeeEnabled = true;
                waterEnabled = true;
                sugarEnabled = true;
                iceEnabled = true;
                typeOrderEnabled = true;
                milkEnabled = true;
                heatedMilkEnabled = true;
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            default:
                coffeeEnabled = waterEnabled = sugarEnabled = iceEnabled = typeOrderEnabled = true;
                break;
        }
    }
}
