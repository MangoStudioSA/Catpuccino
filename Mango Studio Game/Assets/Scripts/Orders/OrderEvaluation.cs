using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class OrderEvaluation : MonoBehaviour
{
    private const int MAX_SCORE_COFFEE = 25;
    private const int MAX_SCORE_SUGAR = 5;
    private const int MAX_SCORE_ICE = 5;
    private const int MAX_SCORE_COVER = 15; 
    private const int MAX_SCORE_WATER = 10;
    private const int MAX_SCORE_MILK = 10;
    private const int MAX_SCORE_HEATEDMILK = 10;
    private const int MAX_SCORE_CONDENSEDMILK = 5;
    private const int MAX_SCORE_CREAM = 5;
    private const int MAX_SCORE_CHOCOLATE = 5;
    private const int MAX_SCORE_WHISKEY = 5;
    private const int MAX_SCORE_COVERFOOD = 10;
    private const int MAX_SCORE_FOODTYPE = 15;
    private const int MAX_SCORE_COOKSTATE = 15;

    private const float MAX_ERROR = 2.0F;

    public EvaluationResult Evaluate(Order npcOrder, Order playerOrder)
    {
        EvaluationResult result = new EvaluationResult();

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
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n del Caf� (Precisi�n): {Mathf.RoundToInt(coffeeScore)}/{MAX_SCORE_COFFEE} pts");
        }

        // MECANICA AGUA
        if (progress.waterEnabled)
        {
            //EVALUACION DEL AGUA (CANTIDAD EXACTA)
            int waterScore = EvaluateWaterPrecision(npcOrder, playerOrder);
            totalScore += waterScore;
            maxPossibleScore += MAX_SCORE_WATER;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n del Agua: {waterScore}/{MAX_SCORE_WATER} pts");
        }

        // MECANICA LECHE FRIA
        if (progress.milkEnabled)
        {
            //EVALUACION DE LA LECHE FRIA (CANTIDAD EXACTA)
            int milkScore = EvaluateMilkPrecision(npcOrder, playerOrder);
            totalScore += milkScore;
            maxPossibleScore += MAX_SCORE_MILK;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n de la Leche fria: {(milkScore)}/{MAX_SCORE_MILK} pts");
        }

        // MECANICA LECHE CALIENTE
        if (progress.heatedMilkEnabled)
        {
            //EVALUACION DE LA LECHE CALIENTE (BOOL)
            int heatedMilkScore = EvaluateHeatedMilkPrecision(npcOrder, playerOrder);
            totalScore += heatedMilkScore;
            maxPossibleScore += MAX_SCORE_HEATEDMILK;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n de la Leche caliente: {(heatedMilkScore)}/{MAX_SCORE_HEATEDMILK} pts");
        }

        // MECANICA LECHE CONDENSADA
        if (progress.condensedMilkEnabled)
        {
            //EVALUACION DE LA LECHE CONDENSADA (CANTIDAD EXACTA)
            int condensedMilkScore = EvaluateCondensedMilkPrecision(npcOrder, playerOrder);
            totalScore += condensedMilkScore;
            maxPossibleScore += MAX_SCORE_CONDENSEDMILK;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n de la Leche condensada: {condensedMilkScore}/{MAX_SCORE_CONDENSEDMILK} pts");
        }

        // MECANICA CREMA
        if (progress.creamEnabled)
        {
            //EVALUACION DE LA CREMA (CANTIDAD EXACTA)
            int creamScore = EvaluateCreamPrecision(npcOrder, playerOrder);
            totalScore += creamScore;
            maxPossibleScore += MAX_SCORE_CREAM;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n de la Crema: {creamScore}/{MAX_SCORE_CREAM} pts");
        }

        // MECANICA CHOCOLATE
        if (progress.chocolateEnabled)
        {
            //EVALUACION DEL CHOCOLATE (CANTIDAD EXACTA)
            int chocolateScore = EvaluateChocolatePrecision(npcOrder, playerOrder);
            totalScore += chocolateScore;
            maxPossibleScore += MAX_SCORE_CHOCOLATE;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n del Chocolate: {chocolateScore}/{MAX_SCORE_CHOCOLATE} pts");
        }

        // MECANICA WHISKEY
        if (progress.whiskeyEnabled)
        {
            //EVALUACION DEL WHISKEY (CANTIDAD EXACTA)
            int whiskeyScore = EvaluateWhiskeyPrecision(npcOrder, playerOrder);
            totalScore += whiskeyScore;
            maxPossibleScore += MAX_SCORE_WHISKEY;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n del Whiskey: {whiskeyScore}/{MAX_SCORE_WHISKEY} pts");
        }

        // MECANICA AZUCAR
        if (progress.sugarEnabled)
        {
            //EVALUACION DEL AZUCAR (CANTIDAD EXACTA)
            int sugarScore = EvaluateSugarPrecision(npcOrder, playerOrder);
            totalScore += sugarScore;
            maxPossibleScore += MAX_SCORE_SUGAR;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n del Az�car: {sugarScore}/{MAX_SCORE_SUGAR} pts");
        }

        // MECANICA HIELO
        if (progress.iceEnabled)
        {
            //EVALUACION DEL HIELO (CANTIDAD EXACTA)
            int iceScore = EvaluateIcePrecision(npcOrder, playerOrder);
            totalScore += iceScore;
            maxPossibleScore += MAX_SCORE_ICE;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n del Hielo: {iceScore}/{MAX_SCORE_ICE} pts");
        }

        // MECANICA TIPO DE PEDIDO
        if (progress.typeOrderEnabled)
        {
            //EVALUACION DEL TIPO DE PEDIDO (CANTIDAD EXACTA)
            int typeScore = EvaluateTypePrecision(npcOrder, playerOrder);
            totalScore += typeScore;
            maxPossibleScore += MAX_SCORE_COVER;

            if (progress.cakesEnabled && npcOrder.foodOrder.category != FoodCategory.no)
            {
                int typeOrderFoodScore = EvaluateTypeOrderFoodPrecision(npcOrder, playerOrder);
                totalScore += typeOrderFoodScore;
                maxPossibleScore += MAX_SCORE_COVERFOOD;
                typeScore += typeOrderFoodScore;
            }
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n del Tipo de pedido: {typeScore}/{MAX_SCORE_COVER + MAX_SCORE_COVERFOOD} pts");
        }

        // MECANICA TIPO DE COMIDA Y ESTADO DE HORNEADO
        if (progress.cakesEnabled && npcOrder.foodOrder.category != FoodCategory.no)
        {
            //EVALUACION DEL TIPO DE COMIDAD (TIPO EXACTO)
            int typeFoodScore = EvaluateTypeFoodPrecision(npcOrder, playerOrder);
            totalScore += typeFoodScore;
            maxPossibleScore += MAX_SCORE_FOODTYPE;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n del Tipo de comida: {typeFoodScore}/{MAX_SCORE_FOODTYPE} pts");

            int cookStateScore = EvaluateCookStatePrecision(npcOrder, playerOrder);
            totalScore += cookStateScore;
            maxPossibleScore += MAX_SCORE_COOKSTATE;
            Debug.Log($"[Cliente {playerOrder.orderId}] Puntuaci�n del Horneado: {cookStateScore}/{MAX_SCORE_COOKSTATE} pts");
        }

        float percentScore = (float) totalScore / maxPossibleScore;

        float baseCoffeePrice = CoffeePriceManager.Instance.GetBaseCoffeePrice(playerOrder.coffeeType);
        float baseFoodPrice = FoodPriceManager.Instance.GetBaseFoodPrice(playerOrder.foodOrder.category);
        float totalBasePrice = baseCoffeePrice + baseFoodPrice;
        result.moneyEarned = Mathf.RoundToInt(totalBasePrice * percentScore);

        result.score = Mathf.RoundToInt(percentScore * 100f);

        // Debug del puntaje TOTAL
        Debug.Log($"--- RESULTADO FINAL DE LA ORDEN {playerOrder.orderId} ---");
        Debug.Log($"Puntuaci�n Total de la Orden {playerOrder.orderId}: {totalScore}/{maxPossibleScore} ({percentScore:F1}%)pts");
        Debug.Log($"Ingresos Totales de la Orden {playerOrder.orderId}: ({result.moneyEarned:F1}%)$");
        Debug.Log($"------------------------------------");

        return result; // Se devuelve la puntuacion y los ingresos del jugador

    }

    public float EvaluateCoffePrecision (Order npcOrder, Order playerOrder)
    {
        //el objetivo es el valor ideal (1.0, 2.0, 3.0)
        float coffeeTarget = npcOrder.coffeeTarget;

        //la precision es donde para el player
        float playerStop = playerOrder.coffeePrecision;
        
        //calculamos el error abs, distancia ente target y playerStop
        float error = Mathf.Abs(playerStop - coffeeTarget);

        Debug.Log($"[Evaluaci�n Caf� Cliente {playerOrder.orderId}] Objetivo: {coffeeTarget:F2} | Jugador: {playerStop:F2} | Error Absoluto: {error:F2}");

        //normalizamos el error de 0 a 1
        float normalizedError = Mathf.Clamp(error / MAX_ERROR, 0f, 1f);
        //calculamos la puntuacion. Puntuacion maxima * (1 - error normalizado)
        float score = MAX_SCORE_COFFEE * (1f - normalizedError);

        Debug.Log($"[Evaluaci�n Caf� Cliente {playerOrder.orderId}] Error Normalizado: {normalizedError:F2} | Puntuaci�n Bruta: {score:F2}");

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

        Debug.Log($"[Evaluaci�n Leche fria Cliente {playerOrder.orderId}] Objetivo: {milkTarget} | Jugador: {playerMilk}");

        return milkScore; // Se devuelve la puntuacion total de la leche

    }

    public int EvaluateHeatedMilkPrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto (0-fria o 2-caliente, existe la opcion 3-quemada, que penalizara siempre)
        int heatedMilkTarget = npcOrder.heatedMilkTarget;
        // La precision es si el jugador ha calentado o no la leche
        int playerHeatedMilk = playerOrder.heatedMilkPrecision;

        int heatedMilkScore = 0;
        if (playerHeatedMilk == heatedMilkTarget) // Si el jugador ha preparado la leche que se pedia suma 10 puntos
        {
            heatedMilkScore = MAX_SCORE_HEATEDMILK;
        }
        else
        {
            heatedMilkScore = -5; // En cualquier otro caso la puntuacion restara 5 
        }

        Debug.Log($"[Evaluaci�n Leche caliente Cliente {playerOrder.orderId}] Objetivo: {heatedMilkTarget} | Jugador: {playerHeatedMilk}");

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

        Debug.Log($"[Evaluaci�n Agua Cliente {playerOrder.orderId}] Objetivo: {waterTarget} | Jugador: {playerWater}");

        return waterScore; // Se devuelve la puntuacion total del agua

    }

    public int EvaluateCondensedMilkPrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto (0-sin o 1-con)
        float condensedMilkTarget = npcOrder.condensedMilkTarget;
        // La precision es la cantidad de leche condensada que ha echado el jugador
        float playerCondensedMilk = playerOrder.condensedMilkPrecision;

        int condensedMilkScore = 0;
        if (playerCondensedMilk == condensedMilkTarget) // Si el jugador ha echado la cantidad de leche condensada que se pedia suma 5 puntos
        {
            condensedMilkScore = MAX_SCORE_CONDENSEDMILK;
        }
        else
        {
            condensedMilkScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluaci�n Agua Cliente {playerOrder.orderId}] Objetivo: {condensedMilkTarget} | Jugador: {playerCondensedMilk}");

        return condensedMilkScore; // Se devuelve la puntuacion total de la leche condensada

    }

    public int EvaluateCreamPrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto (0-sin o 1-con)
        float creamTarget = npcOrder.creamTarget;
        // La precision es la cantidad de crema que ha echado el jugador
        float playerCream = playerOrder.creamPrecision;

        int creamScore = 0;
        if (playerCream == creamTarget) // Si el jugador ha echado la cantidad de crema que se pedia suma 5 puntos
        {
            creamScore = MAX_SCORE_CREAM;
        }
        else
        {
            creamScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluaci�n Crema Cliente {playerOrder.orderId}] Objetivo: {creamTarget} | Jugador: {playerCream}");

        return creamScore; // Se devuelve la puntuacion total de la crema

    }

    public int EvaluateChocolatePrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto (0-sin o 1-con)
        float chocolateTarget = npcOrder.chocolateTarget;
        // La precision es la cantidad de chocolate que ha echado el jugador
        float playerChocolate = playerOrder.chocolatePrecision;

        int chocolateScore = 0;
        if (playerChocolate == chocolateTarget) // Si el jugador ha echado la cantidad de chocolate que se pedia suma 5 puntos
        {
            chocolateScore = MAX_SCORE_CHOCOLATE;
        }
        else
        {
            chocolateScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluaci�n Chocolate Cliente {playerOrder.orderId}] Objetivo: {chocolateTarget} | Jugador: {playerChocolate}");

        return chocolateScore; // Se devuelve la puntuacion total del chocolate

    }

    public int EvaluateWhiskeyPrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto (0-sin o 1-con)
        float whiskeyTarget = npcOrder.whiskeyTarget;
        // La precision es la cantidad de whiskey que ha echado el jugador
        float playerWhiskey = playerOrder.whiskeyPrecision;

        int whiskeyScore = 0;
        if (playerWhiskey == whiskeyTarget) // Si el jugador ha echado la cantidad de whiskey que se pedia suma 5 puntos
        {
            whiskeyScore = MAX_SCORE_WHISKEY;
        }
        else
        {
            whiskeyScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluaci�n Whiskey Cliente {playerOrder.orderId}] Objetivo: {whiskeyTarget} | Jugador: {playerWhiskey}");

        return whiskeyScore; // Se devuelve la puntuacion total del whiskey

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

        Debug.Log($"[Evaluaci�n Az�car Cliente {playerOrder.orderId}] Objetivo: {Starget} | Jugador: {playerSpoons}");

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

        Debug.Log($"[Evaluaci�n Hielo Cliente {playerOrder.orderId}] Objetivo: {Itarget} | Jugador: {playerIceSpoons}");

        return iceScore; // Se devuelve la puntuacion total del hielo

    }

    public int EvaluateTypePrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto del tipo de pedido (0-tomar o 1-llevar)
        float Typetarget = npcOrder.typeTarget;
        // La precision es el numero que muestra si el jugador ha colocado o no la tapa para llevar
        float playerType = playerOrder.typePrecision;

        int typeScore = 0;
        if (playerType == Typetarget) // Si el jugador ha preparado el tipo de pedido que se pedia suma 15 puntos
        {
            typeScore = MAX_SCORE_COVER;
        }
        else
        {
            typeScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluaci�n Tipo de pedido Cliente {playerOrder.orderId}] Objetivo: {Typetarget} | Jugador: {playerType}");

        return typeScore; // Se devuelve la puntuacion total del tipo de pedido
    }

    public int EvaluateTypeOrderFoodPrecision(Order npcOrder, Order playerOrder)
    {
        // El objetivo es el valor exacto del tipo de pedido (0-tomar o 1-llevar)
        float TypeOrderFoodtarget = npcOrder.typeTarget;
        // La precision es el numero que muestra si el jugador ha colocado o no la tapa para llevar
        float playerTypeOrderFoodType = playerOrder.typePrecision;

        int typeOrderFoodScore = 0;
        if (playerTypeOrderFoodType == TypeOrderFoodtarget) // Si el jugador ha preparado el tipo de pedido que se pedia suma 10 puntos
        {
            typeOrderFoodScore = MAX_SCORE_COVERFOOD;
        }
        else
        {
            typeOrderFoodScore = 0; // En cualquier otro caso la puntuacion sera 0 
        }

        Debug.Log($"[Evaluaci�n Tipo de pedido de comida Cliente {playerOrder.orderId}] Objetivo: {TypeOrderFoodtarget} | Jugador: {playerTypeOrderFoodType}");

        return typeOrderFoodScore; // Se devuelve la puntuacion total del tipo de pedido
    }

    public int EvaluateTypeFoodPrecision(Order npcOrder, Order playerOrder)
    {
        if (npcOrder.foodOrder == null || playerOrder.foodOrder == null)
            return 0;

        FoodCategory targetCategory = npcOrder.foodOrder.foodTargetCategory;
        int targetType = npcOrder.foodOrder.foodTargetType;

        FoodCategory playerCategory = playerOrder.foodOrder.foodPrecisionCategory;
        int playerType = playerOrder.foodOrder.foodPrecisionType;

        if (playerCategory == FoodCategory.no || playerType < 0)
            return 0;

        int typeFoodScore = 0;

        if (playerCategory == targetCategory && playerType == targetType)     // Si el jugador ha preparado el tipo de pedido que se pedia suma 15 puntos
        {
            typeFoodScore = MAX_SCORE_FOODTYPE;
        }
        else if (playerCategory == targetCategory && playerType != targetType)
        {
            typeFoodScore = 5; 
        }
        else
        {
            typeFoodScore = 0;
        }

        Debug.Log($"[Evaluaci�n Tipo de commida Cliente {playerOrder.orderId}] Objetivo: {targetCategory} {targetType} | Jugador: {playerCategory} {playerType}");

        return typeFoodScore; // Se devuelve la puntuacion total del tipo de comida
    }

    public int EvaluateCookStatePrecision(Order npcOrder, Order playerOrder)
    {
        if (npcOrder.foodOrder == null || playerOrder.foodOrder == null)
            return 0;

        CookState targetState = npcOrder.foodOrder.targetCookState;
        CookState playerState = playerOrder.foodOrder.precisionCookState;

        if (playerState == CookState.no)
            return 0;

        int cookStateScore = 0;

        if (playerState == targetState)     // Si el jugador ha preparado el tipo de pedido que se pedia suma 15 puntos
        {
            cookStateScore = MAX_SCORE_COOKSTATE;
        }
        else
        {
            cookStateScore = 0;
        }

        Debug.Log($"[Evaluaci�n Horneado Cliente {playerOrder.orderId}] Objetivo: {targetState} | Jugador: {playerState}");

        return cookStateScore; // Se devuelve la puntuacion total del tipo de comida
    }
}
