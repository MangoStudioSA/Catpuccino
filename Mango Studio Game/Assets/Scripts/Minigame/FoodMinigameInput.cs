using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

public class FoodMinigameInput : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerOrder order;
    public FoodManager foodManager;
    public FoodOrder foodOrder;
    public TutorialManager tutorialManager;
    public ButtonUnlockManager buttonManager;
    public CursorManager cursorManager;
    public UnityEngine.UI.Slider bakeSlider;
    public Image fillBakeImage;

    [Header("Objetos fisicos")]
    public GameObject Plato;
    public GameObject BolsaLlevar;

    [Header("Localizaciones")]
    public Transform puntoEncimera;
    public Transform puntoComida;
    public Transform puntoHorno;

    [Header("Comida - Estados del jugador")]
    public bool platoInHand = false;
    public bool carryBagInHand = false;
    public bool foodInHand = false;
    public bool platoIsInEncimera = false;
    public bool carryBagIsInEncimera = false;
    public bool foodIsInBolsaLlevar = false;
    public bool foodIsInPlato = false;
    public bool foodIsInHorno = false;
    public bool isBaking = false;
    public bool foodServed = false;
    public bool foodBaked = false;
    public int countFoodCover = 0;

    [Header("Comida - Categorias y tipos de comida")]
    public FoodCategory foodCategoryInHand = FoodCategory.no;
    public int foodTypeInHand;
    public FoodCategory foodCategoryInPlato = FoodCategory.no;
    public int foodTypeInPlato;
    public FoodCategory foodCategoryInCarryBag = FoodCategory.no;
    public int foodTypeInCarryBag;
    public FoodCategory foodCategoryInHorno = FoodCategory.no;
    public int foodTypeInHorno;

    [Header("Horneados")]
    public float tiempoHorneado = 15f;

    private Coroutine horneadoCoroutine;
    private CookState currentCookState;
    private GameObject foodInPlatoObj = null, foodInHornoObj = null, foodInBolsaLlevarObj = null;

    void Start()
    {
        order = FindFirstObjectByType<PlayerOrder>();
        ResetFoodState();
        ActualizarBotonCogerComida();
    }

    private void Update()
    {
        // Horneado
        if (isBaking)
        {
            buttonManager.hornoButton.gameObject.SetActive(false);
            buttonManager.DisableButton(buttonManager.hornoButton);
        }
        else
        {
            buttonManager.hornoButton.gameObject.SetActive(true);
            buttonManager.EnableButton(buttonManager.hornoButton);
        }

        CheckButtons(); 
    }

    // Funcion para resetear variables de la comida
    public void ResetFoodState()
    {
        buttonManager.EnableButton(buttonManager.cogerPlatoInicioButton);
        buttonManager.EnableButton(buttonManager.cogerBolsaLlevarInicioButton);
        buttonManager.EnableButton(buttonManager.bakeryButton);
        buttonManager.EnableButton(buttonManager.returnBakeryButton);

        buttonManager.DisableButton(buttonManager.hornoButton);

        buttonManager.stopHorneadoButton.gameObject.SetActive(false);
        buttonManager.hornearButton.gameObject.SetActive(true);
        bakeSlider.gameObject.SetActive(false);

        //platoInHand = false;
        platoIsInEncimera = foodIsInHorno = foodIsInPlato = foodIsInBolsaLlevar = carryBagIsInEncimera = false;
        foodServed = foodBaked = isBaking = false;
        countFoodCover = 0;
        //foodInHand = false;

        foodCategoryInHand = FoodCategory.no;
        foodTypeInHand = -1;
        foodCategoryInPlato = FoodCategory.no;
        foodTypeInHorno = -1;
        foodCategoryInCarryBag = FoodCategory.no;
        foodTypeInCarryBag = -1;
        foodCategoryInHorno = FoodCategory.no;
        foodTypeInHorno = -1;

        Plato.SetActive(false);
        BolsaLlevar.SetActive(false);

        if (foodInPlatoObj != null)
        {
            foodInPlatoObj.SetActive(false);
            foodInPlatoObj = null;
        }

        if (foodInHornoObj != null)
        {
            foodInHornoObj.SetActive(false);
            foodInHornoObj = null;
        }

        if (foodInBolsaLlevarObj != null)
        {
            foodInBolsaLlevarObj = null;
        }
    }
    // Funcion para comprobar si se puede coger comida
    public void ActualizarBotonCogerComida()
    {
        bool canTakeFood = carryBagIsInEncimera || platoIsInEncimera && !foodInHand && !foodIsInPlato && !foodServed && !foodIsInHorno;

        Button[] botonesComida =
        {
            buttonManager.cogerBChocolateButton,
            buttonManager.cogerBZanahoriaButton,
            buttonManager.cogerBMantequillaButton,
            buttonManager.cogerBRedVelvetButton,
            buttonManager.cogerGChocolateBlButton,
            buttonManager.cogerGChocolateButton,
            buttonManager.cogerGMantequillaButton,
            buttonManager.cogerMArandanosButton,
            buttonManager.cogerMCerezaButton,
            buttonManager.cogerMPistachoButton,
            buttonManager.cogerMDulceLecheButton
        };

        foreach (Button boton in botonesComida)
        {
            if (boton == null) continue;

            if (canTakeFood)
            {
                buttonManager.EnableButton(boton);
            }
            else
            {
                buttonManager.DisableButton(boton);
            }
        }
    }
    
    public void CheckButtons()
    {
        if (platoInHand || foodInHand)
        {
            buttonManager.DisableButton(buttonManager.returnBakeryButton);
        }
        else if (!tutorialManager.isRunningT2)
        {
            buttonManager.EnableButton(buttonManager.returnBakeryButton);
        }
        if (foodBaked)
        {
            buttonManager.DisableButton(buttonManager.hornearButton);
        }
    }

    // Funcion para colocar el plato en la encima
    public void PlacePlatoEncimera()
    {
        if (platoIsInEncimera || carryBagIsInEncimera) return;
        if (!platoInHand) return;

        Plato.SetActive(true);
        Plato.transform.position = puntoEncimera.position;

        platoInHand = false;
        platoIsInEncimera = true;

        cursorManager.UpdateCursorPlato(true);
        buttonManager.EnableButton(buttonManager.hornoButton);
        buttonManager.DisableButton(buttonManager.cogerBolsaLlevarInicioButton);
        buttonManager.DisableButton(buttonManager.cogerPlatoInicioButton);
        
        ActualizarBotonCogerComida();

        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 2)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep2();
    }
    // Funcion para colocar la bolsa para llevar en la encima
    public void PlaceCarryBagEncimera()
    {
        if (carryBagIsInEncimera || platoIsInEncimera) return;
        if (!carryBagInHand) return;

        BolsaLlevar.SetActive(true);
        BolsaLlevar.transform.position = puntoEncimera.position;

        carryBagInHand = false;
        carryBagIsInEncimera = true;

        cursorManager.UpdateCursorCarryBag(true);
        buttonManager.EnableButton(buttonManager.hornoButton);
        buttonManager.DisableButton(buttonManager.cogerBolsaLlevarInicioButton);
        buttonManager.DisableButton(buttonManager.cogerPlatoInicioButton);
        
        ActualizarBotonCogerComida();
    }
    // Funcion para interactuar con la comida en el plato 
    public void ToggleFoodPlato()
    {
        // Comprobaciones previas
        if (carryBagIsInEncimera) return;
        if (!foodInHand && !foodIsInPlato && !platoIsInEncimera) return;

        // Colocar la comida en el plato
        if (foodInHand)
        {
            GameObject foodObj = foodManager.GetFoodObject(foodCategoryInHand, foodTypeInHand);
            if (foodObj == null)
            {
                Debug.LogWarning("[FoodMiniGameInput] No se encontro el objeto de comida correspondiente");
                return;
            }
            // Se activa y posiciona la comida
            foodObj.SetActive(true);
            foodObj.transform.position = puntoComida.position;
            foodInPlatoObj = foodObj;

            //  Se asocia la categoria y el tipo de comida de la mano al plato
            foodCategoryInPlato = foodCategoryInHand;
            foodTypeInPlato = foodTypeInHand;

            foodIsInPlato = true;
            foodServed = true;
            foodInHand = false;

            cursorManager.UpdateCursorFood(true, foodCategoryInHand, foodTypeInHand);

            if (order.currentOrder.foodOrder != null)
            {
                order.currentOrder.foodOrder.SetFoodPrecision(foodCategoryInHand, foodTypeInHand);
                if (currentCookState == CookState.no)
                {
                    currentCookState = CookState.crudo;
                }
                order.currentOrder.foodOrder.SetCookStatePrecision(currentCookState);
            }
            //  Se resetea la categoria y el tipo de comida de la mano 
            foodCategoryInHand = FoodCategory.no;
            foodTypeInHand = -1;

            if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 7)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep2();
        }
        else if (foodIsInPlato)
        {
            foodInPlatoObj.SetActive(false);
            foodIsInPlato = false;
            foodInHand = true;

            //  Se asocia la categoria y el tipo de comida del plato a la mano
            foodCategoryInHand = foodCategoryInPlato;
            foodTypeInHand = foodTypeInPlato;

            //  Se resetea la categoria y el tipo de comida del plato 
            foodCategoryInPlato = FoodCategory.no;
            foodTypeInPlato = -1;

            cursorManager.UpdateCursorFood(false, foodCategoryInHand, foodTypeInHand);
        }
        ActualizarBotonCogerComida();
    }
    // Funcion para interactuar con la comida en la bolsa para llevar 
    public void ToggleFoodCarryBag()
    {
        if (platoIsInEncimera) return;
        if (!foodInHand && !foodIsInBolsaLlevar && !carryBagIsInEncimera) return;

        if (foodInHand)
        {
            GameObject foodObj = foodManager.GetFoodObject(foodCategoryInHand, foodTypeInHand);
            if (foodObj == null) return;

            foodInBolsaLlevarObj = foodObj;
            //  Se asocia la categoria y el tipo de comida de la mano al plato
            foodCategoryInCarryBag = foodCategoryInHand;
            foodTypeInCarryBag = foodTypeInHand;

            countFoodCover++;
            order.currentOrder.typeOrderFoodPrecision = countFoodCover; // Se guarda el resultado obtenido en la precision del jugador

            foodIsInBolsaLlevar = true;
            foodServed = true;
            foodInHand = false;

            cursorManager.UpdateCursorFood(true, foodCategoryInHand, foodTypeInHand);

            if (order.currentOrder.foodOrder != null)
            {
                order.currentOrder.foodOrder.SetFoodPrecision(foodCategoryInHand, foodTypeInHand);
                if (currentCookState == CookState.no)
                {
                    currentCookState = CookState.crudo;
                }
                order.currentOrder.foodOrder.SetCookStatePrecision(currentCookState);
            }
            //  Se resetea la categoria y el tipo de comida de la mano 
            foodCategoryInHand = FoodCategory.no;
            foodTypeInHand = -1;

            Debug.Log("Comida colocada en la bolsa para llevar");

            if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 7)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep2();
        }
        ActualizarBotonCogerComida();
    }
    // Funcion para interactuar con la comida en el horno 
    public void ToggleFoodHorno()
    {
        if (!foodInHand && !platoIsInEncimera && !carryBagIsInEncimera) return;

        if (foodInHand)
        {
            GameObject foodObj = foodManager.GetFoodObject(foodCategoryInHand, foodTypeInHand);
            if (foodObj == null) return;

            foodObj.SetActive(true);
            foodObj.transform.position = puntoHorno.position;
            foodInHornoObj = foodObj;

            //  Se asocia la categoria y el tipo de comida de la mano al horno
            foodCategoryInHorno = foodCategoryInHand;
            foodTypeInHorno = foodTypeInHand;

            foodIsInHorno = true;
            foodInHand = false;

            cursorManager.UpdateCursorFood(true, foodCategoryInHand, foodTypeInHand);
            buttonManager.EnableButton(buttonManager.hornearButton);

            //  Se resetea la categoria y el tipo de comida de la mano 
            foodCategoryInHand = FoodCategory.no;
            foodTypeInHand = -1;

            if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 4)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep2();
        }
        else if (foodIsInHorno)
        {
            foodInHornoObj.SetActive(false);
            foodIsInHorno = false;
            foodInHand = true;

            //  Se asocia la categoria y el tipo de comida del horno a la mano
            foodCategoryInHand = foodCategoryInHorno;
            foodTypeInHand = foodTypeInHorno;

            //  Se resetea la categoria y el tipo de comida del horno 
            foodCategoryInHorno = FoodCategory.no;
            foodTypeInHorno = -1;

            cursorManager.UpdateCursorFood(false, foodCategoryInHand, foodTypeInHand);
            Debug.Log($"Comida recogida del horno. Estado: {currentCookState}");
        }
        ActualizarBotonCogerComida();
    }
    // Funcion para hornear la comida 
    public void StartHorneado()
    {
        if (!foodIsInHorno || foodInHornoObj == null || foodBaked) return;
         
        if (horneadoCoroutine != null) StopCoroutine(horneadoCoroutine);

        horneadoCoroutine = StartCoroutine(HornearCoroutine());
        isBaking = true;
        buttonManager.stopHorneadoButton.gameObject.SetActive(true);
        buttonManager.hornearButton.gameObject.SetActive(false);

        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 5)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep2();
    }
    // Corrutina para hornear la comida
    private IEnumerator HornearCoroutine()
    {
        bakeSlider.gameObject.SetActive(true);
        bakeSlider.value = 0f;
        float elapsed = 0f;
        currentCookState = CookState.crudo;

        while (elapsed < tiempoHorneado)
        {
            elapsed += Time.deltaTime;
            bakeSlider.value = elapsed / tiempoHorneado;
            UpdateBakingBarColor(bakeSlider.value);
            yield return null;
        }

        currentCookState = CookState.quemado;
        bakeSlider.gameObject.SetActive(false);
        buttonManager.DisableButton(buttonManager.hornearButton);
        buttonManager.stopHorneadoButton.gameObject.SetActive(false);
        Debug.Log("Se ha pasado el tiempo: comida quemada");
        isBaking = false;
    }
    // Funcion para actualizar el slider del horneado
    private void UpdateBakingBarColor(float value)
    {
        Color newColor;

        if (value < 0.4f) newColor = Color.Lerp(Color.yellow, Color.green, value * 2.5f);
        else newColor = Color.Lerp(Color.green, Color.red, (value - 0.45f) * 0.55f);

        fillBakeImage.color = newColor;
    }
    // Funcion para parar el horneado
    public void StopHorneado()
    {
        if (horneadoCoroutine != null)
        {
            StopCoroutine(horneadoCoroutine);
            horneadoCoroutine = null;
        }

        isBaking = false;
        foodBaked = true;

        float progress = bakeSlider.value;
        bakeSlider.gameObject.SetActive(false);

        if (progress < 0.40f) currentCookState = CookState.crudo;
        else if (progress <= 0.70f) currentCookState = CookState.horneado;
        else currentCookState = CookState.quemado;

        Debug.Log($"Horneado detenido: {currentCookState}");
        buttonManager.DisableButton(buttonManager.hornearButton);
        buttonManager.stopHorneadoButton.gameObject.SetActive(false);
    }
}
