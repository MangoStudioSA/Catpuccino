using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;

// Clase encargada de gestionar el comportamiento de los gatos
public class CatBehavior : EditorBehaviourRunner
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    private bool hasArrive = false;
    private Transform objetivoMirada;

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (hasArrive) return;

        if (agent != null && agent.enabled && agent.isOnNavMesh && agent.hasPath)
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                Sentarse();
            }
        }
    }

    // Funcion para marcar el destino del gato y el punto de mira
    public void IrASitio(Vector3 destino, Transform objetivoParaMirar)
    {
        objetivoMirada = objetivoParaMirar;

        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (agent == null || animator == null) return;

        StopAllCoroutines();
        hasArrive = false;

        StartCoroutine(MoverSeguro(destino));
    }

    // Corrutina para el movimiento del gato
    IEnumerator MoverSeguro(Vector3 destino)
    {
        yield return null; // Esperamos 1 frame

        if (animator != null && animator.gameObject.activeInHierarchy)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isSitting", false);
        }

        if (agent.isOnNavMesh)
        {
            agent.isStopped = false; // Se llama a "Resume"
            agent.SetDestination(destino);
        }
        else
        {
            agent.Warp(transform.position);
            agent.SetDestination(destino);
        }
    }

    // Funcion para que el gato reproduzca la animacion de sentarse
    void Sentarse()
    {
        hasArrive = true;

        // Paramos al agente
        if (agent != null) agent.isStopped = true;

        // Cambiamos las animaciones
        if (animator != null)
        {
            animator.SetBool("isWalking", false); // Dejar de andar
            animator.SetTrigger("sitDown");       // Accion de sentarse
            animator.SetBool("isSitting", true);  // Bool de quedarse sentado
        }

        if (objetivoMirada != null)
        {
            StartCoroutine(RotarSuavemente(objetivoMirada));
        }
    }

    // Corrutina para girarse hacia el punto de mira
    IEnumerator RotarSuavemente(Transform target)
    {
        // Se calcula la direccion ignorando la altura
        Vector3 direccion = target.position - transform.position;
        direccion.y = 0;

        if (direccion != Vector3.zero)
        {
            Quaternion rotacionFinal = Quaternion.LookRotation(direccion);

            float tiempo = 0;
            while (tiempo < 1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionFinal, tiempo * 2f); // *2f - velocidad de giro
                tiempo += Time.deltaTime;
                yield return null;
            }

            transform.rotation = rotacionFinal; // Rotacion final
        }
    }

    // Funcion para teletransporta los gatos que tengan marcada la casilla de "Mostrador"
    public void Teletransportarse(Vector3 destino, Transform mirarA)
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // Se apaga y teletransporta el gato
        if (agent != null) agent.enabled = false;
        transform.position = destino;

        // Se orienta hacia el objetivo
        if (mirarA != null)
        {
            Vector3 direccion = mirarA.position - transform.position;
            direccion.y = 0;
            if (direccion != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direccion);
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isSitting", true);

            animator.Play("Sentado", 0, 0f);
        }
    }

    public void Divagando()
    {

    }

    public void SiendoAcariciado()
    {

    }

    public void AvanzaAEscondite()
    {

    }

    public void Escondido()
    {

    }

    public bool ClienteAcaricia()
    {
        return false;
    }

    public bool InteraccionFinaliza()
    {
        return false;
    }

    public bool PacienciaAgotada()
    {
        return false;
    }

    public bool EstaEnEscondite()
    {
        return false;
    }

    public bool TiempoEscondidoFinalizado()
    {
        return false;
    }
}
