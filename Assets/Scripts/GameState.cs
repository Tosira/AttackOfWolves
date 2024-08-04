using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using System;

public class Player
{
    public int life = 10;
    public int money = 200;
}

public class GameState : MonoBehaviour
{    
    private Player player;

    [SerializeField] private List<GameObject> prefabsEnemies;
    List<Level> levels = new List<Level>();
    bool levelsSuccesfullySet = false;
    Level currentGameLevel;
    int levelIndex = 0;
    public int LevelIndex { get { return levelIndex+1; } }
    private bool startWave;

    float timeInstance = 0.8f;
    float copyTimeInstance;

    private static GameState _gs;
    public static GameState gs { get { return _gs; } }
    public static GameObject target;
    private Scene currentScene;
    private bool updateCurrentScene;
    // public Scene CurrentScene { get { return currentScene; } }
    [SerializeField] private List<GameObject> piggies;
    
    [SerializeField] private TextMeshProUGUI txtMeshMoney;
    [SerializeField] private TextMeshProUGUI txtMeshLife;

    Stream streamLevelFile;

    private void Start()
    {
        Debug.Log("START GAME STATE");
    }

    private void Awake()
    {
        Debug.Log("AWAKE GAME STATE");
        if (gs != null) { Destroy(gameObject); return; } // Destruir GameState creado en cambio de escena

        // GameState
        Debug.Log("GS NULL");
        _gs = FindObjectOfType<GameState>();
        DontDestroyOnLoad(gameObject);

        // Meta enemigo
        target = GameObject.Find("Meta");
        if (target == null)
        {
            Debug.LogError("Meta de escena no encontrada");
            return;
        }
        
        // DialogManager
        // Esto genera errores de referencia nula cuando se declara en el DialogsManager a gmDialogsManager como no destruible.
        // if (dm == null)
        // {
        //     GameObject gmDialogsManager = new GameObject("DialogsManager_");
        //     dm = gmDialogsManager.AddComponent<DialogsManager>();
        // }
        
        // DialogsManager
        DialogsManager.Instance.Initialze(piggies);

        // Estadisticas Jugador
        player = new Player();
        txtMeshMoney.text = player.money.ToString();
        txtMeshLife.text = player.life.ToString();
        
        // Niveles del Juego
        if (prefabsEnemies.Count > 0 && ReadFileLevels())
        {
            streamLevelFile.Close();
            if (PrepareLevels())
            {
                //Debug.Log("Cantidad de niveles"+levels.Count);
                levelsSuccesfullySet = true;
                currentGameLevel = levels[levelIndex];
            }
        }
        copyTimeInstance = timeInstance;
        startWave = false;

        // Scene
        currentScene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if (!levelsSuccesfullySet) return;

        if (updateCurrentScene) { updateCurrentScene=false; currentScene=SceneManager.GetActiveScene(); ReConfigure(); }
        if (currentScene.name == "GameOver" || currentScene.name == "Menu") return;

        if (GameOver()) { updateCurrentScene=true; SceneManager.LoadScene("GameOver"); return;}

        txtMeshMoney.text = player.money.ToString();
        txtMeshLife.text = player.life.ToString();

        // Dialogs
        // Considere no llamar en GameState::Start porque metodo DialogsManager::Start es llamado luego y dialog vuelve a "".
        if (currentGameLevel.CurrentWaveNotStarted() && !GameObject.FindWithTag("Enemigo"))
            DialogsManager.Instance.SetCharacterDialogue(levelIndex+1, currentGameLevel.GetWaveIndex()+1);
        if (Input.GetKeyDown(KeyCode.D)) { DialogsManager.Instance.ShowRestDialog(); }
        if (Input.GetKeyDown(KeyCode.Space)) { DialogsManager.Instance.Close(); DialogsManager.Instance.SkipTime(); }
        DialogsManager.Instance.ShowDialog();
        if (DialogsManager.Instance.DialogueInProgress) return;

        // Niveles
        if (currentGameLevel.WithoutWaves() && !GameObject.FindWithTag("Enemigo"))
        {
            if (!UpdateCurrentLevel())
            {
                Debug.Log("Niveles Agotados");
                SceneManager.LoadScene("Menu");
                return; 
            }
            LoadLevel();
            startWave = false;
        }
        if (currentGameLevel.CurrentWaveWithoutEnemies()) { currentGameLevel.UpdateLevel(); startWave = false;}
        if (currentGameLevel.CurrentWaveNotStarted() && Input.GetKeyDown(KeyCode.X)) startWave = true;
        if (startWave)
        {
            if (!currentGameLevel.PrioritizeTimeInstance()) timeInstance -= Time.deltaTime;
            if ((timeInstance -= Time.deltaTime) <= 0 || currentGameLevel.PrioritizeTimeInstance())
            {
                currentGameLevel.UpdateLevel();
                timeInstance = copyTimeInstance;
            }
        }
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(currentGameLevel.Name);
        InputHandler.Instance.mainCamera = Camera.main;
        InputHandler.Instance.mainCanvas = FindObjectOfType<Canvas>();
        updateCurrentScene = true;
        Debug.Log("NUEVO NIVEL " + (levelIndex+1));
    }

    private bool ReadFileLevels()
    {
        TextAsset levelFile = Resources.Load<TextAsset>("Texto/Niveles");
        if (levelFile == null)
        {
            Debug.LogError("No se pudo cargar el Archivo");
            return false;
        }
        streamLevelFile = ConvertTextAssetToStream(levelFile);
        try
        {
            using (StreamReader sr = new StreamReader(streamLevelFile))
            {
                Level currentLevel = new Level();
                Wave currentWave = new Wave();
                bool initializeTimes = false;   // Inidica que lineas del archivo corresponden a los datos para inicializar los tiempos. 
                string line;

                while((line = sr.ReadLine()) != null)
                {
                    // Lee directamente a los datos
                    if (line.StartsWith("Level1")) { currentLevel.ChangeName(line.Split('\n')[0]); continue; }
                    if (line.StartsWith("Wave 1")) continue;
                    
                    if (line.StartsWith("Wave"))
                    {
                        currentLevel.AddWave(currentWave);  // agrega ola anterior
                        currentWave = new Wave();
                        initializeTimes = false;
                        continue; 
                    }
                    if (line.StartsWith("Level"))
                    {
                        if (currentLevel == null || currentWave == null)
                        {
                            Debug.LogError("ERROR");
                            return false;
                        }
                        //  Cuando se llega al siguiente nivel, se guarda la ultima ola del nivel anterior. No hay error en que la primera ola del nuevo nivel
                        //  guarde una ola vacia ya que la condicion "Ola 1" evita que se llegue a la condicion "Ola" antes de que esa ola se haya inicializado.
                        currentLevel.AddWave(currentWave);
                        levels.Add(currentLevel);
                        currentWave = new Wave();
                        currentLevel = new Level(); // Finaliza nivel
                        currentLevel.ChangeName(line.Split('\n')[0]);
                        initializeTimes = false;    // Luego de "Nivel" empiezan los tiempos de instanciamiento
                        continue; 
                    }   
                    if (line.StartsWith("/"))   //  Despues de este caracter se encuentran los tiempos de instanciamiento.
                    {
                        initializeTimes = true; 
                        continue;
                    }
                    if (initializeTimes)
                    {
                        var data = line.Split(',');
                        float time = float.Parse(data[0]);
                        currentWave.AddTimeInstance(time); 
                    }
                    else
                    {
                        var data = line.Split(',');
                        int quantity = int.Parse(data[0]);
                        string enemy = data[1];
                        currentWave.AddPair(quantity, enemy); 
                    }
                }//while
            }//using
        }
        catch (Exception e)
        {
            Debug.LogError("ERROR LECTURA NIVELES: " + e.Message);
            return false;
        }
        Debug.Log("Lectura Niveles Realizada");
        return true;
    }

    private Stream ConvertTextAssetToStream(TextAsset textAsset)
    {
        // Convertir el contenido del TextAsset a un byte array
        // byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(textAsset.text);

        // Crear un MemoryStream a partir del byte array
        return new MemoryStream(textAsset.bytes);
    }

    private bool PrepareLevels()
    {
        foreach (Level lvl in levels)
        {
            if (!lvl.PrepareLevel(prefabsEnemies))
            {
                Debug.LogError("ERROR ARCHIVO LEIDO. Los nombres de tipos, cantidades o tiempos de instancia pueden ser incorrectos. Revise el archivo .txt de Niveles.");
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

    private void ReConfigure()
    {
        if (currentScene.name == "GameOver" || currentScene.name == "Menu")
        {
            player.life = 10;
            player.money = 200;
            DialogsManager.Instance.RestartDialogs();
            foreach (Level lvl in levels)
            {
                lvl.Restart();
            }
            levelIndex = 0;
            currentGameLevel = levels[levelIndex];
            Debug.Log("REINICIO JUEGO");
            Debug.Log(currentGameLevel.Name + " wave " + (currentGameLevel.GetWaveIndex()+1));
        }
        else
        {
            TextMeshProUGUI[] tmAll;
            tmAll = FindObjectsOfType<TextMeshProUGUI>();
            foreach (TextMeshProUGUI tm in tmAll)
            {
                if (tm.name == "Monedas")
                {
                    txtMeshMoney = tm;
                }
                if (tm.name == "VidaMeta")
                {
                    txtMeshLife = tm;
                }
            }
            target = GameObject.Find("Meta");
            Debug.Log("BUSQUEDA REFERENCIAS");
        }
    }

    public void UpdateCurrentScene() { updateCurrentScene = true; }

    ///////////////////PLAYER///////////////////
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
    ///////////////////PLAYER///////////////////
}
