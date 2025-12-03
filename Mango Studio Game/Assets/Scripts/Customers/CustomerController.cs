using UnityEngine;

// Clase encargada de gestionar la posicion y spawn de clientes
public class CustomerController : MonoBehaviour
{
    [Header("Referencias")]
    public float speed = 5f;
    public Vector3 direction = Vector3.forward;
    public bool atCounter = false, atQueue = false;
    public CustomerManager manager;
    public int model = 0;

    void Start()
    {
        manager = GameObject.FindWithTag("CustomerManager").GetComponent<CustomerManager>();
        manager.customers.Enqueue(this);
        model = Random.Range(0, (int)transform.childCount); // Se accede al prefab de los distintos sprites y fbx de clientes
        transform.GetChild(model).gameObject.SetActive(true);
    }

    void Update()
    {
        if (!atCounter && !atQueue)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);
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
        else
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
        else
        {
            atQueue= false;
        }
    }
}
