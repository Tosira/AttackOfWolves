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

    [SerializeField] private GameObject prefabEnemigo;
    [SerializeField] private Transform target;     
    private float[] tiempos = new float[] { 2, 4, 5 };
    private float tiempo = -1;    

    // Update is called once per frame
    void Update()
    {
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
        if (prefabEnemigo == null)
        {
            Debug.Log("Prefab nullo");
            tiempo = -1;
            return;
        }

        if (target == null)
        {
            Debug.Log("El target es nulo");
            tiempo = -1;
            return;
        }

        GameObject gameObjectEnemigo = GameObject.Instantiate(prefabEnemigo);
        gameObjectEnemigo.transform.position = this.transform.position;

        // Enemigo especifico: Lobo normal
        Enemigo enemy = gameObjectEnemigo.GetComponent<Lobo>();

        if (enemy == null)
        {
            Debug.Log("Lobo nulo");
            tiempo = -1;
            return;
        }

        if (!enemy.CheckAgent())
        {
            Debug.Log("Agnete nulo");
            tiempo = -1;
            return;
        }

        enemy.SetRoute(route1);
        tiempo = -1; 
    }          
}


