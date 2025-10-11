using UnityEngine;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public GameObject spawn;
    public GameObject customer;
    private float nextSpawn;
    public int minTime = 5, maxTime = 10;
    public int clients = 0;
    public int maxClients = 7;
    public GameObject orderButton;
    public GameObject orderingCustomer;
    public Queue<CustomerController> customers;

    void Awake()
    {
        customers = new Queue<CustomerController>();
    }

    void Start()
    {
        nextSpawn = Random.Range(minTime / 2, maxTime / 2);
    }

    void Update()
    {
        nextSpawn -= Time.deltaTime;

        if (nextSpawn <= 0 && clients < maxClients && TimeManager.Instance.IsOpen)
        {
            nextSpawn = Random.Range(minTime, maxTime);
            Instantiate(customer, spawn.transform.position, spawn.transform.rotation);
            clients += 1;
        }
    }

    public void ResetForNewDay()
    {
        Debug.Log("Reiniciando clientes para el nuevo día.");

        foreach (CustomerController customer in customers)
        {
            if (customer != null)
            {
                Destroy(customer.gameObject);
            }
        }

        customers.Clear();
        clients = 0;

        if (orderingCustomer != null)
        {
            Destroy(orderingCustomer);
            orderingCustomer = null;
        }
        if (orderButton != null)
        {
            orderButton.SetActive(false);
        }
    }
}