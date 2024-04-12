using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.src.Enemigos;
using UnityEngine.SceneManagement;

public class RouteManager : MonoBehaviour
{
    public static RouteManager rm;  

    [SerializeField] private List<GameObject> path1;
    [SerializeField] private List<GameObject> path2;
    [SerializeField] private List<GameObject> path3;
    [SerializeField] private List<GameObject> path4;
    List<List<GameObject>> paths;

     
    [SerializeField] private List<Transform> spawnPoints;

    private Scene currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        rm = this;
        if(currentScene.name == "Level1")
        {
            paths = new List<List<GameObject>>() { path1, path2 };
        }

        if (currentScene.name == "Level2")
        {
            paths = new List<List<GameObject>>() { path1, path2, path3, path4 };
        }

        if(currentScene.name == "Level3")
        {
            paths = new List<List<GameObject>>() { path1, path2, path3, path4 };
        }

    }

    public void InstanciateEnemy(GameObject prefabEnemy)
    {
        GameObject gameObjectEnemigo = GameObject.Instantiate(prefabEnemy);
        int spawnRandom = Random.Range(0, spawnPoints.Count);
        gameObjectEnemigo.transform.position = spawnPoints[spawnRandom].position;
        Enemigo enemyComponent = gameObjectEnemigo.GetComponent<Enemigo>();
        if (enemyComponent == null)
        {
            Debug.Log("Lobo nulo");
            return;
        }
        List<GameObject> definedPath;
        definedPath = new List<GameObject>();
        if (currentScene.name == "Level1")
        {
            definedPath = paths[Random.Range(0, paths.Count)];
        }else if(currentScene.name == "Level2"){
            if(spawnRandom == 0)
            {
                definedPath = paths[Random.Range(0,2)];
            }
            else
            {
                definedPath = paths[Random.Range(2,4)];
            }
            
        }else if (currentScene.name == "Level3")
        {
            definedPath = paths[spawnRandom];
        }
        
        enemyComponent.SetRoute(definedPath);
    }       
}


