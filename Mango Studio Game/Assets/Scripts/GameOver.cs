using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    SaveDataManager dataManager;
    public TextMeshProUGUI text;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
