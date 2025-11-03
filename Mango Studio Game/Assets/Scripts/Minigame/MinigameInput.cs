using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

public class MinigameInput : MonoBehaviour
{
    #region Variables
    [Header("Referencias")]
    public PlayerOrder order;
    public OrderNoteUI orderNoteUI;
    public TutorialManager tutorialManager;
    public GameObject coffeBar; //panel que contiene la barra
    public ButtonUnlockManager buttonManager;
    public CursorManager cursorManager;

    [Header("Mecánica cantidad café")]
    public UnityEngine.UI.Slider coffeeSlider; //la barrita que se mueve
    public float slideSpeed = 0.8f;
    public float maxAmount = 4.0f;
    float currentSlideTime = 0f;
    bool isSliding = false;
    public bool coffeeDone = false;

    [Header("Mecánica echar café")]
    public Slider serveCoffeeSlider;
    public float sliderSpeed = 0.8f;
    public float currentSlideServedCoffee = 0f;
    public bool isSlidingServeCoffee = false;
    public bool movingRight = true;
    public bool coffeeServed = false;

    [Header("Mecánica moler café")]
    [SerializeField] private GameObject molerPanel;
    [SerializeField] private Image molerFillImage;
    [SerializeField] private float molerFillSpeed = 0.5f;
    [SerializeField] private float maxFillMoler = 1f;
    [SerializeField] private float currentMolido = 0f;
    [SerializeField] private bool isMoliendo = false;

    [Header("Mecánica espumador")]
    [SerializeField] private GameObject heatPanel;
    [SerializeField] private Image curvedFillImage;
    [SerializeField] private float fillSpeed = 0.5f;
    [SerializeField] private float currentHeat = 0f;
    [SerializeField] private bool isHeating = false;
    [SerializeField] public bool heatedMilk = false;


    [Header("Variables café")]
    int countSugar = 0; 
    int countIce = 0;
    int countCover = 0;
    int countWater = 0;
    int countMilk = 0;
    int countCondensedMilk = 0;
    int countCream = 0;
    int countChocolate = 0;
    int countWhiskey = 0;
    public bool milkServed = false;
    public bool cupServed = false;

    public bool filtroInHand = false, cucharaInHand = false, tazaInHand = false, vasoInHand = false, platoTazaInHand = false, tazaMilkInHand = false, iceInHand = false, coverInHand = false, 
        waterInHand = false, milkInHand = false, condensedMilkInHand = false, creamInHand = false, chocolateInHand = false, whiskeyInHand = false;
    
    public bool tazaIsInCafetera = false, tazaIsInPlato = false, vasoIsInCafetera = false, vasoIsInTable = false, platoTazaIsInTable = false, tazaMilkIsInEspumador = false,
        vasoTapaPuesta = false, filtroIsInCafetera = false;


    [Header("Objetos fisicos")]
    public GameObject Taza;
    public GameObject Vaso;
    public GameObject TazaLeche; 
    public GameObject PlatoTaza;
    public GameObject Filtro;

    public Transform puntoCafetera;
    public Transform puntoEspumador;
    public Transform puntoMesa;
    public Transform puntoTazaPlato;
    public Transform puntoFiltroCafetera;

    [Header("Sprites")]
    public Sprite vasoConTapa;
    public Sprite vasoConCafe;
    public Sprite vasoSinCafe;
    public Sprite tazaSinCafe;
    public Sprite tazaConCafe;
    #endregion

    public void Start()
    {
        order = FindFirstObjectByType<PlayerOrder>();

        orderNoteUI.ResetNote();
        ResetCafe();
        
        ActualizarBotonCogerEnvase();
    }

    public void Update()
    {
        HandleCoffeeSlider();
        HandleMoler();
        HandleHeating();

        if (isSlidingServeCoffee && !coffeeServed) MoveSliderServedCoffee();

        CheckButtons();
    }
    public void ResetCafe()
    {
        buttonManager.coffeeButton.gameObject.SetActive(true);
        buttonManager.filtroCafeteraButton.gameObject.SetActive(false);
        buttonManager.filtroButton.gameObject.SetActive(false);

        buttonManager.molerButton.gameObject.SetActive(true);
        buttonManager.echarCafeButton.gameObject.SetActive(true);
        buttonManager.cogerTazaLecheButton.gameObject.SetActive(true);
        buttonManager.milkButton.gameObject.SetActive(true);
        buttonManager.calentarButton.gameObject.SetActive(true);

        buttonManager.EnableButton(buttonManager.cogerTazaInicioButton);
        buttonManager.EnableButton(buttonManager.cogerVasoInicioButton);
        buttonManager.EnableButton(buttonManager.cogerPlatoTazaButton);

        buttonManager.DisableButton(buttonManager.coffeeButton);
        buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);
        buttonManager.DisableButton(buttonManager.molerButton);
        buttonManager.DisableButton(buttonManager.filtroCafeteraButton);
        buttonManager.DisableButton(buttonManager.waterButton);
        buttonManager.DisableButton(buttonManager.milkButton);
        buttonManager.DisableButton(buttonManager.condensedMilkButton);
        buttonManager.DisableButton(buttonManager.creamButton);
        buttonManager.DisableButton(buttonManager.chocolateButton);
        buttonManager.DisableButton(buttonManager.calentarButton);
        buttonManager.DisableButton(buttonManager.echarCafeButton);
        buttonManager.DisableButton(buttonManager.pararEcharCafeButton);
        DisableMechanics();

        heatPanel.SetActive(false);
        molerPanel.SetActive(false);

        Taza.SetActive(false);
        Vaso.SetActive(false);
        PlatoTaza.SetActive(false);
        TazaLeche.SetActive(false);

        Image taza = Taza.GetComponent<Image>();
        taza.sprite = tazaSinCafe;

        Image vaso = Vaso.GetComponent<Image>();
        vaso.sprite = vasoSinCafe;

        currentSlideTime = currentSlideServedCoffee = currentHeat = currentMolido = 0f;
        isSliding = isSlidingServeCoffee = movingRight = coffeeDone = coffeeServed = cupServed = milkServed = heatedMilk = isHeating = isMoliendo = false;
        tazaIsInCafetera = tazaIsInPlato = vasoIsInCafetera = vasoIsInTable = platoTazaIsInTable = tazaMilkIsInEspumador = filtroIsInCafetera = false;
        countSugar = countIce = countCover = countWater = countMilk = countCondensedMilk = countCream = countChocolate = countWhiskey = 0;

        if (coffeeSlider != null)
        {
            coffeeSlider.minValue = 0f;
            coffeeSlider.maxValue = maxAmount;
            coffeeSlider.value = 0f;
        }

        if (serveCoffeeSlider != null)
        {
            serveCoffeeSlider.minValue = 0f;
            serveCoffeeSlider.maxValue = 1f;
            serveCoffeeSlider.value = 0f;
        }
    }
    private void HandleCoffeeSlider()
    {
        // Movimiento slider cantidad cafe
        if (isSliding)
        {
            currentSlideTime += Time.unscaledDeltaTime * slideSpeed;

            // El slider se actualiza con el tiempo de deslizamiento
            coffeeSlider.value = currentSlideTime;


            if (currentSlideTime > maxAmount)
            {
                currentSlideTime = maxAmount;
                StopCoffee();
                Debug.Log("La barrita llego al limite");
            }
        }
    }
    private void HandleMoler()
    {
        if (isMoliendo && Input.GetMouseButton(0))
        {
            currentMolido += molerFillSpeed * Time.unscaledDeltaTime;
            currentMolido = Mathf.Clamp01(currentMolido);

            molerFillImage.fillAmount = currentMolido * 0.5f;
            molerFillImage.color = Color.Lerp(Color.yellow, Color.red, currentMolido);

            if (currentMolido == maxFillMoler)
            {
                StopMoler();
            }
        }
    }
    private void HandleHeating()
    {
        if (!tazaMilkIsInEspumador) return;

        // Movimiento circunferencia calentar leche
        if (isHeating && Input.GetMouseButton(0))
        {
            currentHeat += fillSpeed * Time.unscaledDeltaTime;
            currentHeat = Mathf.Clamp01(currentHeat);

            curvedFillImage.fillAmount = currentHeat;
            curvedFillImage.color = Color.Lerp(Color.blue, Color.red, currentHeat);
        }

        if (isHeating && Input.GetMouseButtonUp(0))
        {
            StopHeating();
        }

        // Espumador
        if (isHeating)
        {
            buttonManager.espumadorButton.gameObject.SetActive(false);
            buttonManager.DisableButton(buttonManager.espumadorButton);
        }
        else
        {
            buttonManager.espumadorButton.gameObject.SetActive(true);
            buttonManager.EnableButton(buttonManager.espumadorButton);
        }
    }

    public bool TengoOtroObjetoEnLaMano()
    {
        return  cucharaInHand || waterInHand || milkInHand || condensedMilkInHand || creamInHand || chocolateInHand || whiskeyInHand || iceInHand || coverInHand;
    }
    public void DisableMechanics()
    {
        buttonManager.DisableButton(buttonManager.sugarButton);
        buttonManager.DisableButton(buttonManager.iceButton);
        buttonManager.DisableButton(buttonManager.whiskeyButton);
        buttonManager.DisableButton(buttonManager.coverButton);
    }
    public void CheckButtons()
    {
        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 20)
            buttonManager.EnableButton(buttonManager.endDeliveryButton);
        else if (tutorialManager.isRunningT1)
            buttonManager.DisableButton(buttonManager.endDeliveryButton);

        if (tutorialManager.isRunningT1 && cupServed)
            buttonManager.EnableButton(buttonManager.submitOrderButton);
        else if (tutorialManager.isRunningT1)
            buttonManager.DisableButton(buttonManager.submitOrderButton);

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 21 || tutorialManager.currentStep == 22)
            buttonManager.DisableButton(buttonManager.gameButton);
        else
            buttonManager.EnableButton(buttonManager.gameButton);

        if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 0)
            buttonManager.DisableButton(buttonManager.bakeryButton);
        else
            buttonManager.EnableButton(buttonManager.bakeryButton);

        if (tutorialManager.isRunningT2 && tutorialManager.currentStep != 9)
            buttonManager.DisableButton(buttonManager.returnBakeryButton);
        else if (tutorialManager.isRunningT2 && tutorialManager.currentStep == 9)
            buttonManager.EnableButton(buttonManager.returnBakeryButton);

        if (tazaIsInCafetera || vasoIsInCafetera)
            buttonManager.EnableButton(buttonManager.coffeeButton);
            buttonManager.EnableButton(buttonManager.papeleraButton);

        if (tazaInHand || vasoInHand || TengoOtroObjetoEnLaMano())
        {
            buttonManager.DisableButton(buttonManager.submitOrderButton);
            buttonManager.DisableButton(buttonManager.bakeryButton);
            buttonManager.DisableButton(buttonManager.recipesBookButton);
            buttonManager.DisableButton(buttonManager.orderNoteButton);
            buttonManager.DisableButton(buttonManager.papeleraButton);
        }
        else if (cupServed || vasoIsInTable)
        {
            buttonManager.EnableButton(buttonManager.submitOrderButton);
        }
        else
        {
            buttonManager.EnableButton(buttonManager.bakeryButton);
            buttonManager.EnableButton(buttonManager.recipesBookButton);
            buttonManager.EnableButton(buttonManager.orderNoteButton);
        }
    }
    
    #region Mecanicas cafe
    public void ActualizarBotonCogerEnvase()
    {
        if (tazaInHand || tazaIsInCafetera || vasoInHand || vasoIsInCafetera)
        {
            buttonManager.DisableButton(buttonManager.cogerTazaInicioButton);
            buttonManager.DisableButton(buttonManager.cogerVasoInicioButton);
        }
        else
        {
            buttonManager.EnableButton(buttonManager.cogerTazaInicioButton);
            buttonManager.EnableButton(buttonManager.cogerVasoInicioButton);
        }
    }
   
    public void ToggleTazaCafetera()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand || filtroInHand)
            return;

        if (!tazaInHand && !tazaIsInCafetera)
            return;

        if (!tazaIsInCafetera && tazaInHand)
        {
            // Poner en la cafetera
            Taza.SetActive(true);
            Taza.transform.position = puntoCafetera.position;

            tazaInHand = false;
            tazaIsInCafetera = true;

            buttonManager.EnableButton(buttonManager.waterButton);
            buttonManager.EnableButton(buttonManager.milkButton);
            buttonManager.EnableButton(buttonManager.cogerTazaLecheButton);
            buttonManager.EnableButton(buttonManager.condensedMilkButton);
            buttonManager.EnableButton(buttonManager.creamButton);
            buttonManager.EnableButton(buttonManager.chocolateButton);

            cursorManager.UpdateCursorTaza(true);
            Debug.Log($"Taza colocada: {tazaIsInCafetera}");

        }
        else if (tazaIsInCafetera && !tazaInHand)
        {
            //Recoger de la cafetera
            Taza.SetActive(false);
            tazaInHand = true;
            tazaIsInCafetera = false;

            cursorManager.UpdateCursorTaza(false);
        }
        else
        {
            Debug.Log("No hay ninguna taza en la cafetera");
        }
        if (filtroIsInCafetera == true && coffeeServed == false)
        {
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }

        ActualizarBotonCogerEnvase();
    }
    public void ToggleTazaPlato()
    {
        if (TengoOtroObjetoEnLaMano() || filtroInHand || tazaMilkInHand)
            return;

        if (!tazaInHand)
            return;

        if (!platoTazaIsInTable)
            return;

        if (!tazaIsInPlato && tazaInHand)
        {
            // Poner en el plato
            Taza.SetActive(true);
            Taza.transform.position = puntoTazaPlato.position;

            tazaInHand = false;
            tazaIsInPlato = true;
            cupServed = true;

            cursorManager.UpdateCursorTaza(true);
            DisableMechanics();
            Debug.Log($"Taza colocada: {tazaIsInPlato}");

        }
        else if (tazaIsInPlato && !tazaInHand)
        {
            //Recoger de la cafetera
            Taza.SetActive(false);
            tazaInHand = true;
            tazaIsInPlato = false;
            cupServed = false;

            cursorManager.UpdateCursorTaza(false);
        }
        else
        {
            Debug.Log("No hay ninguna taza en el plato");
        }
    }
    public void ToggleVasoCafetera()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand || filtroInHand)
            return;

        if (!vasoInHand && !vasoIsInCafetera)
            return;

        if (!vasoIsInCafetera && vasoInHand)
        {
            // Poner en la cafetera
            Vaso.SetActive(true);
            Vaso.transform.position = puntoCafetera.position;

            vasoInHand = false;
            vasoIsInCafetera = true;

            buttonManager.EnableButton(buttonManager.waterButton);
            buttonManager.EnableButton(buttonManager.milkButton);
            buttonManager.EnableButton(buttonManager.cogerTazaLecheButton);
            buttonManager.EnableButton(buttonManager.condensedMilkButton);
            buttonManager.EnableButton(buttonManager.creamButton);
            buttonManager.EnableButton(buttonManager.chocolateButton);

            cursorManager.UpdateCursorVaso(true);
            Debug.Log($"Vaso colocado: {vasoIsInCafetera}");
        }
        else if (vasoIsInCafetera && !vasoInHand)
        {
            //Recoger de la cafetera
            Vaso.SetActive(false);
            vasoInHand = true;
            vasoIsInCafetera = false;

            cursorManager.UpdateCursorVaso(false);
        }
        else
        {
            Debug.Log("No hay ningun vaso en la cafetera");
        }
        if (filtroIsInCafetera == true && coffeeServed == false)
        {
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }

        ActualizarBotonCogerEnvase();
    }
    public void ToggleVasoMesa()
    {
        if (TengoOtroObjetoEnLaMano() || tazaMilkInHand)
            return;

        if (!vasoInHand && !vasoIsInTable)
            return;

        if (!vasoIsInTable && vasoInHand)
        {
            // Poner en la cafetera
            Vaso.SetActive(true);
            Vaso.transform.position = puntoMesa.position;

            vasoInHand = false;
            vasoIsInTable = true;
            cupServed = true;

            cursorManager.UpdateCursorVaso(true);

            DisableMechanics();
            Debug.Log($"Vaso colocado: {vasoIsInTable}");
        }
        else if (vasoIsInTable && !vasoInHand)
        {
            //Recoger de la cafetera
            Vaso.SetActive(false);
            vasoInHand = true;
            vasoIsInTable = false;
            cupServed = false;

            cursorManager.UpdateCursorVaso(false);
        }
        else
        {
            Debug.Log("No hay ningun vaso en la cafetera");
        }
        if (filtroIsInCafetera == true && coffeeServed == false)
        {
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }
    }
    public void PlacePlatoTazaMesa()
    {
        if (platoTazaIsInTable)
            return;

        if (TengoOtroObjetoEnLaMano())
            return;

        if (!platoTazaInHand && !platoTazaIsInTable)
            return;

        if (platoTazaInHand)
        {
            // Poner en la mesa
            PlatoTaza.SetActive(true);
            PlatoTaza.transform.position = puntoMesa.position;

            platoTazaInHand = false;
            platoTazaIsInTable = true;

            cursorManager.UpdateCursorPlato(true);
            buttonManager.DisableButton(buttonManager.cogerPlatoTazaButton);
            Debug.Log($"Plato colocado: {platoTazaIsInTable}");
        }
    }
    public void StartCoffee()
    {
        if  (!isSliding && !coffeeDone)
        {
            //reiniciamos la pos de la barra
            currentSlideTime = 0f;

            isSliding = true;
            Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Carga de cafe iniciada.");
        }
    }
    public void StopCoffee()
    {
        if (isSliding)
        {
            // detenemos el movimiento
            isSliding = false;
            coffeeDone = true;

            buttonManager.coffeeButton.gameObject.SetActive(false);
            buttonManager.DisableButton(buttonManager.coffeeButton);
            buttonManager.EnableButton(buttonManager.molerButton);

            // guarda la pos del slider
            if (order != null && order.currentOrder != null)
            {
                order.currentOrder.coffeePrecision = currentSlideTime;
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Cafe detenido en: {currentSlideTime:F2}. Valor guardado.");
            } 
            else
            {
                Debug.LogWarning($"[Cliente {order.currentOrder.orderId}] Preparacion: Cafe detenido en: {currentSlideTime:F2}, pero no se pudo guardar porque no hay un pedido activo.");
            }

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 8)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
    }
    public void StartMoler()
    {
        if (!isMoliendo)
        {
            currentMolido = 0f;
            isMoliendo = true;

            molerPanel.SetActive(true);
            molerFillImage.fillAmount = 0f;
            molerFillImage.color = Color.yellow;
            Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Moliendo cafe...");
        }
    }
    public void StopMoler()
    {
        if (isMoliendo && currentMolido == maxFillMoler)
        {
            isMoliendo = false;
            molerPanel.SetActive(false);

            Debug.Log($"[Cliente {order.currentOrder.orderId}] Cafe molido");
            buttonManager.molerButton.gameObject.SetActive(false);
            buttonManager.DisableButton(buttonManager.molerButton);
            buttonManager.filtroButton.gameObject.SetActive(true);
            buttonManager.EnableButton(buttonManager.filtroButton);

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 9)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }
    }
    public void TakeFiltro()
    {
        if (TengoOtroObjetoEnLaMano() || tazaInHand || vasoInHand || tazaMilkInHand) return;

        if (!filtroIsInCafetera)
        {
            filtroInHand = true;
            buttonManager.DisableButton(buttonManager.filtroButton);
            buttonManager.EnableButton(buttonManager.filtroCafeteraButton);
            buttonManager.filtroButton.gameObject.SetActive(false);
            buttonManager.filtroCafeteraButton.gameObject.SetActive(true);
        }
    }
    public void PutFiltro()
    {
        if (filtroIsInCafetera == false)
        {
            filtroIsInCafetera = true;
            filtroInHand = false;

            // Poner en la mesa
            Filtro.SetActive(true);
            Filtro.transform.position = puntoFiltroCafetera.position;

            if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 10)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
        }

        if (tazaIsInCafetera == true || vasoIsInCafetera == true && coffeeServed == false)
        {
            buttonManager.echarCafeButton.gameObject.SetActive(true);
            buttonManager.EnableButton(buttonManager.echarCafeButton);
        }
    }
    public void StartServingCoffee()
    {
        if (coffeeServed) return;
        bool recipienteEnCafetera = tazaIsInCafetera || vasoIsInCafetera;

        if (recipienteEnCafetera && filtroIsInCafetera != false && coffeeServed == false)
        {
            Debug.Log($"[Cliente {order.currentOrder.orderId}] Preparacion: Echando cafe...");

        }

        isSlidingServeCoffee = true;
        movingRight = true;

        buttonManager.DisableButton(buttonManager.echarCafeButton);
        buttonManager.DisableButton(buttonManager.filtroCafeteraButton);
        buttonManager.DisableButton(buttonManager.waterButton);
        buttonManager.DisableButton(buttonManager.milkButton);
        buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);
        buttonManager.DisableButton(buttonManager.condensedMilkButton);
        buttonManager.DisableButton(buttonManager.creamButton);
        buttonManager.DisableButton(buttonManager.chocolateButton);

        buttonManager.EnableButton(buttonManager.pararEcharCafeButton);

        buttonManager.echarCafeButton.gameObject.SetActive(false);
        buttonManager.calentarButton.gameObject.SetActive(false);
        buttonManager.pararEcharCafeButton.gameObject.SetActive(true);
    }

    public void MoveSliderServedCoffee()
    {
        float value = serveCoffeeSlider.value;
        float step = sliderSpeed * Time.deltaTime;

        if (movingRight)
        {
            value += step;
            if (value >= 1f)
            {
                value = 1f;
                movingRight = false;
            }
        }
        else
        {
            value -= step;
            if (value <= 0f)
            {
                value = 0f;
                movingRight = true;
            }
        }

        serveCoffeeSlider.value = value;
        currentSlideServedCoffee = value;
    }

    public void StopServingCoffee()
    {
        if (!isSlidingServeCoffee || coffeeServed) return;

        isSlidingServeCoffee = false;
        coffeeServed = true;

        buttonManager.DisableButton(buttonManager.pararEcharCafeButton);

        if (order != null && order.currentOrder != null)
        {
            order.currentOrder.coffeeServedPrecision = currentSlideServedCoffee;
            Debug.Log($"[Cliente {order.currentOrder.orderId}] Echar cafe detenido en: {currentSlideServedCoffee}");
        }

        buttonManager.EnableButton(buttonManager.sugarButton);
        buttonManager.EnableButton(buttonManager.iceButton);
        buttonManager.EnableButton(buttonManager.coverButton);
        buttonManager.EnableButton(buttonManager.whiskeyButton);

        buttonManager.DisableButton(buttonManager.pararEcharCafeButton);
        buttonManager.pararEcharCafeButton.gameObject.SetActive(false);
        buttonManager.filtroCafeteraButton.gameObject.SetActive(false);

        if (tazaIsInCafetera)
        {
            Image taza = Taza.GetComponent<Image>();
            taza.sprite = tazaConCafe;
        }
        else if (vasoIsInCafetera)
        {
            Image vaso = Vaso.GetComponent<Image>();
            vaso.sprite = vasoConCafe;
        }

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 12)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();
    }
    #endregion

    #region Mecanicas leche
    public void CogerLeche()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            milkInHand = true;
        }
        else if (milkInHand == true)
        {
            milkInHand = false;
        }
    }
    public void EcharLecheFria()
    {
        //Si se tiene la leche en la mano y el cafe no esta servido entonces se puede echar
        if (milkInHand == true && coffeeServed == false)
        {
            if (countMilk <= 1)
            {
                countMilk += 1; //Se incrementa el contador de leche
                order.currentOrder.milkPrecision = countMilk; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Cantidad de leche: " + countMilk);
            }
            milkServed = true;
            buttonManager.cogerTazaLecheButton.gameObject.SetActive(false);
        }
    }
    public void ToggleTazaLecheEspumador()
    {
        if (TengoOtroObjetoEnLaMano() | vasoInHand | tazaInHand)
            return;
        if (!tazaMilkInHand && !tazaMilkIsInEspumador)
            return;

        if (!tazaMilkIsInEspumador && tazaMilkInHand)
        {
            // Poner en el espumador
            TazaLeche.SetActive(true);
            TazaLeche.transform.position = puntoEspumador.position;

            tazaMilkInHand = false;
            tazaMilkIsInEspumador = true;

            buttonManager.DisableButton(buttonManager.waterButton);
            buttonManager.milkButton.gameObject.SetActive(false);
            buttonManager.DisableButton(buttonManager.milkButton);
            buttonManager.DisableButton(buttonManager.cogerTazaLecheButton);

            buttonManager.EnableButton(buttonManager.calentarButton);

            cursorManager.UpdateCursorTazaMilk(true);
            Debug.Log($"Taza con leche colocada en espumador: {tazaMilkIsInEspumador}");

            if (tutorialManager.isRunningT3 && tutorialManager.currentStep == 1)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep3();

        }
        else if (tazaMilkIsInEspumador && !tazaMilkInHand)
        {
            //Recoger del espumador
            TazaLeche.SetActive(false);

            tazaMilkInHand = true;
            tazaMilkIsInEspumador = false;

            cursorManager.UpdateCursorTazaMilk(false);
        }
    }
    public void StartHeating()
    {
        if (!tazaMilkIsInEspumador || TengoOtroObjetoEnLaMano()) return;

        if (!isHeating && !heatedMilk)
        {
            currentHeat = 0f;
            isHeating = true;

            heatPanel.SetActive(true);
            curvedFillImage.fillAmount = 0f;
            curvedFillImage.color = Color.blue;

            Debug.Log($"[Cliente {order.currentOrder.orderId}] Calentando la leche...");

            if (tutorialManager.isRunningT3 && tutorialManager.currentStep == 2)
                FindFirstObjectByType<TutorialManager>().CompleteCurrentStep3();
        }
    }
    public void StopHeating()
    {
        if (isHeating)
        {
            isHeating = false;
            heatedMilk = true;

            heatPanel.SetActive(false);
            buttonManager.calentarButton.gameObject.SetActive(false);
            buttonManager.DisableButton(buttonManager.calentarButton);

            Debug.Log($"[Cliente {order.currentOrder.orderId}] Leche calentada");
        }
    }
    public void EcharLecheCaliente()
    {
        //Si se tiene la leche caliente en la mano y el cafe no esta servido entonces se puede echar
        if (tazaMilkInHand == true && coffeeServed == false)
        {
            if (countMilk <= 1)
            {
                if (currentHeat < 0.4f)
                {
                    order.currentOrder.heatedMilkPrecision = 0;
                    Debug.Log("Leche fria echada");
                }
                else if (currentHeat < 0.8f)
                {
                    order.currentOrder.heatedMilkPrecision = 1;
                    Debug.Log("Leche caliente echada");
                }
                else
                {
                    order.currentOrder.heatedMilkPrecision = 2;
                    Debug.Log("Leche quemada echada");
                }
            }
            milkServed = true;
            TazaLeche.SetActive(false);
            tazaMilkInHand = false;
            cursorManager.UpdateCursorTazaMilk(true);
        }
    }
    #endregion

    #region Mecanica agua
    public void CogerAgua()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            waterInHand = true;
        }
        else if (waterInHand == true)
        {
            waterInHand = false;
        }
    }
    public void EcharAgua()
    {
        //Si se tiene el agua en la mano y el cafe no esta servido entonces se puede echar 
        if (waterInHand == true && coffeeServed == false)
        {
            if (countWater <= 1)
            {
                countWater += 1; //Se incrementa el contador de agua
                order.currentOrder.waterPrecision = countWater; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado agua.");
            }
        }
    }
    #endregion

    #region Mecanica leche condensada
    public void CogerLecheCondensada()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            condensedMilkInHand = true;
        }
        else if (condensedMilkInHand == true)
        {
            condensedMilkInHand = false;
        }
    }
    public void EcharLecheCondensada()
    {
        //Si se tiene la leche condensada en la mano y el cafe no esta servido entonces se puede echar 
        if (condensedMilkInHand == true && coffeeServed == false)
        {
            if (countCondensedMilk <= 1)
            {
                countCondensedMilk += 1; //Se incrementa el contador de leche condensada
                order.currentOrder.condensedMilkPrecision = countCondensedMilk; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado leche condensada.");
            }
        }
    }
    #endregion

    #region Mecanica crema
    public void CogerCrema()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            creamInHand = true;
        }
        else if (creamInHand == true)
        {
            creamInHand = false;
        }
    }
    public void EcharCrema()
    {
        //Si se tiene la crema en la mano y el cafe no esta servido entonces se puede echar
        if (creamInHand == true && coffeeServed == false)
        {
            if (countCream <= 1)
            {
                countCream += 1; //Se incrementa el contador de crema
                order.currentOrder.creamPrecision = countCream; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado crema.");
            }
        }
    }
    #endregion

    #region Mecanica chocolate
    public void CogerChocolate()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            chocolateInHand = true;
        }
        else if (chocolateInHand == true)
        {
            chocolateInHand = false;
        }
    }
    public void EcharChocolate()
    {
        //Si se tiene el chocolate en la mano y el cafe no esta servido entonces se puede echar
        if (chocolateInHand == true && coffeeServed == false)
        {
            if (countChocolate <= 3)
            {
                countChocolate += 1; //Se incrementa el contador de chocolate
                order.currentOrder.chocolatePrecision = countChocolate; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado chocolate.");
            }
        }
    }
    #endregion

    #region Mecanica whiskey
    public void CogerWhiskey()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            whiskeyInHand = true;
        }
        else if (whiskeyInHand == true)
        {
            whiskeyInHand = false;
        }
    }
    public void EcharWhiskey()
    {
        //Si se tiene el whiskey en la mano y el cafe esta servido entonces se puede echar
        if (whiskeyInHand == true && coffeeServed == true)
        {
            if (countWhiskey <= 1)
            {
                countWhiskey += 1; //Se incrementa el contador de hielo
                order.currentOrder.whiskeyPrecision = countWhiskey; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado whiskey.");
            }
        }
    }
    #endregion

    #region Mecanica azucar
    public void CogerAzucar()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            cucharaInHand = true;
        }
        else if (cucharaInHand == true)
        {
            cucharaInHand = false;
        }
    }
    public void EcharAzucar()
    {
        //Si se tiene la cuchara de azucar en la mano y el cafe esta servido entonces se puede echar
        if (cucharaInHand == true && coffeeServed == true)
        {
            if (countSugar <= 1)
            {
                countSugar += 1; //Se incrementa el contador de hielo
                order.currentOrder.sugarPrecision = countSugar; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Cantidad de azucar: " + countSugar);
            }
        }
    }
    #endregion

    #region Mecanica hielo
    public void CogerHielo()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand)
        {
            iceInHand = true;
        }
        else if (iceInHand == true)
        {
            iceInHand = false;
        }
    }
    public void EcharHielo()
    {
        //Si se tiene la cuchara de hielo en la mano y el cafe esta servido entonces se puede echar
        if (iceInHand == true && coffeeServed == true)
        {
            if (countIce <= 1)
            {
                countIce += 1; //Se incrementa el contador de hielo
                order.currentOrder.icePrecision = countIce; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Has echado hielo.");
            }
        }
    }
    #endregion

    #region Mecanica tipo de pedido
    public void CogerTapa()
    {
        if (!TengoOtroObjetoEnLaMano() && !tazaInHand && !vasoInHand && !tazaMilkInHand)
        {
            coverInHand = true;
        }
        else if (coverInHand == true)
        {
            coverInHand = false;
        }
    }
    public void PonerTapa()
    {
        //Si se tiene la tapa en la mano y el cafe esta servido entonces se puede poner
        if (coverInHand == true && coffeeServed == true && vasoIsInCafetera)
        {
            if (countCover <= 1)
            {
                countCover += 1; //Se incrementa el contador de hielo
                order.currentOrder.typePrecision = countCover; // Se guarda el resultado obtenido en la precision del jugador
                Debug.Log($"[Cliente {order.currentOrder.orderId}] Tapa puesta.");
            }
            Image vaso = Vaso.GetComponent<Image>();
            vaso.sprite = vasoConTapa;

            buttonManager.DisableButton(buttonManager.coverButton);
            cursorManager.SetDefaultCursor();
            coverInHand = false;
        }
    }
    #endregion

    #region Sonidos
    public void BotonDownMachine()
    {
        SoundsMaster.Instance.PlaySound_CoffeeMachine();
    }

    public void BotonUpMachine()
    {
        SoundsMaster.Instance.PlaySound_CoffeeReady();
    }
    #endregion
}
