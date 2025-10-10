using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class OrderEvaluation : MonoBehaviour
{
    private const int MAX_SCORE_COFFEE = 25;
    private const int MAX_SCORE_SUGAR = 25;
    private const float MAX_ERROR = 2.0F;
    public int Evaluate(Order npcOrder, Order playerOrder)
    {
        int totalScore = 0; // Inicializa la puntuacion del jugador en 0

        //EVALUACION DEL CAFE (PRECISION SLIDER)
        //evalua la precision y redondea la puntiacion a un entero
        float coffeeScore = EvaluateCoffePrecision(npcOrder, playerOrder);
        totalScore += Mathf.RoundToInt(coffeeScore);

        //EVALUACION DEL AZUCAR (CANTIDAD EXACTA)
        if (npcOrder.sugarAm == playerOrder.sugarAm)
        {
            totalScore += MAX_SCORE_SUGAR;
        }

        // Debug del puntaje TOTAL
        Debug.Log($"--- RESULTADO FINAL DE LA ORDEN ---");
        Debug.Log($"Puntuación del Café (Precisión): {Mathf.RoundToInt(coffeeScore)}/{MAX_SCORE_COFFEE} pts");
        Debug.Log($"Puntuación del Azúcar: {(npcOrder.sugarAm == playerOrder.sugarAm ? MAX_SCORE_SUGAR : 0)}/{MAX_SCORE_SUGAR} pts");
        Debug.Log($"Puntuación Total de la Orden: {totalScore} pts");
        Debug.Log($"------------------------------------");

        //EL SCORE MAXIMO AHORA ES 50 (25 +25)
        return totalScore; // Se devuelve la puntuacion del jugador

    }

    public float EvaluateCoffePrecision (Order npcOrder, Order playerOrder)
    {
        //el objetivo es el valor ideal (1.0, 2.0, 3.0)
        float target = npcOrder.coffeeTarget;
        //la precision es donde para el player
        float playerStop = playerOrder.coffeePrecision;
        
        //calculamos el error abs, distancia ente target y playerStop
        float error = Mathf.Abs(playerStop - target);

        Debug.Log($"[Evaluación Café] Objetivo: {target:F2} | Jugador: {playerStop:F2} | Error Absoluto: {error:F2}");


        //normalizamos el error de 0 a 1
        float normalizedError = Mathf.Clamp(error / MAX_ERROR, 0f, 1f);
        //calculamos la puntuacion. Puntuacion maxima * (1 - error normalizado)
        float score = MAX_SCORE_COFFEE * (1f - normalizedError);

        Debug.Log($"[Evaluación Café] Error Normalizado: {normalizedError:F2} | Puntuación Bruta: {score:F2}");

        return score;

    }
}
