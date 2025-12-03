using System.Collections;
using UnityEngine;

public class BookRAnimation : MonoBehaviour
{
    public Animator animator;
    public float animationDuration = 2f; // Ajusta según tu clip
    public GameObject pagesContainer;

    public void PlayOpenAnimation()
    {
        if (animator != null)
            animator.SetTrigger("OpenBook");

        // Desactiva páginas durante la animación
        if (pagesContainer != null)
            pagesContainer.SetActive(false);

        StartCoroutine(ShowPagesAfterAnimation());
    }

    private IEnumerator ShowPagesAfterAnimation()
    {
        yield return new WaitForSecondsRealtime(animationDuration);

        if (pagesContainer != null)
            pagesContainer.SetActive(true);
    }
}
