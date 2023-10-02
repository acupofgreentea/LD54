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

    public static int CurrentMoney
    {
        get => PlayerPrefs.GetInt("Money", 10);
        set 
        {
            PlayerPrefs.SetInt("Money", value);
            CurrencyUpdate?.Invoke();
        }
    }

    public static event UnityAction CurrencyUpdate;
}
