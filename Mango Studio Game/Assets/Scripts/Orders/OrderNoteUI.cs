using System.Collections;
using TMPro;
using UnityEngine;

// Clase encargada de mostrar los requisitos del pedido actual en forma de nota 
public class OrderNoteUI : MonoBehaviour
{
    [SerializeField] private RectTransform notePanelOrder;
    [SerializeField] private RectTransform notePanelOrderB;
    [SerializeField] private TextMeshProUGUI noteTxt;
    [SerializeField] private TextMeshProUGUI noteTxtB;
    [SerializeField] private float slideDuration = 0.5f;
    
    public CustomerOrder npc;

    private Vector2 visiblePosA, hiddenPosA;
    private Vector2 visiblePosB, hiddenPosB;

    private Coroutine slideCoroutineA;
    private Coroutine slideCoroutineB;

    public bool isVisible = false;
    private Order currentOrder;
    public TutorialManager tutorialManager;

    private void Awake()
    {
        visiblePosA = notePanelOrder.anchoredPosition;
        hiddenPosA = new Vector2(visiblePosA.x, visiblePosA.y + notePanelOrder.rect.height);
        notePanelOrder.anchoredPosition = hiddenPosA;
        notePanelOrder.gameObject.SetActive(false);

        if (notePanelOrderB != null)
        {
            visiblePosB = notePanelOrderB.anchoredPosition;
            hiddenPosB = new Vector2(visiblePosB.x, visiblePosB.y + notePanelOrderB.rect.height);
            notePanelOrderB.anchoredPosition = hiddenPosB;
            notePanelOrderB.gameObject.SetActive(false);
        }

        isVisible = false;
    }
    // Accede al pedido actual
    public void SetCurrentOrder(Order order)
    {
        currentOrder = order;
        UpdateNoteText(order);
    }
    // Activa/desactiva el panel
    public void ToggleNote()
    {
        if (currentOrder == null) return; 

        isVisible = !isVisible;
        notePanelOrder.gameObject.SetActive(true);
        UpdateNoteText(currentOrder);

        if (slideCoroutineA != null) StopCoroutine(slideCoroutineA);
        slideCoroutineA = StartCoroutine(SlideNote(isVisible, notePanelOrder, visiblePosA, hiddenPosA));

        if (notePanelOrderB != null)
        {
            notePanelOrderB.gameObject.SetActive(true);
            if (slideCoroutineB != null) StopCoroutine(slideCoroutineB);
            slideCoroutineB = StartCoroutine(SlideNote(isVisible, notePanelOrderB, visiblePosB, hiddenPosB));
        }

        if (tutorialManager.isRunningT1 && tutorialManager.currentStep == 6)
            FindFirstObjectByType<TutorialManager>().CompleteCurrentStep();        
    }
    public void ResetNote()
    {
        notePanelOrder.gameObject.SetActive(false);
        if (notePanelOrderB != null)
            notePanelOrderB.gameObject.SetActive(false);
    }
    // Actualiza el texto segun el pedido actual
    private void UpdateNoteText(Order order)
    {
        if (order == null)
        {
            noteTxt.text = "";
            if (noteTxtB != null) noteTxtB.text = "";
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
        if (noteTxtB != null) noteTxtB.text = note;
    }
    // Animacion de deslizar la nota 
    private IEnumerator SlideNote(bool show, RectTransform panel, Vector2 visiblePos, Vector2 hiddenPos)
    {
        Vector2 start = panel.anchoredPosition;
        Vector2 end = show ? visiblePos : hiddenPos;

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration);
            panel.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        panel.anchoredPosition = end;

        if (!show)
            panel.gameObject.SetActive(false);
    }
}
