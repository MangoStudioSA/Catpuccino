using UnityEngine;
using TMPro;

public class CoffeeGameManager : MonoBehaviour
{
    public CustomerOrder npc;
    public PlayerOrder player;
    public OrderEvaluation evaluation;

    // GameObjects de texto para la interfaz
    public TextMeshProUGUI orderFeedbackTxt;
    public TextMeshProUGUI earnedMoneyTxt;
    public TextMeshProUGUI servedCustomersTxt;
    public TextMeshProUGUI earnedTipTxt;

    private int totalScore = 0;
    private int customersServed = 0;


    void Start()
    {
        //npc.GenRandomOrder();     
    }

    public void SubmitOrder()
    {
        // Se calcula la puntuacion del pedido comparando lo que se pedia con el resultado del jugador
        int playerScore = evaluation.Evaluate(npc.currentOrder, player.currentOrder);
        // Se suma la puntuacion obtenida a la total
        totalScore += playerScore; 
        // Se aumenta en 1 el total de clientes atendidos
        customersServed++;
        // Se inicializa la propina en 0
        int tip = 0;

        // Añade monedas dependiendo de la puntuación
        GameManager.Instance.AnadirMonedas(playerScore);

        // Calcula la satisfaccion del cliente
        GameManager.Instance.AddSatisfactionPoint(playerScore);

        // Se mostrara un feedback distinto en funcion de la puntuacion obtenida
        if (playerScore <= 40) 
        {
            orderFeedbackTxt.text = "Esto no es lo que había pedido...";
        }
        else if (playerScore <= 90)
        {
            orderFeedbackTxt.text = "No está mal.";
        }
        else if (playerScore > 90)
        {
            orderFeedbackTxt.text = "¡Me encanta! ¡Es justo lo que había pedido!";
        }
        else
        {
            orderFeedbackTxt.text = "¡Me encanta! ¡Es justo lo que había pedido!";
            tip += 7;
            // Añade la propina a los ingresos totales
            GameManager.Instance.AnadirMonedas(tip);
        }
        earnedMoneyTxt.text = $"¡Has ganado {playerScore:F2}$!";

        // Se mostrara un feedback distinto en funcion del numero de clientes atendidos
        if (customersServed == 1)
        {
            servedCustomersTxt.text = $"¡Ya has servido a {customersServed:F0} cliente en la jornada de hoy!";
        }
        else
        {
            servedCustomersTxt.text = $"¡Ya has servido a {customersServed:F0} clientes en la jornada de hoy!";
        }

        // Se mostrara un feedback distinto en funcion de si hay o no propina
        if (tip > 0)
        {
            earnedTipTxt.text = $"¡El cliente ha dejado una propina de {tip:F0}$!";
        }
        else
        {
            earnedTipTxt.text = $"El cliente no ha dejado propina.";
        }
    }


}
