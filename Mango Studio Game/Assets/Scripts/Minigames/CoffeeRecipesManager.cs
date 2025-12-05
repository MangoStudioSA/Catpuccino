using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Clase utilizada para mostrar las recetas en el recetario 
public class CoffeeRecipesManager : MonoBehaviour
{

    [Header("Referencias")]
    public List<GameObject> pages;            // Páginas del libro
    public Animator bookAnimator;             // Animador del libro
    public GameObject bookPagesContainer;     // Contenedor de páginas

    [Header("Coffee Unlocker")]
    public CoffeeUnlockerManager coffeeUnlocker;
    public FoodUnlockerManager foodUnlocker;

    private int _targetPage = 0;
    private int currentPage = 0;
    private bool isTurningPage = false;

    void Awake()
    {
        if (bookAnimator != null)
            bookAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void OnEnable()
    {
        StartCoroutine(OpenBookRoutine());
    }

    void OnDisable()
    {
        _targetPage = 0;
    }

    public void OpenBookAtPage(int pageIndex)
    {
        _targetPage = pageIndex; // Guardamos que queremos ir a la 7 (o la que sea)
        gameObject.SetActive(true); // Activamos el libro, lo que dispara OnEnable
    }

    private IEnumerator OpenBookRoutine()
    {
        // Oculta las páginas mientras está la animación
        bookPagesContainer.SetActive(false);

        // Animación de abrir libro
        if (bookAnimator != null)
            bookAnimator.SetTrigger("OpenBook");

        yield return new WaitForSecondsRealtime(1.4f);

        // Muestra la primera página
        ShowPage(_targetPage);
        bookPagesContainer.SetActive(true);
    }

    public void ShowPage(int pageIndex)
    {
        currentPage = Mathf.Clamp(pageIndex, 0, pages.Count - 1);

        // Activa solo la página actual
        for (int i = 0; i < pages.Count; i++)
            pages[i].SetActive(i == currentPage);

        UpdateRecipesOnPage();
    }

    public void NextPage()
    {
        if (isTurningPage || currentPage >= pages.Count - 1) return;
        StartCoroutine(PageTurnRoutine(currentPage + 1, "NextPage"));
    }

    public void PreviousPage()
    {
        if (isTurningPage || currentPage <= 0) return;
        StartCoroutine(PageTurnRoutine(currentPage - 1, "PreviousPage"));
    }

    private IEnumerator PageTurnRoutine(int newPageIndex, string trigger)
    {
        isTurningPage = true;

        // Oculta las páginas
        bookPagesContainer.SetActive(false);

        // Reproduce animación de pasar página
        if (bookAnimator != null)
            bookAnimator.SetTrigger(trigger);

        yield return new WaitForSecondsRealtime(1.1f);

        // Muestra la nueva página
        ShowPage(newPageIndex);
        bookPagesContainer.SetActive(true);

        isTurningPage = false;
    }

    private void UpdateRecipesOnPage()
    {
        int currentDay = TimeManager.Instance.currentDay;
        CoffeeType[] unlockedCoffees = coffeeUnlocker.GetAvailableCoffees(currentDay); // Todas las desbloqueadas hasta el día actual
        FoodCategory[] unlockedFoods = foodUnlocker.GetAvailableFood(currentDay);

        GameObject current = pages[currentPage];
        RecipeSlot[] slots = current.GetComponentsInChildren<RecipeSlot>();

        foreach (var slotData in slots)
        {
            Image img = slotData.GetComponent<Image>();
            if (img == null) continue;

            bool unlocked = false;

            if (slotData.category == RecipeCategory.Coffee)
            {
                unlocked = System.Array.Exists(unlockedCoffees, ct => ct == slotData.coffeeType);
            }
            else if (slotData.category == RecipeCategory.Food)
            {
                unlocked = System.Array.Exists(unlockedFoods, ft => ft == slotData.foodType);
            }

            img.sprite = slotData.recipeSprite;

            if (unlocked)
                img.color = Color.white;             // visible
            else
                img.color = new Color(1, 1, 1, 0f);  // bloqueado
        }
    }
}

