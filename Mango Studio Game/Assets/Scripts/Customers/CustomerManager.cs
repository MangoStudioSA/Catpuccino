using UnityEngine;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public GameObject spawn;
    public GameObject customer;
    private float nextSpawn;
    public float minTime = 5, maxTime = 10;
    public float minMinTime = 1, minMaxTime = 2;
    public int clients = 0;
    public int maxClients = 7;
    public GameObject orderButton;
    public GameObject orderingCustomer;
    public Queue<CustomerController> customers;
    TimeManager timeManager;
    public float timeDecay = 1f;

    void Awake()
    {
        customers = new Queue<CustomerController>();
        timeManager = FindFirstObjectByType<TimeManager>();

    }

    void Start()
    {
        minTime -= timeDecay * (timeManager.currentDay - 1);
        maxTime -= timeDecay * (timeManager.currentDay - 1);

        if (minTime<minMinTime)
        {
            minTime = minMinTime;
        }

        if (minTime < minMinTime)
        {
            maxTime = minMaxTime;
        }

        nextSpawn = Random.Range(minTime / 4, maxTime / 4);
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