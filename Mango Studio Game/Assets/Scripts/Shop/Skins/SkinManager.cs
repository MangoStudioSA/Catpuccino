using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;

    [Header("Lista de skins disponibles")]
    public List<SkinCupTheme> skinsCups = new();
    public List<SkinVasoTheme> skinsVaso = new();

    [Header("Skin activa actual")]
    public string activeSkinCupName = "Default";
    public string activeSkinVasoName = "Default";

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        LoadActiveCupSkin();
        LoadActiveVasoSkin();
    }

    #region Tazas
    public SkinCupTheme GetActiveCupSkin()
    {
        return skinsCups.Find(s => s.skinName == activeSkinCupName);
    }

    public void SetActiveCupSkin(string skinName)
    {
        activeSkinCupName = skinName;
        PlayerPrefs.SetString("ActiveCupSkin", skinName);
    }

    public void LoadActiveCupSkin()
    {
        activeSkinCupName = PlayerPrefs.GetString("ActiveCupSkin", "Default");
    }

    public void UnlockCupSkin(string skinName)
    {
        var skin = skinsCups.Find(s => s.skinName == skinName);
        if (skin != null)
        {
            skin.isUnlocked = true;
            PlayerPrefs.SetInt($"SkinCupUnlocked {skinName}", 1);
        }
    }
    public bool IsCupSkinUnlocked(string skinName)
    {
        return PlayerPrefs.GetInt($"SkinCupUnlocked {skinName}", 0) == 1;
    }
    #endregion

    #region Vasos
    public SkinVasoTheme GetActiveVasoSkin()
    {
        return skinsVaso.Find(s => s.skinName == activeSkinVasoName);
    }

    public void SetActiveVasoSkin(string skinName)
    {
        activeSkinVasoName = skinName;
        PlayerPrefs.SetString("ActiveVasoSkin", skinName);
    }

    public void LoadActiveVasoSkin()
    {
        activeSkinVasoName = PlayerPrefs.GetString("ActiveVasoSkin", "Default");
    }

    public void UnlockVasoSkin(string skinName)
    {
        var skin = skinsVaso.Find(s => s.skinName == skinName);
        if (skin != null)
        {
            skin.isUnlocked = true;
            PlayerPrefs.SetInt($"SkinVasoUnlocked {skinName}", 1);
        }
    }
    public bool IsVasoSkinUnlocked(string skinName)
    {
        return PlayerPrefs.GetInt($"SkinVasoUnlocked {skinName}", 0) == 1;
    }
    #endregion
}