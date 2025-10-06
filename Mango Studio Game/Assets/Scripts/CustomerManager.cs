using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public GameObject spawn;
    public GameObject customer;
    private float nextSpawn;
    public int minTime = 5, maxTime = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextSpawn = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        nextSpawn -= Time.deltaTime;

        if (nextSpawn <= 0)
        {
            nextSpawn = Random.Range(minTime, maxTime);
            Instantiate(customer, spawn.transform);
        }
    }
}
