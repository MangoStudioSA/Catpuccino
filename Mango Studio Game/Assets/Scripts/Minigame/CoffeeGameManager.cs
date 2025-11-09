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

    [Header("UI Texts")]
    public TextMeshProUGUI orderFeedbackTxt;
    public TextMeshProUGUI earnedMoneyTxt;
    public TextMeshProUGUI servedCustomersTxt;
    public TextMeshProUGUI earnedTipTxt;
    public TextMeshProUGUI scoreTxt;

    private int totalScore = 0;
    private int customersServed = 0;

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
        GameManager.Instance.AnadirMonedas(result.moneyEarned);
        GameManager.Instance.AddServedCustomers(customersServed);
        GameManager.Instance.AddSatisfactionPoint(result.score);

        // Se calcula la propina obtenida
        int tip = CalculateTip(result.score);
        // Se mostrara un feedback distinto en funcion de la puntuacion obtenida
        if (tip > 0) 
        {
            GameManager.Instance.AnadirMonedas(tip);
            //earnedTipTxt.text = $"¡El cliente ha dejado una propina de {tip}$!";
            PopUpMechanicsMsg.Instance.ShowMessage($"Propina recibida: {tip}$", new Vector3(300, -87, 0), 5f);
        }
        else 
        {
            //earnedTipTxt.text = "El cliente no ha dejado propina.";
            PopUpMechanicsMsg.Instance.ShowMessage("El cliente no ha dejado propina.", new Vector3(300, -87, 0), 6f);
        }

        string feedback = GenerateFeedbackText(
            result.score,
            evaluation.isOrderWithFood,
            evaluation.playerForgotFood,
            evaluation.lastWrongFoodType,
            evaluation.lastBadCookStateRaw,
            evaluation.lastBadCookStateBurned
        );

        orderFeedbackTxt.text = feedback;
        //earnedMoneyTxt.text = $"¡Has ganado {result.moneyEarned}$!";
        PopUpMechanicsMsg.Instance.ShowMessage($"+{result.moneyEarned}$", new Vector3(-427,-50,0), 6f);
        servedCustomersTxt.text = $"Clientes servidos en la jornada de hoy: {customersServed}";
        scoreTxt.text = $"Puntuación total: {result.score}/100"; 
    }

    // Funcion para calcular la propina
    private int CalculateTip(int score)
    {
        if (score <= 80) return 0;
        if (score <= 92) return 1;
        else return 2;
    }
    // Funcion para generar el texto en funcion de la puntuacion obtenida
    private string GenerateFeedbackText(int score, bool isOrderWithFood, bool playerForgotFood, bool wrongFoodType, bool badCookStateRaw, bool badCookStateBurned)
    {
        string feedback = "";
        if (score <= 40) feedback = "Este café no es lo que había pedido...";
        else if (score <= 80) feedback = "El café no está mal.";
        else feedback = "¡Me encanta! ¡Es justo el café que había pedido!";

        if (isOrderWithFood)
        {
            if (playerForgotFood) feedback += " Te has olvidado de preparar la comida...";
            else if (wrongFoodType) feedback += " Esta comida no es la que había pedido... ¿Te has equivocado de plato?";
            else if (badCookStateRaw) feedback += " Esta comida está cruda... ¡Así no se puede comer!";
            else if (badCookStateBurned) feedback += " La comida está quemada... ¡Así no se puede comer!";
            else feedback += " La comida está bien preparada.";
        }
        else
        {
            feedback += "";
        }
        return feedback;
    }

}
