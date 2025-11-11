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
    public Animator clipsAnimator;
    public GameObject bookPagesContainer;
    public GameObject clipsContainer;

    private int currentPage = 0;
    private HashSet<string> unlockedCards;
    private bool isTurningPage = false;

    void Awake()
    {
        if (bookAnimator != null)
            bookAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

        clipsAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void OnEnable()
    {
        StartCoroutine(OpenBookAndShowFirstPage());
    }
    private IEnumerator OpenBookAndShowFirstPage()
    {
        // Oculta las paginas mientras esta la animacion
        bookPagesContainer.SetActive(false);
        CanvasGroup clipsCG = clipsAnimator.GetComponent<CanvasGroup>();
        if (clipsCG != null) clipsCG.alpha = 0;

        if (bookAnimator != null)
            bookAnimator.SetTrigger("OpenBook");

        yield return new WaitForSecondsRealtime(1.57f);

        // Carga las cartas desbloqueadas y las muestra
        unlockedCards = PlayerDataManager.instance?.GetUnlockedCards() ?? new HashSet<string>();
        ShowPage(0);
        bookPagesContainer.SetActive(true);
        if (clipsCG != null) clipsCG.alpha = 1;
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

        // Oculta las paginas
        bookPagesContainer.SetActive(false);
        CanvasGroup clipsCG = clipsAnimator.GetComponent<CanvasGroup>();
        if (clipsCG != null) clipsCG.alpha = 0;

        // Animacion
        if (bookAnimator != null)
            bookAnimator.SetTrigger(trigger);
        yield return new WaitForSecondsRealtime(0.7f);

        // Muestra la nueva pagina
        ShowPage(newPageIndex);
        bookPagesContainer.SetActive(true);
        if (clipsCG != null) clipsCG.alpha = 1;

        isTurningPage = false;
    }
    public void GoToTazasPage()
    {
        if (isTurningPage) return;
        StartCoroutine(PlayCategoryAnimation("MoveTazas", 3)); // Pagina envases
    }

    public void GoToGatosPage()
    {
        if (isTurningPage) return;
        StartCoroutine(PlayCategoryAnimation("MoveGatos", 0)); // 1 pagina gatos
    }

    private IEnumerator PlayCategoryAnimation(string trigger, int targetPage)
    {
        isTurningPage = true;

        // Oculta las paginas y el bookanimator mientras se reproduce la animacion
        CanvasGroup bookCG = bookAnimator.GetComponent<CanvasGroup>();
        if (bookCG != null) bookCG.alpha = 0;
        bookPagesContainer.SetActive(true);

        // Animacion
        if (clipsAnimator != null)
            clipsAnimator.SetTrigger(trigger);
        yield return new WaitForSecondsRealtime(1f);

        // Muestra la pagina correspondiente
        ShowPage(targetPage);
        bookPagesContainer.SetActive(true);
        if (bookCG != null) bookCG.alpha = 1;

        isTurningPage = false;
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
                    // Cargar sprite carta 
                    Sprite unlockedSprite = Resources.Load<Sprite>($"cards/{card.name}");
                    if (unlockedSprite != null)
                        card.sprite = unlockedSprite;

                    card.color = Color.white;
                }
                else
                {
                    // Mostrar sprite vacio si no esta desbloqueada
                    card.sprite = null;
                    card.color = new Color(1, 1, 1, 0);
                }
            }
        }
    }

}
