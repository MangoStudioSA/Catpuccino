using UnityEngine;
using TMPro;

public class CoffeeGameManager : MonoBehaviour
{
    public CustomerOrder npc;
    public PlayerOrder player;
    public OrderEvaluation evaluation;
    public TextMeshProUGUI orderFeedbackTxt;
    public TextMeshProUGUI earnedMoneyTxt;
    public TextMeshProUGUI servedCustomersTxt;

    private int totalScore = 0;
    private int customersServed = 0;


    void Start()
    {
        //npc.GenRandomOrder();     
    }

    public void SubmitOrder()
    {
        int playerScore = evaluation.Evaluate(npc.currentOrder, player.currentOrder);
        totalScore += playerScore;
        customersServed++;

        // Añade monedas dependiendo de la puntuación
        GameManager.Instance.AnadirMonedas(playerScore);

        //calcula la satisfaccion
        GameManager.Instance.AddSatisfactionPoint(playerScore);

        if (playerScore <= 20)
        {
            orderFeedbackTxt.text = "Esto no es lo que había pedido...";
        }
        else if (playerScore <= 40)
        {
            orderFeedbackTxt.text = "No está mal.";
        }
        else
        {
            orderFeedbackTxt.text = "¡Me encanta! ¡Es justo lo que había pedido!";
        }

        earnedMoneyTxt.text = $"¡Has ganado {playerScore:F2}$!";
        servedCustomersTxt.text = $"¡Ya has servido a {customersServed:F0} clientes en la jornada de hoy!";

    }


}
