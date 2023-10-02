using UnityEngine;
using UnityEngine.Events;

public static class DataManager
{
    public static float MusicVolume 
    {
        get => PlayerPrefs.GetFloat("MusicVolume", 1);
        set => PlayerPrefs.SetFloat("MusicVolume", value);
    }
    public static float SfxVolume 
    {
        get => PlayerPrefs.GetFloat("SfxVolume", 1);
        set => PlayerPrefs.SetFloat("SfxVolume", value);
    }

    private static int currentMoney = 250;

    public static void ResetMoney() => currentMoney = 250;
    public static int CurrentMoney
    {
        get => currentMoney;
        set 
        {
            currentMoney = value;
            CurrencyUpdate?.Invoke();
        }
    }

    public static event UnityAction CurrencyUpdate;
}
