using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class OrderEvaluation : MonoBehaviour
{
    private const int MAX_SCORE_COFFEE = 25;
    private const int MAX_SCORE_SUGAR = 5;
    private const int MAX_SCORE_ICE = 5;
    private const int MAX_SCORE_COVER = 15; // Puntuacion tipo de pedido
    private const int MAX_SCORE_WATER = 10;
    private const int MAX_SCORE_MILK = 10;
    private const int MAX_SCORE_HEATEDMILK = 5;
    private const float MAX_ERROR = 2.0F;
    public int Evaluate(Order npcOrder, Order playerOrder)
    {
        int totalScore = 0; // Inicializa la puntuacion del jugador en 0
        int maxPossibleScore = 0;

        var progress = GameProgressManager.Instance;

        // MECANICA CAFE
        if (progress.coffeeEnabled)
        {
            //EVALUACION DEL CAFE (PRECISION SLIDER)
            //evalua la precision y redondea la puntiacion a un entero
            float coffeeScore = EvaluateCoffePrecision(npcOrder, playerOrder);
            totalScore += Mathf.RoundToInt(coffeeScore);
            maxPossibleScore += MAX_SCORE_COFFEE;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuación del Café (Precisión): {Mathf.RoundToInt(coffeeScore)}/{MAX_SCORE_COFFEE} pts");
        }

        // MECANICA LECHE FRIA
        if (progress.milkEnabled)
        {
            //EVALUACION DE LA LECHE (CANTIDAD EXACTA)
            int milkScore = EvaluateMilkPrecision(npcOrder, playerOrder);
            totalScore += milkScore;
            maxPossibleScore += MAX_SCORE_MILK;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuación de la Leche fria: {(milkScore)}/{MAX_SCORE_MILK} pts");
        }

        // MECANICA LECHE CALIENTE
        if (progress.heatedMilkEnabled)
        {
            //EVALUACION DE LA LECHE CALIENTE (BOOL)
            int heatedMilkScore = EvaluateHeatedMilkPrecision(npcOrder, playerOrder);
            totalScore += heatedMilkScore;
            maxPossibleScore += MAX_SCORE_HEATEDMILK;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuación de la Leche caliente: {(heatedMilkScore)}/{MAX_SCORE_HEATEDMILK} pts");
        }

        // MECANICA AGUA
        if (progress.waterEnabled)
        {
            //EVALUACION DEL AGUA (CANTIDAD EXACTA)
            int waterScore = EvaluateWaterPrecision(npcOrder, playerOrder);
            totalScore += waterScore;
            maxPossibleScore += MAX_SCORE_WATER;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuación del Agua: {waterScore}/{MAX_SCORE_WATER} pts");
        }

        // MECANICA AZUCAR
        if (progress.sugarEnabled)
        {
            //EVALUACION DEL AZUCAR (CANTIDAD EXACTA)
            int sugarScore = EvaluateSugarPrecision(npcOrder, playerOrder);
            totalScore += sugarScore;
            maxPossibleScore += MAX_SCORE_SUGAR;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuación del Azúcar: {sugarScore}/{MAX_SCORE_SUGAR} pts");
        }

        // MECANICA HIELO
        if (progress.iceEnabled)
        {
            //EVALUACION DEL HIELO (CANTIDAD EXACTA)
            int iceScore = EvaluateIcePrecision(npcOrder, playerOrder);
            totalScore += iceScore;
            maxPossibleScore += MAX_SCORE_ICE;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuación del Hielo: {iceScore}/{MAX_SCORE_ICE} pts");
        }

        // MECANICA TIPO DE PEDIDO
        if (progress.typeOrderEnabled)
        {
            //EVALUACION DEL TIPO DE PEDIDO (CANTIDAD EXACTA)
            int typeScore = EvaluateTypePrecision(npcOrder, playerOrder);
            totalScore += typeScore;
            maxPossibleScore += MAX_SCORE_COVER;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuación del Tipo de pedido: {typeScore}/{MAX_SCORE_COVER} pts");
        }

        float percentScore = ((float)totalScore / maxPossibleScore) * 100f;

        // Debug del puntaje TOTAL
        Debug.Log($"--- RESULTADO FINAL DE LA ORDEN {playerOrder.orderId} ---");
        Debug.Log($"Puntuación Total de la Orden {playerOrder.orderId}: {totalScore}/{maxPossibleScore} ({percentScore:F1}%)pts");
        Debug.Log($"------------------------------------");

        //EL SCORE MAXIMO AHORA ES 50 (25+12+13)
        return Mathf.RoundToInt(percentScore); // Se devuelve la puntuacion del jugador

    }

    public float EvaluateCoffePrecision (Order npcOrder, Order playerOrder)
    {
        //el objetivo es el valor ideal (1.0, 2.0, 3.0)
        float coffeeTarget = npcOrder.coffeeTarget;

        //la precision es donde para el player
        float playerStop = playerOrder.coffeePrecision;
        
        //calculamos el error abs, distancia ente target y playerStop
        float error = Mathf.Abs(playerStop - coffeeTarget);

        Debug.Log($"[Evaluación Café Cliente {playerOrder.orderId}] Objetivo: {coffeeTarget:F2} | Jugador: {playerStop:F2} | Error Absoluto: {error:F2}");

        //normalizamos el error de 0 a 1
        float normalizedError = Mathf.Clamp(error / MAX_ERROR, 0f, 1f);
        //calculamos la puntuacion. Puntuacion maxima * (1 - error normalizado)
        float score = MAX_SCORE_COFFEE * (1f - normalizedError);

        Debug.Log($"[Evaluación Café Cliente {playerOrder.orderId}] Error Normalizado: {normalizedError:F2} | Puntuación Bruta: {score:F2}");

        return score;

    }

    public int EvaluateMilkPrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto (0-sin o 1-con)
        float milkTarget = npcOrder.milkTarget;
        // La precision es la cantidad de leche que ha echado el jugador
        float playerMilk = playerOrder.milkPrecision;

        int milkScore = 0;
        if (playerMilk == milkTarget) // Si el jugador ha echado la cantidad de leche que se pedia suma 10 puntos
        {
            milkScore = MAX_SCORE_MILK;
        }
        else
        {
            milkScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluación Leche fria Cliente {playerOrder.orderId}] Objetivo: {milkTarget} | Jugador: {playerMilk}");

        return milkScore; // Se devuelve la puntuacion total de la leche

    }

    public int EvaluateHeatedMilkPrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto (false-fria o true-caliente)
        bool heatedMilkTarget = npcOrder.heatedMilkTarget;
        // La precision es si el jugador ha calentado o no la leche
        bool playerHeatedMilk = playerOrder.heatedMilkPrecision;

        int heatedMilkScore = 0;
        if (playerHeatedMilk == heatedMilkTarget) // Si el jugador ha preparado la leche que se pedia suma 10 puntos
        {
            heatedMilkScore = MAX_SCORE_HEATEDMILK;
        }
        else
        {
            heatedMilkScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluación Leche caliente Cliente {playerOrder.orderId}] Objetivo: {heatedMilkTarget} | Jugador: {playerHeatedMilk}");

        return heatedMilkScore; // Se devuelve la puntuacion total de la leche

    }

    public int EvaluateWaterPrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto (0-sin o 1-con)
        float waterTarget = npcOrder.waterTarget;
        // La precision es la cantidad de agua que ha echado el jugador
        float playerWater = playerOrder.waterPrecision;

        int waterScore = 0;
        if (playerWater == waterTarget) // Si el jugador ha echado la cantidad de agua que se pedia suma 10 puntos
        {
            waterScore = MAX_SCORE_WATER;
        }
        else
        {
            waterScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluación Agua Cliente {playerOrder.orderId}] Objetivo: {waterTarget} | Jugador: {playerWater}");

        return waterScore; // Se devuelve la puntuacion total del agua

    }

    public int EvaluateSugarPrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto de cucharadas de azucar (0, 1, 2)
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

        Debug.Log($"[Evaluación Azúcar Cliente {playerOrder.orderId}] Objetivo: {Starget} | Jugador: {playerSpoons}");

        return sugarScore; // Se devuelve la puntuacion total del azucar

    }

    public int EvaluateIcePrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto de cubitos de hielo (0 o 1)
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

        Debug.Log($"[Evaluación Hielo Cliente {playerOrder.orderId}] Objetivo: {Itarget} | Jugador: {playerIceSpoons}");

        return iceScore; // Se devuelve la puntuacion total del hielo

    }

    public int EvaluateTypePrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto del tipo de pedido (0-tomar o 1-llevar)
        float Typetarget = npcOrder.typeTarget;
        // La precision es el numero que muestra si el jugador ha colocado o no la tapa para llevar
        float playerType = playerOrder.typePrecision;

        int typeScore = 0;
        if (playerType == Typetarget) // Si el jugador ha preparado el tipo de pedido que se pedia suma 5 puntos
        {
            typeScore = MAX_SCORE_COVER;
        }
        else
        {
            typeScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluación Tipo de pedido Cliente {playerOrder.orderId}] Objetivo: {Typetarget} | Jugador: {playerType}");

        return typeScore; // Se devuelve la puntuacion total del tipo de pedido

    }
}
