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

    //gato
    [Header("Ajustes Gato")]
    public Transform gatoObject;
    public float distancDetection = 10.0f;

    public float catNecesity = 60.0f;
    private bool isInteracting = false;

    private float _timerPetting = 0f;
    public float pettingTime = 3f;

    public float timesPet = 0;
    public float maxPetting = 2;


    [Header("Referencias Tienda")]
    public Transform exitPoint; //meterlo en el spawn
    public bool hasOrdered = false;

    [Header("Sistema de Comidas y Pedidos")]
    public bool orderIsEatIn = false; // Si es para tomar o llevar
    public bool receivedOrder = false; // Se activa cuando le das la comida

    // Variables para sentarse y comer
    private Seat currentSeat = null;  // La silla asignada
    private float _eatingTimer = 0f;  // Cronómetro para comer
    public float eatingDuration = 5.0f; // Tiempo que tarda en comer

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

        if (Random.Range(0, 2) == 0) orderIsEatIn = true; // 50%
        else orderIsEatIn = false; // 50%
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

        //Debug.Log($"[3] Distancia Plana: {distanciaPlana}. Gato: {gatoObject.position}");

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

        Debug.Log("termino de acariciar. Paciencia: " + patience);

        // futuro: cuando tengamos animaciones
        // animator.SetBool("IsPetting", false);

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
    // NODO: "¿Pedido recibido?"
    public Status CheckOrderReceived()
    {
        // Si ya tengo mi pedido, paso a la siguiente fase
        if (receivedOrder)
        {
            return Status.Success;
        }

        // Si no, me quedo esperando en el mostrador
        return Status.Running;
    }

    // NODO: "¿Tipo pedido == tomar?"
    public Status CheckOrderIsEatIn()
    {
        if (orderIsEatIn) return Status.Success;
        return Status.Failure; // Si es para llevar, salta a la otra rama
    }

    // NODO: "Buscar Asientos"
    public Status FindAndGoToSeat()
    {
        // 1. Si no tengo silla, busco una
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

        // 2. LÓGICA CORREGIDA: Distancia Plana (Como el gato)
        Vector3 miPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 asientoPos = new Vector3(currentSeat.transform.position.x, 0, currentSeat.transform.position.z);

        float distanciaPlana = Vector3.Distance(miPos, asientoPos);

        // Aumentamos un poco el margen a 0.6f por si el collider es gordo
        if (distanciaPlana > 1.0f)
        {
            // Moverse
            Vector3 target = currentSeat.transform.position;
            target.y = transform.position.y; // Mantener altura
            transform.LookAt(target);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            return Status.Running;
        }

        // 3. AL LLEGAR: ELIMINAR TEMBLEQUE
        // Teletransportamos al cliente al centro exacto de la silla para que quede perfecto
        // (Ajusta la Y si se queda flotando o hundido)
        transform.position = new Vector3(currentSeat.transform.position.x, transform.position.y, currentSeat.transform.position.z);
        transform.rotation = currentSeat.transform.rotation; // Opcional: Que se siente con la misma rotación que la silla

        // Salimos de la cola lógica
        atQueue = false;
        atCounter = false;

        Debug.Log("Sentado en la mesa.");
        return Status.Success;
    }

    public void StartEating()
    {
        _eatingTimer = 0f;
        isInteracting = true; // Bloqueamos bajada de paciencia
        Debug.Log("Empiezo a comer...");
    }

    // NODO: "Consumir pedido"
    public Status EatFood()
    {
        // Usamos la bandera para bloquear la bajada de paciencia
        isInteracting = true;
        _eatingTimer += Time.deltaTime;

        if (_eatingTimer < eatingDuration)
        {
            return Status.Running; // Ñam ñam...
        }

        Debug.Log("He terminado de comer. Estaba rico.");
        isInteracting = false; // Ya he terminado
        return Status.Success;
    }

    // NODO: "Liberar asiento"
    public Status ReleaseSeat()
    {
        if (currentSeat != null)
        {
            currentSeat.isOccupied = false; // Queda libre para el siguiente
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

    // NODO: "Pedir y pagar"
    public Status OrderAndPay()
    {
        Debug.Log("haciendo pedido...");

        hasOrdered = true;

        return Status.Success;
    }


    // NODO: "Esperar"
    public Status WaitInQueue()
    {
        return Status.Running;
    }
    #endregion
}