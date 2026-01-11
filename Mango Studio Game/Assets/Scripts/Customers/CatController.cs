using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using UnityEngine.AI;

public class CatController : EditorBehaviourRunner
{
    NavMeshAgent agent;

    public Transform[] puntosDivagar;
    public Transform esconditePos;

    [System.NonSerialized] public bool acariciado;
    [System.NonSerialized] public bool divagando;
    [System.NonSerialized] public float paciencia;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        acariciado = false;
        paciencia = 100;
    }

    public void Divagando()
    {
        divagando = true;
        if (agent.isStopped)
        {
            agent.SetDestination(puntosDivagar[Random.Range(0, puntosDivagar.Length)].position);
        }
    }

    public void SiendoAcariciado()
    {
        divagando = false;
        paciencia = Mathf.Max(paciencia - Time.deltaTime * 2, 0);
    }

    public void AvanzaAEscondite()
    {
        agent.SetDestination(esconditePos.position);
    }

    public void Escondido()
    {
        paciencia = Mathf.Min(paciencia + Time.deltaTime * 2, 100);
    }

    public bool ClienteAcaricia()
    {
        return acariciado;
    }

    public bool InteraccionFinaliza()
    {
        return !acariciado;
    }

    public bool PacienciaAgotada()
    {
        return paciencia <= 0;
    }

    public bool EstaEnEscondite()
    {
        return Vector3.Distance(transform.position, esconditePos.position) < 0.5;
    }

    public bool TiempoEscondidoFinalizado()
    {
        return paciencia >= 100;
    }
}
