using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.src.Enemigos;

public class RouteManager : MonoBehaviour
{
    public static RouteManager rm;  

    [SerializeField] private List<GameObject> path1;
    [SerializeField] private List<GameObject> path2;
    [SerializeField] private List<GameObject> path3;
    [SerializeField] private List<GameObject> path4;
    List<List<GameObject>> paths;

     
    [SerializeField] private List<Transform> spawnPoints;    

    private void Start()
    {
        rm = this;
        paths = new List<List<GameObject>>() {path1,path2};       
    }

    public void InstanciateEnemy(GameObject prefabEnemy)
    {
        GameObject gameObjectEnemigo = GameObject.Instantiate(prefabEnemy);
        gameObjectEnemigo.transform.position = spawnPoints[Random.Range(0,spawnPoints.Count)].position;
        Enemigo enemyComponent = gameObjectEnemigo.GetComponent<Enemigo>();
        if (enemyComponent == null)
        {
            Debug.Log("Lobo nulo");
            return;
        }
        List<GameObject> definedPath = paths[Random.Range(0,paths.Count)];
        enemyComponent.SetRoute(definedPath);
    }       
}


