using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;


// Clase para gestionar los tutoriales
public class TutorialManager : MonoBehaviour
{
    // Bocadillos
    public enum StepType
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    // Gatos
    public enum CatPose
    {
        CatDown,
        CatUp,
        CatRight,
        CatLeft,
        CatCenter
    }

    // Se crea una clase basica para los componentes del tutorial
    [System.Serializable]
    public class TutorialStep
    {
        public string message;
        public StepType stepType;
        public CatPose catPose;
        public Vector2 catPosition;
        public Vector2 position;
        public bool autoAdvance;

        public List<GameObject> highlightObjects;
        public Material glowMaterial;
        [HideInInspector] public List<Material> originalMaterials = new();

        public System.Action onStepStart;
        public System.Action onStepComplete;
    }

    [Header("Prefabs sprites bocadillos")]
    [SerializeField] private GameObject prefabArribaIzq;
    [SerializeField] private GameObject prefabArribaDer;
    [SerializeField] private GameObject prefabAbajoIzq;
    [SerializeField] private GameObject prefabAbajoDer;

    [Header("Prefabs Gatos")]
    [SerializeField] private GameObject catDownPrefab;
    [SerializeField] private float catDownRotation = 0f;
    [SerializeField] private GameObject catUpPrefab;
    [SerializeField] private float catUpRotation = 180f;
    [SerializeField] private GameObject catRightPrefab;
    [SerializeField] private float catRightRotation = -90f;
    [SerializeField] private GameObject catLeftPrefab;
    [SerializeField] private float catLeftRotation = 90f;
    [SerializeField] private GameObject catCenterPrefab;
    [SerializeField] private float catCenterRotation = 0f;

    [Header("Elementos tutorial")]
    [SerializeField] public Transform tutorialContainer;
    public GameObject clickHintObject;

    private GameObject currentCatInstance;
    private GameObject currentBubbleObject;
    private RectTransform currentPanelRect;
    private CanvasGroup currentCanvasGroup;
    private TextMeshProUGUI currentTextTMP;

    [Header("Pasos tutoriales")]
    [SerializeField] private List<TutorialStep> steps;

    [Header("Configuracion")]
    public float fadeDuration = 0.5f;
    public float bounceScale = 1.04f;
    public float bounceSpeed = 0.7f;
    public int currentStep = 0;
    public bool isRunningT1 = false;
    public bool isRunningT2 = false;
    public bool isRunningT3 = false;

    [Header("Referencias")]
    public GameObject skipTutorialButton;
    public ButtonUnlockManager buttonManager;
    public Material glowMaterial;

    [Header("Imagenes")]
    public Image estanteImage;
    public Image bandejaImage;
    public Image espumadorImage;
    public Image cafeteraImage;
    public Image bandejaBImage;
    public Image platoBakeryImage;
    public Image bolsaBakeryImage;
    public Image hornearSliderImage;
    public Image BChocolateImage;
    public Image BMantequillaImage;
    public Image BZanahoriaImage;
    public Image BRedVelvetImage;

    [Header("Imagenes envases")]
    public Image tapaBImage;
    public Image tapaPImage;
    public Image tazaBImage;
    public Image tazaPImage;
    public Image tazaB2Image;
    public Image platoBImage;
    public Image platoPImage;

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
        ClearActualStep();
        ClearCurrentCat();
        yield return new WaitForSeconds(0.1f);

        int currentDay = TimeManager.Instance.currentDay;
        HandleDayStarted(currentDay);
    }

    // Funcion para obtener el prefab correspondiente del bocadillo
    private GameObject GetPrefab(StepType tipo)
    {
        switch (tipo)
        {
            case StepType.UpLeft: return prefabArribaIzq;
            case StepType.UpRight: return prefabArribaDer;
            case StepType.DownLeft: return prefabAbajoIzq;
            case StepType.DownRight: return prefabAbajoDer;
            default: return prefabArribaIzq;
        }
    }

    // Funcion para actualizar el sprite del gato
    private void UpdateCatVisuals(TutorialStep step)
    {
        ClearCurrentCat();

        // Elegir prefab
        GameObject prefabToSpawn = null;
        float fixedRotation = 0f;

        switch (step.catPose)
        {
            case CatPose.CatCenter: 
                prefabToSpawn = catCenterPrefab;
                fixedRotation = catCenterRotation;
                break;
            case CatPose.CatUp: 
                prefabToSpawn = catUpPrefab;
                fixedRotation = catUpRotation;
                break;
            case CatPose.CatDown: 
                prefabToSpawn = catDownPrefab;
                fixedRotation = catDownRotation;
                break;
            case CatPose.CatLeft: 
                prefabToSpawn = catLeftPrefab;
                fixedRotation = catLeftRotation;
                break;
            case CatPose.CatRight: 
                prefabToSpawn = catRightPrefab;
                fixedRotation = catRightRotation;
                break;
        }

        // Instanciar y posicionar
        if (prefabToSpawn != null && tutorialContainer != null)
        {
            currentCatInstance = Instantiate(prefabToSpawn, tutorialContainer);

            RectTransform catRect = currentCatInstance.GetComponent<RectTransform>();
            if (catRect != null)
            {
                catRect.localScale = Vector3.one;
                catRect.anchoredPosition = step.catPosition;

                catRect.localRotation = Quaternion.Euler(0f, 0f, fixedRotation);
            }
            currentCatInstance.transform.SetAsFirstSibling();
        }
    }

    // Funcion para borrar sprite del gato actual
    private void ClearCurrentCat()
    {
        if (currentCatInstance != null)
        {
            Destroy(currentCatInstance);
            currentCatInstance = null;
        }
    }

    // Referencias a los dias que se muestra cada tutorial
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
                stepType = StepType.UpLeft,
                catPose = CatPose.CatCenter,
                catPosition = new Vector2(-433, 193)
            });
            // Paso 1
            steps.Add(new TutorialStep
            {
                message = "Comienza aceptando el pedido del primer cliente.",
                position = new Vector2(-430, 210f),
                autoAdvance = false,
                stepType = StepType.UpLeft,
                catPose = CatPose.CatUp,
                catPosition = new Vector2(-812, 285),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.gameButton.gameObject,
                },
            });
            // Paso 2
            steps.Add(new TutorialStep
            {
                message = "¡Ya sabes que quiere el cliente! ¡Manos a la obra, haz clic en \"Aceptar pedido\"!",
                position = new Vector2(513f, -161f),
                autoAdvance = false,
                stepType = StepType.UpRight,
                catPose = CatPose.CatRight,
                catPosition = new Vector2(707, 131),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.acceptOrderButton.gameObject,
                },
            });
            // Paso 3
            steps.Add(new TutorialStep
            {
                message = "Coloca una taza en la cafetera. Si se trata de un pedido para llevar coloca un vaso.",
                position = new Vector2(-416f, -110f),
                autoAdvance = false,
                stepType = StepType.DownLeft,
                catPose = CatPose.CatDown,
                catPosition = new Vector2(-776, -286.4f),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    tazaB2Image.gameObject,
                    tazaBImage.gameObject,
                    tazaPImage.gameObject,
                    cafeteraImage.gameObject,
                },
            });
            // Paso 4
            steps.Add(new TutorialStep
            {
                message = "Si es para tomar, deberás clicar sobre un plato y colocarlo en la bandeja.",
                position = new Vector2(589f, -90f),
                autoAdvance = false,
                stepType = StepType.UpRight,
                catPose = CatPose.CatRight,
                catPosition = new Vector2(706, 222),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    bandejaImage.gameObject,
                    platoBImage.gameObject,
                    platoPImage.gameObject,
                }
            });
            // Paso 5
            steps.Add(new TutorialStep
            {
                message = "Si es para llevar, al finalizar deberás clicar sobre la tapa y colocarla en el vaso.",
                position = new Vector2(-595f, 2f),
                autoAdvance = true,
                stepType = StepType.UpLeft,
                catPose = CatPose.CatLeft,
                catPosition = new Vector2(-706.3f, 191),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    estanteImage.gameObject,
                    tapaBImage.gameObject,
                    tapaPImage.gameObject,
                }
            });
            // Paso 6
            steps.Add(new TutorialStep
            {
                message = "Puedes comprobar los requisitos de la comanda clicando en \"Pedido\"",
                position = new Vector2(157f, 280f),
                autoAdvance = false,
                stepType = StepType.UpLeft,
                catPose = CatPose.CatUp,
                catPosition = new Vector2(-243, 286.7f),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.orderNoteButton.gameObject,
                },
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.orderNoteButton);
                }
            });
            // Paso 7
            steps.Add(new TutorialStep
            {
                message = "Para saber cómo preparar los cafés haz clic en el libro de recetas.",
                position = new Vector2(-556f, 183f),
                autoAdvance = false,
                stepType = StepType.UpRight,
                catPose = CatPose.CatUp,
                catPosition = new Vector2(-187, 286),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.recipesBookButton.gameObject,
                },
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.recipesBookButton);
                }
            });
            // Paso 8
            steps.Add(new TutorialStep
            {
                message = "Ahora que ya sabes cómo preparar el café, mantén presionado el botón para seleccionar la cantidad de café correspondiente.",
                position = new Vector2(-301f, -150f),
                autoAdvance = false,
                stepType = StepType.DownLeft,
                catPose = CatPose.CatDown,
                catPosition = new Vector2(-683, -288),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.coffeeButton.gameObject,
                },
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.coffeeButton);
                }
            });
            // Paso 9
            steps.Add(new TutorialStep
            {
                message = "¡Mantén presionada la palanca para moler el café!",
                position = new Vector2(260f, 114f),
                autoAdvance = false,
                stepType = StepType.UpRight,
                catPose = CatPose.CatCenter,
                catPosition = new Vector2(678, 295),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.molerButton.gameObject,
                },
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.molerButton);
                }
            });
            // Paso 10
            steps.Add(new TutorialStep
            {
                message = "Una vez molido el café, mueve el filtro a la cafetera clicando sobre él.",
                position = new Vector2(-98f, -270f),
                autoAdvance = false,
                stepType = StepType.DownRight,
                catPose = CatPose.CatDown,
                catPosition = new Vector2(309, -286.5f),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.filtroButton.gameObject,
                    cafeteraImage.gameObject,
                },
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.filtroButton);
                }
            });
            // Paso 11
            steps.Add(new TutorialStep
            {
                message = "Algunos cafés se prepararán con más ingredientes. Cuando sea así, clica sobre ellos e interactúa con el recipiente.",
                position = new Vector2(30f, -390f),
                autoAdvance = true,
                stepType = StepType.DownRight,
                catPose = CatPose.CatDown,
                catPosition = new Vector2(451, -377),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.waterButton.gameObject,
                },
            });
            // Paso 12
            steps.Add(new TutorialStep
            {
                message = "¡Ya puedes clicar para echar el café! ¡Presiona el botón superior y, cuando el marcador esté cerca de la zona marcada, clica en el inferior para pararlo!",
                position = new Vector2(-390f, 180f),
                autoAdvance = false,
                stepType = StepType.UpLeft,
                catPose = CatPose.CatLeft,
                catPosition = new Vector2(-752, 359),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.echarCafeButton.gameObject,
                    buttonManager.pararEcharCafeButton.gameObject,
                },
            });
            // Paso 13
            steps.Add(new TutorialStep
            {
                message = "Ahora puedes echar el azúcar y los hielos clicando sobre ellos e interactuando con el recipiente mediante clic.",
                position = new Vector2(30f, -390f),
                autoAdvance = false,
                stepType = StepType.DownRight,
                catPose = CatPose.CatDown,
                catPosition = new Vector2(431, -371),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.sugarButton.gameObject,
                    buttonManager.iceButton.gameObject
                },
            });
            // Paso 14
            steps.Add(new TutorialStep
            {
                message = "¡Ten cuidado con la preparación! ¡Comprueba en el recetario en qué orden echar los ingredientes o perderás puntos!",
                position = new Vector2(10f, -390f),
                autoAdvance = true,
                stepType = StepType.UpLeft,
                catPose = CatPose.CatDown,
                catPosition = new Vector2(-419, -286),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.waterButton.gameObject,
                },
            });
            // Paso 15
            steps.Add(new TutorialStep
            {
                message = "¡Si te equivocas con la preparación puedes empezar de 0 clicando sobre la basura!",
                position = new Vector2(-18f, -384f),
                autoAdvance = true,
                stepType = StepType.UpLeft,
                catPose = CatPose.CatDown,
                catPosition = new Vector2(-437, -286),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.papeleraButton.gameObject,
                },
            });
            // Paso 16
            steps.Add(new TutorialStep
            {
                message = "Cuando tengas todo listo, coloca el vaso sobre la mesa. Si se trata de una taza, colócala sobre el plato.",
                position = new Vector2(600f, -100f),
                autoAdvance = false,
                stepType = StepType.UpRight,
                catPose = CatPose.CatRight,
                catPosition = new Vector2(706.9f, 179),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    bandejaImage.gameObject,
                },
            });
            // Paso 17
            steps.Add(new TutorialStep
            {
                message = "Presiona \"Entregar\" para entregarle el pedido al cliente.",
                position = new Vector2(195f, -390f),
                autoAdvance = false,
                stepType = StepType.UpLeft,
                catPose = CatPose.CatDown,
                catPosition = new Vector2(-291, -288),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.submitOrderButton.gameObject,
                },
            });
            // Paso 18
            steps.Add(new TutorialStep
            {
                message = "El cliente expondrá su valoración y pagará en función de la puntuación que hayas obtenido al preparar su comanda.",
                position = new Vector2(-541f, 140f),
                autoAdvance = true,
                stepType = StepType.UpLeft,
                catPose = CatPose.CatUp,
                catPosition = new Vector2(-842, 357)
            });
            // Paso 19
            steps.Add(new TutorialStep
            {
                message = "¡Si la puntuación es alta te dará una propina!",
                position = new Vector2(489f, 7f),
                autoAdvance = true,
                stepType = StepType.DownRight,
                catPose = CatPose.CatRight,
                catPosition = new Vector2(707, -182)
            });
            // Paso 20
            steps.Add(new TutorialStep
            {
                message = "Haz clic en \"Finalizar\" para volver a la cafetería.",
                position = new Vector2(508f, -149f),
                autoAdvance = false,
                stepType = StepType.UpRight,
                catPose = CatPose.CatRight,
                catPosition = new Vector2(707, 142),
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.endDeliveryButton.gameObject,
                },
            });
            // Paso 21
            steps.Add(new TutorialStep
            {
                message = "¡Ya has atendido a tu primer cliente! Sigue atendiendo más para poder pagar las facturas al final del día. ¡La cafetería cierra a las 20:00pm!",
                position = new Vector2(0f, 0f),
                autoAdvance = true,
                stepType = StepType.DownRight,
                catPose = CatPose.CatCenter,
                catPosition = new Vector2(481, -139)
            });
            // Paso 22
            steps.Add(new TutorialStep
            {
                message = "Al cerrar, siempre ganarás 50 monedas de café, que podrás gastar en los sobres de la tienda.",
                position = new Vector2(-520f, 262f),
                autoAdvance = true,
                stepType = StepType.DownLeft,
                catPose = CatPose.CatLeft,
                catPosition = new Vector2(-707, -62),
                highlightObjects = new List<GameObject>
                {
                    buttonManager.shopButton.gameObject,
                },
            });
            // Paso 23
            steps.Add(new TutorialStep
            {
                message = "¡Si compras sobres de cartas, podrás tener los gatos que desbloquees acompañándote en la cafetería!",
                position = new Vector2(628f, 2f),
                autoAdvance = true,
                stepType = StepType.UpRight,
                catPose = CatPose.CatRight,
                catPosition = new Vector2(709, 311),
            });
            // Paso 24
            steps.Add(new TutorialStep
            {
                message = "Como recompensa, se te ingresarán 220$ para ayudarte a pasar el primer día y 20 monedas de café. ¡Disfruta de Catpuccino!",
                position = new Vector2(0f, 0f),
                autoAdvance = true,
                stepType = StepType.UpRight,
                catPose = CatPose.CatCenter,
                catPosition = new Vector2(353, 225)
            });
        }
    }

    public void StartTutorial1()
    {
        currentStep = 0;
        skipTutorialButton.SetActive(true);
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

        ClearActualStep();

        GameObject prefabToSpawn = GetPrefab(step.stepType);
        if (prefabToSpawn != null && tutorialContainer != null)
        {
            currentBubbleObject = Instantiate(prefabToSpawn, tutorialContainer);

            currentPanelRect = currentBubbleObject.GetComponent<RectTransform>();
            currentCanvasGroup = currentBubbleObject.GetComponent<CanvasGroup>();
            currentTextTMP = currentBubbleObject.GetComponentInChildren<TextMeshProUGUI>();

            // Configuracion inicial
            currentPanelRect.localScale = Vector3.one;
            currentPanelRect.localRotation = Quaternion.identity;
            currentPanelRect.anchoredPosition = step.position; // Se pone la posicion indicada

            if (currentCanvasGroup == null) currentCanvasGroup = currentBubbleObject.AddComponent<CanvasGroup>();
            if (currentTextTMP != null) currentTextTMP.text = step.message;
        }

        UpdateCatVisuals(step);

        if (clickHintObject != null)
        {
            if (currentStep == 0)
            {
                clickHintObject.SetActive(true);
                StartCoroutine(BounceSpecificObject(clickHintObject.GetComponent<RectTransform>()));
            }
            else
            {
                clickHintObject.SetActive(false);
            }
        }

        step.onStepStart?.Invoke();

        // Efecto fade + rebote al mostrar el paso del turorial
        yield return StartCoroutine(FadeInPanel());
        StartCoroutine(BouncePanelLoop());

        // Efecto glow para marcar el objeto con el que interactuar
        if (step.highlightObjects != null && step.glowMaterial != null)
        {
            step.originalMaterials.Clear();

            foreach (var obj in step.highlightObjects)
            {
                if (obj == null) continue;
                var img = obj.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                {
                    step.originalMaterials.Add(img.material);
                    img.material = step.glowMaterial;
                }
            }
        }

        // Los pasos sin interaccion vinculada podran saltarse mediante clic izquierdo
        if (step.autoAdvance)
        {
            while (!Input.GetMouseButtonDown(0))
                yield return null;
            CompleteCurrentStep();
        }
    }

    // Funcion para el efecto bounce del texto
    private IEnumerator BounceSpecificObject(RectTransform targetRect)
    {
        if (targetRect == null) yield break;

        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = Vector3.one * bounceScale;

        while (targetRect != null && targetRect.gameObject.activeInHierarchy)
        {
            float elapsed = 0f;
            // Escalar hacia arriba
            while (elapsed < bounceSpeed)
            {
                if (targetRect == null || !targetRect.gameObject.activeInHierarchy) yield break;
                elapsed += Time.deltaTime;
                float t = Mathf.Sin((elapsed / bounceSpeed) * Mathf.PI * 0.5f);
                targetRect.localScale = Vector3.Lerp(originalScale, targetScale, t);
                yield return null;
            }

            elapsed = 0f;
            // Escalar hacia abajo
            while (elapsed < bounceSpeed)
            {
                if (targetRect == null || !targetRect.gameObject.activeInHierarchy) yield break;
                elapsed += Time.deltaTime;
                float t = Mathf.Sin((elapsed / bounceSpeed) * Mathf.PI * 0.5f);
                targetRect.localScale = Vector3.Lerp(targetScale, originalScale, t);
                yield return null;
            }
        }
    }

    public void CompleteCurrentStep()
    {
        // Eliminar glow del paso actual
        var step = steps[currentStep];

        if (step.highlightObjects != null && step.originalMaterials != null)
        {
            for (int i = 0; i < step.highlightObjects.Count; i++)
            {
                var obj = step.highlightObjects[i];
                if (obj == null) continue;

                var img = obj.GetComponent<UnityEngine.UI.Image>();
                if (img != null && i < step.originalMaterials.Count)
                    img.material = step.originalMaterials[i];
            }
        }

        StopAllCoroutines();
        StartCoroutine(FadeOutPanel(() =>
        {
            ClearCurrentCat();
            step.onStepComplete?.Invoke();

            currentStep++;
            if (currentStep < steps.Count) StartCoroutine(ShowStepT1());
            else EndTutorialT1();
        }));
    }

    public void EndTutorialT1()
    {
        ClearCurrentCat();
        isRunningT1 = false;
        tutorialContainer.gameObject.SetActive(false);
        skipTutorialButton.SetActive(false);
        GameManager.Instance.AnadirMonedas(220);
        PlayerDataManager.instance.AddBasicCoins(20);
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
                position = new Vector2(406f, 203f),
                autoAdvance = true,
                stepType = StepType.UpRight,
                //stepImage = ,
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.milkButton.gameObject,
                },
            });
            // Paso 1
            steps.Add(new TutorialStep
            {
                message = "¡Ahora puedes visitar la zona de pastelería! Haz clic sobre el bótón para ir.",
                position = new Vector2(117f, 232f),
                autoAdvance = false,
                stepType = StepType.UpRight,
                //stepImage = ,
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.bakeryButton.gameObject,
                },
            });
            // Paso 2
            steps.Add(new TutorialStep
            {
                message = "Comienza poniendo un plato o una bolsa para llevar en la bandeja según el tipo de pedido.",
                position = new Vector2(173f, -370f),
                autoAdvance = false,
                stepType = StepType.DownRight,
                //stepImage = ,
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    platoBakeryImage.gameObject,
                    bolsaBakeryImage.gameObject
                },
            });
            // Paso 3
            steps.Add(new TutorialStep
            {
                message = "Ahora, selecciona el tipo de bizcocho correspondiente. Si no sabes identificarlo, comprueba los nombres en el libro de recetas.",
                position = new Vector2(-587f, 181f),
                autoAdvance = false,
                stepType = StepType.UpRight,
                //stepImage = ,
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    BChocolateImage.gameObject,
                    BZanahoriaImage.gameObject,
                    BMantequillaImage.gameObject,
                    BRedVelvetImage.gameObject,
                    buttonManager.recipesBookBButton.gameObject,
                },
            });
            // Paso 4
            steps.Add(new TutorialStep
            {
                message = "Mediante clic, coloca el bizcocho en el horno.",
                position = new Vector2(130f, -160f),
                autoAdvance = false,
                stepType = StepType.UpLeft,
                //stepImage = ,
            });
            // Paso 5
            steps.Add(new TutorialStep
            {
                message = "¡Pulsa el botón para hornearlo!",
                position = new Vector2(183f, 199f),
                autoAdvance = false,
                stepType = StepType.UpRight,
                //stepImage = ,
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.hornearButton.gameObject,
                },
            });
            // Paso 6
            steps.Add(new TutorialStep
            {
                message = "Vigila el tiempo y clica el botón inferior. ¡Si lo paras antes quedará crudo! ¡Si te pasas se quemará!",
                position = new Vector2(183f, 199f),
                autoAdvance = true,
                stepType = StepType.UpRight,
                //stepImage = ,
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    hornearSliderImage.gameObject,
                    buttonManager.stopHorneadoButton.gameObject,
                },
            });
            // Paso 7
            steps.Add(new TutorialStep
            {
                message = "Cuando finalice el horneado mueve el bizcocho al recipiente mediante clic.",
                position = new Vector2(87f, -224f),
                autoAdvance = false,
                stepType = StepType.DownLeft,
                //stepImage = ,
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    bandejaBImage.gameObject,
                },
            });
            // Paso 8
            steps.Add(new TutorialStep
            {
                message = "Si te equivocas preparando la comida, puedes comenzar de 0 clicando sobre la basura.",
                position = new Vector2(-5f, -380f),
                autoAdvance = true,
                stepType = StepType.DownLeft,
                //stepImage = ,
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.papeleraRButton.gameObject,
                },
            });
            // Paso 9
            steps.Add(new TutorialStep
            {
                message = "¡Ya has finalizado la preparación del dulce! Vuelve a la zona de los cafés para continuar la comanda.",
                position = new Vector2(113f, 289f),
                autoAdvance = false,
                stepType = StepType.UpRight,
                //stepImage = ,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.returnBakeryButton);
                },
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.returnBakeryButton.gameObject,
                },
            });
        }
    }

    public void StartTutorial2()
    {
        currentStep = 0;
        skipTutorialButton.SetActive(true);
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        StartCoroutine(ShowStepT2());
    }

    public void CompleteCurrentStep2()
    {
        // Eliminar glow del paso actual
        var step = steps[currentStep];

        if (step.highlightObjects != null && step.originalMaterials != null)
        {
            for (int i = 0; i < step.highlightObjects.Count; i++)
            {
                var obj = step.highlightObjects[i];
                if (obj == null) continue;

                var img = obj.GetComponent<UnityEngine.UI.Image>();
                if (img != null && i < step.originalMaterials.Count)
                    img.material = step.originalMaterials[i];
            }
        }

        StopAllCoroutines();
        StartCoroutine(FadeOutPanel(() =>
        {
            ClearCurrentCat();
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
        ClearActualStep();

        GameObject prefabToSpawn = GetPrefab(step.stepType);
        if (prefabToSpawn != null && tutorialContainer != null)
        {
            currentBubbleObject = Instantiate(prefabToSpawn, tutorialContainer);

            currentPanelRect = currentBubbleObject.GetComponent<RectTransform>();
            currentCanvasGroup = currentBubbleObject.GetComponent<CanvasGroup>();
            currentTextTMP = currentBubbleObject.GetComponentInChildren<TextMeshProUGUI>();

            // Configuracion inicial
            currentPanelRect.localScale = Vector3.one;
            currentPanelRect.localRotation = Quaternion.identity;
            currentPanelRect.anchoredPosition = step.position; // Se pone la posicion indicada

            if (currentCanvasGroup == null) currentCanvasGroup = currentBubbleObject.AddComponent<CanvasGroup>();
            if (currentTextTMP != null) currentTextTMP.text = step.message;
        }

        UpdateCatVisuals(step);
        step.onStepStart?.Invoke();

        // Efecto fade + rebote al mostrar el paso del turorial
        yield return StartCoroutine(FadeInPanel());
        StartCoroutine(BouncePanelLoop());

        // Efecto glow para marcar el objeto con el que interactuar
        if (step.highlightObjects != null && step.glowMaterial != null)
        {
            step.originalMaterials.Clear();

            foreach (var obj in step.highlightObjects)
            {
                if (obj == null) continue;

                var img = obj.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                {
                    step.originalMaterials.Add(img.material);
                    img.material = step.glowMaterial;
                }
            }
        }

        // Los pasos sin interaccion vinculada podran saltarse mediante clic izquierdo
        if (step.autoAdvance)
        {
            while (!Input.GetMouseButtonDown(0))
                yield return null;
            CompleteCurrentStep2();
        }
    }

    public void EndTutorialT2()
    {
        ClearCurrentCat();
        isRunningT2 = false;
        tutorialContainer.gameObject.SetActive(false);
        skipTutorialButton.SetActive(false);
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
                position = new Vector2(610f, 135f),
                autoAdvance = true,
                stepType = StepType.UpLeft,
                //stepImage = ,
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.cogerTazaLecheButton.gameObject,
                },
            });
            // Paso 1
            steps.Add(new TutorialStep
            {
                message = "Clica sobre la taza de leche y colócala en el espumador.",
                position = new Vector2(151f, -289f),
                autoAdvance = false,
                stepType = StepType.DownLeft,
                //stepImage = ,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.cogerTazaLecheButton);
                },
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    espumadorImage.gameObject,
                    buttonManager.cogerTazaLecheButton.gameObject,
                },
            });
            // Paso 2
            steps.Add(new TutorialStep
            {
                message = "Ahora, mantén presionada la rueda para calentar hasta el punto indicado.",
                position = new Vector2(-184f, 235f),
                autoAdvance = false,
                stepType = StepType.UpRight,
                //stepImage = ,
                onStepStart = () =>
                {
                    buttonManager.EnableButton(buttonManager.calentarButton);
                },
                glowMaterial = glowMaterial,
                highlightObjects = new List<GameObject>
                {
                    buttonManager.calentarButton.gameObject,
                },
            });
            // Paso 3
            steps.Add(new TutorialStep
            {
                message = "¡Ten cuidado de no pasarte calentando la leche ni de dejarla fría!",
                position = new Vector2(-184f, 235f),
                autoAdvance = true,
                stepType = StepType.UpRight,
                //stepImage = ,
            });
            // Paso 4
            steps.Add(new TutorialStep
            {
                message = "¡Ya has aprendido cómo preparar la leche caliente para tus cafés!",
                position = new Vector2(-149f, 181f),
                autoAdvance = true,
                stepType = StepType.UpLeft,
                //stepImage = ,
            });
        }
    }

    public void StartTutorial3()
    {
        currentStep = 0;
        skipTutorialButton.SetActive(true);
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        StartCoroutine(ShowStepT3());
    }

    public void CompleteCurrentStep3()
    {
        // Eliminar glow del paso actual
        var step = steps[currentStep];

        if (step.highlightObjects != null && step.originalMaterials != null)
        {
            for (int i = 0; i < step.highlightObjects.Count; i++)
            {
                var obj = step.highlightObjects[i];
                if (obj == null) continue;

                var img = obj.GetComponent<UnityEngine.UI.Image>();
                if (img != null && i < step.originalMaterials.Count)
                    img.material = step.originalMaterials[i];
            }
        }

        StopAllCoroutines();
        StartCoroutine(FadeOutPanel(() =>
        {
            ClearCurrentCat();
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
        ClearActualStep();

        GameObject prefabToSpawn = GetPrefab(step.stepType);
        if (prefabToSpawn != null && tutorialContainer != null)
        {
            currentBubbleObject = Instantiate(prefabToSpawn, tutorialContainer);

            currentPanelRect = currentBubbleObject.GetComponent<RectTransform>();
            currentCanvasGroup = currentBubbleObject.GetComponent<CanvasGroup>();
            currentTextTMP = currentBubbleObject.GetComponentInChildren<TextMeshProUGUI>();

            // Configuracion inicial
            currentPanelRect.localScale = Vector3.one;
            currentPanelRect.localRotation = Quaternion.identity;
            currentPanelRect.anchoredPosition = step.position; // Se pone la posicion indicada

            if (currentCanvasGroup == null) currentCanvasGroup = currentBubbleObject.AddComponent<CanvasGroup>();
            if (currentTextTMP != null) currentTextTMP.text = step.message;
        }

        UpdateCatVisuals(step);
        step.onStepStart?.Invoke();

        // Efecto fade + rebote al mostrar el paso del turorial
        yield return StartCoroutine(FadeInPanel());
        StartCoroutine(BouncePanelLoop());

        // Efecto glow para marcar el objeto con el que interactuar
        if (step.highlightObjects != null && step.glowMaterial != null)
        {
            step.originalMaterials.Clear();

            foreach (var obj in step.highlightObjects)
            {
                if (obj == null) continue;

                var img = obj.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                {
                    step.originalMaterials.Add(img.material);
                    img.material = step.glowMaterial;
                }
            }
        }

        // Los pasos sin interaccion vinculada podran saltarse mediante clic izquierdo
        if (step.autoAdvance)
        {
            while (!Input.GetMouseButtonDown(0))
                yield return null;
            CompleteCurrentStep3();
        }
    }

    public void EndTutorialT3()
    {
        ClearCurrentCat();
        isRunningT3 = false;
        tutorialContainer.gameObject.SetActive(false);
        skipTutorialButton.SetActive(false);
        Debug.Log("Tutorial 3 completado");
    }
    #endregion

    // Funcion para saltar el tutorial
    public void SkipTutorial()
    {
        StopAllCoroutines();
        ClearAllGlow();
        ClearActualStep();
        ClearCurrentCat();

        if (skipTutorialButton != null)
            skipTutorialButton.SetActive(false);

        if (isRunningT1)
        {
            GameManager.Instance.AnadirMonedas(220);
            PlayerDataManager.instance.AddBasicCoins(20);
        }

        isRunningT1 = false;
        isRunningT2 = false;
        isRunningT3 = false;

        Debug.Log("Tutorial saltado manualmente");
    }

    // Funcion para resetear el bocadillo del paso actual
    private void ClearActualStep()
    {
        if (currentBubbleObject != null)
        {
            Destroy(currentBubbleObject);
            currentBubbleObject = null;
        }
    }

    // Funcion encargada de eliminar el efecto glow si se salta el tutorial
    public void ClearAllGlow()
    {
        // Recorre todos los pasos
        foreach (var step in steps)
        {
            if (step.highlightObjects == null || step.originalMaterials == null)
                continue;

            // Para cada uno, reestablece el material predeterminado
            for (int i = 0; i < step.highlightObjects.Count; i++)
            {
                GameObject obj = step.highlightObjects[i];
                if (obj == null) continue;

                var img = obj.GetComponent<UnityEngine.UI.Image>();
                if (img != null && step.originalMaterials.Count > i && step.originalMaterials[i] != null)
                {
                    img.material = step.originalMaterials[i];
                }
            }
        }
    }

    // Funciones para la animacion y el fade in/out de los mensajes 
    private IEnumerator FadeInPanel()
    {
        if (currentCanvasGroup == null) yield break;

        currentCanvasGroup.alpha = 0f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            if (currentCanvasGroup != null)
                currentCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
    }

    private IEnumerator FadeOutPanel(System.Action onFinish)
    {
        if (currentCanvasGroup != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                if (currentCanvasGroup != null)
                    currentCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
        }
        onFinish?.Invoke();
    }

    // Animacion de rebote del mensaje
    private IEnumerator BouncePanelLoop()
    {
        if (currentPanelRect == null) yield break;
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = Vector3.one * bounceScale;

        while (currentBubbleObject != null) // Mientras el objeto exista
        {
            float elapsed = 0f;
            while (elapsed < bounceSpeed)
            {
                if (currentPanelRect == null) yield break;
                elapsed += Time.deltaTime;
                float t = Mathf.Sin((elapsed / bounceSpeed) * Mathf.PI * 0.5f);
                currentPanelRect.localScale = Vector3.Lerp(originalScale, targetScale, t);
                yield return null;
            }
            elapsed = 0f;
            while (elapsed < bounceSpeed)
            {
                if (currentPanelRect == null) yield break;
                elapsed += Time.deltaTime;
                float t = Mathf.Sin((elapsed / bounceSpeed) * Mathf.PI * 0.5f);
                currentPanelRect.localScale = Vector3.Lerp(targetScale, originalScale, t);
                yield return null;
            }
        }
    }
}
