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
    public float patience = 100f;
    public float patienceDecrease = 0.25f;
    bool patient = false;
    bool atNormalQueue = false;

    //gato
    [Header("Ajustes Gato")]
    public Transform gatoObject;
    public float distancDetection = 50.0f;

    public float catNecesity = 90.0f;

    private float _timerPetting = 0f;   // Variable interna para contar
    public float pettingTime = 3f;  // El tiempo que quieres que pare (3 segundos)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{    
    //    manager.customers.Enqueue(this);
    //}

    public void Spawn()
    {
        Debug.Log("He entrado a la tienda");

        manager = FindFirstObjectByType<CustomerManager>();

        GameObject gatoReal = GameObject.FindGameObjectWithTag("Gato");

        if (gatoReal != null)
        {
            gatoObject = gatoReal.transform; // ¡Ahora sí apuntamos al gato vivo!
        }
        else
        {
            Debug.LogError("¡No encuentro al gato! ¿Le has puesto el Tag 'Gato'?");
        }

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
        return Status.Success;
    }

    public void UseBathroom()
    {

    }

    public Status ReturnToQueue()
    {
        return Status.Success;
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

    public Status CheckCat()
    {
        float distancia = Vector3.Distance(this.transform.position, gatoObject.position);

        if (distancia < distancDetection)
        {
            return Status.Success;
        }

        return Status.Failure; 
    }

    public Status CheckNeedToPet()
    {
        if (patience < catNecesity && patience > 0)
        {
            Debug.Log("paciencia baja, necesita acariciar al gato");
            return Status.Success; 
        }

        return Status.Failure;
    }

    public void StartPetting()
    {
        _timerPetting = 0f; 
        Debug.Log("acariciando al gato...");

        // futuro: cuando tengamos animaciones
        // animator.SetBool("IsPetting", true);
    }

    public Status PerformPetting()
    {
        _timerPetting += Time.deltaTime;

        if (_timerPetting < pettingTime)
        {
            return Status.Running; 
        }

        patience += 15f;
        if (patience > 100f) patience = 100f;

        Debug.Log("Termino de acariciar. Paciencia: " + patience);

        // futuro: cuando tengamos animaciones
        // animator.SetBool("IsPetting", false);

        return Status.Success;
    }

    public Status ResumeTask()
    {
        Debug.Log("He terminado con el gato. Vuelvo a lo mío.");

        if (patience < catNecesity)
        {
            patience = catNecesity + 5f;
        }

        return Status.Success;
    }
}
