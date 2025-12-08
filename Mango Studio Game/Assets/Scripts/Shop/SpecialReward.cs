using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Clase que gestiona el logro de llegar al 7 dia
public class SpecialReward : MonoBehaviour
{
    public static SpecialReward instance;

    [Header("Referencias")]
    public GameObject rewardPanel;
    public Image rewardImage;
    public TextMeshProUGUI rewardText;
    public Button closeButton;
    public TimeManager timeManager;

    [Header("Configuracion animacion")]
    public float animationDuration = 0.8f;
    public float timeToShowButton = 5f;
    public float floatSpeed = 4f;
    public float floatAmp = 10f;

    [Header("Carta a guardar")]
    public Sprite cardToAdd;

    private Vector2 textOriginalPos;
    private Vector2 imageOriginalPos;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (rewardPanel != null) rewardPanel.SetActive(false);
        if (rewardText != null) textOriginalPos = rewardText.rectTransform.anchoredPosition;

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
            closeButton.gameObject.SetActive(false);
        }

        CheckDayCondition();
    }

    // Funcion para comprobar el dia actual y dar el logro
    public void CheckDayCondition()
    {
        if (PlayerDataManager.instance != null && PlayerDataManager.instance.IsDay7Claimed())
        {
            return;
        }

        if (timeManager != null && timeManager.currentDay == 7)
        {
            ShowDay7Reward();
        }
    }

    // Funcion para mostrar el panel del logro
    public void ShowDay7Reward()
    {
        if (rewardPanel != null)
        {
            if (PlayerDataManager.instance != null)
            {
                PlayerDataManager.instance.MarkDay7AsClaimed();
            }

            rewardPanel.SetActive(true);
            Time.timeScale = 0.0f;

            AddRewardsToSaveData();
            StartCoroutine(AnimateSequence());
        }
    }

    // Funcion para guardar la carta
    private void AddRewardsToSaveData()
    {
        if (PlayerDataManager.instance != null)
        {   
                PlayerDataManager.instance.AddCard(cardToAdd);
        }
    }

    // Animacion de la carta y el texto
    private IEnumerator AnimateSequence()
    {
        rewardImage.transform.localScale = Vector3.zero;
        rewardText.transform.localScale = Vector3.zero;
        rewardText.gameObject.SetActive(false);

        if (closeButton != null)
        {
            closeButton.transform.localScale = Vector3.zero;
            closeButton.gameObject.SetActive(false);
        }

        // Bounce imagen
        float timer = 0f;
        while (timer < animationDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / animationDuration;

            float scale = EaseOutElastic(t);
            rewardImage.transform.localScale = Vector3.one * scale;

            yield return null;
        }
        rewardImage.transform.localScale = Vector3.one; // Asegurar escala final

        // Bounce texto
        rewardText.gameObject.SetActive(true);
        timer = 0f;
        while (timer < animationDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / animationDuration;

            float scale = EaseOutElastic(t);
            rewardText.transform.localScale = Vector3.one * scale;

            yield return null;
        }
        rewardText.transform.localScale = Vector3.one;

        // Rebote texto
        StartCoroutine(IdleFloatAnimation());

        yield return new WaitForSecondsRealtime(timeToShowButton);

        // Boton
        if (closeButton != null)
        {
            closeButton.gameObject.SetActive(true);
            timer = 0f;
            while (timer < 0.5f)
            {
                timer += Time.unscaledDeltaTime;
                float t = timer / 0.5f;
                float scale = EaseOutElastic(t);
                closeButton.transform.localScale = Vector3.one * scale;
                yield return null;
            }
            closeButton.transform.localScale = Vector3.one;
        }
    }

    // Animacion para el texto de flotar
    private IEnumerator IdleFloatAnimation()
    {
        float timer = 0f;
        while (rewardPanel.activeSelf)
        {
            timer += Time.unscaledDeltaTime * floatSpeed;
            float yOffset = Mathf.Sin(timer) * floatAmp;

            if (rewardText != null)
                rewardText.rectTransform.anchoredPosition = textOriginalPos + new Vector2(0, yOffset);

            if (rewardImage != null)
                rewardImage.rectTransform.anchoredPosition = imageOriginalPos + new Vector2(0, yOffset);

            yield return null;
        }
    }

    // Funcion para cerrar el panel
    public void ClosePanel()
    {
        rewardPanel.SetActive(false);
        StopAllCoroutines();
        Time.timeScale = 1.0f;
    }

    // Funcion para calcular el efecto de la animacion
    private float EaseOutElastic(float x)
    {
        float c4 = (2 * Mathf.PI) / 3;
        return x == 0 ? 0 : x == 1 ? 1 : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1;
    }
}
