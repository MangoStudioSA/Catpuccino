using UnityEngine;

public class LinkOpenner : MonoBehaviour
{
    public void AbrirURL(string url)
    {
        Application.OpenURL(url);
        Debug.Log("Abriendo URL: " + url);
    }
}
