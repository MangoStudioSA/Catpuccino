using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Clase encargada de generar los slots del menu de seleccion de gatos
public class CatUISlot : MonoBehaviour
{
    [Header("Referencias UI")]
    public Image imagenGato;
    public TextMeshProUGUI textoNombre;
    public Toggle miToggle;

    [HideInInspector] public int indexGato; // Indice en la lista original
    private CatSelectionUI uiManager;

    // Funcion que inicializa el prefab de cada gato
    public void Initialize(int index, string nombre, Sprite icono, bool desbloqueado, bool isActive, CatSelectionUI manager)
    {
        indexGato = index;
        textoNombre.text = nombre;
        imagenGato.sprite = icono;
        uiManager = manager;

        miToggle.onValueChanged.RemoveAllListeners();

        // Configuracion inicial
        if (!desbloqueado)
        {
            miToggle.interactable = false;
            miToggle.isOn = false;
            imagenGato.color = Color.white;
            textoNombre.text = "???";
        }
        else
        {
            miToggle.interactable = true;
            miToggle.isOn = isActive;
            imagenGato.color = Color.white;
            miToggle.onValueChanged.AddListener(ChangeValue);
        }
    }

    // Funcion que se llama automaticamente cuando el jugador marca/desmarca el checkbox
    void ChangeValue(bool isMarked)
    {
        uiManager.OnGatoToggled(indexGato, isMarked);
    }

    // Funcion para bloquear el checkbox si ya hay 5 seleccionados
    public void SetInteractable(bool state)
    {
        if (!miToggle.isOn)
        {
            miToggle.interactable = state;
        }
    }
}
