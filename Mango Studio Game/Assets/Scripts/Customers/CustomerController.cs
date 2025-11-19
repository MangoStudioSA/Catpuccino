using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using NUnit.Framework.Internal.Filters;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 direction = Vector3.forward;
    public bool atCounter = false, atQueue = false;
    CustomerManager manager;
    float patience = 100f;
    public float patienceDecrease = 0.25f;
    bool patient = false;
    bool atNormalQueue = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{    
    //    manager.customers.Enqueue(this);
    //}

    public void Spawn()
    {
        Debug.Log("He entrado a la tienda");

        manager = FindFirstObjectByType<CustomerManager>();

        if (Random.Range(0, 2) == 0)
        {
            patient = true;
        }
        else
        {
            patient = false;
        }
    }

    public Status GoToQueue()
    {
        if (!atCounter && !atQueue)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);
            return Status.Running;
        }

        Debug.Log("He llegado a la cola");
        manager.customers.Enqueue(this);
        atNormalQueue = true;
        return Status.Success;
    }

    public Status LineFull()
    {
        if (manager.clients > manager.maxClients)
        {
            Debug.Log("Esta llena la cola");
            return Status.Success;
        }

        Debug.Log("No esta llena la cola");
        return Status.Failure;
    }

    public void LeaveQueue()
    {
        Debug.Log("Me voy");
        manager.orderButton.SetActive(false);
        manager.clients -= 1;
        manager.customers.Dequeue();
        Destroy(this.gameObject);
    }

    public Status NeedBathromm()
    {
        if (Random.Range(0f, 100f) <= 25f && !atCounter)
        {
            Debug.Log("Tengo que usar el baño");
            return Status.Success;
        }

        Debug.Log("No tengo que usar el baño");
        return Status.Failure;
    }

    public Status ResetPatience()
    {
        patience = 100f;
        Debug.Log("Reseteo mi paciencia");
        return Status.Success;
    }

    public Status BathroomQueue()
    {
        manager.customers.Dequeue();
        atNormalQueue = false;
        manager.customersBathroom.Enqueue(this);
        Debug.Log("Cambio de cola");
        return Status.Success;
    }

    public Status BathroomLineFull()
    {
        if (manager.customersBathroom.Count > manager.maxClientsBathroom)
        {
            Debug.Log("Esta llena la cola del baño");
            return Status.Success;
        }

        Debug.Log("No esta llena la cola");
        return Status.Failure;
    }

    public void ChangeQueue()
    {
        transform.position = new Vector3(manager.spawnBathroom.transform.position.x, transform.position.y, manager.spawnBathroom.transform.position.z);
        Debug.Log("Me voy al baño");
    }

    public Status GoToBathRoom()
    {
        if (!atCounter && !atQueue)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);
            return Status.Running;
        }

        Debug.Log("He llegado a la cola del baño");
        return Status.Success;
    }

    public Status CheckPatience()
    {
        if (patience <= 0)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status BathroomMyTurn()
    {

    }

    public void UseBathroom()
    {

    }

    public Status ReturnToQueue()
    {

    }

    public void LeaveBathroomQueue()
    {
        Debug.Log("Me voy");
        manager.customersBathroom.Dequeue();
        Destroy(this.gameObject);
    }


    void Update()
    {
        if (patient)
        {
            patience -= patienceDecrease * Time.deltaTime;
        }
        else
        {
            patience -= (patienceDecrease * 2) * Time.deltaTime;
        }

        //if (!atCounter && !atQueue)
        //{
        //    transform.Translate(direction.normalized * speed * Time.deltaTime);
        //}

        //if (!atCounter && manager.customers.Count > 0 && manager.customers.Peek() == this)
        //{
        //    atQueue = false;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CustomerManager" && atNormalQueue)
        {
            manager.orderButton.SetActive(true);
            atCounter = true;
            manager.orderingCustomer = this.gameObject;
        }
        else
        {
            atQueue = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CustomerManager")
        {
            atCounter = false;
            manager.orderingCustomer = null;
        }
        else
        {
            atQueue = false;
        }
    }
}
