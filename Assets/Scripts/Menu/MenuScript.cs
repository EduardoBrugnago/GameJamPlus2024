using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject menu;
    public GameObject creditos;
    public GameObject opcoes;

    private void Awake()
    {
        audioMixer.SetFloat("VolumeP", Mathf.Log10(PlayerPrefs.GetFloat("VolumeP", 1)) * 20);
        audioMixer.SetFloat("VolumeM", Mathf.Log10(PlayerPrefs.GetFloat("VolumeM", 1)) * 20);
        audioMixer.SetFloat("VolumeE", Mathf.Log10(PlayerPrefs.GetFloat("VolumeE", 1)) * 20);
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void GoToOpcoes()
    {
        DisableAll();
        opcoes.SetActive(true);
    }

    public void GoToCreditos()
    {
        DisableAll();
        creditos.SetActive(true);
    }

    public void GoToMenu()
    {
        DisableAll();
        menu.SetActive(true);
    }

    void DisableAll()
    {
        menu.SetActive(false);
        creditos.SetActive(false);
        opcoes.SetActive(false);
    }


}
