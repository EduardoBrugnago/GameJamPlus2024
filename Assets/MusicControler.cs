using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControler : MonoBehaviour
{
    public enum GameState { Menu, InGame, GameOver, Win }
    public GameState currentState;

    // Referências às músicas para cada estado
    public AudioClip menuMusic;
    public AudioClip inGameMusic;
    public AudioClip gameOverMusic;
    public AudioClip winMusic;

    private AudioSource audioSource;

    private static MusicControler instance;

    void Awake()
    {
        // Torna este objeto persistente entre as cenas
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Obtém o componente AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        PlayMusic(GameState.Menu);
    }

    public void PlayMusic(GameState state)
    {
        if (currentState == state && audioSource.isPlaying)
            return; // Não troca a música se já estiver tocando a do estado atual

        currentState = state;
        switch (state)
        {
            case GameState.Menu:
                audioSource.clip = menuMusic;
                break;
            case GameState.InGame:
                audioSource.clip = inGameMusic;
                break;
            case GameState.GameOver:
                audioSource.clip = gameOverMusic;
                break;
            case GameState.Win:
                audioSource.clip = winMusic;
                break;
        }

        audioSource.Play();
    }
}
