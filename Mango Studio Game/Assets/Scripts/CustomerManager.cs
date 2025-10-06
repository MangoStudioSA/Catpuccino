using NUnit.Framework;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        customers = new Queue<CustomerController>();
        nextSpawn = Random.Range(minTime/2, maxTime/2);
    }

    // Update is called once per frame
    void Update()
    {
        nextSpawn -= Time.deltaTime;

        if (nextSpawn <= 0 && clients < maxClients)
        {
            nextSpawn = Random.Range(minTime, maxTime);
            Instantiate(customer, spawn.transform.position, spawn.transform.rotation);
            clients += 1;
        }
    }
}
