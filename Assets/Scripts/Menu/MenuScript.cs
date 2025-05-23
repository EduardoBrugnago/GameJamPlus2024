using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject menu;
    public GameObject creditos;
    public GameObject opcoes;
    public GameObject imgLogo;
    public GameObject firstInputOptions;
    public GameObject firstMenuOptions;
    public GameObject backButton;
    private void Awake()
    {
        FindFirstObjectByType<MusicControler>().PlayMusic(MusicControler.GameState.Menu);
        audioMixer.SetFloat("VolumeP", Mathf.Log10(PlayerPrefs.GetFloat("VolumeP", 1f)) * 20);
        audioMixer.SetFloat("VolumeM", Mathf.Log10(PlayerPrefs.GetFloat("VolumeM", 0.3f)) * 20);
        audioMixer.SetFloat("VolumeE", Mathf.Log10(PlayerPrefs.GetFloat("VolumeE", 0.25f)) * 20);
    }
   
    public void GoToGame()
    {
        SceneManager.LoadScene("Lvl_1");
        FindFirstObjectByType<MusicControler>().PlayMusic(MusicControler.GameState.InGame);
    }
    public void GoToOpcoes()
    {
        DisableAll();
        opcoes.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstInputOptions); 
    }

    public void GoToCreditos()
    {
        DisableAll();
        creditos.SetActive(true);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void GoToMenu()
    {
        DisableAll();
        menu.SetActive(true);
        imgLogo.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstMenuOptions);
    }

    void DisableAll()
    {
        menu.SetActive(false);
        creditos.SetActive(false);
        opcoes.SetActive(false);
        imgLogo.SetActive(false);
    }

    public void Close()
    {
        Application.Quit();
    }


}
