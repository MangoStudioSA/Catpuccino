using UnityEngine;

public class UICoffeeStation : MonoBehaviour
{
    public GameObject preparationPanel;
    public GameObject deliveryPanel;

    public void SubmitOrderUI()
    {
        preparationPanel.SetActive(false);
        deliveryPanel.SetActive(true);
    }
}
