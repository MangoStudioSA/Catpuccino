using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CakeSpriteStock
{
    public CakeType cakeType;
    public Image image; // El SpriteRenderer a modificar
    public Sprite[] stages;         // Sprites desde completo a vacío
    [HideInInspector] public int currentStage = 0;

    public bool IsDepleted()
    {
        return currentStage >= stages.Length - 1;
    }

    public void Reset()
    {
        currentStage = 0;
        image.sprite = stages[currentStage];
    }

    public bool ConsumeStage()
    {
        if (IsDepleted())
            return false;

        currentStage++;
        image.sprite = stages[currentStage];
        return true;
    }
}
