using System.Collections;
using TMPro;
using UnityEngine;

public class OrderNoteUI : MonoBehaviour
{
    [SerializeField] private RectTransform notePanelOrder;
    [SerializeField] private TextMeshProUGUI noteTxt;
    [SerializeField] private float slideDuration = 0.5f;
    
    public CustomerOrder npc;

    private Vector2 hiddenPos;
    private Vector2 visiblePos;

    private bool isVisible = false;
    private Order currentOrder;
    public TutorialManager tutorialManager;

    private void Awake()
    {
        visiblePos = notePanelOrder.anchoredPosition;

        hiddenPos = new Vector2(visiblePos.x, visiblePos.y + notePanelOrder.rect.height);
        notePanelOrder.anchoredPosition = hiddenPos;
        notePanelOrder.gameObject.SetActive(false);
        isVisible = false;
    }
    public void SetCurrentOrder(Order order)
    {
        currentOrder = order;
        UpdateNoteText(order);
    }

    public void ToggleNote()
    {
        if (currentOrder == null) return; 

        isVisible = !isVisible;
        notePanelOrder.gameObject.SetActive(true);
        UpdateNoteText(currentOrder);

        StopAllCoroutines();
        StartCoroutine(SlideNote(isVisible));

        if (tutorialManager.isRunning && tutorialManager.currentStep == 6)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();        
    }
    public void ResetNote()
    {
        notePanelOrder.gameObject.SetActive(false);
    }
    private void UpdateNoteText(Order order)
    {
        if (order == null)
        {
            noteTxt.text = "";
            return;
        }

        string note = "";

        note += $"- Café: {order.coffeeType}.\n";
        note += $"- Azúcar: {order.sugarAm}.\n";
        note += $"- Hielo: {(order.iceAm == IceAmount.no ? "sin hielo" : "con hielo")}.\n";
        note += $"- Tipo de pedido: para {order.orderType}.\n";

        if (order.foodOrder.category != FoodCategory.no)
        {
            note += $"- Comida: {currentOrder.foodOrder.GetSimpleFoodDescription()}";
        }
        else
        {
            note += "- Sin comida.";
        }

        noteTxt.text = note;
    }

    private IEnumerator SlideNote (bool show)
    {
        Vector2 start = notePanelOrder.anchoredPosition;
        Vector2 end = show ? visiblePos : hiddenPos;

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            t = Mathf.SmoothStep(0f, 1f, t);
            notePanelOrder.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        notePanelOrder.anchoredPosition = end;

        if (!show)
            notePanelOrder.gameObject.SetActive(false);
    }
}
