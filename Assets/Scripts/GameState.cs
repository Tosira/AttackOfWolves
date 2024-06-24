using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.src;
using System.IO;
using TMPro;


public class GameState : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabsEnemies;
    private string path = "Assets/Scripts/Niveles.txt";
    List<Level> levels = new List<Level>();
    bool levelsSuccesfullySet = false;
    Level currentGameLevel;
    int levelIndex = 0;

    float timeInstance = 0.8f;
    float copyTimeInstance;

    private int money = 100;
    public static GameState gs;

    //  Variables temporales hasta que se ajusten interfaces. 
    private string moneyTxt = "Monedas: "; 
    [SerializeField] private TextMeshProUGUI txtMesh;

    private void Start()
    {
        // 1. Concerning other
        gs = this;         
        txtMesh.text = moneyTxt + money.ToString();
        
        // 2. Concerning levels
        if (prefabsEnemies.Count > 0 && ReadFileLevels())
        {
            if (PrepareLevels()) levelsSuccesfullySet = true;
        }
        currentGameLevel = levels[levelIndex];        
        copyTimeInstance = timeInstance;

        //Debug.Log("Cantidad de niveles " + levels.Count);

        //foreach (var lvl in levels)
        //{
        //    foreach (Wave wv in lvl.GetWaves())
        //    {
        //        foreach (float time in wv.GetTimeInstance())
        //        {
        //            Debug.Log("Time " + time);
        //        }
        //        foreach (var pair in wv.GetPairs())
        //        {
        //            Debug.Log("Cantidad " + pair.Quantity + " Enemigo " + pair.Enemy);
        //        }
        //    }
        //}

        //Wave wv = levels[0].GetWaves()[1];
        //foreach(float time in wv.GetTimeInstance())
        //{
        //    Debug.Log("Time " + time);
        //}
    }

    private void Update()
    {
        if (!levelsSuccesfullySet) return;
        if (currentGameLevel.WithoutWaves())
        {
            if (!UpdateCurrentLevel())
            {
                Debug.Log("Niveles Agotados");
                return;
            }
        }
        if (!currentGameLevel.PrioritizeTimeInstance()) timeInstance -= Time.deltaTime;
        if ((timeInstance -= Time.deltaTime) <= 0 || currentGameLevel.PrioritizeTimeInstance())
        {
            currentGameLevel.UpdateLevel();
            timeInstance = copyTimeInstance;
        }
    }

    private bool ReadFileLevels()
    {
        Level currentLevel = new Level();
        Wave currentWave = new Wave();

        using (StreamReader sr = new StreamReader(path))
        {            
            bool initializeTimes = false;   // Inidica que lineas del archivo corresponden a los datos para inicializar los tiempos. 
            string line; 
            while((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("Nivel 1") || line.StartsWith("Ola 1"))
                {
                    //Debug.Log("Salto"); 
                    continue;
                } 
                if (line.StartsWith("Ola"))
                {
                    currentLevel.AddWave(currentWave); 
                    currentWave = new Wave();
                    initializeTimes = false;
                    continue; 
                }
                if (line.StartsWith("Nivel"))
                {
                    if (currentLevel == null || currentWave == null)
                    {
                        Debug.LogError("ERROR");
                        return false;
                    }
                    //  Cuando se llega al siguiente nivel, se guarda la ultima ola del nivel anterior. No hay error en que la primera ola del nuevo nivel 
                    //  guarde una ola vacia ya que la condicion "Ola 1" salta las primeras olas de cada nivel.
                    currentLevel.AddWave(currentWave);
                    currentWave = new Wave();

                    levels.Add(currentLevel);
                    currentLevel = new Level();
                    initializeTimes = false; 
                    continue; 
                }   
                if (line.StartsWith("/"))   //  Despues de este caracter se encuentran los tiempos de instanciamiento.
                {                    
                    initializeTimes = true; 
                    continue;
                }                
                if (initializeTimes)
                {
                    //Debug.Log("Times");
                    var data = line.Split(',');
                    float time = float.Parse(data[0]);
                    currentWave.AddTimeInstance(time); 
                }
                else
                {
                    var data = line.Split(',');
                    int quantity = int.Parse(data[0]);
                    string enemy = data[1];
                    //Debug.Log(quantity+" "+enemy+" "+currentWave.GetPairs().Count);
                    currentWave.AddPair(quantity, enemy); 
                }
            }//while            
        }//using
        
        Debug.Log("Lectura Realizada");
        return true;
    }    

    private bool PrepareLevels()
    {
        foreach (Level lvl in levels)
        {
            if (!lvl.PrepareLevel(prefabsEnemies))
            {
                Debug.LogError("Error de Lectura");
                return false;
            }
        }
        return true;
    }

    private bool UpdateCurrentLevel()
    {
        if (levelIndex >= levels.Count - 1) return false;
        levelIndex++;
        currentGameLevel = levels[levelIndex];        
        return true;
    }

    //
    public void AddMoney(int _money)
    {
        money += _money; 
    }

    public bool Buy(int cost)
    {
        if (money < cost) return false; 

        money -= cost;
        return true; 
    }    
}
