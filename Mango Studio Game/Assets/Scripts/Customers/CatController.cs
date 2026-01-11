using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using UnityEngine.AI;

public class CatController : EditorBehaviourRunner
{
    public NavMeshAgent agent;

    public Transform[] puntosDivagar;
    public Transform esconditePos;

    [System.NonSerialized] public bool acariciado = false;
    [System.NonSerialized] public bool divagando;
    [System.NonSerialized] public float paciencia = 100;

    public Status Divagando()
    {
        divagando = true;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (agent.velocity.sqrMagnitude == 0f)
            {
                agent.SetDestination(puntosDivagar[Random.Range(0, puntosDivagar.Length)].position);
            }
        }

        return Status.Running;
    }

    public Status SiendoAcariciado()
    {
        divagando = false;
        paciencia = Mathf.Max(paciencia - Time.deltaTime * 2, 0);
        return Status.Running;
    }

    public Status AvanzaAEscondite()
    {
        agent.SetDestination(esconditePos.position);
        return Status.Running;
    }

    public Status Escondido()
    {
        paciencia = Mathf.Min(paciencia + Time.deltaTime * 2, 100);
        return Status.Running;
    }

    public bool ClienteAcaricia()
    {
        return acariciado;
    }

    public bool InteraccionFinalizada()
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
