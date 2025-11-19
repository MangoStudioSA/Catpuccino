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
    bool patient = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{    
    //    manager.customers.Enqueue(this);
    //}

    void Spawn()
    {
        if (Random.Range(0, 2) == 0)
        {
            patient = true;
        }
        else
        {
            patient = false;
        }
    }

    Status GoToQueue()
    {
        if (!atCounter && !atQueue)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);
            return Status.Running;
        }

        return Status.Success;
    }

    Status LineFull()
    {
        if (manager.customers.Count >= manager.maxClients)
        {
            return Status.Success;
        }
        else
        {
            return Status.Failure;
        }
    }

    void Irse()
    {
        manager.orderButton.SetActive(false);
        manager.clients -= 1;
        manager.customers.Dequeue();
        Destroy(this);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (!atCounter && !atQueue)
    //    {
    //        transform.Translate(direction.normalized * speed * Time.deltaTime);
    //    }

    //    if (!atCounter && manager.customers.Count > 0 && manager.customers.Peek() == this)
    //    {
    //        atQueue = false;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CustomerManager")
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
