using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Clase encargada de gestionar la seleccion de gatos en el menu
public class CatSelectionUI : MonoBehaviour
{
    [Header("Referencias")]
    public CatCafeManager cafeManager;
    public GameObject slotPrefab;
    public Transform contentPanel;

    [Header("Sprites")]
    public Sprite spriteLocked;
    public TextMeshProUGUI contText;

    private List<CatUISlot> generatedSlots = new List<CatUISlot>();
    private List<int> selectedCats = new List<int>();
    private const int MAX_GATOS = 5;

    void OnEnable()
    {
        LoadSelection();
        GenerateMenu();
    }

    // Funcion para actualizar el menu
    public void RefreshMenu()
    {
        GenerateMenu();
    }

    // Funcion para generar el menu con los gatos
    void GenerateMenu()
    {
        // Se limpia la lista anterior
        foreach (Transform child in contentPanel) Destroy(child.gameObject);
        generatedSlots.Clear();

        // Se crean los elementos en el scroll
        for (int i = 0; i < cafeManager.listaDeGatos.Count; i++)
        {
            var info = cafeManager.listaDeGatos[i];
            bool unlocked = PlayerDataManager.instance.HasCard(info.nombreCarta);
            bool shouldBeActive = selectedCats.Contains(i);

            string name = unlocked ? info.nombreGato : "???????";

            Sprite img = unlocked ? info.iconoGato : spriteLocked; // Se comprueba si usar la imagen del gato o la de no desbloqueado

            GameObject nuevoSlot = Instantiate(slotPrefab, contentPanel);
            CatUISlot scriptSlot = nuevoSlot.GetComponent<CatUISlot>();

            scriptSlot.Initialize(i, name, img, unlocked, shouldBeActive, this);
            generatedSlots.Add(scriptSlot);
        }
        cafeManager.UpdateCafeCats(selectedCats);
        CheckLimits();
        UpdateContText();
    }

    // Funcion para actualizar el texto del contador de gatos
    void UpdateContText()
    {
        if (contText != null)
        {
            contText.text = $"{selectedCats.Count}/{MAX_GATOS}";
        }
    }

    // Funcion que gestiona el cambio del menu cuando el jugador marca/desmarca un gato
    public void OnGatoToggled(int index, bool isChecked)
    {
        if (isChecked) 
        {
            if (!selectedCats.Contains(index))
            {
                selectedCats.Add(index);
            }
        }
        else
        {
            if (selectedCats.Contains(index))
            {
                selectedCats.Remove(index);
            }
        }

        cafeManager.UpdateCafeCats(selectedCats);
        CheckLimits(); // Comprobar que no se pase el limite
        UpdateContText();
        SaveSelection();
    }

    // Funcion para deshabilitar los check boxes si hay el numero maximo de gatos en la cafeteria
    void CheckLimits()
    {
        bool limitTaken = selectedCats.Count >= MAX_GATOS;

        for (int i = 0; i < generatedSlots.Count; i++)
        {
            CatUISlot slot = generatedSlots[i];
            bool esDesbloqueado = PlayerDataManager.instance.HasCard(cafeManager.listaDeGatos[slot.indexGato].nombreCarta);

            if (esDesbloqueado)
            {
                if (limitTaken)
                {
                    slot.SetInteractable(slot.miToggle.isOn);
                }
                else
                {
                    slot.SetInteractable(true);
                }
            }
        }
    }

    // Funcion para guardar los gatos activos
    void SaveSelection()
    {
        PlayerPrefs.DeleteKey("NumGatosSeleccionados"); // Se borran los antiguos
        PlayerPrefs.SetInt("NumGatosSeleccionados", selectedCats.Count); // Se guardan los actuales

        // Se guardan uno a uno
        for (int i = 0; i < selectedCats.Count; i++)
        {
            PlayerPrefs.SetInt("GatoSeleccionado_" + i, selectedCats[i]);
        }

        PlayerPrefs.Save();
        Debug.Log("Guardado exitoso. Gatos guardados: " + selectedCats.Count);
    }

    // Funcion para cargar los gatos activos
    void LoadSelection()
    {
        selectedCats.Clear();

        // Comprueba si hay datos guardados
        if (PlayerPrefs.HasKey("NumGatosSeleccionados"))
        {
            int num = PlayerPrefs.GetInt("NumGatosSeleccionados");

            // Se recuperan
            for (int i = 0; i < num; i++)
            {
                int idGato = PlayerPrefs.GetInt("GatoSeleccionado_" + i);
                selectedCats.Add(idGato);
            }

            Debug.Log("Carga exitosa. Gatos recuperados: " + num);
        }
    }
}
