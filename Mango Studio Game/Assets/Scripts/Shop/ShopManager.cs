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

    public void OpenCoinsShop()
    {
        coinsShopPanel.SetActive(true);
    }

    public void CloseCoinsShop()
    {
        coinsShopPanel.SetActive(false);
    }

    public void CloseOpenPackPanel()
    {
        openPackPanel.SetActive(false);
    }

    public void OnBuyPackCoinsClicked()
    {
        message.Show("Aún no puedes realizar esta acción. Requiere dinero real.");
    }

    public void BuyBasicPack()
    {
        if (PlayerDataManager.instance.SpendBasicCoins(basicPackCost))
        {
            Debug.Log("Sobre básico abierto");
            UpdateUI();

            cardPackManager.ShowPackPanel("basic");
        }
        else
        {
            Debug.LogWarning("No tienes suficientes monedas para el sobre básico.");
        }
    }

    public void BuyPremiumPack()
    {
        if (PlayerDataManager.instance.SpendPremiumCoins(premiumPackCost))
        {
            Debug.Log("Sobre premium abierto");
            UpdateUI();

            cardPackManager.ShowPackPanel("premium");
        }
        else
        {
            Debug.LogWarning("No tienes suficientes monedas para el sobre premium.");
        }
    }
}
