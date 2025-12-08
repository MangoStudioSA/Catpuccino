using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Clase auxiliar para crear el resultado de la evaluacion
[System.Serializable]
public class EvaluationResult
{
    public int score;
    public int moneyEarned;
}

// Clase auxiliar para manejar los sprites del feedback de los clientes
[System.Serializable]
public class FeedBackSprites
{
    public Sprite[] feedBackS;
}

// Clase encargada de gestionar la entrega del pedido y la valoracion de los clientes
public class CoffeeGameManager : MonoBehaviour
{
    [Header("Referencias")]
    public CustomerOrder npc;
    public PlayerOrder player;
    public OrderEvaluation evaluation;
    public CustomerManager customerManager;

    [Header("UI Texts")]
    public TextMeshProUGUI orderFeedbackTxt;
    public TextMeshProUGUI earnedMoneyTxt;
    public TextMeshProUGUI servedCustomersTxt;
    public TextMeshProUGUI earnedTipTxt;
    public TextMeshProUGUI scoreTxt;

    [Header("Sprites clientes")]
    public Image clientImage;
    public Sprite[] clienteSprites;

    [Header("Sprites feedback clientes")]
    public Image clientFeedbackImage;
    public FeedBackSprites[] feedbakClientSprites;

    public int customersServed = 0;
    private int totalScore = 0;

    private void Awake()
    {
        customerManager = FindFirstObjectByType<CustomerManager>();
    }

    // Funcion para entregar el pedido y vincular la propina y el feedback
    public void SubmitOrder()
    {
        // Se calcula la puntuacion del pedido comparando lo que se pedia con el resultado del jugador
        EvaluationResult result = evaluation.Evaluate(npc.currentOrder, player.currentOrder);
        SoundsMaster.Instance.PlaySound_Entregar();

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
            PopUpMechanicsMsg.Instance.ShowMessage($"Propina recibida: {tip}$", new Vector3(250, -87, 0), 5f);
        }
        else 
        {
            PopUpMechanicsMsg.Instance.ShowMessage("El cliente no ha dejado propina.", new Vector3(250, -87, 0), 6f);
        }

        if (customerManager.orderingCustomer != null)
        {
            CustomerController currentCustomer = customerManager.orderingCustomer.GetComponent<CustomerController>();
            int tipoCliente = currentCustomer.model;
            int tipoFeedback = CalculateFeedbackSprite(result.score);
            MostrarFeedback(tipoCliente, tipoFeedback);
        }

        // Generar feedback
        string feedback = GenerateFeedbackText(
            result.score,
            evaluation.isOrderWithFood,
            evaluation.playerForgotFood,
            evaluation.lastWrongFoodType,
            evaluation.lastBadCookStateRaw,
            evaluation.lastBadCookStateBurned
        );

        PopUpMechanicsMsg.Instance.ShowMessage($"+{result.moneyEarned}$", new Vector3(-510, -50, 0), 6f); // Mostrar mensaje en popup

        // Actualizar textos
        orderFeedbackTxt.text = feedback;
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

    // Funcion para mostrar el sprite del cliente actual 
    public void MostrarCliente(int tipoCliente)
    {
        if (tipoCliente < 0 || tipoCliente >= clienteSprites.Length)
        {
            clientImage.gameObject.SetActive(false); 
            return;
        }

        clientImage.sprite = clienteSprites[tipoCliente];
        clientImage.gameObject.SetActive(true);
    }

    // Funcion para mostrar el sprite del cliente actual segun la puntuacion obtenido
    public void MostrarFeedback(int tipoCliente, int tipoFeedback)
    {
        if (tipoCliente < 0 || tipoCliente >= feedbakClientSprites.Length ||
           tipoFeedback < 0 || tipoFeedback >= feedbakClientSprites[tipoCliente].feedBackS.Length)
        {
            clientFeedbackImage.gameObject.SetActive(false);
            return;
        }

        clientFeedbackImage.sprite = feedbakClientSprites[tipoCliente].feedBackS[tipoFeedback];
        clientFeedbackImage.gameObject.SetActive(true);
    }

    // Funcion para asignar el sprite segun la puntuacion obtenida
    public int CalculateFeedbackSprite(int score)
    {
        if (score < 40)
            return 1;
        else if (score > 40 && score < 85)
            return 0;
        else
            return 2;
    }

}
