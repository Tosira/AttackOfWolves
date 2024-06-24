using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Game()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); 
#endif
    }
}
