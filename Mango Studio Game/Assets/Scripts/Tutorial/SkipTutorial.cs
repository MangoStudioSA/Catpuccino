using UnityEngine;

public class SkipTutorial : MonoBehaviour
{
    public TutorialManager tutorialManager;

    public void SkipTutorialOnClick()
    {
        if (tutorialManager != null)
            tutorialManager.SkipTutorial();
    }
}
