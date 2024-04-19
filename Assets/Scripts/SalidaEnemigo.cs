using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalidaEnemigo : MonoBehaviour
{
    [SerializeField] private GameObject prefabEnemigo;
    [SerializeField] private Transform target;     
    private float[] tiempos = new float[] { 3, 5, 7 };
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
                GameObject gameObjectEnemigo = GameObject.Instantiate(prefabEnemigo);
                gameObjectEnemigo.transform.position = this.transform.position; 
                Enemigo enemigo = gameObjectEnemigo.GetComponent<Enemigo>();
                enemigo.target = target;
                tiempo = -1; 
            }
        }            
    }
}
