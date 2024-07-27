using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Wave
{
    List<float> timesInstance;
    List<(int Quantity, string Enemy)> pairs;    
    float currentInstanceTime;  // Evita sobreescritura al contar tiempo.
    int currentEnemyQuantity;
    int enemyIndex = 0, timeIndex = 0;

    public Wave()
    {
        timesInstance = new List<float>();
        pairs = new List<(int Quantity, string Enemy)>(); 
    }

    public void AddTimeInstance(float time)
    {
        timesInstance.Add(time);
    }

    public void AddPair(int quantity, string enemy)
    {
        (int Quantity, string Enemy) pair = (Quantity: quantity, Enemy: enemy); 
        pairs.Add(pair); 
    }    

    // Test function
    public List<float> GetTimeInstance()
    {
        return timesInstance;
    }

    // Test function
    public List<(int Quantity, string Enemy)> GetPairs()
    {
        return pairs; 
    }

    //  Este metodo debe ser llamado luego de la lectura y antes que la actualizacion y obtencion de datos.
    public void PrepareWave()
    {
        currentInstanceTime = timesInstance[0];
        currentEnemyQuantity = pairs[0].Quantity;
    }

    public bool CheckPairs(List<GameObject> prefabsEnemy)
    {
        string enemyType;
        string lastEnemyType = prefabsEnemy[prefabsEnemy.Count - 1].GetComponent<Enemigo>().GetType().Name;
        foreach ((int Quantity, string Enemy) pair in pairs)
        {
            if (pair.Quantity <= 0)
            {
                Debug.LogError("Cantidad de Enemigo Invalida");
                return false;
            }
            foreach (GameObject gm in prefabsEnemy)
            {
                enemyType = pair.Enemy;
                string gmEnemyType = gm.GetComponent<Enemigo>().GetType().Name;
                if (enemyType != gmEnemyType && gmEnemyType == lastEnemyType) // Se ha llegado al ultimo prefab y enemyType aun no se encuentra
                {
                    Debug.LogError("Tipo de Enemigo no encontrado");
                    return false;
                }
                if (enemyType == gmEnemyType) break;
            }
        }        
        return true;
    }

    public bool CheckInstanceTime()
    {
        foreach (float time in timesInstance)
        {
            if (time < 0) // Se admite tiempos iguales que cero
            {
                Debug.LogError("Tiempo de Instancia Invalido");
                return false;
            }
        }
        return true;
    }

    public bool ReduceEnemies() // Retorna verdadero tras reducir un enemigo.
    {
        if (currentEnemyQuantity <= 0) return false;
        currentEnemyQuantity--;
        return true;
    }

    public bool ReduceInstanceTime() // Retorna verdadero tras reducir el tiempo de aparicion del enemigo.
    {
        if (currentInstanceTime <= 0) return false;
        currentInstanceTime -= Time.deltaTime;
        return true;
    }

    public float GetCurrentInstanceTime()
    {
        return currentInstanceTime;
    }

    public string GetCurrentEnemyType()
    {        
        return pairs[enemyIndex].Enemy;
    }

    public void NextInstanceTime()
    {
        if (timeIndex >= timesInstance.Count-1) return;
        timeIndex++;
        currentInstanceTime = timesInstance[timeIndex];
    }

    public void NextPairEnemyQuantity()
    {        
        if (enemyIndex >= pairs.Count-1) return;
        enemyIndex++;
        currentEnemyQuantity = pairs[enemyIndex].Quantity;
    }

    public bool WithoutEnemies()
    {
        return currentEnemyQuantity <= 0
               && currentInstanceTime <= 0
               && enemyIndex >= pairs.Count - 1
               && timeIndex >= timesInstance.Count - 1;
    }

    public bool WaveNotStarted()
    {
        // Los indices no son los primeros en variar al ejecutar las olas, estos varian solo
        // cuando los datos actuales(current) han sido sido agotados. Por eso se comprueba con
        // los datos actuales.
        return currentEnemyQuantity == pairs[enemyIndex].Quantity
               && currentInstanceTime == timesInstance[timeIndex]
               && enemyIndex == 0
               && timeIndex == 0;
    }
}// Wave



class Level
{
    private List<Wave> waves;
    Wave currentWave;
    List<GameObject> referencesPrefabEnemies;
    int waveIndex = 0;
    bool readyLevel = false;

    public Level()
    {
        waves = new List<Wave>();
    }

    //  Este metodo debe ser llamado luego de la lectura y antes que la actualizacion y obtencion de datos.
    public bool PrepareLevel(List<GameObject> prefabsEnemies)
    {
        if (!CheckWaves(prefabsEnemies)) return false;
        referencesPrefabEnemies = prefabsEnemies.ToList();
        currentWave = waves[waveIndex];
        currentWave.PrepareWave();
        readyLevel = true;
        return true;
    }
    
    private bool CheckWaves(List<GameObject> prefabsEnemies)
    {
        if (waves.Count == 0) return false;
        foreach (Wave wv in waves)
        {
            if (!wv.CheckInstanceTime() || !wv.CheckPairs(prefabsEnemies)) return false;            
        }
        return true;
    }

    public void AddWave(Wave _wave)
    {
        waves.Add(_wave);
    }

    //  Test function
    public List<Wave> GetWaves()
    {
        return waves;
    }

    public void UpdateLevel()
    {
        if (!readyLevel) return;
        if (currentWave.WithoutEnemies())
        {
            ReduceWaves();
            return;
        }        
        if (currentWave.ReduceInstanceTime()) /*Si es falso, no se ha podido reducir el tiempo y se debe instanciar el enemigo*/
        {
            //Debug.Log("Tiempo reduciendose");
            return;
        }
        if (!currentWave.ReduceEnemies())
        {
            //  Cantidad de enemigos agotada
            currentWave.NextInstanceTime();
            currentWave.NextPairEnemyQuantity();
            //  Luego de la actualizacion no se debe crear ningun enemigo. De hecho, una actualizacion
            //  no asegura que haya enemigos disponibles en los siguientes datos, por lo que podria terminar 
            //  retornando "-" de forma indefinida.
            Debug.Log("Siguientes enemigos de la Ola");
            return;
        }

        //Debug.Log("PUNTO INSTANCIAMIENTO");
        //Debug.Log(currentWave.GetCurrentEnemyType());
        foreach (GameObject prefabEnemy in referencesPrefabEnemies)
        {
            Enemigo enemyComponent = prefabEnemy.GetComponent<Enemigo>();
            if (enemyComponent.GetType().Name == currentWave.GetCurrentEnemyType())
            {
                Debug.Log(currentWave.GetCurrentEnemyType());
                RouteManager.rm.InstanciateEnemy(prefabEnemy);
                return;
            }
        }        
    }

    private void ReduceWaves()
    {
        if (!CurrentWaveWithoutEnemies()) return;   // La ola tiene enemigos
        if (waveIndex >= waves.Count-1) return;
        waveIndex++; 
        currentWave = waves[waveIndex];
        currentWave.PrepareWave();
        Debug.Log("Actualizacion de Ola");
    }

    public int GetWaveIndex()
    {
        return waveIndex;
    }  

    // Inidica si el nivel no ha iniciado ni una de sus olas
    public bool NotStarted()
    {
        return waveIndex == 0
               && currentWave.WaveNotStarted();
    }

    public bool CurrentWaveNotStarted()
    {
        return currentWave.WaveNotStarted();
    }

    public bool CurrentWaveWithoutEnemies()
    {
        return currentWave.WithoutEnemies();
    }

    // Indica si el nivel ha terminado
    public bool WithoutWaves()
    {
        return waveIndex >= waves.Count - 1 && currentWave.WithoutEnemies();
    }

    public bool PrioritizeTimeInstance()
    {
        return currentWave.GetCurrentInstanceTime() > 0;
    }
}// Level
