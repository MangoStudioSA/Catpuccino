using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using UnityEngine.AI;

public class ManagerController : EditorBehaviourRunner
{
    public TechnicianController tech;

    [System.NonSerialized] public bool aseosAveriados = false;
    [System.NonSerialized] public bool hayClienteFrustrado = false;

    public Status NotificacionAveria()
    {
        if (aseosAveriados)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status TecnicoDisponible()
    {
        if (tech != null)
        {
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status LlamarTecnico()
    {
        tech.tieneTarea = true;
        return Status.Success;
    }

    public Status ClienteFrustrado()
    {
        if (hayClienteFrustrado)
        { 
            return Status.Success;
        }

        return Status.Failure;
    }

    public Status CalmarCliente()
    {
        if (hayClienteFrustrado)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    public Status NingunProblema()
    {
        return Status.Success;
    }

    public Status EsperarIncidencias()
    {
        return Status.Success;
    }
}
