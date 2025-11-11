using UnityEngine;
using UnityEngine.UI;

// Clase encargada de hacer que se muestren las preparaciones en la bandeja del minijuego
public class CoffeeFoodManager : MonoBehaviour
{
    public static CoffeeFoodManager Instance;

    [Header("Cafes")]
    public Image taza_CoffeePanel;
    public Image taza_FoodPanel;
    public Image vaso_CoffeePanel;
    public Image vaso_FoodPanel;

    private Image cafeContenedorActivo; 
    private Sprite cafeSpriteActual;

    [Header("Comida")]
    public Image plato_CoffeePanel;
    public Image plato_FoodPanel;
    public Image bolsa_CoffeePanel;
    public Image bolsa_FoodPanel;

    private Image comidaContenedorActivo; 
    private Sprite comidaSpriteActual;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Funcion para activaar/desactivar el sprite del cafe
    public void ToggleCafe(bool preparar, Image contenedor, Sprite sprite)
    {
        if (!preparar)
        {
            // Desactivar ambos paneles
            if (taza_CoffeePanel) taza_CoffeePanel.gameObject.SetActive(false);
            if (taza_FoodPanel) taza_FoodPanel.gameObject.SetActive(false);
            if (vaso_CoffeePanel) vaso_CoffeePanel.gameObject.SetActive(false);
            if (vaso_FoodPanel) vaso_FoodPanel.gameObject.SetActive(false);

            cafeContenedorActivo = null;
            cafeSpriteActual = null;
            return;
        }

        // Guardamos el contenedor activo y sprite
        cafeContenedorActivo = contenedor;
        cafeSpriteActual = sprite;

        // Activamos el contenedor en ambos paneles
        if (contenedor == taza_CoffeePanel || contenedor == taza_FoodPanel)
        {
            taza_CoffeePanel.gameObject.SetActive(true);
            taza_FoodPanel.gameObject.SetActive(true);
            if (cafeSpriteActual)
            {
                taza_CoffeePanel.sprite = cafeSpriteActual;
                taza_FoodPanel.sprite = cafeSpriteActual;
            }
            // Desactivar el otro contenedor
            vaso_CoffeePanel.gameObject.SetActive(false);
            vaso_FoodPanel.gameObject.SetActive(false);
        }
        else if (contenedor == vaso_CoffeePanel || contenedor == vaso_FoodPanel)
        {
            vaso_CoffeePanel.gameObject.SetActive(true);
            vaso_FoodPanel.gameObject.SetActive(true);
            if (cafeSpriteActual)
            {
                vaso_CoffeePanel.sprite = cafeSpriteActual;
                vaso_FoodPanel.sprite = cafeSpriteActual;
            }
            // Desactivar el otro contenedor
            taza_CoffeePanel.gameObject.SetActive(false);
            taza_FoodPanel.gameObject.SetActive(false);
        }
    }

    // Funcion para activaar/desactivar el sprite de la comida
    public void ToggleComida(bool preparar, Image contenedor, Sprite sprite)
    {
        if (!preparar)
        {
            if (plato_CoffeePanel) plato_CoffeePanel.gameObject.SetActive(false);
            if (plato_FoodPanel) plato_FoodPanel.gameObject.SetActive(false);
            if (bolsa_CoffeePanel) bolsa_CoffeePanel.gameObject.SetActive(false);
            if (bolsa_FoodPanel) bolsa_FoodPanel.gameObject.SetActive(false);

            comidaContenedorActivo = null;
            comidaSpriteActual = null;
            return;
        }

        comidaContenedorActivo = contenedor;
        comidaSpriteActual = sprite;

        if (contenedor == plato_CoffeePanel || contenedor == plato_FoodPanel)
        {
            plato_CoffeePanel.gameObject.SetActive(true);
            plato_FoodPanel.gameObject.SetActive(true);
            if (comidaSpriteActual)
            {
                plato_CoffeePanel.sprite = comidaSpriteActual;
                plato_FoodPanel.sprite = comidaSpriteActual;
            }
            bolsa_CoffeePanel.gameObject.SetActive(false);
            bolsa_FoodPanel.gameObject.SetActive(false);
        }
        else if (contenedor == bolsa_CoffeePanel || contenedor == bolsa_FoodPanel)
        {
            bolsa_CoffeePanel.gameObject.SetActive(true);
            bolsa_FoodPanel.gameObject.SetActive(true);
            plato_CoffeePanel.gameObject.SetActive(false);
            plato_FoodPanel.gameObject.SetActive(false);
        }
    }

    
    /*public void ActualizarVisual()
    {
        // Café
        if (cafeContenedorActivo)
        {
            cafeContenedorActivo.gameObject.SetActive(true);
            if (cafeSpriteActual) cafeContenedorActivo.sprite = cafeSpriteActual;

            // Activar el otro panel
            if (cafeContenedorActivo == taza_CoffeePanel) taza_FoodPanel.gameObject.SetActive(true);
            if (cafeContenedorActivo == taza_FoodPanel) taza_CoffeePanel.gameObject.SetActive(true);
            if (cafeContenedorActivo == vaso_CoffeePanel) vaso_FoodPanel.gameObject.SetActive(true);
            if (cafeContenedorActivo == vaso_FoodPanel) vaso_CoffeePanel.gameObject.SetActive(true);
        }

        // Comida
        if (comidaContenedorActivo)
        {
            comidaContenedorActivo.gameObject.SetActive(true);
            if (comidaContenedorActivo == plato_CoffeePanel || comidaContenedorActivo == plato_FoodPanel)
            {
                if (comidaSpriteActual) comidaContenedorActivo.sprite = comidaSpriteActual;
                plato_CoffeePanel.gameObject.SetActive(true);
                plato_FoodPanel.gameObject.SetActive(true);
            }
            else if (comidaContenedorActivo == bolsa_CoffeePanel || comidaContenedorActivo == bolsa_FoodPanel)
            {
                bolsa_CoffeePanel.gameObject.SetActive(true);
                bolsa_FoodPanel.gameObject.SetActive(true);
            }
        }
    }*/

}
