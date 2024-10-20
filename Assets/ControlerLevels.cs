using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlerLevels : MonoBehaviour
{
    private static ControlerLevels instance;
    public string[] scenes = { "Lvl_1", "Lvl_2", "Lvl_3" }; // Adicione os nomes das suas cenas aqui
    public int currentSceneIndex = 0;
    public bool firstLoad = false;

    void Awake()
    {
        // Implementa o padrão Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void LoadNextScene()
    {
        currentSceneIndex++;
        if (currentSceneIndex >= scenes.Length)
        {
            currentSceneIndex = 0;
            SceneManager.LoadScene("Menu");
            return; 
        }
        LoadScene(currentSceneIndex);
    }

    // Método auxiliar para carregar a cena com base no índice
    private void LoadScene(int index)
    {
        firstLoad = false;
        SceneManager.LoadScene(scenes[index]);
    }
}
