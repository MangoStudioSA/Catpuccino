using UnityEngine;

public class MiniGameSoundManager : MonoBehaviour
{
    public static MiniGameSoundManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void PlayTakeCup() => SoundsMaster.Instance.PlaySound_TakeCup();
    public void PlayTakeVase() => SoundsMaster.Instance.PlaySound_TakeVase();
    public void PlayTakePlate() => SoundsMaster.Instance.PlaySound_TakePlate();
    public void PlayTakeBag() => SoundsMaster.Instance.PlaySound_TakeBag();
    public void PlayButtonDown() => SoundsMaster.Instance.PlaySound_CoffeeAmountMachine();
    public void PlayButtonUp() => SoundsMaster.Instance.PlaySound_CoffeeAmountReady();
    public void PlayMolerCafe() => SoundsMaster.Instance.PlaySound_MolerCafe();
    public void PlayEcharCafe() => SoundsMaster.Instance.PlaySound_EcharCafe();
    public void PlayEcharLiquido() => SoundsMaster.Instance.PlaySound_EcharLiquido();
    public void PlayIntObjeto() => SoundsMaster.Instance.PlaySound_CogerDejarObj();
    public void PlayCogerHielo() => SoundsMaster.Instance.PlaySound_CogerHielo();
    public void PlayEcharHielo() => SoundsMaster.Instance.PlaySound_EcharHielo();
    public void PlayDejarHielo() => SoundsMaster.Instance.PlaySound_DejarHielo();
    public void PlayCuchara() => SoundsMaster.Instance.PlaySound_Cuchara();
    public void PlaySugar() => SoundsMaster.Instance.PlaySound_Azucar();
    public void PlayTakeFood() => SoundsMaster.Instance.PlaySound_TakeFood();
    public void PlayPapelera() => SoundsMaster.Instance.PlaySound_Papelera();

    public void PlayMachinePour() => SoundsMaster.Instance.PlayAudio("CoffeeMachine");
    public void StopMachinePour() => SoundsMaster.Instance.StopAudio("CoffeeMachine");
    public void PlayEspumadorPour() => SoundsMaster.Instance.PlayAudio("Espumador");
    public void StopEspumadorPour() => SoundsMaster.Instance.StopAudio("Espumador");
    public void PlayMicroondasPour() => SoundsMaster.Instance.PlayAudio("Microondas");
    public void StopMicroondasPour() => SoundsMaster.Instance.StopAudio("Microondas");

}
