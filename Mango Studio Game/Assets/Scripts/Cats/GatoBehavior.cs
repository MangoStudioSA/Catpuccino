using UnityEngine;

public class GatoBehavior : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    private bool estaSentado = false;

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Llamamos a esta funci�n para decirle a qu� silla ir
    public void IrASitio(Vector3 destino)
    {
        // Activamos animaci�n de andar
        animator.SetBool("isWalking", true);
        animator.SetBool("isSitting", false);

        agent.SetDestination(destino);
    }

    void Update()
    {
        // Si el gato est� caminando y le falta poco para llegar
        if (!estaSentado && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Sentarse();
        }
    }

    void Sentarse()
    {
        estaSentado = true;
        agent.isStopped = true; // Detenemos el NavMesh

        // Cambiamos animaciones
        animator.SetBool("isWalking", false);
        animator.SetTrigger("sitDown"); // Trigger para la transici�n de sentarse
        animator.SetBool("isSitting", true); // Loop de estar sentado

        // Opcional: Rotar el gato para que mire al frente de la silla
        // transform.rotation = ...
    }
}
