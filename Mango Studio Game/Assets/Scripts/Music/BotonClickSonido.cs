using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BotonClickSonido : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();

        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
    }
}