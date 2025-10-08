using UnityEngine;

public class UICoffeeStation : MonoBehaviour
{
    public GameObject roomPanel;
    public GameObject preparationPanel;
    CustomerManager manager;

    private void Start()
    {
        manager = GameObject.FindWithTag("CustomerManager").GetComponent<CustomerManager>();
    }

    public void SubmitOrderUI()
    {
        preparationPanel.SetActive(false);
        roomPanel.SetActive(true);
        manager.orderButton.SetActive(false);
        manager.clients -= 1;
        manager.customers.Dequeue();
        Destroy(manager.orderingCustomer);
    }
}
