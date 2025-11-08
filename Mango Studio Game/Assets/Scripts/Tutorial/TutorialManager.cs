using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public string message;
        public Vector2 position;
        public bool autoAdvance;
        public float autoDelay = 5f;
        public System.Action onStepStart;
        public System.Action onStepComplete;
    }

    [Header("Tutorial")]
    [SerializeField] public RectTransform tutorialPanel;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] private List<TutorialStep> steps;

    public float fadeDuration = 0.5f;
    public float bounceScale = 1.04f;
    public float bounceSpeed = 0.7f;

    public int currentStep = 0;
    public bool isRunningT1 = false;
    public bool isRunningT2 = false;
    public bool isRunningT3 = false;

    public ButtonUnlockManager buttonManager;

    private void OnEnable()
    {
        if (TimeManager.Instance != null) TimeManager.Instance.onDayStarted += HandleDayStarted;
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null) TimeManager.Instance.onDayStarted -= HandleDayStarted;
    }

    private IEnumerator Start()
    {
        tutorialPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);

        int currentDay = TimeManager.Instance.currentDay;
        HandleDayStarted(currentDay);
    }

    private void HandleDayStarted (int day)
    {
        Debug.Log($"[TutorialManager] HandleDayStarted: {day}");

        switch (day)
        {
            case 1:
                SetupDay1Tutorial();
                StartTutorial1();
                isRunningT1 = true;
                Debug.Log("Comenzando tutorial día 1");
                break;

            case 2:
                SetupDay2Tutorial();
                isRunningT2 = true;
                Debug.Log("Comenzando tutorial día 2");
                break;

            case 3:
                SetupDay3Tutorial();
                isRunningT3 = true;
                Debug.Log("Comenzando tutorial día 3");
                break;

            default:
                Debug.Log("[TutorialManager] No hay tutorial configurado para hoy");
                break;
        }
    }
    #region Tutorial dia 1
    private void SetupDay1Tutorial()
    {
        steps = new List<TutorialStep>();
        {
            // Paso 0
            steps.Add(new TutorialStep
            {
                message = "¡Bienvenido/a a Catpuccino! Tu primer día en la cafetería ha comenzado.",
                position = new Vector2(0f, 0f),
                autoAdvance = true,
                autoDelay = 6f
            });
            // Paso 1
            steps.Add(new TutorialStep
            {
                message = "Comienza aceptando el pedido del primer cliente.",
                position = new Vector2(-430, 210f),
                autoAdvance = false,
                onStepStart = () =>
                {
                }
            });
            // Paso 2
            steps.Add(new TutorialStep
            {
                message = "¡Ya sabes que quiere el cliente! ¡Manos a la obra, haz clic en \"Aceptar pedido\"!",
                position = new Vector2(400f, -91f),
                autoAdvance = false,
                onStepStart = () =>
                {
                }
            });
            // Paso 3
            steps.Add(new TutorialStep
            {
                message = "Coloca una taza para poder echar el café. Si se trata de un pedido para llevar coloca un vaso.",
                position = new Vector2(-723f, -110f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 4
            steps.Add(new TutorialStep
            {
                message = "Si es para tomar, deberás clicar sobre un plato y colocarlo en la zona de entrega.",
                position = new Vector2(575f, -125f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 5
            steps.Add(new TutorialStep
            {
                message = "Si es para llevar, al finalizar deberás clicar sobre la tapa y colocarla en el vaso.",
                position = new Vector2(-654f, -325f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 6
            steps.Add(new TutorialStep
            {
                message = "Puedes comprobar los requisitos de la comanda clicando en \"Pedido\"",
                position = new Vector2(120f, 280f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.orderNoteButton);
                }
            });
            // Paso 7
            steps.Add(new TutorialStep
            {
                message = "Para saber cómo preparar los cafés haz clic en el libro de recetas.",
                position = new Vector2(-685f, 225f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.recipesBookButton);
                }
            });
            // Paso 8
            steps.Add(new TutorialStep
            {
                message = "Ahora que ya sabes cómo preparar el café, mantén presionado el botón para seleccionar la cantidad de café correspondiente.",
                position = new Vector2(-380f, -120f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.coffeeButton);
                }
            });
            // Paso 9
            steps.Add(new TutorialStep
            {
                message = "¡Mantén presionada la palanca para moler el café!",
                position = new Vector2(160f, 114f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.molerButton);
                }
            });
            // Paso 10
            steps.Add(new TutorialStep
            {
                message = "Una vez molido el café, mueve el filtro a la cafetera clicando sobre él.",
                position = new Vector2(-157f, -231f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.filtroButton);
                }
            });
            // Paso 11
            steps.Add(new TutorialStep
            {
                message = "Comprueba si el café se prepara con agua. Si es así, clica sobre ella e interactúa con el recipiente.",
                position = new Vector2(30f, -390f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 12
            steps.Add(new TutorialStep
            {
                message = "¡Ya puedes clicar para echar el café! ¡Presiona el botón cuando el marcador esté cerca de la zona marcada!",
                position = new Vector2(-390f, 180f),
                autoAdvance = false,
                onStepStart = () =>
                {
                }
            });
            // Paso 13
            steps.Add(new TutorialStep
            {
                message = "Ahora puedes echar el azúcar y los hielos clicando sobre ellos e interactuando con el recipiente mediante clic.",
                position = new Vector2(30f, -390f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 14
            steps.Add(new TutorialStep
            {
                message = "¡Ten cuidado con el orden! ¡Algunos elementos se bloquearan a medida que realices acciones!",
                position = new Vector2(30f, -390f),
                autoAdvance = true,
                autoDelay = 4f
            });
            // Paso 15
            steps.Add(new TutorialStep
            {
                message = "¡Si te equivocas con la preparación puedes empezar de 0 clicando sobre la basura!",
                position = new Vector2(-200f, 100f),
                autoAdvance = true,
                autoDelay = 4f
            });
            // Paso 16
            steps.Add(new TutorialStep
            {
                message = "Cuando tengas todo listo, coloca el vaso sobre la mesa. Si se trata de una taza, colócala sobre el plato.",
                position = new Vector2(600f, -100f),
                autoAdvance = true,
                autoDelay = 4f
            });
            // Paso 17
            steps.Add(new TutorialStep
            {
                message = "Presiona \"Entregar\" para entregarle el pedido al cliente.",
                position = new Vector2(195f, -390f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.submitOrderButton);
                }
            });
            // Paso 18
            steps.Add(new TutorialStep
            {
                message = "El cliente expondrá su valoración y pagará en función de la puntuación que hayas obtenido al preparar su comanda.",
                position = new Vector2(-480f, -160f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 19
            steps.Add(new TutorialStep
            {
                message = "¡Si la puntuación es alta te dará una propina!",
                position = new Vector2(350f, 375f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 20
            steps.Add(new TutorialStep
            {
                message = "Haz clic en \"Finalizar\" para volver a la cafetería.",
                position = new Vector2(-80f, -275f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.endDeliveryButton);
                }
            });
            // Paso 21
            steps.Add(new TutorialStep
            {
                message = "¡Ya has atendido a tu primer cliente! Sigue atendiendo más para poder pagar las facturas al final del día. ¡La cafetería cierra a las 20:00pm!",
                position = new Vector2(0f, 0f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 22
            steps.Add(new TutorialStep
            {
                message = "Como recompensa, se te ingresarán 100$ para ayudarte a pasar el primer día y 50 monedas de café. ¡Disfruta de Catpuccino!",
                position = new Vector2(0f, 0f),
                autoAdvance = true,
                autoDelay = 5f
            });
        }
    }

    public void StartTutorial1()
    {
        currentStep = 0;
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        StartCoroutine(ShowStepT1());
    }

    private IEnumerator ShowStepT1()
    {
        if (currentStep >= steps.Count)
        {
            EndTutorialT1();
            yield break;
        }

        var step = steps[currentStep];

        // Mover el panel a la posicion indicada
        tutorialPanel.anchoredPosition = step.position;
        tutorialText.text = step.message;
        tutorialPanel.gameObject.SetActive(true);

        step.onStepStart?.Invoke();

        // Efecto fade + rebote al mostrar el paso del turorial
        yield return StartCoroutine(FadeInPanel());
        StartCoroutine(BouncePanelLoop());

        if (step.autoAdvance)
        {
            yield return new WaitForSeconds(step.autoDelay);
            CompleteCurrentStep();
        }
    }

    public void CompleteCurrentStep()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutPanel(() =>
        {
            var step = steps[currentStep];
            step.onStepComplete?.Invoke();

            currentStep++;
            if (currentStep < steps.Count) StartCoroutine(ShowStepT1());
            else EndTutorialT1();
        }));
    }

    public void EndTutorialT1()
    {
        isRunningT1 = false;
        tutorialPanel.gameObject.SetActive(false);
        GameManager.Instance.AnadirMonedas(100);
        Debug.Log("Tutorial completado");
    }
    #endregion

    #region Tutorial dia 2
    private void SetupDay2Tutorial()
    {
        steps = new List<TutorialStep>();
        {
            // Paso 0
            steps.Add(new TutorialStep
            {
                message = "¡Cada día desbloquearás nuevas recetas e ingredientes! Interactúa con ellos haciendo clic.",
                position = new Vector2(245f, -390f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 1
            steps.Add(new TutorialStep
            {
                message = "¡Ahora puedes visitar la zona de pastelería! Haz clic sobre el bótón para ir.",
                position = new Vector2(50f, 290f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.bakeryButton);
                }
            });
            // Paso 2
            steps.Add(new TutorialStep
            {
                message = "Comienza poniendo un plato o una bolsa para llevar en la encimera según el tipo de pedido.",
                position = new Vector2(-600f, 60f),
                autoAdvance = false,
                onStepStart = () =>
                {
                }
            });
            // Paso 3
            steps.Add(new TutorialStep
            {
                message = "Ahora, selecciona el tipo de bizcocho correspondiente.",
                position = new Vector2(460f, 100f),
                autoAdvance = false,
                onStepStart = () =>
                {
                }
            });
            // Paso 4
            steps.Add(new TutorialStep
            {
                message = "Mediante clic, coloca el bizcocho en el horno.",
                position = new Vector2(-186f, 195f),
                autoAdvance = false,
                onStepStart = () =>
                {
                }
            });
            // Paso 5
            steps.Add(new TutorialStep
            {
                message = "¡Pulsa el botón para hornearlo!",
                position = new Vector2(-160f, -360f),
                autoAdvance = false,
                onStepStart = () =>
                {
                }
            });
            // Paso 6
            steps.Add(new TutorialStep
            {
                message = "Vigila el tiempo de horneado. ¡Si lo paras antes quedará crudo! ¡Si te pasas se quemará!",
                position = new Vector2(-146f, 306f),
                autoAdvance = true,
                autoDelay = 4f
            });
            // Paso 7
            steps.Add(new TutorialStep
            {
                message = "Una vez finalice el horneado. Mueve el bizcocho al recipiente mediante clic.",
                position = new Vector2(-680f, 100f),
                autoAdvance = false,
                onStepStart = () =>
                {
                }
            });
            // Paso 8
            steps.Add(new TutorialStep
            {
                message = "Si el cliente no había pedido comida, puedes comenzar de 0 clicando sobre la basura.",
                position = new Vector2(-600f, 60f),
                autoAdvance = true,
                autoDelay = 4f
            });
            // Paso 9
            steps.Add(new TutorialStep
            {
                message = "¡Ya has finalizado la preparación del dulce! Vuelve a la zona de los cafés para continuar la comanda.",
                position = new Vector2(280f, 221f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.returnBakeryButton);
                }
            });
        }
    }

    public void StartTutorial2()
    {
        currentStep = 0;
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        StartCoroutine(ShowStepT2());
    }

    public void CompleteCurrentStep2()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutPanel(() =>
        {
            var step = steps[currentStep];
            step.onStepComplete?.Invoke();

            currentStep++;
            if (currentStep < steps.Count) StartCoroutine(ShowStepT2());
            else EndTutorialT2();
        }));
    }

    private IEnumerator ShowStepT2()
    {
        if (currentStep >= steps.Count)
        {
            EndTutorialT2();
            yield break;
        }

        var step = steps[currentStep];

        // Mover el panel a la posicion indicada
        tutorialPanel.anchoredPosition = step.position;
        tutorialText.text = step.message;
        tutorialPanel.gameObject.SetActive(true);

        step.onStepStart?.Invoke();

        // Efecto fade + rebote al mostrar el paso del turorial
        yield return StartCoroutine(FadeInPanel());
        StartCoroutine(BouncePanelLoop());

        if (step.autoAdvance)
        {
            yield return new WaitForSeconds(step.autoDelay);
            CompleteCurrentStep2();
        }
    }

    public void EndTutorialT2()
    {
        isRunningT2 = false;
        tutorialPanel.gameObject.SetActive(false);
        Debug.Log("Tutorial 2 completado");
    }
    #endregion

    #region Tutorial dia 3
    private void SetupDay3Tutorial()
    {
        steps = new List<TutorialStep>();
        {
            // Paso 0
            steps.Add(new TutorialStep
            {
                message = "¡Hoy has desbloqueado la opción de calentar la leche!",
                position = new Vector2(0f, 0f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 1
            steps.Add(new TutorialStep
            {
                message = "Clica sobre la taza de leche y colócala en el espumador.",
                position = new Vector2(-140f, -170f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.cogerTazaLecheButton);
                }
            });
            // Paso 2
            steps.Add(new TutorialStep
            {
                message = "Ahora, presiona la rueda para calentar hasta el punto indicado.",
                position = new Vector2(100f, 260f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.calentarButton);
                }
            });
            // Paso 3
            steps.Add(new TutorialStep
            {
                message = "¡Ten cuidado de no pasarte calentando la leche ni de dejarla fría!",
                position = new Vector2(581f, 54f),
                autoAdvance = true,
                autoDelay = 4f
            });
            // Paso 4
            steps.Add(new TutorialStep
            {
                message = "¡Ahora puedes echarle la leche caliente a tus cafés!",
                position = new Vector2(581f, 54f),
                autoAdvance = true,
                autoDelay = 4f
            });
        }
    }

    public void StartTutorial3()
    {
        currentStep = 0;
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        StartCoroutine(ShowStepT3());
    }

    public void CompleteCurrentStep3()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutPanel(() =>
        {
            var step = steps[currentStep];
            step.onStepComplete?.Invoke();

            currentStep++;
            if (currentStep < steps.Count) StartCoroutine(ShowStepT3());
            else EndTutorialT3();
        }));
    }

    private IEnumerator ShowStepT3()
    {
        if (currentStep >= steps.Count)
        {
            EndTutorialT3();
            yield break;
        }

        var step = steps[currentStep];

        // Mover el panel a la posicion indicada
        tutorialPanel.anchoredPosition = step.position;
        tutorialText.text = step.message;
        tutorialPanel.gameObject.SetActive(true);

        step.onStepStart?.Invoke();

        // Efecto fade + rebote al mostrar el paso del turorial
        yield return StartCoroutine(FadeInPanel());
        StartCoroutine(BouncePanelLoop());

        if (step.autoAdvance)
        {
            yield return new WaitForSeconds(step.autoDelay);
            CompleteCurrentStep3();
        }
    }

    public void EndTutorialT3()
    {
        isRunningT2 = false;
        tutorialPanel.gameObject.SetActive(false);
        Debug.Log("Tutorial 3 completado");
    }
    #endregion

    // Funciones para la animacion y el fade in/out de los mensajes 
    private IEnumerator FadeInPanel()
    {
        canvasGroup.alpha = 0f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
    }

    private IEnumerator FadeOutPanel(System.Action onFinish)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
        tutorialPanel.gameObject.SetActive(false);
        onFinish?.Invoke();
    }

    private IEnumerator BouncePanelLoop()
    {
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = Vector3.one * bounceScale;

        while (tutorialPanel.gameObject.activeSelf)
        {
            // Escalar hacia afuera
            float elapsed = 0f;
            while (elapsed < bounceSpeed)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Sin((elapsed / bounceSpeed) * Mathf.PI * 0.5f);
                tutorialPanel.localScale = Vector3.Lerp(originalScale, targetScale, t);
                yield return null;
            }

            // Escalar de vuelta
            elapsed = 0f;
            while (elapsed < bounceSpeed)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Sin((elapsed / bounceSpeed) * Mathf.PI * 0.5f);
                tutorialPanel.localScale = Vector3.Lerp(targetScale, originalScale, t);
                yield return null;
            }
        }
    }
}
