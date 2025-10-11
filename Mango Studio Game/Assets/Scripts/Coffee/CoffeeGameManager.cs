using UnityEngine;
using TMPro;

public class CoffeeGameManager : MonoBehaviour
{
    public CustomerOrder npc;
    public PlayerOrder player;
    public OrderEvaluation evaluation;
    //public TextMeshProUGUI feedbackTxt;
    //public TextMeshProUGUI scoreTxt;

    private int totalScore = 0;
    private int customersServed = 0;


    void Start()
    {
        //npc.GenRandomOrder();
        //feedbackTxt.text = "";
        //scoreTxt.text = "";
        
    }

    public void SubmitOrder()
    {

        int playerScore = evaluation.Evaluate(npc.currentOrder, player.currentOrder);
        totalScore += playerScore;
        customersServed++;

        // Añade monedas dependiendo de la puntuación
        GameManager.Instance.AnadirMonedas(playerScore);

        //feedbackTxt.text = playerScore == 50 ? "Perfecto!" :
        //playerScore >= 25 ? "No esta mal" : "Esto no es lo que habia pedido!";

    }
}
