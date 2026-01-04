using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;

public class TechnicianController : EditorBehaviourRunner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TieneTareaAsignada()
    {
        return true;
    }

    public bool EstaEnElObjeto()
    {
        return true;
    }

    public Status ReparacionEnCurso()
    {
        return Status.Success;
    }

    public Status AvanzarASalida()
    {
        return Status.Success;
    }
    public Status LlegoALaSalida()
    {
        return Status.Success;
    }

    public Status ResetearTarea()
    {
        return Status.Success;
    }

    public Status AvanzarAObjeto()
    {
        return Status.Success;
    }

    public Status Reparar()
    {
        return Status.Success;
    }

    public Status InformarReparacion()
    {
        return Status.Success;
    }

    public Status Esperando()
    {
        return Status.Success;
    }
}
