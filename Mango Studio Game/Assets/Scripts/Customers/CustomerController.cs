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
    public bool leaving = false;
    float leavingCounter = 0;

    public Animator anim;


    NavMeshAgent agent;
    bool acabaDeEntrar;
    bool haIdoAlBanyo;
    bool haUsadoBanyo;
    float paciencia;
    ManagerController gerente;
    Transform salidaPos;
    Transform aseosPos;
    Transform colaBanyo;
    Transform cola;

    void Awake()
    {
        manager = GameObject.FindWithTag("CustomerManager").GetComponent<CustomerManager>();
        manager.customers.Enqueue(this);
        model = Random.Range(0, (int)transform.childCount); // Se accede al prefab de los distintos sprites y fbx de clientes
        transform.GetChild(model).gameObject.SetActive(true);

        anim = transform.GetChild(model).GetChild(0).GetComponent<Animator>();


        acabaDeEntrar = true;
        haIdoAlBanyo = false;
        haUsadoBanyo = false;
        paciencia = 100;
        agent = GetComponent<NavMeshAgent>();
        gerente = FindFirstObjectByType<ManagerController>();
        salidaPos = GameObject.FindWithTag("Salida").transform;
        aseosPos = GameObject.FindWithTag("Aseos").transform;
        colaBanyo = GameObject.FindWithTag("AseosCola").transform;
        cola = GameObject.FindWithTag("Cola").transform;
    }

    void Update()
    {
        if (leavingCounter >= 10)
        {
            Destroy(this.gameObject);
        }

        if (!leaving)
        {
            if (!atCounter && !atQueue)
            {
                transform.Translate(direction.normalized * speed * Time.deltaTime);
                anim.SetBool("Idle", false);
            }
            else
            {
                anim.SetBool("Idle", true);
            }
        }
        else
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);
            leavingCounter += Time.deltaTime;
        }

        if (!atCounter && manager.customers.Count > 0 && manager.customers.Peek() == this)
        {
            atQueue = false;
        }


        paciencia -= Time.deltaTime * 2;
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
        agent.SetDestination(gerente.transform.position);
        return Status.Success;
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
            haIdoAlBanyo = Random.Range(0, 100) < 20;

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
        transform.position = cola.position;
        haUsadoBanyo = true;
        manager.clientsBathroom -= 1;
        return Status.Success;
    }

    public Status Esperar()
    {
        if (!atQueue && !atCounter)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);
        }

        return Status.Success;
    }

    public Status GatoEnRango()
    {
        return Status.Success;
    }

    public Status NecesidadDeAcariciar()
    {
        return Status.Success;
    }

    public Status AcariciarGato()
    {
        return Status.Success;
    }

    public Status PedidoRecibido()
    {
        return Status.Success;
    }

    public Status TipoPedidoLlevar()
    {
        return Status.Success;
    }

    public Status TipoPedidoTomar()
    {
        return Status.Success;
    }

    public Status BuscarAsiento()
    {
        return Status.Success;
    }

    public Status AvanzarAAsiento()
    {
        return Status.Success;
    }

    public Status ConsumirPedido()
    {
        return Status.Success;
    }

    public Status LiberarAsiento()
    {
        return Status.Success;
    }

    public Status HaPedido()
    {
        return Status.Success;
    }

    public Status EsperarEnMostrador()
    {
        return Status.Success;
    }

    public Status RecogerPedido()
    {
        return Status.Success;
    }

    public Status PedidoEntregado()
    {
        return Status.Success;
    }

    public Status PedirYPagar()
    {
        return Status.Success;
    }

    public Status AvanzarAColaMostrador()
    {
        return Status.Success;
    }
}
