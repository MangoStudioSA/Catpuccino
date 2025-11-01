using UnityEngine;
using TMPro;

[System.Serializable]
public class EvaluationResult
{
    public int score;
    public int moneyEarned;
}

public class CoffeeGameManager : MonoBehaviour
{
    [Header("Referencias")]
    public CustomerOrder npc;
    public PlayerOrder player;
    public OrderEvaluation evaluation;
    [SerializeField] private GameManager gameManager;

    [Header("UI Texts")]
    public TextMeshProUGUI orderFeedbackTxt;
    public TextMeshProUGUI earnedMoneyTxt;
    public TextMeshProUGUI servedCustomersTxt;
    public TextMeshProUGUI earnedTipTxt;

    private int totalScore = 0;
    private int customersServed = 0;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();
    }
    public void SubmitOrder()
    {
        // Se calcula la puntuacion del pedido comparando lo que se pedia con el resultado del jugador
        EvaluationResult result = evaluation.Evaluate(npc.currentOrder, player.currentOrder);

        if (result == null)
        {
            Debug.LogWarning("Evaluation result null");
            return;
        }

        // Se suma la puntuacion obtenida a la total y se suma 1 al numero de clientes atendidos
        totalScore += result.score;
        customersServed++;
        // Añade el dinero y la puntuacion para calcular la satisfaccion media
        gameManager.AnadirMonedas(result.moneyEarned);
        gameManager.AddSatisfactionPoint(result.score);

        // Se calcula la propina obtenida
        int tip = CalculateTip(result.score);
        // Se mostrara un feedback distinto en funcion de la puntuacion obtenida
        if (tip > 0) 
        {
            gameManager.AnadirMonedas(tip);
            earnedTipTxt.text = $"¡El cliente ha dejado una propina de {tip}$!";
        }
        else 
        {
            earnedTipTxt.text = "El cliente no ha dejado propina.";
        }

        orderFeedbackTxt.text = GenerateFeedbackText(result.score);
        earnedMoneyTxt.text = $"¡Has ganado {result.moneyEarned}$!";
        servedCustomersTxt.text = customersServed == 1
            ? "¡Ya has servido a 1 cliente en la jornada de hoy!"
            : $"¡Ya has servido a {customersServed} clientes en la jornada de hoy!";
    }

    // Funcion para calcular la propina
    private int CalculateTip(int score)
    {
        if (score <= 80) return 0;
        if (score <= 92) return 1;
        else return 2;
    }
    // Funcion para generar el texto en funcion de la puntuacion obtenida
    private string GenerateFeedbackText(int score)
    {
        if (score <= 40) return "Esto no es lo que había pedido...";
        else if (score <= 80) return "No está mal.";
        else return "¡Me encanta! ¡Es justo lo que había pedido!";
    }

}
