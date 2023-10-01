using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioMixer sfxMixer;    
    [SerializeField] private AudioMixer musicMixer;    

    public static GameManager Instance;

    void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator Start()
    {
        yield return null;

        musicMixer.SetFloat("MusicVolume", Mathf.Log10(DataManager.MusicVolume) * 20);
        sfxMixer.SetFloat("SfxVolume", Mathf.Log10(DataManager.SfxVolume) * 20);
    }
}
