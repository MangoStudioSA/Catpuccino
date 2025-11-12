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

    [Header("Sprites objetos")]
    public Sprite botonH_N;
    public Sprite botonH_P;
    public Sprite botonPH_N;
    public Sprite botonPH_P;

    
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
    /*public void ResetVisualFood()
    {
        // Limpiar objetos visibles
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
            foodInBolsaLlevarObj.SetActive(false);
            foodInBolsaLlevarObj = null;
        }

        Plato.SetActive(false);
        BolsaLlevar.SetActive(false);
        bakeSlider.gameObject.SetActive(false);

        // Reset estados de jugador/objetos
        platoInHand = false;
        carryBagInHand = false;
        foodInHand = false;

        platoIsInEncimera = false;
        carryBagIsInEncimera = false;
        foodIsInPlato = false;
        foodIsInBolsaLlevar = false;
        foodIsInHorno = false;

        // Reset categorías en mano
        foodCategoryInHand = FoodCategory.no;
        foodTypeInHand = -1;

        ActualizarBotonCogerComida();
        UpdateStartSprites();
    }*/


    // Funcion para resetear variables de la comida
    public void ResetFoodState()
    {
        buttonManager.EnableButton(buttonManager.cogerPlatoInicioButton);
        buttonManager.EnableButton(buttonManager.cogerBolsaLlevarInicioButton);
        buttonManager.EnableButton(buttonManager.bakeryButton);
        buttonManager.EnableButton(buttonManager.returnBakeryButton);
        buttonManager.DisableButton(buttonManager.hornoButton);
        buttonManager.DisableButton(buttonManager.stopHorneadoButton);
        
        platoIsInEncimera = foodIsInHorno = foodIsInPlato = foodIsInBolsaLlevar = carryBagIsInEncimera = false;
        foodServed = foodBaked = isBaking = false;
        countFoodCover = 0;

        foodCategoryInHand = FoodCategory.no;
        foodTypeInHand = -1;
        foodCategoryInPlato = FoodCategory.no;
        foodTypeInPlato = -1;
        foodCategoryInCarryBag = FoodCategory.no;
        foodTypeInCarryBag = -1;
        foodCategoryInHorno = FoodCategory.no;
        foodTypeInHorno = -1;

        Plato.SetActive(false);
        BolsaLlevar.SetActive(false);
        bakeSlider.gameObject.SetActive(false);
        UpdateStartSprites();

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
            foodInBolsaLlevarObj.SetActive(false);
            foodInBolsaLlevarObj = null;
        }
    }
    // Funcion para comprobar si se puede coger comida
    public void ActualizarBotonCogerComida()
    {
        bool canTakeFood = (carryBagIsInEncimera || platoIsInEncimera) && !foodInHand && !foodIsInPlato && !foodServed && !foodIsInHorno;

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

    // Funcion para resetear los sprites
    private void UpdateStartSprites()
    {
        Image hornearBut = buttonManager.hornearButton.GetComponent<Image>();
        hornearBut.sprite = botonH_N;

        Image pararHorneadoBut = buttonManager.stopHorneadoButton.GetComponent<Image>();
        pararHorneadoBut.sprite = botonPH_N;
    }

    // Funcion para comprobar que botones activar/desactivar
    public void CheckButtons()
    {
        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 9)
            buttonManager.EnableButton(buttonManager.returnBakeryButton);
        else if (tutorialManager.isRunningT2 && tutorialManager.currentStep != 9)
            buttonManager.DisableButton(buttonManager.returnBakeryButton);

        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 5)
            buttonManager.EnableButton(buttonManager.hornearButton);
        else if (tutorialManager.isRunningT2 && tutorialManager.currentStep != 5)
            buttonManager.DisableButton(buttonManager.hornearButton);

        if (foodIsInHorno && !tutorialManager.isRunningT2)
            buttonManager.EnableButton(buttonManager.hornearButton);
        else if (isBaking && !tutorialManager.isRunningT2)
            buttonManager.DisableButton(buttonManager.hornearButton);

        if (foodInHand || platoInHand || carryBagInHand)
        {
            buttonManager.DisableButton(buttonManager.orderNoteBButton);
            buttonManager.DisableButton(buttonManager.papeleraButton);
        }
        else
        {
            buttonManager.EnableButton(buttonManager.orderNoteBButton);
        }

        if (platoInHand || foodInHand)
            buttonManager.DisableButton(buttonManager.returnBakeryButton);

        else if (!tutorialManager.isRunningT2)
            buttonManager.EnableButton(buttonManager.returnBakeryButton);
        
        if (foodBaked)
            buttonManager.DisableButton(buttonManager.hornearButton);  
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

            //var sprite = foodObj.GetComponent<Image>()?.sprite;

            // Se asocia a la bandeja
            //CoffeeFoodManager.Instance.ToggleComida(true, Plato.GetComponent<Image>(), sprite);

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

            // Se quita de la bandeja
            //CoffeeFoodManager.Instance.ToggleComida(false, null, null);

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

            // Se asocia a la bandeja
            //CoffeeFoodManager.Instance.ToggleComida(true, BolsaLlevar.GetComponent<Image>(), BolsaLlevar.GetComponent<Image>().sprite);

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

        buttonManager.EnableButton(buttonManager.stopHorneadoButton);
        Image hornearBut = buttonManager.hornearButton.GetComponent<Image>();
        hornearBut.sprite = botonH_P;

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
        isBaking = false;
        currentCookState = CookState.quemado;

        bakeSlider.gameObject.SetActive(false);
        Debug.Log("Se ha pasado el tiempo: comida quemada");
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
        buttonManager.DisableButton(buttonManager.stopHorneadoButton);

        if (progress < 0.40f) currentCookState = CookState.crudo;
        else if (progress <= 0.70f) currentCookState = CookState.horneado;
        else currentCookState = CookState.quemado;

        Debug.Log($"Horneado detenido: {currentCookState}");
        Image stopHornearBut = buttonManager.stopHorneadoButton.GetComponent<Image>();
        stopHornearBut.sprite = botonPH_P;
    }
}
