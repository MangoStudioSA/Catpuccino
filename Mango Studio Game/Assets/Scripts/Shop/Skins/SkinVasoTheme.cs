using UnityEngine;

[System.Serializable]
public class SkinVasoTheme 
{
    public string skinName;
    public bool isUnlocked;

    [Header("Sprites Vasos")]
    public Sprite vasoEspresso;
    public Sprite vasoAmericanoLungo;
    public Sprite vasoMocaIrishLatteCapucino;
    public Sprite vasoMachiatoBombon;
    public Sprite vasoVienes;
    public Sprite vasoFrappe;

    [Header("Sprites Extras")]
    public Sprite vasoConTapa;
    public Sprite vasoSinTapa;
}
