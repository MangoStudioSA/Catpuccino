using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using UnityEngine.AI;

public class TechnicianController : EditorBehaviourRunner
{
    public NavMeshAgent agent;

    public GameObject model;

    public Transform aseosPos;
    public Transform salidaPos;

    [System.NonSerialized] public bool tieneTarea = false;
    [System.NonSerialized] public bool reparacionCompleta = false;

    public bool TieneTareaAsignada()
    {
        if (tieneTarea)
        {
            model.SetActive(true);
        }

        return tieneTarea;
    }

    public bool EstaEnElObjeto()
    {
        if (Vector3.Distance(transform.position, aseosPos.position) < 0.5)
        {
            return true;
        }

        return false;
    }

    public Status ReparacionCompletada()
    {
        if (reparacionCompleta)
        {
            return Status.Success;
        }
        
        return Status.Failure;
    }

    public Status AvanzarASalida()
    {
        agent.SetDestination(salidaPos.position);
        return Status.Success;
    }
    public Status LlegoALaSalida()
    {
        if (Vector3.Distance(transform.position, salidaPos.position) < 0.5)
        {
            model.SetActive(false);
            return Status.Success;
        }

        return Status.Running;
    }

    public Status ResetearTarea()
    {
        tieneTarea = false;
        reparacionCompleta = false;
        return Status.Success;
    }

    public Status AvanzarAObjeto()
    {
        agent.SetDestination(aseosPos.position);
        return Status.Success;
    }

    public Status Reparar()
    {
        return Status.Success;
    }

    public Status InformarReparacion()
    {
        reparacionCompleta = true;
        return Status.Success;
    }

    public Status Esperando()
    {
        return Status.Success;
    }
}
