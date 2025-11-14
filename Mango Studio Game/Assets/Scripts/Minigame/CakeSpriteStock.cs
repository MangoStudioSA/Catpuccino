using UnityEngine;
using UnityEngine.UI;

// Clase encargada del stock de los bizcochos
[System.Serializable]
public class CakeSpriteStock
{
    public CakeType cakeType;
    public Image image; // Imagen a modificar
    public Sprite[] stages; // Lista de sprites: 0-Completo ... vacio
    [HideInInspector] public int currentStage = 0;

    // Funcion para comprobar si quedan porciones
    public bool IsDepleted()
    {
        return currentStage >= stages.Length - 1;
    }

    // Reestablecer la imagen 
    public void Reset()
    {
        currentStage = 0;
        image.sprite = stages[currentStage];
    }

    // Gastar 1 unidad
    public bool ConsumeStage()
    {
        if (IsDepleted())
            return false;

        currentStage++;
        image.sprite = stages[currentStage];
        return true;
    }
}
