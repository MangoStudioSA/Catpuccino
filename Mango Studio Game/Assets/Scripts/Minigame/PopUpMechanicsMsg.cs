using System.Collections.Generic;
using System.Linq;
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

    private List<GameObject> activePopUps = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Mostrar el mensaje 
    public void ShowMessage(string msg, Vector3? localOffset = null, float customFadeTime = -1f)
    {
        GameObject popUp = Instantiate(popUpMsgPrefab, canvasParent);
        activePopUps.Add(popUp);
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

    // Animacion de aparecer/desaparecer
    private System.Collections.IEnumerator FadeAndDestroy(GameObject popUp, CanvasGroup cg, float duration)
    {
        yield return new WaitForSeconds(0.5f);
        float t = 0;
        RectTransform rect = popUp.GetComponent<RectTransform>();
        Vector3 startPos = rect.localPosition;

        while (t < duration)
        {
            t += Time.deltaTime;
            // Si el panel padre se desactiva o el objeto ha sido destruido
            if (popUp == null || canvasParent == null || !canvasParent.gameObject.activeInHierarchy)
            {
                if (popUp != null) Destroy(popUp);
                yield break;
            }

            if (cg != null)
                cg.alpha = 1 - (t / duration);

            if (rect != null)
                rect.localPosition = startPos + new Vector3(0, 30f * (t / duration), 0);

            yield return null;
        }
        Destroy(popUp); 
    }

    public void DestroyAllPopUps()
    {
        foreach (var popUp in activePopUps.ToList())
            if (popUp != null) Destroy(popUp);
        activePopUps.Clear();
    }
}
