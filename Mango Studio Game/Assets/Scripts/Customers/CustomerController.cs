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
    CustomerManager manager;
    public int model = 0;
    public Animator anim;


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
    public int queueIndex;
    ManagerController gerente;
    CleanerController cleaner;
    CatController gato;
    Transform salidaPos;
    Transform aseosPos;
    Transform colaBanyo;
    Transform cola;
    Transform asientoOcupado;

    //void Update()
    //{
    //    //if (leavingCounter >= 10)
    //    //{
    //    //    Destroy(this.gameObject);
    //    //}

    //    //if (!leaving)
    //    //{
    //    //    if (!atCounter && !atQueue)
    //    //    {
    //    //        transform.Translate(direction.normalized * speed * Time.deltaTime);
    //    //        anim.SetBool("Idle", false);
    //    //    }
    //    //    else
    //    //    {
    //    //        anim.SetBool("Idle", true);
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    transform.Translate(direction.normalized * speed * Time.deltaTime);
    //    //    leavingCounter += Time.deltaTime;
    //    //}

    //    //if (!atCounter && manager.customers.Count > 0 && manager.customers.Peek() == this)
    //    //{
    //    //    atQueue = false;
    //    //}

    //    if (impatient)
    //    {
    //        paciencia -= Time.deltaTime * 2;
    //    }
    //    else
    //    {
    //        paciencia -= Time.deltaTime;
    //    }
    //}

    // Funcion que comprueba si el cliente ha llegado al mostrador
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("CustomerManager"))
        {
            atCounter = true;

            if (!haIdoAlBanyo || (haIdoAlBanyo && haUsadoBanyo))
            {
                manager.orderButton.SetActive(true);
                manager.orderingCustomer = this.gameObject;
            }

            CoffeeGameManager coffeManager = FindFirstObjectByType<CoffeeGameManager>();
            if (coffeManager != null)
            {
                coffeManager.MostrarCliente(model);
            }
        }
        else if (other.gameObject.CompareTag("Customer") && other.gameObject.transform.position.z > transform.position.z)
        {
            atQueue = true;
        }
    }

    // Funcion que comprueba si el cliente se ha ido del mostrador
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("CustomerManager"))
        {
            atCounter = false;

            if (haPedido)
            {
                manager.orderingCustomer = null;
            }

            CoffeeGameManager coffeManager = FindFirstObjectByType<CoffeeGameManager>();
            if (coffeManager != null)
            {
                coffeManager.MostrarCliente(-1);
            }
        }
        else if (other.gameObject.CompareTag("Customer") && other.gameObject.transform.position.z > transform.position.z)
        {
            atQueue = false;
        }
    }

    public Status AcabaDeEntrar()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (acabaDeEntrar)
        {
            manager = GameObject.FindWithTag("CustomerManager").GetComponent<CustomerManager>();
            manager.customers.Enqueue(this);
            queueIndex = manager.customers.Count - 1;
            model = Random.Range(0, (int)transform.childCount - 1);

            anim = transform.GetChild(model).GetChild(0).GetComponent<Animator>();

            impatient = Random.Range(0, 100) < 50;

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
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (manager.clients > manager.maxClients)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status BuscarGerente()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (atCounter && manager.orderingCustomer == this.gameObject)
        {
            manager.orderButton.SetActive(false);
            manager.orderingCustomer = null;
        }

        gerente.hayClienteFrustrado = true;
        return Status.Success;
    }

    public Status AvanzarAGerente()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        transform.position = new Vector3(gerente.transform.position.x - 1, transform.position.y, gerente.transform.position.z - 1);
        return Status.Success;
    }

    public Status Quejarse()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        gerente.hayClienteFrustrado = false;
        return Status.Success;
    }

    public Status AvanzarASalida()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        transform.position = new Vector3(salidaPos.position.x, transform.position.y, salidaPos.position.z);
        return Status.Success;
    }

    public Status Irse()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

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
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (atCounter && (!haIdoAlBanyo || (haIdoAlBanyo && haUsadoBanyo)))
        {
            return Status.Failure;
        }

        if (!haIdoAlBanyo)
        {
            if (Random.Range(0, 100) < 1)
            {
                haIdoAlBanyo = true;
                manager.clientsBathroom += 1;
                transform.position = new Vector3(colaBanyo.position.x, transform.position.y, colaBanyo.position.z);
            }
        }

        if (haIdoAlBanyo && !haUsadoBanyo)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status PausarTareaActual()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        return Status.Success;
    }

    public Status ResetearPaciencia()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        paciencia = 100;
        return Status.Success;
    }

    public Status AvanzarAColaBanyo()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (atQueue || atCounter)
        {
            return Status.Success;
        }

        transform.Translate(direction.normalized * speed * Time.deltaTime);
        return Status.Running;
    }

    public Status BanyoMantenimiento()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (gerente.aseosAveriados)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status ColaBanyoLlena()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (manager.clientsBathroom > manager.maxClientsBathroom)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status ReanudarTarea()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        return Status.Success;
    }

    public Status EstaEnCola()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (atQueue || atCounter)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status Paciencia0()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (paciencia <= 0)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status EsMiTurno()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (atCounter)
        {
            return Status.Success;
        }
        
        return Status.Failure;
    }

    public Status UsarBanyo()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (Random.Range(0, 100) < 20)
        {
            gerente.aseosAveriados = true;
        }

        if (Random.Range(0, 100) < 33)
        {
            cleaner.aseosSucios = true;
        }

        transform.position = new Vector3(cola.position.x, transform.position.y, cola.position.z);
        haUsadoBanyo = true;
        manager.clientsBathroom -= 1;
        return Status.Success;
    }

    public Status Esperar()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        return Status.Success;
    }

    public Status GatoEnRango()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (gato.divagando && !gato.acariciado)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status NecesidadDeAcariciar()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (!haAcariciadoGato)
        {
            haAcariciadoGato = Random.Range(0, 100) < 1;

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
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        gato.acariciado = false;
        return Status.Success;
    }

    public Status PedidoRecibido()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (pedidoRecibido)
        {
            manager.orderButton.SetActive(false);
            manager.orderingCustomer = null;
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status TipoPedidoLlevar()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (pedidoLlevar)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status TipoPedidoTomar()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (!pedidoLlevar)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status BuscarAsiento()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        for (int i = 0; i < manager.asientos.Length; i++)
        {
            if (!manager.asientosOcupados[i])
            {
                asientoOcupado = manager.asientos[i];
                manager.asientosOcupados[i] = true;
                asientoIdx = i;
                return Status.Success;
            }
        }

        return Status.Failure;
    }

    public Status AvanzarAAsiento()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        transform.position = new Vector3(asientoOcupado.position.x, transform.position.y, asientoOcupado.position.z);
        return Status.Success;
    }

    public Status ConsumirPedido()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        return Status.Success;
    }

    public Status LiberarAsiento()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        asientoOcupado = null;
        manager.asientosOcupados[asientoIdx] = false;
        return Status.Success;
    }

    public Status HaPedido()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (haPedido)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status EsperarEnMostrador()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        return Status.Success;
    }

    public Status PedirYPagar()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        haPedido = true;
        return Status.Success;
    }

    public Status AvanzarAColaMostrador()
    {
        if (impatient)
        {
            paciencia -= Time.deltaTime * 2;
        }
        else
        {
            paciencia -= Time.deltaTime;
        }

        if (atCounter || atQueue)
        {
            return Status.Success;
        }

        transform.Translate(direction.normalized * speed * Time.deltaTime);
        return Status.Running;
    }
}
