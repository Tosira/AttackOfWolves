using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.src.Enemigos; 

public class RouteManager : MonoBehaviour
{   
    [SerializeField] private List<GameObject> route1;
    [SerializeField] private List<GameObject> route2;
    [SerializeField] private List<GameObject> route3;
    [SerializeField] private List<GameObject> route4;

    [SerializeField] private List<GameObject> prefabsEnemy; 
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<int> enemyQuantity;
    [SerializeField] private List<float> enemyInstanceTime;
    private bool withoutEnemies = false;
    private bool endTimes = false; 

    private float[] tiempos = new float[] { 2, 4, 5 };
    private float tiempo = -1;

    private void Start()
    {
        if (enemyInstanceTime.Count != enemyQuantity.Count || prefabsEnemy.Count != enemyQuantity.Count)
        {
            Debug.Log("Listas enemyInstanceTime, enemyQuantity y prefabsEnemy no concuerdan. Deben tener misma cantidad de elementos."); 
            withoutEnemies = true; 
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (withoutEnemies) return;

        UpdateTime(); 
        if (tiempo == -1)
        {
            int indiceTiempo = Random.Range(0, 3);
            tiempo = tiempos[indiceTiempo];
            //Debug.Log("indice: " + indiceTiempo + ", timepo: " + tiempo);  
        }
        else
        {
            tiempo -= Time.deltaTime;
            if (tiempo <= 0)
            {
                InstanciateEnemy(); 
            }
        }
    }

    private void InstanciateEnemy()
    {
        for (int i = 0; i < enemyInstanceTime.Count; ++i)
        {
            if (enemyInstanceTime[i] <= 0 && enemyQuantity[i] > 0 && prefabsEnemy[i] != null) 
            {
                GameObject gameObjectEnemigo = GameObject.Instantiate(prefabsEnemy[i]);
                gameObjectEnemigo.transform.position = spawnPoints[0].position;                
                Enemigo enemy = gameObjectEnemigo.GetComponent<Enemigo>();

                if (enemy == null)
                {
                    Debug.Log("Lobo nulo");
                    tiempo = -1;
                    return;
                }

                if (!enemy.CheckAgent())
                {
                    Debug.Log("Agente nulo");
                    tiempo = -1;
                    return;
                }

                enemy.SetRoute(route1);
                enemyQuantity[i] -= 1;
                if (enemyQuantity[i] <= 0) CheckEnemyQuantity(); 
            }
        }                
        tiempo = -1; 
    }

    private void UpdateTime()
    {
        if (endTimes) return; 

        float time = 0; 
        for (int i = 0; i < enemyInstanceTime.Count; ++i)
        {            
            time += (enemyInstanceTime[i] -= Time.deltaTime); 
        }

        if (time <= 0) endTimes = true; 
    }
    
    private void CheckEnemyQuantity()
    {        
        foreach (int quantity in enemyQuantity)
        {
            if (quantity >= 0)
            {
                Debug.Log("WithoutEnemies");
                withoutEnemies = false;
            } 
        }

        withoutEnemies = true; 
    }

    public bool WithoutEnemies()
    {
        return withoutEnemies; 
    }

    private void SetEnemyInstanceTime(float[] _enemyInstanceTime)
    {
        if (!endTimes)
        {
            Debug.Log("Tiempos no finalizados"); 
            return; 
        }

        for (int i = 0; i < _enemyInstanceTime.Length; ++i)
        {
            enemyInstanceTime[i] = _enemyInstanceTime[i]; 
        }
    }
    private void SetEnemyQuantity(int[] _enemyQuantity)
    {
        for (int i = 0; i < _enemyQuantity.Length; ++i)
        {
            enemyQuantity[i] = _enemyQuantity[i];
        }
    }

    public void SetInstanceVariables(float[] _enemyInstanceTime, int[] _enemyQuantity)
    {
        SetEnemyInstanceTime(_enemyInstanceTime);
        SetEnemyQuantity(_enemyQuantity);

        withoutEnemies = false;
        endTimes = false; 
    }

}


