using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;

public class ManagerController : EditorBehaviourRunner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Status NotificacionAveria()
    {
        return Status.Success;
    }

    public Status ComprobarTecnico()
    {
        return Status.Success;
    }
    public Status TecnicoDisponible()
    {
        return Status.Success;
    }

    public Status LlamarTecnico()
    {
        return Status.Success;
    }

    public Status ClienteFrustrado()
    {
        return Status.Success;
    }

    public Status CalmarCliente()
    {
        return Status.Success;
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
