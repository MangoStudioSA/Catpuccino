using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Textos UI")]
    public TextMeshProUGUI basicCoinsText;
    public TextMeshProUGUI premiumCoinsText;
    [SerializeField] private MessagePopUp message;
    [SerializeField] private MessagePopUp messageBuyBP;
    [SerializeField] private MessagePopUp messageBuyPP;

    [Header("Botones")]
    public Button basicPackButton;
    public Button premiumPackButton;
    public Button coinsShopButton;
    public Button closeCoinsShopButton;
    public Button openCollectionButton;
    public Button closeCollectionButton;
    public Button buyPack1Button;
    public Button buyPack2Button;
    public Button buyPack3Button;
    public Button buyPack4Button;
    public Button acceptBuyBasicPackButton;
    public Button acceptBuyPremiumPackButton;

    [Header("Costes de los sobres")]
    public int basicPackCost = 70;
    public int premiumPackCostBC = 120;
    public int premiumPackCostPC = 220;

    [Header("Panel UI")]
    public GameObject coinsShopPanel;
    public GameObject openPackPanel;
    public GameObject collectionPanel;
    public GameObject buyBasicPackMsgPanel;
    public GameObject buyPremiumPackMsgPanel;

    [Header("Referencias")]
    public CardPackManager cardPackManager;

    private void Start()
    {
        UpdateUI();
;
        coinsShopButton.onClick.AddListener(OpenCoinsShop);
        closeCoinsShopButton.onClick.AddListener(CloseCoinsShop);

        buyPack1Button.onClick.AddListener(OnBuyPackCoinsClicked);
        buyPack2Button.onClick.AddListener(OnBuyPackCoinsClicked);
        buyPack3Button.onClick.AddListener(OnBuyPackCoinsClicked);
        buyPack4Button.onClick.AddListener(OnBuyPackCoinsClicked);

        basicPackButton.onClick.AddListener(OnBuyBasicPackClicked);
        premiumPackButton.onClick.AddListener(OnBuyPremiumPackClicked);

        acceptBuyBasicPackButton.onClick.AddListener(OnFinalBuyBasicPackClicked);
        acceptBuyPremiumPackButton.onClick.AddListener(OnFinalBuyPremiumPackClicked);

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

    public void OnBuyBasicPackClicked()
    {
        messageBuyBP.Show("Si compras este sobre gastarás 70 monedas de café. ¿Deseas continuar y abrirlo?"); // Mensaje cuando el jugador intenta comprar un sobre básico
    }

    public void OnBuyPremiumPackClicked()
    {
        messageBuyPP.Show("Si compras este sobre gastarás 120 croquetas doradas o 220 monedas de café. ¿Deseas continuar y abrirlo?"); // Mensaje cuando el jugador intenta comprar un sobre premium
    }

    public void OnFinalBuyBasicPackClicked()
    {
        BuyBasicPack();
        buyBasicPackMsgPanel.SetActive(false);
    }

    public void OnFinalBuyPremiumPackClicked()
    {
        BuyPremiumPack();
        buyPremiumPackMsgPanel.SetActive(false);
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
        if (PlayerDataManager.instance.SpendPremiumCoins(premiumPackCostBC) || PlayerDataManager.instance.SpendPremiumCoins(premiumPackCostPC))
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
