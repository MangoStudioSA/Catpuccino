using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using UnityEngine;

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

    void Start()
    {
        manager = GameObject.FindWithTag("CustomerManager").GetComponent<CustomerManager>();
        manager.customers.Enqueue(this);
        model = Random.Range(0, (int)transform.childCount); // Se accede al prefab de los distintos sprites y fbx de clientes
        transform.GetChild(model).gameObject.SetActive(true);

        anim = transform.GetChild(model).GetChild(0).GetComponent<Animator>();
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
        return Status.Success;
    }

    public Status ColaMostradorLlena()
    {
        return Status.Success;
    }

    public Status BuscarGerente()
    {
        return Status.Success;
    }

    public Status AvanzarAGerente()
    {
        return Status.Success;
    }

    public Status Quejarse()
    {
        return Status.Success;
    }

    public Status AvanzarASalida()
    {
        return Status.Success;
    }

    public Status Irse()
    {
        return Status.Success;
    }

    public Status NecesidadDeBanyo()
    {
        return Status.Success;
    }

    public Status PausarTareaActual()
    {
        return Status.Success;
    }

    public Status ResetearPaciencia()
    {
        return Status.Success;
    }

    public Status AvanzarAColaBanyo()
    {
        return Status.Success;
    }

    public Status BanyoMantenimiento()
    {
        return Status.Success;
    }

    public Status ColaBanyoLlena()
    {
        return Status.Success;
    }

    public Status ReanudarTarea()
    {
        return Status.Success;
    }

    public Status EstaEnCola()
    {
        return Status.Success;
    }

    public Status Paciencia0()
    {
        return Status.Success;
    }

    public Status EsMiTurno()
    {
        return Status.Success;
    }

    public Status UsarBanyo()
    {
        return Status.Success;
    }

    public Status Esperar()
    {
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
