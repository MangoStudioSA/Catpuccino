using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    SaveDataManager dataManager;
    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dataManager = FindFirstObjectByType<SaveDataManager>();

        float averageSatisfaction = 100;
        if (dataManager.rated > 0)
        {
            averageSatisfaction = ((float)dataManager.score / dataManager.rated);
        }

        text.text = averageSatisfaction + " / 100";

        if (averageSatisfaction > 75)
        {
            text2.text = "SIGUE ASÍ!!";
        }
        else if (averageSatisfaction > 50)
        {
            text2.text = "NO ESTÁ MAL :)";
        }
        else if (averageSatisfaction > 25)
        {
            text2.text = "MEJORABLE...";
        }
        else
        {
            text2.text = ":((";
        }
    }
}
