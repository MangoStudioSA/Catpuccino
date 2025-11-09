using TMPro;
using UnityEngine;

// Mostrar mensaje cuando el jugador ha interactuado con el cafe echando algun ingrediente
public class PopUpMechanicsMsg : MonoBehaviour
{
    public static PopUpMechanicsMsg Instance;

    [Header("UI Referencias")]
    public GameObject popUpMsgPrefab;
    public Transform canvasParent;
    public float fadeTime = 1.5f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowMessage(string msg, Vector3? localOffset = null, float customFadeTime = -1f)
    {
        GameObject popUp = Instantiate(popUpMsgPrefab, canvasParent);
        TMP_Text text = popUp.GetComponentInChildren<TMP_Text>();
        text.text = msg;

        RectTransform rect = popUp.GetComponent<RectTransform>();
        if (localOffset != null)
            rect.localPosition = (Vector3)localOffset;
        else
            rect.localPosition = Vector3.zero;

        CanvasGroup cg = popUp.AddComponent<CanvasGroup>();
        float duration = (customFadeTime > 0f) ? customFadeTime : fadeTime;
        StartCoroutine(FadeAndDestroy(popUp, cg, duration));
    }

    private System.Collections.IEnumerator FadeAndDestroy(GameObject popUp, CanvasGroup cg, float duration)
    {
        yield return new WaitForSeconds(0.5f);
        float t = 0;
        RectTransform rect = popUp.GetComponent<RectTransform>();
        Vector3 startPos = rect.localPosition;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = 1 - (t / duration);
            rect.localPosition = startPos + new Vector3(0, 30f * (t / duration), 0);
            yield return null;
        }
        Destroy(popUp); 
    }
}
