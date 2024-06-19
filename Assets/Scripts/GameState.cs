using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.src; 

public class GameState : MonoBehaviour
{ 
    private RouteManager routeManager;
    private int level = 1;    
    private int wave = 1;   // actualizacion desde la ola 2 

    private void Start()
    {
        routeManager = FindObjectOfType<RouteManager>();
        if (routeManager == null)
        {
            Debug.Log("RouteManager no encontrado");
        } 
    }

    private void Update()
    {
        if (routeManager == null) return;  
        UpdateWave();
        UpdateScene(); 
    }

    private void UpdateScene()
    {
        Enemigo enemy = FindObjectOfType<Enemigo>();       
        if (!(routeManager.WithoutEnemies() && enemy == null && wave == 3)) return;
        
        if (level == 1)
        {
            SceneManager.LoadScene("Level2");
            return;
        }
        if (level == 2)
        {
            SceneManager.LoadScene("Level3");
            return;
        }
        level += 1; 
        wave = 1; 
    }

    private void UpdateWave()
    {
        if (!routeManager.WithoutEnemies() || wave >= 3) return;        

        wave += 1;
        //Debug.Log("SUMA. Wave " + wave); 
        //  ESTO ESTA HORRIBLE!!
        ////////////////////////////////Nivel1/////////////////////////////
        if (level == 1 && wave == 2)
        {
            Nivel1 nivel1 = new Nivel1();
            routeManager.SetInstanceVariables(nivel1.ola2_enemyInstanceTime, nivel1.ola2_enemyQuantity);
        }
        else if (level == 1 && wave == 3)
        {
            Nivel1 nivel1 = new Nivel1();
            routeManager.SetInstanceVariables(nivel1.ola3_enemyInstanceTime, nivel1.ola3_enemyQuantity);
        }////////////////////////////////Nivel2/////////////////////////////
        else if (level == 2 && wave == 2)
        {
            Nivel2 nivel2 = new Nivel2();
            routeManager.SetInstanceVariables(nivel2.ola2_enemyInstanceTime, nivel2.ola2_enemyQuantity);
        }
        else if (level == 2 && wave == 3)
        {
            Nivel2 nivel2 = new Nivel2();
            routeManager.SetInstanceVariables(nivel2.ola2_enemyInstanceTime, nivel2.ola2_enemyQuantity);
        }////////////////////////////////Nivel3/////////////////////////////
        else if (level == 3 && wave == 2)
        {
            Nivel3 nivel3 = new Nivel3();
            routeManager.SetInstanceVariables(nivel3.ola2_enemyInstanceTime, nivel3.ola2_enemyQuantity);
        }
        else if (level == 3 && wave == 3)
        {
            Nivel3 nivel3 = new Nivel3();
            routeManager.SetInstanceVariables(nivel3.ola2_enemyInstanceTime, nivel3.ola2_enemyQuantity);
        }
    }

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
