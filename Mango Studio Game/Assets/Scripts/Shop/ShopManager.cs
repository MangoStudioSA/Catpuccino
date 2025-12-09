using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Funcion principal de la tienda
public class ShopManager : MonoBehaviour
{
    [Header("Textos UI")]
    public TextMeshProUGUI basicCoinsText;
    public TextMeshProUGUI premiumCoinsText;
    // Mensajes para aceptar/rechazar acciones
    [SerializeField] private MessagePopUp message;
    [SerializeField] private MessagePopUp messageBuyBP;
    [SerializeField] private MessagePopUp messageBuyPP;
    [SerializeField] private MessagePopUp messageNotEnoughBMoney;
    [SerializeField] private MessagePopUp messageNotEnoughPMoney;

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
    public Button returnNotEnoughBMoney;
    public Button returnNotEnoughPMoney;

    [Header("Costes de los sobres")]
    public int basicPackCost = 70;
    public int premiumPackCostBC = 220;
    public int premiumPackCostPC = 100;

    [Header("Panel UI")]
    public GameObject coinsShopPanel;
    public GameObject openPackPanel;
    public GameObject collectionPanel;
    public GameObject buyBasicPackMsgPanel;
    public GameObject buyPremiumPackMsgPanel;
    public GameObject notEnoughBMoneyMsgPanel;
    public GameObject notEnoughPMoneyMsgPanel;

    [Header("Referencias")]
    public CardPackManager cardPackManager;
    public static ShopManager Instance;

    private void OnEnable()
    {
        if (PlayerDataManager.instance != null)
            UpdateUI();
    }
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } else { Instance = this; }
    }

    // Se vinculan los botones con sus acciones
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

        returnNotEnoughBMoney.onClick.AddListener(ReturnNotEnoughBasicMoney);
        returnNotEnoughPMoney.onClick.AddListener(ReturnNotEnoughPremiumMoney);

        coinsShopPanel.SetActive(false); 
    }

    // Actualizar la interfaz si se realizan compras
    public void UpdateUI()
    {
        var data = PlayerDataManager.instance.data;
        basicCoinsText.text = $"{data.basicCoins}";
        premiumCoinsText.text = $"{data.premiumCoins}";
    }

    #region Interaccion paneles
    public void OpenCoinsShop()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
        coinsShopPanel.SetActive(true); // Abrir panel de comprar monedas premium
    }

    public void CloseCoinsShop()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
        coinsShopPanel.SetActive(false); // Cerrar panel de comprar monedas premium
    }

    public void OpenCollectionPanel()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
        collectionPanel.SetActive(true); // Abrir panel de comprar monedas premium
    }

    public void CloseCollectionPanel()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
        collectionPanel.SetActive(false); // Cerrar panel de comprar monedas premium
    }

    public void CloseOpenPackPanel()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
        openPackPanel.SetActive(false); // Cerrar panel de abrir sobre
    }
    #endregion

    #region Funciones botones
    public void OnBuyPackCoinsClicked()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
        message.Show("Funcionalidad en desarrollo."); // Mensaje cuando el jugador intenta comprar monedas premium
    }

    public void OnBuyBasicPackClicked()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
        messageBuyBP.Show("Si compras este sobre gastarás 70 monedas de café. ¿Deseas continuar y abrirlo?"); // Mensaje cuando el jugador intenta comprar un sobre básico
    }

    public void OnBuyPremiumPackClicked()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
        messageBuyPP.Show("Si compras este sobre gastarás 100 croquetas doradas o 200 monedas de café. ¿Deseas continuar y abrirlo?"); // Mensaje cuando el jugador intenta comprar un sobre premium
    }

    public void OnFinalBuyBasicPackClicked()
    {
        MiniGameSoundManager.instance.PlaySpendMoney();
        BuyBasicPack(); // Si acepta comprar el sobre basico se llama a la funcion correspondiente
        buyBasicPackMsgPanel.SetActive(false); 
    }

    public void OnFinalBuyPremiumPackClicked()
    {
        MiniGameSoundManager.instance.PlaySpendMoney();
        BuyPremiumPackPC(); // Si acepta comprar el sobre premium se llama a la funcion correspondiente
        buyPremiumPackMsgPanel.SetActive(false);
    }

    public void NotEnoughBasicMoney()
    {
        MiniGameSoundManager.instance.PlayNotEnoughMoney();
        messageNotEnoughBMoney.Show("No tienes suficientes monedas de café para comprar este sobre."); // Mensaje cuando el jugador intenta comprar un sobre básico y no tiene suficiente dinero
    }

    public void NotEnoughPremiumMoney()
    {
        MiniGameSoundManager.instance.PlayNotEnoughMoney();
        messageNotEnoughPMoney.Show("No tienes suficientes monedas de café/croquetas doradas para comprar este sobre"); // Mensaje cuando el jugador intenta comprar un sobre premium y no tiene suficiente dinero
    }

    public void ReturnNotEnoughBasicMoney()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
        notEnoughBMoneyMsgPanel.SetActive(false); // Volver cuando no tiene suficientes monedas basicas
    }

    public void ReturnNotEnoughPremiumMoney()
    {
        SoundsMaster.Instance.PlaySound_ClickMenu();
        notEnoughPMoneyMsgPanel.SetActive(false); // Volver cuando no tiene suficientes monedas premium
    }
    #endregion

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
            NotEnoughBasicMoney();
        }
    }

    // Interaccion con el boton de comprar un sobre premium con monedas premium
    public void BuyPremiumPackPC()
    {
        var player = PlayerDataManager.instance;
        bool paid = false;

        if (player.SpendPremiumCoins(100)) paid = true;
        
        if (paid)
        {
            UpdateUI();
            cardPackManager.PreparePack("premium");
        }
        else BuyPremiumPackBC();
    }

    // Interaccion con el boton de comprar un sobre premium con monedas basicas
    public void BuyPremiumPackBC()
    {
        var player = PlayerDataManager.instance;
        bool paid = false;

        if (player.SpendBasicCoins(200)) paid = true;

        if (paid)
        {
            UpdateUI();
            cardPackManager.PreparePack("premium");
        }
        else NotEnoughPremiumMoney();
    }
}
