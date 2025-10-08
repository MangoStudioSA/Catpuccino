using UnityEngine;
using UnityEngine.UI;


public class UIDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject preparationPanel;
    public GameObject deliveryPanel;
    public GameObject roomPanel;
    CustomerManager manager;

    public Button acceptButton;

    void Start()
    {
        preparationPanel.SetActive(false);
        acceptButton.onClick.AddListener(StartPreparation);
        manager = GameObject.FindWithTag("CustomerManager").GetComponent<CustomerManager>();
    }

    public void StartPreparation()
    {
        dialoguePanel.SetActive(false);
        preparationPanel.SetActive(true);
    }

    public void EndDelivery()
    {
        deliveryPanel.SetActive(false);
        roomPanel.SetActive(true);
        manager.orderButton.SetActive(false);
        manager.clients -= 1;
        manager.customers.Dequeue();
        Destroy(manager.orderingCustomer);
    }
}
