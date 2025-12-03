using System.Collections.Generic;
using UnityEngine;

// Clase encargada de gestionar el spawn de los gatos
public class CatCafeManager : MonoBehaviour
{
    // Clase auxiliar con la informacion de cada gato
    [System.Serializable]
    public class InfoGato
    {
        public string nombreCarta;    
        public GameObject prefabGato; // Modelo 3D del gato
        public Transform destino;  // Destino

        [Tooltip("Marcar si el lugar esta fuera del NavMesh")]
        public bool mostrador;

        [System.NonSerialized] public GameObject gatoInstancia;
    }

    public List<InfoGato> listaDeGatos; 
    public Transform puertaEntrada;
    public Transform puntoDeAtencion;

    void Start()
    {
        // Limpieza inicial de seguridad
        for (int i = 0; i < listaDeGatos.Count; i++)
        {
            listaDeGatos[i].gatoInstancia = null;
        }

        RevisarGatosDesbloqueados();
    }

    // Funcion para comprobar que gatos se han desbloqueado
    public void RevisarGatosDesbloqueados()
    {
        for (int i = 0; i < listaDeGatos.Count; i++)
        {
            if (listaDeGatos[i].gatoInstancia != null) // Si ya se encuentra desbloqueado se continua
            {
                continue;
            }

            bool desbloqueado = PlayerDataManager.instance.HasCard(listaDeGatos[i].nombreCarta); // Se accede a las cartas del jugador

            if (desbloqueado)
            {
                SpawnearGato(i);
            }
        }
    }

    // Funcion para hacer que el gato spawnee
    void SpawnearGato(int index)
    {
        InfoGato info = listaDeGatos[index];

        // Instanciar gato en la puerta y activarlo
        GameObject nuevoGato = Instantiate(info.prefabGato, puertaEntrada.position, puertaEntrada.rotation);
        nuevoGato.SetActive(true);

        // Añadirlo a la lista de gatos
        listaDeGatos[index].gatoInstancia = nuevoGato;

        GatoBehavior comportamiento = nuevoGato.GetComponent<GatoBehavior>();
        if (comportamiento != null)
        {
            if (info.mostrador)
            {
                comportamiento.Teletransportarse(info.destino.position, puntoDeAtencion); // NO tener en cuenta el navmesh y colocar el gato en una posicion concreta
            }
            else // Generar el gato en la puerta y marcar su destino
            {
                nuevoGato.transform.position = puertaEntrada.position;
                nuevoGato.transform.rotation = puertaEntrada.rotation;
                comportamiento.IrASitio(info.destino.position, puntoDeAtencion);
            }
        }
    }
}
