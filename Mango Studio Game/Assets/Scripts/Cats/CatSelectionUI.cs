using System.Collections.Generic;
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

    private List<CatUISlot> generatedSlots = new List<CatUISlot>();
    private List<int> selectedCats = new List<int>();
    private const int MAX_GATOS = 5;

    void Start()
    {
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
        selectedCats.Clear();

        // Se crean los elementos en el scroll
        for (int i = 0; i < cafeManager.listaDeGatos.Count; i++)
        {
            var info = cafeManager.listaDeGatos[i];
            bool unlocked = PlayerDataManager.instance.HasCard(info.nombreCarta);
            bool isActive = (info.gatoInstancia != null);

            if (isActive) selectedCats.Add(i);
             
            Sprite img = unlocked ? info.iconoGato : spriteLocked; // Se comprueba si usar la imagen del gato o la de no desbloqueado

            GameObject nuevoSlot = Instantiate(slotPrefab, contentPanel);
            CatUISlot scriptSlot = nuevoSlot.GetComponent<CatUISlot>();

            scriptSlot.Initialize(i, info.nombreCarta, img, unlocked, isActive, this);
            generatedSlots.Add(scriptSlot);
        }

        CheckLimits();
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
}
