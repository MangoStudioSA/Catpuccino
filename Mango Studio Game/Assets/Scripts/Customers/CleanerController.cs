using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using UnityEngine.AI;

public class CleanerController : EditorBehaviourRunner
{
    NavMeshAgent agent;

    public Transform aseosPos;
    public GameObject[] superficies;
    GameObject superficieSucia;

    public Transform[] puntosRuta;
    int iRuta;

    [System.NonSerialized] public bool aseosSucios;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        aseosSucios = false;
        iRuta = 0;
    }

    public Status AseosSucios()
    {
        if (aseosSucios)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status AvanzarAAseos()
    {
        agent.SetDestination(aseosPos.position);
        return Status.Success;
    }

    public Status PosicionAlcanzada()
    {
        if (Vector3.Distance(transform.position, aseosPos.position) < 0.5)
        {
            return Status.Success;
        }
        
        return Status.Failure;
    }

    public Status LimpiarAseos()
    {
        return Status.Success;
    }

    public Status SuperficieSucia()
    {
        superficieSucia = null;
        foreach (GameObject sup in superficies)
        {
            if (sup.gameObject.activeSelf)
            {
                superficieSucia = sup;
                break;
            }
        }

        if (Random.Range(0, 100) < 20 && superficieSucia == null)
        {
            int idx = Random.Range(0, superficies.Length);
            superficies[idx].gameObject.SetActive(true);
            superficieSucia = superficies[idx];
        }

        if (superficieSucia != null)
        {
            return Status.Success;
        }
        
        return Status.Failure;
    }

    public Status AvanzarASuperficie()
    {
        agent.SetDestination(superficieSucia.transform.position);
        return Status.Success;
    }

    public Status PosicionMasCercanaAlcanzada()
    {
        if (Vector3.Distance(transform.position, superficieSucia.transform.position) < 0.5)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status LimpiarSuperficie()
    {
        superficieSucia.SetActive(false);
        return Status.Success;
    }

    public Status NadaSucio()
    {
        return Status.Success;
    }

    public Status AvanzarASiguientePuntoDeRuta()
    {
        agent.SetDestination(puntosRuta[iRuta].position);
        return Status.Success;
    }

    public Status HaLLegadoAlDestino()
    {
        if (Vector3.Distance(transform.position, puntosRuta[iRuta].position) < 0.5)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status RepetirRutaDePatrulla()
    {
        iRuta++;

        if (iRuta > puntosRuta.Length - 1)
        {
            iRuta = 0;
        }

        return Status.Success;
    }
}
