using UnityEngine;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public GameObject spawn;
    public GameObject spawnBathroom;
    public GameObject customer;
    private float nextSpawn;
    public float minTimeBase = 10, maxTimeBase = 20;
    public float minTime = 0, maxTime = 0;
    public float minMinTime = 1, minMaxTime = 2;
    public int clients = 0;
    public int maxClients = 7;
    public int maxClientsBathroom = 1;
    public GameObject orderButton;
    public GameObject orderingCustomer;
    public Queue<CustomerController> customers;
    public Queue<CustomerController> customersBathroom;
    TimeManager timeManager;
    public float timeDecay = 1f;
    private bool startedDecay = false;

    void Awake()
    {
        customers = new Queue<CustomerController>();
        timeManager = FindFirstObjectByType<TimeManager>();

    }

    void Start()
    {
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

        minTime = minTimeBase - (timeDecay * timeManager.currentDay);
        maxTime = maxTimeBase - (timeDecay * timeManager.currentDay); ;

        if (minTime < minMinTime)
        {
            minTime = minMinTime;
        }

        if (minTime < minMinTime)
        {
            maxTime = minMaxTime;
        }

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

        startedDecay = false;
    }
}