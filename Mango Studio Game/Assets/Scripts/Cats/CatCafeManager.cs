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
        public GameObject prefabGato;
        public Transform destino;
        public bool posFija;
        public Sprite iconoGato;

        [System.NonSerialized] public GameObject gatoInstancia;
    }

    public List<InfoGato> listaDeGatos; 
    public Transform puertaEntrada;
    public Transform puntoDeAtencion;

    public void Start()
    {
        for (int i = 0; i < listaDeGatos.Count; i++)
        {
            listaDeGatos[i].gatoInstancia = null;
        }
    }

    // Funcion que recibe los indices de los gatos que deben estar visibles
    public void UpdateCafeCats(List<int> indicesSeleccionados)
    {
        for (int i = 0; i < listaDeGatos.Count; i++)
        {
            bool shouldExist = indicesSeleccionados.Contains(i);
            bool exists = listaDeGatos[i].gatoInstancia != null;

            // El jugador lo ha seleccionado y no esta en escena -> se crea
            if (shouldExist && !exists)
            {
                SpawnearGato(i);
            }
            // El jugador lo ha desmarcado y esta en escena -> se borra
            else if (!shouldExist && exists)
            {
                DespawnearGato(i);
            }
        }
    }

    // Funcion para hacer que el gato spawnee
    void SpawnearGato(int index)
    {
        InfoGato info = listaDeGatos[index];

        // Se instancia y activa el gato en la puerta
        GameObject nuevoGato = Instantiate(info.prefabGato, puertaEntrada.position, puertaEntrada.rotation);
        nuevoGato.SetActive(true);

        // Se añade a la lista de gatos
        listaDeGatos[index].gatoInstancia = nuevoGato;

        GatoBehavior comportamiento = nuevoGato.GetComponent<GatoBehavior>();
        if (comportamiento != null)
        {
            if (info.posFija)
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

    // Funcion para borrar el gato
    void DespawnearGato(int index)
    {
        if (listaDeGatos[index].gatoInstancia != null)
        {
            Destroy(listaDeGatos[index].gatoInstancia);
            listaDeGatos[index].gatoInstancia = null;
        }
    }
}
