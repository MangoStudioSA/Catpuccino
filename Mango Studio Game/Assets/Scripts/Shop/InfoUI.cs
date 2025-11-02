using System.Collections;
using TMPro;
using UnityEngine;

public class InfoUI : MonoBehaviour
{
    [SerializeField] private RectTransform noteInfo;
    [SerializeField] private TextMeshProUGUI infoTxt;
    [SerializeField] private float slideDuration = 0.5f;
    
    private Vector2 hiddenPos;
    private Vector2 visiblePos;

    private bool isVisible = false;

    private void Awake()
    {
        visiblePos = noteInfo.anchoredPosition;

        hiddenPos = new Vector2(visiblePos.x, visiblePos.y - noteInfo.rect.height);
        noteInfo.anchoredPosition = hiddenPos;
        noteInfo.gameObject.SetActive(false);
    }

    public void ToggleNote()
    {
        isVisible = !isVisible;
        noteInfo.gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(SlideNote(isVisible));    
    }

    private IEnumerator SlideNote (bool show)
    {
        Vector2 start = noteInfo.anchoredPosition;
        Vector2 end = show ? visiblePos : hiddenPos;

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            t = Mathf.SmoothStep(0f, 1f, t);
            noteInfo.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        noteInfo.anchoredPosition = end;

        if (!show)
            noteInfo.gameObject.SetActive(false);
    }
}
