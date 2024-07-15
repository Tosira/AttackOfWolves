using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEditor.SceneManagement;

public class Player
{
    public int life = 100;
    public int money = 100;
}

public class GameState : MonoBehaviour
{    
    private Player player;

    [SerializeField] private List<GameObject> prefabsEnemies;
    List<Level> levels = new List<Level>();
    bool levelsSuccesfullySet = false;
    Level currentGameLevel;
    int levelIndex = 0;

    float timeInstance = 0.8f;
    float copyTimeInstance;

    public static GameState gs;
    private DialogsManager dm;
    private Piggy currentPiggy;
    public static GameObject target;
    
    private string moneyTxt = "Monedas: ";
    private string lifeTxt = "Vidas: ";
    [SerializeField] private TextMeshProUGUI txtMeshMoney;
    [SerializeField] private TextMeshProUGUI txtMeshLife;
    [SerializeField] private TextMeshProUGUI txtMeshDialog;

    private void Start()
    {
        // GameState
        gs = this;

        // Meta de Enemigos
        target = GameObject.Find("Meta");
        if (target == null)
        {
            Debug.LogError("Meta de escena no encontrada");
            return;
        }
        
        // DialogManager
        GameObject gmDialogsManager = new GameObject("DialogsManager");
        dm = gmDialogsManager.AddComponent<DialogsManager>();
        
        // Piggy
        currentPiggy = new Piggy("Cerdito Ladrillo", txtMeshDialog);

        // Estadisticas Jugador
        player = new Player();
        txtMeshMoney.text = moneyTxt + player.money.ToString();
        txtMeshLife.text = lifeTxt + player.life.ToString();
        
        // Niveles del Juego
        // if (prefabsEnemies.Count > 0 && ReadFileLevels())
        // {
        //     if (PrepareLevels())
        //     {
        //         //Debug.Log("Cantidad de niveles"+levels.Count);
        //         levelsSuccesfullySet = true;
        //         currentGameLevel = levels[levelIndex];
        //     }
        // }
        // copyTimeInstance = timeInstance;

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
    }

    private void Update()
    {
        // 1. Related Dialogs
        // Considere no llamar en GameState::Start porque metodo DialogsManager::Start es llamado luego y dialog vuelve a "".
        dm.SetCharacterDialogue(currentPiggy);
        dm.ShowDialog(currentPiggy);

        // if (GameOver()) SceneManager.LoadScene("GameOver");
        // txtMeshMoney.text = moneyTxt + player.money.ToString();
        // txtMeshLife.text = lifeTxt + player.life.ToString();

        // if (!levelsSuccesfullySet) return;
        // if (currentGameLevel.WithoutWaves())
        // {
        //     if (!UpdateCurrentLevel())
        //     {
        //         //Debug.Log("Niveles Agotados");
        //         return;
        //     }
        //     Debug.Log("NUEVO NIVEL " + (levelIndex+1));
        // }
        // if (!currentGameLevel.PrioritizeTimeInstance()) timeInstance -= Time.deltaTime;
        // if ((timeInstance -= Time.deltaTime) <= 0 || currentGameLevel.PrioritizeTimeInstance())
        // {
        //     currentGameLevel.UpdateLevel();
        //     timeInstance = copyTimeInstance;
        // }
    }

    private bool ReadFileLevels()
    {
        Level currentLevel = new Level();
        Wave currentWave = new Wave();
        TextAsset levelFile = Resources.Load<TextAsset>("Niveles");
        if (levelFile == null)
        {
            Debug.LogError("No se pudo cargar el Archivo");
            return false;
        }
        Stream StreamLevelFile = ConvertTextAssetToStream(levelFile);
        using (StreamReader sr = new StreamReader(StreamLevelFile))
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
            sr.Close();
        }//using
        
        Debug.Log("Lectura Realizada");
        return true;
    }

    private Stream ConvertTextAssetToStream(TextAsset textAsset)
    {
        // Convertir el contenido del TextAsset a un byte array
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(textAsset.text);

        // Crear un MemoryStream a partir del byte array
        return new MemoryStream(byteArray);
    }

    private bool PrepareLevels()
    {
        foreach (Level lvl in levels)
        {
            if (!lvl.PrepareLevel(prefabsEnemies))
            {
                Debug.LogError("ERROR DE LECTURA. Los nombres de tipos, cantidades o tiempos de instancia pueden ser incorrectos. Revise el archivo .txt de Niveles.");
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
    
    public void AddMoney(int _money)
    {
        player.money += _money; 
    }

    public bool Buy(int cost)
    {
        if (player.money < cost) return false; 

        player.money -= cost;
        return true; 
    }

    public void ReceiveEnemyAttack()
    {
        player.life -= 1;
    }

    private bool GameOver()
    {
        return player.life <= 0;
    }
}
