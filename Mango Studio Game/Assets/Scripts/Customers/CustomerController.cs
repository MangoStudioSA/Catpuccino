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
    public float distancDetection = 10.0f;

    public float catNecesity = 60.0f;
    private bool isInteracting = false;

    private float _timerPetting = 0f;
    public float pettingTime = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{    
    //    manager.customers.Enqueue(this);
    //}

    public void Spawn()
    {
        Debug.Log("He entrado a la tienda");

        manager = FindFirstObjectByType<CustomerManager>();

        GameObject gato = GameObject.FindGameObjectWithTag("Gato");

        if (gato != null)
        {
            gatoObject = gato.transform;
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
        if (isInteracting) return Status.Running;

        if (!atCounter && !atQueue)
        {
            if (manager != null)
            {
                Vector3 objetivo = manager.transform.position;
                objetivo.y = transform.position.y;
                transform.LookAt(objetivo);
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            return Status.Running;
        }

        Debug.Log("He llegado a la cola");
        manager.customers.Enqueue(this);
        atNormalQueue = true;
        atQueue = true;
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
        if (isInteracting) return;

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

    public Status CheckNeedToPet()
    {
        Debug.Log($"Comprobando Necesidad... Paciencia: {patience}");

        if (patience < catNecesity && patience > 0)
        {
            Debug.Log("paciencia baja, necesita acariciar al gato");
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status GoToCat()
    {
        if (gatoObject == null)
        {
            GameObject g = GameObject.FindGameObjectWithTag("Gato");
            if (g != null) gatoObject = g.transform;
            else return Status.Failure;
        }

        Vector3 miPosPlana = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 gatoPosPlana = new Vector3(gatoObject.position.x, 0, gatoObject.position.z);

        float distanciaPlana = Vector3.Distance(miPosPlana, gatoPosPlana);

        Debug.Log($"[3] Distancia Plana: {distanciaPlana}. Gato: {gatoObject.position}");

        if (distanciaPlana < 0.5f)
        {
            Debug.Log("[4] Llegué (Ignorando altura)");
            return Status.Success;
        }

        Vector3 direccion = (gatoObject.position - transform.position).normalized;
        direccion.y = 0;

        transform.Translate(direccion * speed * Time.deltaTime, Space.World);


        Vector3 lookPos = new Vector3(gatoObject.position.x, transform.position.y, gatoObject.position.z);
        transform.LookAt(lookPos);

        return Status.Running;
    }

    public void StartPetting()
    {
        _timerPetting = 0f;
        isInteracting = true;
        Debug.Log("acariciando al gato...");

        // futuro: cuando tengamos animaciones
        // animator.SetBool("IsPetting", true);
    }

    public Status PerformPetting()
    {
        _timerPetting += Time.deltaTime;

        patience += 20f * Time.deltaTime;

        if (patience > 100f) patience = 100f;

        if (_timerPetting < pettingTime)
        {
            return Status.Running;
        }

        patience = 100f;

        Debug.Log("Termino de acariciar. Paciencia: " + patience);

        // futuro: cuando tengamos animaciones
        // animator.SetBool("IsPetting", false);

        return Status.Success;
    }

    public Status ResumeTask()
    {
        Debug.Log("He terminado con el gato. Vuelvo a lo mío.");

        patience = 100f;

        isInteracting = false;

        return Status.Success;
    }
}