using UnityEngine;

public class UICoffeeStation : MonoBehaviour
{
    public GameObject roomPanel;
    public GameObject preparationPanel;

    public void SubmitOrderUI()
    {
        preparationPanel.SetActive(false);
        roomPanel.SetActive(true);
    }
}
