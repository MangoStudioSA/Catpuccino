using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;

public class CleanerController : EditorBehaviourRunner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Status AseosSucios()
    {
        return Status.Success;
    }

    public Status AvanzarAAseos()
    {
        return Status.Success;
    }

    public Status PosicionAlcanzada()
    {
        return Status.Success;
    }

    public Status LimpiarAseos()
    {
        return Status.Success;
    }

    public Status SuperficieSucia()
    {
        return Status.Success;
    }

    public Status AvanzarASuperficie()
    {
        return Status.Success;
    }

    public Status PosicionMasCercanaAlcanzada()
    {
        return Status.Success;
    }

    public Status LimpiarSuperficie()
    {
        return Status.Success;
    }

    public Status NadaSucio()
    {
        return Status.Success;
    }

    public Status AvanzarASiguientePuntoDeRuta()
    {
        return Status.Success;
    }

    public Status HaLLegadoAlDestino()
    {
        return Status.Success;
    }

    public Status RepetirRutaDePatrulla()
    {
        return Status.Success;
    }
}
