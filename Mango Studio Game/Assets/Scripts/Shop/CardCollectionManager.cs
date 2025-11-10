using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Gestionar el album de cartas
public class CardCollectionManager : MonoBehaviour
{
    [Header("Referencias")]
    public List<GameObject> pages;
    public Animator bookAnimator;
    public GameObject bookPagesContainer;

    private int currentPage = 0;
    private HashSet<string> unlockedCards;
    private bool isTurningPage = false;

    void Awake()
    {
        if (bookAnimator != null)
            bookAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void OnEnable()
    {
        //UpdateCardsOnPage();
        StartCoroutine(OpenBookAndShowFirstPage());
    }
    private IEnumerator OpenBookAndShowFirstPage()
    {
        // Oculta las páginas mientras el libro se abre
        bookPagesContainer.SetActive(false);

        // Forzar animación manualmente
        if (bookAnimator != null)
            bookAnimator.SetTrigger("OpenBook");

        // Espera la duración de la animación (ajusta según tu clip)
        yield return new WaitForSecondsRealtime(1.57f);

        // Carga datos del jugador y muestra la primera página
        unlockedCards = PlayerDataManager.instance?.GetUnlockedCards() ?? new HashSet<string>();
        ShowPage(0);

        // Muestra las páginas ahora que el libro está abierto
        bookPagesContainer.SetActive(true);
    }    

    public void ShowPage(int pageIndex)
    {
        for (int i = 0; i < pages.Count; i++)
            pages[i].SetActive(i == pageIndex);

        // Muestra solo la seleccionada
        currentPage = Mathf.Clamp(pageIndex, 0, pages.Count - 1);
        pages[currentPage].SetActive(true);

        UpdateCardsOnPage();
    }
    public void NextPage()
    {
        if (isTurningPage || currentPage >= pages.Count - 1)
            return;

        StartCoroutine(PageTurnRoutine(currentPage + 1, "NextPage"));
    }

    public void PreviousPage()
    {
        if (isTurningPage || currentPage <= 0)
            return;

        StartCoroutine(PageTurnRoutine(currentPage - 1, "PreviousPage"));
    }

    private IEnumerator PageTurnRoutine(int newPageIndex, string trigger)
    {
        isTurningPage = true;

        // Oculta temporalmente las páginas
        bookPagesContainer.SetActive(false);

        // Reproduce la animación
        if (bookAnimator != null)
            bookAnimator.SetTrigger(trigger);

        // Espera la duración de la animación (ajusta según el clip)
        yield return new WaitForSecondsRealtime(0.7f);

        // Muestra la nueva página
        ShowPage(newPageIndex);

        // Vuelve a activar las páginas
        bookPagesContainer.SetActive(true);

        isTurningPage = false;
    }


    public void OnPageTurnFinished()
    {
        ShowPage(currentPage + 1); 
    }

    private void UpdateCardsOnPage()
    {
        var current = pages[currentPage];

        foreach (var card in current.GetComponentsInChildren<Image>())
        {
            if (card.CompareTag("Card"))
            {
                bool unlocked = unlockedCards.Contains(card.name);

                if (unlocked)
                {
                    // Cargar sprite de carta real
                    Sprite unlockedSprite = Resources.Load<Sprite>($"cards/{card.name}");
                    if (unlockedSprite != null)
                        card.sprite = unlockedSprite;

                    card.color = Color.white;
                }
                else
                {
                    // Mostrar gris o silueta vacía
                    card.sprite = null;
                    card.color = new Color(1, 1, 1, 0);
                }
            }
        }
    }

}
