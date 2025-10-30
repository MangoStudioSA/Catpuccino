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

    private int currentStep = 0;
    private int tutorialDay = 0;
    private bool isRunning = false;

    public ButtonUnlockManager buttonManager;

    private void Start()
    {
        tutorialPanel.gameObject.SetActive(false);

        int currentDay = FindFirstObjectByType<TimeManager>().currentDay;

        if (currentDay == tutorialDay)
        {
            SetupSteps();
            StartTutorial();
        }
    }

    private void SetupSteps()
    {
        steps = new List<TutorialStep>();
        {
            // Paso 0
            steps.Add(new TutorialStep
            {
                message = "¡Bienvenidx a Catpuccino! Tu primer día en la cafetería ha comenzado.",
                position = new Vector2(0f,0f),
                autoAdvance = true,
                autoDelay = 10f
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
                position = new Vector2(400f, -90f),
                autoAdvance = false,
                onStepStart = () =>
                {

                }
            });
            // Paso 3
            steps.Add(new TutorialStep
            {
                message = "Para saber cómo preparar el café puedes abrir el libro de recetas",
                position = new Vector2(-250f, 410),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.recipesBookButton);
                }
            });
            // Paso 4
            steps.Add(new TutorialStep
            {
                message = "Ahora que ya sabes cómo preparar el café, haz clic en el botón para seleccionar la cantidad de café correspondiente.",
                position = new Vector2(-325f, -202f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.coffeeButton);
                }
            });
            // Paso 5
            steps.Add(new TutorialStep
            {
                message = "¡Presiona la palanca para moler el café!",
                position = new Vector2(243f, 114f),
                autoAdvance = true,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.molerButton);
                }
            });
            // Paso 6
            steps.Add(new TutorialStep
            {
                message = "Una vez molido el café, mueve el filtro a la cafetera clicando sobre él.",
                position = new Vector2(-96f, -288f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.filtroButton);
                }
            });
            // Paso 7
            steps.Add(new TutorialStep
            {
                message = "Coloca una taza para poder echar el café. Si se trata de un pedido para llevar coloca un vaso.",
                position = new Vector2(20f, 280f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 8
            steps.Add(new TutorialStep
            {
                message = "Si es un pedido para tomar, deberás colocar un plato, mientras que si es para llevar deberás colocar la tapa del vaso.",
                position = new Vector2(-654f, 225f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 9
            steps.Add(new TutorialStep
            {
                message = "Puedes comprobar los requisitos de la orden clicando en \"Pedido\"",
                position = new Vector2(195f, 280f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.orderNoteButton);
                }
            });
            // Paso 10
            steps.Add(new TutorialStep
            {
                message = "¡Ya puedes clicar para echar el café!",
                position = new Vector2(-488f, -31f),
                autoAdvance = false,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.echarCafeButton);
                }
            });
            // Paso 11
            steps.Add(new TutorialStep
            {
                message = "Ahora puedes echar el azúcar y los hielos clicando sobre ellos.",
                position = new Vector2(20f, 280f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 12
            steps.Add(new TutorialStep
            {
                message = "¡Ten cuidado con el orden! ¡Algunos elementos se bloquearan a medida que realizas acciones!",
                position = new Vector2(30f, -390f),
                autoAdvance = true,
                autoDelay = 4f
            });
            // Paso 13
            steps.Add(new TutorialStep
            {
                message = "¡Si te equivocas con la preparación puedes empezar de 0 clicando sobre la basura!",
                position = new Vector2(-146f, 141f),
                autoAdvance = true,
                autoDelay = 4f
            });
            // Paso 14
            steps.Add(new TutorialStep
            {
                message = "Cuando tengas todo listo coloca el vaso sobre la mesa. Si es una taza coloca el plato y posteriormente la taza.",
                position = new Vector2(600f, -49f),
                autoAdvance = true,
                autoDelay = 4f
            });
            // Paso 15
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
            // Paso 16
            steps.Add(new TutorialStep
            {
                message = "El cliente te dará una valoración y pagará en función de la puntuación que hayas obtenido al preparar su comanda.",
                position = new Vector2(-480f, -160f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 17
            steps.Add(new TutorialStep
            {
                message = "¡Si está muy contento podrá darte una propina!",
                position = new Vector2(350f, 375f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 18
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
            // Paso 19
            steps.Add(new TutorialStep
            {
                message = "¡Ya has atendido a tu primer cliente! Sigue atendiendo a más para poder pagar las facturas al final del día. La cafetería cierra a las 20:00pm.",
                position = new Vector2(0f, 0f),
                autoAdvance = true,
                autoDelay = 5f
            });
            // Paso 20
            steps.Add(new TutorialStep
            {
                message = "Aquí tienes 100$ para ayudarte a pasar el primer día. ¡Disfruta de Catpuccino!",
                position = new Vector2(0f, 0f),
                autoAdvance = true,
                autoDelay = 5f
            });
        }
    }

    public void StartTutorial()
    {
        isRunning = true;
        currentStep = 0;
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        StartCoroutine(ShowStep());
    }

    private IEnumerator ShowStep()
    {
        if (currentStep >= steps.Count)
        {
            EndTutorial();
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

    // Llamar cuando el jugador haga la acción correcta
    public void CompleteCurrentStep()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutPanel(() =>
        {
            var step = steps[currentStep];
            step.onStepComplete?.Invoke();

            currentStep++;
            if (currentStep < steps.Count) StartCoroutine(ShowStep());
            else EndTutorial();
        }));
    }

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

    private void EndTutorial()
    {
        isRunning = false;
        tutorialPanel.gameObject.SetActive(false);
        HUDManager.Instance.UpdateMonedas(100);
        Debug.Log("Tutorial completado");
    }
}

