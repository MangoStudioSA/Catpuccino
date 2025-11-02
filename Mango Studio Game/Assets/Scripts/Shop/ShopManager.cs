using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Textos UI")]
    public TextMeshProUGUI basicCoinsText;
    public TextMeshProUGUI premiumCoinsText;
    [SerializeField] private MessagePopUp message;
     
    [Header("Botones")]
    public Button basicPackButton;
    public Button premiumPackButton;
    public Button coinsShopButton;
    public Button closeCoinsShopButton;
    public Button openCollectionButton;
    public Button closeCollectionButton;
    public Button buyPack1;
    public Button buyPack2;
    public Button buyPack3;
    public Button buyPack4;

    [Header("Costes de los sobres")]
    public int basicPackCost = 70;
    public int premiumPackCost = 120;

    [Header("Panel UI")]
    public GameObject coinsShopPanel;
    public GameObject openPackPanel;
    public GameObject collectionPanel;

    [Header("Referencias")]
    public CardPackManager cardPackManager;

    private void Start()
    {
        UpdateUI();

        basicPackButton.onClick.AddListener(BuyBasicPack);
        premiumPackButton.onClick.AddListener(BuyPremiumPack);
        coinsShopButton.onClick.AddListener(OpenCoinsShop);
        closeCoinsShopButton.onClick.AddListener(CloseCoinsShop);

        buyPack1.onClick.AddListener(OnBuyPackCoinsClicked);
        buyPack2.onClick.AddListener(OnBuyPackCoinsClicked);
        buyPack3.onClick.AddListener(OnBuyPackCoinsClicked);
        buyPack4.onClick.AddListener(OnBuyPackCoinsClicked);

        coinsShopPanel.SetActive(false); 
    }

    public void UpdateUI()
    {
        var data = PlayerDataManager.instance.data;
        basicCoinsText.text = $"{data.basicCoins}$";
        premiumCoinsText.text = $"{data.premiumCoins}";
    }

    #region Interaccion paneles
    public void OpenCoinsShop()
    {
        coinsShopPanel.SetActive(true); // Abrir panel de comprar monedas premium
    }

    public void CloseCoinsShop()
    {
        coinsShopPanel.SetActive(false); // Cerrar panel de comprar monedas premium
    }

    public void OpenCollectionPanel()
    {
        collectionPanel.SetActive(true); // Abrir panel de comprar monedas premium
    }

    public void CloseCollectionPanel()
    {
        collectionPanel.SetActive(false); // Cerrar panel de comprar monedas premium
    }

    public void CloseOpenPackPanel()
    {
        openPackPanel.SetActive(false); // Cerrar panel de abrir sobre
    }
    #endregion

    public void OnBuyPackCoinsClicked()
    {
        message.Show("Aún no puedes realizar esta acción. Requiere dinero real."); // Mensaje cuando el jugador intenta comprar monedas premium
    }

    // Interaccion con el boton de comprar un sobre basico
    public void BuyBasicPack()
    {
        if (PlayerDataManager.instance.SpendBasicCoins(basicPackCost))
        {
            Debug.Log("Sobre básico abierto");
            UpdateUI();
            cardPackManager.PreparePack("basic");
        }
        else
        {
            Debug.LogWarning("No tienes suficientes monedas para el sobre básico.");
        }
    }
    // Interaccion con el boton de comprar un sobre premium
    public void BuyPremiumPack()
    {
        if (PlayerDataManager.instance.SpendPremiumCoins(premiumPackCost))
        {
            Debug.Log("Sobre premium abierto");
            UpdateUI();

            cardPackManager.PreparePack("premium");
        }
        else
        {
            Debug.LogWarning("No tienes suficientes monedas para el sobre premium.");
        }
    }
}
