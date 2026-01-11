using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

// Clase encargada de gestionar la posicion y spawn de clientes
public class CustomerController : EditorBehaviourRunner
{
    [Header("Referencias")]
    public float speed = 5f;
    public Vector3 direction = Vector3.forward;
    public bool atCounter = false, atQueue = false;
    public CustomerManager manager;
    public int model = 0;
    public Animator anim;


    public NavMeshAgent agent;
    bool acabaDeEntrar = true;
    bool haIdoAlBanyo = false;
    bool haAcariciadoGato = false;
    bool haUsadoBanyo = false;
    bool haPedido = false;
    [System.NonSerialized] public bool pedidoLlevar;
    [System.NonSerialized] public bool pedidoRecibido;
    bool impatient;
    float paciencia = 100;
    int asientoIdx;
    ManagerController gerente;
    CleanerController cleaner;
    CatController gato;
    Transform salidaPos;
    Transform aseosPos;
    Transform colaBanyo;
    Transform cola;
    Transform asientoOcupado;


    void Start()
    {
        Debug.Log("Spawned");
        manager = GameObject.FindWithTag("CustomerManager").GetComponent<CustomerManager>();
        manager.customers.Enqueue(this);
        model = Random.Range(0, (int)transform.childCount); // Se accede al prefab de los distintos sprites y fbx de clientes
        //transform.GetChild(model).gameObject.SetActive(true);

        anim = transform.GetChild(model).GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        //if (leavingCounter >= 10)
        //{
        //    Destroy(this.gameObject);
        //}

        //if (!leaving)
        //{
        //    if (!atCounter && !atQueue)
        //    {
        //        transform.Translate(direction.normalized * speed * Time.deltaTime);
        //        anim.SetBool("Idle", false);
        //    }
        //    else
        //    {
        //        anim.SetBool("Idle", true);
        //    }
        //}
        //else
        //{
        //    transform.Translate(direction.normalized * speed * Time.deltaTime);
        //    leavingCounter += Time.deltaTime;
        //}

        if (!atCounter && manager.customers.Count > 0 && manager.customers.Peek() == this)
        {
            atQueue = false;
        }

        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }
    }

    // Funcion que comprueba si el cliente ha llegado al mostrador
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CustomerManager")
        {
            manager.orderButton.SetActive(true);
            atCounter = true;
            manager.orderingCustomer = this.gameObject;

            CoffeeGameManager coffeManager = FindFirstObjectByType<CoffeeGameManager>();
            if (coffeManager != null)
            {
                coffeManager.MostrarCliente(model);
            }
        }
        else if (other.CompareTag("Customer"))
        {
            atQueue = true;
        }
    }

    // Funcion que comprueba si el cliente se ha ido del mostrador
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CustomerManager")
        {
            atCounter = false;
            manager.orderingCustomer = null;

            CoffeeGameManager coffeManager = FindFirstObjectByType<CoffeeGameManager>();
            if (coffeManager != null)
            {
                coffeManager.MostrarCliente(-1);
            }
        }
        else if (other.CompareTag("Customer"))
        {
            atQueue = false;
        }
    }

    public Status AcabaDeEntrar()
    {
        if (acabaDeEntrar)
        {
            impatient = Random.Range(0, 100) < 50;

            agent = GetComponent<NavMeshAgent>();
            gerente = FindFirstObjectByType<ManagerController>();
            cleaner = FindFirstObjectByType<CleanerController>();
            gato = FindFirstObjectByType<CatController>();
            salidaPos = GameObject.FindWithTag("Salida").transform;
            colaBanyo = GameObject.FindWithTag("AseosCola").transform;
            cola = GameObject.FindWithTag("Cola").transform;

            acabaDeEntrar = false;
            return Status.Success;
        }
        
        return Status.Failure;
    }

    public Status ColaMostradorLlena()
    {
        if (manager.clients > manager.maxClients)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status BuscarGerente()
    {
        if (atCounter && manager.orderingCustomer == this.gameObject)
        {
            manager.orderButton.SetActive(false);
            manager.orderingCustomer = null;
        }

        if (!gerente.hayClienteFrustrado)
        {
            agent.SetDestination(gerente.transform.position);
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status AvanzarAGerente()
    {
        if (Vector3.Distance(transform.position, gerente.transform.position) < 0.5)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    public Status Quejarse()
    {
        return Status.Success;
    }

    public Status AvanzarASalida()
    {
        if (agent.isStopped)
        {
            agent.SetDestination(salidaPos.position);
        }

        if (Vector3.Distance(transform.position, salidaPos.position) < 0.5)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    public Status Irse()
    {
        if (haIdoAlBanyo && !haUsadoBanyo)
        {
            manager.clientsBathroom -= 1;
        }

        manager.clients -= 1;
        Destroy(this.gameObject);
        return Status.Success;
    }

    public Status NecesidadDeBanyo()
    {
        if (!haIdoAlBanyo && !atCounter)
        {
            haIdoAlBanyo = Random.Range(0, 100) < 10;

            if (haIdoAlBanyo)
            {
                manager.clientsBathroom += 1;
                transform.position = colaBanyo.position;
                return Status.Success;
            }
        }

        return Status.Failure;
    }

    public Status PausarTareaActual()
    {
        return Status.Success;
    }

    public Status ResetearPaciencia()
    {
        paciencia = 100;
        return Status.Success;
    }

    public Status AvanzarAColaBanyo()
    {
        if (atQueue || atCounter)
        {
            return Status.Success;
        }

        transform.Translate(direction.normalized * speed * Time.deltaTime);
        return Status.Running;
    }

    public Status BanyoMantenimiento()
    {
        if (gerente.aseosAveriados)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status ColaBanyoLlena()
    {
        if (manager.clientsBathroom > manager.maxClientsBathroom)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status ReanudarTarea()
    {
        return Status.Success;
    }

    public Status EstaEnCola()
    {
        if (atQueue || atCounter)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status Paciencia0()
    {
        if (paciencia <= 0)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status EsMiTurno()
    {
        if (atCounter)
        {
            return Status.Success;
        }
        
        return Status.Success;
    }

    public Status UsarBanyo()
    {
        if (Random.Range(0, 100) < 5)
        {
            gerente.aseosAveriados = true;
        }

        if (Random.Range(0, 100) < 10)
        {
            cleaner.aseosSucios = true;
        }

        transform.position = cola.position;
        haUsadoBanyo = true;
        manager.clientsBathroom -= 1;
        return Status.Success;
    }

    public Status Esperar()
    {
        return Status.Success;
    }

    public Status GatoEnRango()
    {
        if (gato.divagando && !gato.acariciado)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status NecesidadDeAcariciar()
    {
        if (!haAcariciadoGato && !atCounter)
        {
            haAcariciadoGato = Random.Range(0, 100) < 10;

            if (haAcariciadoGato)
            {
                gato.acariciado = true;
                return Status.Success;
            }
        }

        return Status.Failure;
    }

    public Status AcariciarGato()
    {
        gato.acariciado = false;
        return Status.Success;
    }

    public Status PedidoRecibido()
    {
        if (pedidoRecibido)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status TipoPedidoLlevar()
    {
        if (pedidoLlevar)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status TipoPedidoTomar()
    {
        if (!pedidoLlevar)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status BuscarAsiento()
    {
        for (int i = 0; i < manager.asientos.Length; i++)
        {
            if (!manager.asientosOcupados[i])
            {
                asientoOcupado = manager.asientos[i];
                manager.asientosOcupados[i] = true;
                asientoIdx = i;
                agent.SetDestination(asientoOcupado.position);
            }
        }

        return Status.Success;
    }

    public Status AvanzarAAsiento()
    {
        if (Vector3.Distance(transform.position, asientoOcupado.position) < 0.5)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    public Status ConsumirPedido()
    {
        return Status.Success;
    }

    public Status LiberarAsiento()
    {
        asientoOcupado = null;
        manager.asientosOcupados[asientoIdx] = false;
        return Status.Success;
    }

    public Status HaPedido()
    {
        if (haPedido)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status EsperarEnMostrador()
    {
        return Status.Success;
    }

    public Status PedirYPagar()
    {
        haPedido = true;
        return Status.Success;
    }

    public Status AvanzarAColaMostrador()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
        return Status.Success;
    }
}
