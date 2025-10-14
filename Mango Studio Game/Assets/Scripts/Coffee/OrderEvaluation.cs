using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class OrderEvaluation : MonoBehaviour
{
    private const int MAX_SCORE_COFFEE = 25;
    private const int MAX_SCORE_SUGAR = 5;
    private const int MAX_SCORE_ICE = 5;
    private const int MAX_SCORE_COVER = 15; // Puntuacion tipo de pedido
    private const float MAX_ERROR = 2.0F;
    public int Evaluate(Order npcOrder, Order playerOrder)
    {
        int totalScore = 0; // Inicializa la puntuacion del jugador en 0

        //EVALUACION DEL CAFE (PRECISION SLIDER)
        //evalua la precision y redondea la puntiacion a un entero
        float coffeeScore = EvaluateCoffePrecision(npcOrder, playerOrder);
        totalScore += Mathf.RoundToInt(coffeeScore);

        //EVALUACION DEL AZUCAR (CANTIDAD EXACTA)
        int sugarScore = EvaluateSugarPrecision(npcOrder, playerOrder);
        totalScore += sugarScore;

        //EVALUACION DEL HIELO (CANTIDAD EXACTA)
        int iceScore = EvaluateIcePrecision(npcOrder, playerOrder);
        totalScore += iceScore;

        //EVALUACION DEL TIPO DE PEDIDO (CANTIDAD EXACTA)
        int typeScore = EvaluateTypePrecision(npcOrder, playerOrder);
        totalScore += typeScore;


        // Debug del puntaje TOTAL
        Debug.Log($"--- RESULTADO FINAL DE LA ORDEN ---");
        Debug.Log($"Puntuación del Café (Precisión): {Mathf.RoundToInt(coffeeScore)}/{MAX_SCORE_COFFEE} pts");
        Debug.Log($"Puntuación del Azúcar: {sugarScore}/{MAX_SCORE_SUGAR} pts");
        Debug.Log($"Puntuación del Hielo: {iceScore}/{MAX_SCORE_ICE} pts");
        Debug.Log($"Puntuación del Tipo de pedido: {typeScore}/{MAX_SCORE_COVER} pts");
        Debug.Log($"Puntuación Total de la Orden: {totalScore} pts");
        Debug.Log($"------------------------------------");

        //EL SCORE MAXIMO AHORA ES 50 (25+12+13)
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

    public int EvaluateSugarPrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto de cucharadas de azucar (0, 1, 2 o 3)
        float Starget = npcOrder.sugarTarget;
        // La precision es el numero de cucharadas que ha echado el player
        float playerSpoons = playerOrder.sugarPrecision;

        int sugarScore = 0;
        if (playerSpoons == Starget) // Si el jugador ha echado las mismas cucharadas de azucar que las que se pedian suma 5 puntos
        {
            sugarScore = MAX_SCORE_SUGAR;
        } 
        else
        {
            sugarScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluación Azúcar] Objetivo: {Starget} | Jugador: {playerSpoons}");

        return sugarScore; // Se devuelve la puntuacion total del azucar

    }

    public int EvaluateIcePrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto de cubitos de hielo (0, 1 o 2)
        float Itarget = npcOrder.iceTarget;
        // La precision es el numero de hielos que ha echado el player
        float playerIceSpoons = playerOrder.icePrecision;

        int iceScore = 0;
        if (playerIceSpoons == Itarget) // Si el jugador ha echado los mismos hielos que los que se pedian suma 5 puntos
        {
            iceScore = MAX_SCORE_ICE;
        }
        else
        {
            iceScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluación Hielo] Objetivo: {Itarget} | Jugador: {playerIceSpoons}");

        return iceScore; // Se devuelve la puntuacion total del hielo

    }

    public int EvaluateTypePrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto del tipo de pedido (0-tomar o 1-llevar)
        float Typetarget = npcOrder.typeTarget;
        // La precision es el numero que muestra si el jugador ha colocado o no la tapa para llevar
        float playerType = playerOrder.typePrecision;

        int typeScore = 0;
        if (playerType == Typetarget) // Si el jugador ha colocado la tapa para llevar suma 10 puntos
        {
            typeScore = MAX_SCORE_COVER;
        }
        else
        {
            typeScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluación Tipo de pedido] Objetivo: {Typetarget} | Jugador: {playerType}");

        return typeScore; // Se devuelve la puntuacion total del tipo de pedido

    }
}
