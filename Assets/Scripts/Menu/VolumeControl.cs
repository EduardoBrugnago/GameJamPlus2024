using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumePrincipalSlider;
    public Slider volumeMusicSlider;
    public Slider volumeEffectSlider;

    public void SetVolumeMaster(float volume)
    {
        audioMixer.SetFloat("VolumeP", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("VolumeP", volume);
    }
    public void SetVolumeMusic(float volume)
    {
        audioMixer.SetFloat("VolumeM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("VolumeM", volume);
    }
    public void SetVolumeSfx(float volume)
    {
        audioMixer.SetFloat("VolumeE", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("VolumeE", volume);
    }

    // Inicializa o slider com o valor inicial
    void Start()
    {
        float volumeP = PlayerPrefs.GetFloat("VolumeP", 1);
        float volumeM = PlayerPrefs.GetFloat("VolumeM", 1);
        float volumeE = PlayerPrefs.GetFloat("VolumeE", 1);
        volumePrincipalSlider.value = volumeP;
        volumeMusicSlider.value = volumeM;
        volumeEffectSlider.value = volumeE;
    }
}
