using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.src.Enemigos; 

public class RouteManager : MonoBehaviour
{
    public static RouteManager rm;  

    [SerializeField] private List<GameObject> route1;
    [SerializeField] private List<GameObject> route2;
    [SerializeField] private List<GameObject> route3;
    [SerializeField] private List<GameObject> route4;
     
    [SerializeField] private List<Transform> spawnPoints;    

    private void Start()
    {
        rm = this;                
    }

    public void InstanciateEnemy(GameObject prefabEnemy)
    {
        GameObject gameObjectEnemigo = GameObject.Instantiate(prefabEnemy);
        gameObjectEnemigo.transform.position = spawnPoints[0].position;
        Enemigo enemyComponent = gameObjectEnemigo.GetComponent<Enemigo>();

        if (enemyComponent == null)
        {
            Debug.Log("Lobo nulo");
            return;
        }

        //if (!enemyComponent.CheckAgent())
        //{
        //    Debug.Log("Agente nulo");
        //    return;
        //}

        enemyComponent.SetRoute(route1);        
    }       
}


