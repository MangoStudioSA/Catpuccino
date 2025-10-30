using System.Collections;
using UnityEngine;

public class GameButtonPos : MonoBehaviour
{
    [SerializeField] private GameObject acceptClientButton;
    [SerializeField] private Vector2 fixedPos = new Vector2(-3f, 228f);
    
    void Start()
    {
        if (acceptClientButton != null)
        {
            RectTransform rt = acceptClientButton.GetComponent<RectTransform>();
            rt.anchoredPosition = fixedPos;
            acceptClientButton.SetActive(false);

        }
    }
    public void ShowButton()
    {
        if (acceptClientButton != null)
            acceptClientButton.SetActive(true);
    }

    public void HideButton()
    {
        if (acceptClientButton != null)
            acceptClientButton.SetActive(false);
    }

    public IEnumerator ShowButtonAnimated()
    {
        acceptClientButton.SetActive(true);
        RectTransform rt = acceptClientButton.GetComponent<RectTransform>();
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 3f;
            rt.localScale = Vector3.LerpUnclamped(startScale, endScale, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        rt.localScale = endScale;
    }
}
