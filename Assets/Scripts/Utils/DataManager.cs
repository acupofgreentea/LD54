using UnityEngine;

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
}
