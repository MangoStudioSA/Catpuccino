using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using NUnit.Framework.Internal.Filters;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [Header("Ajustes Gato")]
    public Transform gatoObject;
    public float distancDetection = 10.0f;

    public float catNecesity = 60.0f;
    private bool isInteracting = false;

    private float _timerPetting = 0f;
    public float pettingTime = 3f;

    public float timesPet = 0;
    public float maxPetting = 2;

    [Header("Sistema de Baño")]
    public bool needsBathroom = false;
    public Transform bathroomPoint; 
    private bool isInBathroomQueue = false;
    private float _bathroomTimer = 0f;
    public float bathroomDuration = 4.0f;


    [Header("Referencias Tienda")]
    public Transform exitPoint; 
    public bool hasOrdered = false;

    [Header("Sistema de Comidas y Pedidos")]
    public bool orderIsEatIn = false; 
    public bool receivedOrder = false; 

    private Seat currentSeat = null;  
    private float _eatingTimer = 0f;  
    public float eatingDuration = 5.0f; 

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
            Debug.LogError("no se encuentra gato");
        }

        if (Random.Range(0, 2) == 0)
        {
            patient = true;
        }
        else
        {
            patient = false;
        }

        if (Random.Range(0, 2) == 0) orderIsEatIn = true; 
        else orderIsEatIn = false; 
    }

    public Status GoToQueue()
    {
        if (isInteracting) return Status.Running;

        if (!atCounter && !atQueue)
        {
            if (manager != null)
            {
                Vector3 target = manager.transform.position;
                target.y = transform.position.y;
                transform.LookAt(target);
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


    #region baño
    public Status CheckNeedBathroom()
    {
        if (needsBathroom && !atCounter)
        {
            return Status.Success;
        }

        if (!isInteracting && Random.Range(0, 2000) < 1)
        {
            needsBathroom = true;
            Debug.Log("necesito ir al baño.");
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status ResetPatience()
    {
        patience = 100f;
        Debug.Log("Reseteo mi paciencia");
        return Status.Success;
    }

    public Status GoToBathroomQueue()
    {
        if (bathroomPoint == null)
        {
            if (manager != null && manager.spawnBathroom != null)
                bathroomPoint = manager.spawnBathroom.transform;
            else return Status.Failure;
        }

        if (isInBathroomQueue) return Status.Success;

        Vector3 miPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 banoPos = new Vector3(bathroomPoint.position.x, 0, bathroomPoint.position.z);

        if (Vector3.Distance(miPos, banoPos) > 0.5f)
        {
            Vector3 dir = (bathroomPoint.position - transform.position).normalized;
            dir.y = 0;
            transform.Translate(dir * speed * Time.deltaTime, Space.World);

            Vector3 lookPos = new Vector3(bathroomPoint.position.x, transform.position.y, bathroomPoint.position.z);
            transform.LookAt(lookPos);

            return Status.Running;
        }

        if (!isInBathroomQueue && manager != null)
        {
            manager.customersBathroom.Enqueue(this);
            isInBathroomQueue = true;

            atQueue = false;
            atNormalQueue = false;
        }
        return Status.Success;
    }

    public Status CheckBathroomQueueFull()
    {
        if (manager != null)
        {
            if (manager.customersBathroom.Count > manager.maxClientsBathroom)
            {
                Debug.Log("Cola del baño llena. Me voy.");
                return Status.Success; 
            }
        }
        return Status.Failure;
    }

    public Status CheckAtBathroomQueue()
    {
        if (isInBathroomQueue) return Status.Success;
        return Status.Failure;
    }

    public Status CheckBathroomTurn()
    {
        if (manager != null && manager.customersBathroom.Count > 0)
        {
            if (manager.customersBathroom.Peek() == this)
            {
                return Status.Success;
            }
        }
        return Status.Failure;
    }

    public Status UseBathroom()
    {
        isInteracting = true;

        _bathroomTimer += Time.deltaTime;

        if (_bathroomTimer < bathroomDuration)
        {
            return Status.Running;
        }

        Debug.Log("¡Fin del baño! Saliendo...");

        needsBathroom = false;
        _bathroomTimer = 0;

        isInBathroomQueue = false;

        if (manager != null && manager.customersBathroom.Count > 0)
        {
            if (manager.customersBathroom.Peek() == this)
            {
                manager.customersBathroom.Dequeue();
            }
        }

        isInteracting = false;

        return Status.Success;
    }

    #endregion

    void Update()
    {
        if (isInteracting) return;

        float decrease = patient ? patienceDecrease : (patienceDecrease * 2);
        patience -= decrease * Time.deltaTime;
        if (patience < 0) patience = 0;

        if (!needsBathroom && !atCounter && Random.Range(0, 1000) < 1)
        {
            needsBathroom = true;
            Debug.Log("necesito ir al baño");
        }
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

    #region rama cliente gato
    public Status CheckNeedToPet()
    {
        if (timesPet >= maxPetting) return Status.Failure;

        Debug.Log($"comprobando Paciencia: {patience}");

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

        if (distanciaPlana < 0.5f)
        {
            Debug.Log("[4] llega a gato");
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

        Debug.Log("termino de acariciar. Paciencia: " + patience);

        return Status.Success;
    }

    public Status ResumeTask()
    {
        Debug.Log("he terminado con el gato. Vuelvo a lo mío.");

        timesPet++;

        patience = 100f;

        isInteracting = false;

        return Status.Success;
    }
    #endregion

    #region consumir
    public Status CheckOrderReceived()
    {
        if (receivedOrder)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    public Status CheckOrderIsEatIn()
    {
        if (orderIsEatIn) return Status.Success;
        return Status.Failure; 
    }

    public Status FindAndGoToSeat()
    {
        if (currentSeat == null)
        {
            GameObject[] asientos = GameObject.FindGameObjectsWithTag("Asiento");
            foreach (GameObject obj in asientos)
            {
                Seat seat = obj.GetComponent<Seat>();
                if (seat != null && !seat.isOccupied)
                {
                    currentSeat = seat;
                    currentSeat.isOccupied = true;
                    break;
                }
            }
            if (currentSeat == null) return Status.Failure;
        }

        Vector3 miPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 asientoPos = new Vector3(currentSeat.transform.position.x, 0, currentSeat.transform.position.z);

        float distanciaPlana = Vector3.Distance(miPos, asientoPos);

        if (distanciaPlana > 1.0f)
        {
            Vector3 target = currentSeat.transform.position;
            target.y = transform.position.y; 
            transform.LookAt(target);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            return Status.Running;
        }

        transform.position = new Vector3(currentSeat.transform.position.x, transform.position.y, currentSeat.transform.position.z);
        transform.rotation = currentSeat.transform.rotation; 

        atQueue = false;
        atCounter = false;

        Debug.Log("Sentado en la mesa.");
        return Status.Success;
    }

    public void StartEating()
    {
        _eatingTimer = 0f;
        isInteracting = true;
        Debug.Log("empiezo a comer...");
    }

    public Status EatFood()
    {
        isInteracting = true;
        _eatingTimer += Time.deltaTime;

        if (_eatingTimer < eatingDuration)
        {
            return Status.Running;
        }

        Debug.Log("He terminado de comer. Estaba rico.");
        isInteracting = false; 
        return Status.Success;
    }

    public Status ReleaseSeat()
    {
        if (currentSeat != null)
        {
            currentSeat.isOccupied = false; 
            currentSeat = null;
        }
        return Status.Success;
    }

    #endregion

    #region rama cola y paciencia
    public Status CheckAtQueue()
    {
        if (atQueue || atCounter)
        {
            return Status.Success;
        }
        return Status.Failure;
    }

    public Status CheckPatienceDepleted()
    {
        if (patience <= 0)
        {
            Debug.Log("me voy");
            return Status.Success; 
        }
        return Status.Failure; 
    }

    public Status GoToExit()
    {
        if (exitPoint == null)
        {
            GameObject salida = GameObject.FindGameObjectWithTag("Salida"); 
            if (salida != null) exitPoint = salida.transform;
            else return Status.Failure;
        }

        float distancia = Vector3.Distance(transform.position, exitPoint.position);

        if (distancia > 1.0f)
        {
            Vector3 direction = (exitPoint.position - transform.position).normalized;
            direction.y = 0;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            Vector3 lookPos = new Vector3(exitPoint.position.x, transform.position.y, exitPoint.position.z);
            transform.LookAt(lookPos);

            return Status.Running;
        }

        return Status.Success;
    }

    public Status LeaveShop()
    {
        Debug.Log("cliente se va ).");

        if (manager != null)
        {
            manager.clients--;

            if (manager.orderingCustomer == this.gameObject)
            {
                manager.orderingCustomer = null;
                manager.orderButton.SetActive(false);
            }
        }

        Destroy(this.gameObject);
        return Status.Success;
    }

    public Status CheckIsMyTurn()
    {
        if (atCounter)
        {
            return Status.Success;
        }
        return Status.Failure;
    }

    public Status OrderAndPay()
    {
        Debug.Log("haciendo pedido...");

        hasOrdered = true;

        return Status.Success;
    }

    public Status WaitInQueue()
    {
        return Status.Running;
    }
    #endregion
}