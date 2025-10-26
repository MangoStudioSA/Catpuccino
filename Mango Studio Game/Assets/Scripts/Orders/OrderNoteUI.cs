using System.Collections;
using TMPro;
using UnityEngine;

public class OrderNoteUI : MonoBehaviour
{
    [SerializeField] private RectTransform notePanel;
    [SerializeField] private TextMeshProUGUI noteTxt;
    [SerializeField] private float slideDuration = 0.5f;
    
    public CustomerOrder npc;

    private Vector2 hiddenPos;
    private Vector2 visiblePos;

    private bool isVisible = false;
    private Order currentOrder;

    private void Awake()
    {
        visiblePos = notePanel.anchoredPosition;

        hiddenPos = new Vector2(visiblePos.x, visiblePos.y + notePanel.rect.height);
        notePanel.anchoredPosition = hiddenPos;
        notePanel.gameObject.SetActive(false);
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
        notePanel.gameObject.SetActive(true);
        UpdateNoteText(currentOrder);

        StopAllCoroutines();
        StartCoroutine(SlideNote(isVisible));    
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
        Vector2 start = notePanel.anchoredPosition;
        Vector2 end = show ? visiblePos : hiddenPos;

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            t = Mathf.SmoothStep(0f, 1f, t);
            notePanel.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        notePanel.anchoredPosition = end;

        if (!show)
            notePanel.gameObject.SetActive(false);
    }
}
