using System.Collections;
using UnityEngine;

// Clase encargada de gestionar la animacion al ir a la tienda
public class StoreAnimation : MonoBehaviour
{
    [Header("Referencias")]
    public Animator storeAnimator;
    public GameObject animationPanel;
    public string openTrigger = "OpenStore";
    public GameUIManager gameUIManager;

    private bool isTransitioning = false;

    // Funcion vinculada al boton para ir a la tienda
    public void GoToShop()
    {
        if (isTransitioning) return;

        MiniGameSoundManager.instance.PlayShop();
        if (storeAnimator != null)

            StartCoroutine(PlayTransition());
        else
            ActivateShopDirectly();
    }

    // Se muestra la animacion y despues se activa el panel de la tienda
    private IEnumerator PlayTransition()
    {
        isTransitioning = true;

        if (animationPanel != null)
            animationPanel.SetActive(true);

        if (storeAnimator != null)
            storeAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

        storeAnimator.SetTrigger(openTrigger);
        yield return new WaitForSecondsRealtime(1.6f);

        if (animationPanel != null)
            animationPanel.SetActive(false);

        gameUIManager.OpenShopMenu();
        isTransitioning = false;
    }

    // Si falla se activa directamente la tienda
    private void ActivateShopDirectly()
    {
        gameUIManager.OpenShopMenu();
    }
}
