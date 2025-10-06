using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class OrderEvaluation : MonoBehaviour
{
    public int Evaluate(Order npcOrder, Order playerOrder)
    {
        int playerScore = 0; // Inicializa la puntuacion del jugador en 0

        if (npcOrder.coffeeAm == playerOrder.coffeeAm) // Si coinciden las cantidades (cafe) del cliente y del pedido se suman 25 puntos
        {
            playerScore += 25;
        }

        if (npcOrder.sugarAm == playerOrder.sugarAm) // Si coinciden las cantidades (azucar) del cliente y del pedido se suman 25 puntos
        {
            playerScore += 25;
        }

        return playerScore; // Se devuelve la puntuacion del jugador

    }
}
