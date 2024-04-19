using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    int damage = 1; 
    public float velocidad;
    private Transform objetivo;    

    public void SetObjetivo(Transform _obejtivo)
    {
        objetivo = _obejtivo;
    }

    // Update is called once per frame
    void Update()
    {
        //  Previene referencia a enemigo destruido
        if (objetivo == null)
        {
            Destroy(gameObject);
            return;
        }
        
        transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad*Time.deltaTime);

        if (Vector3.Distance(transform.position, objetivo.position) < 1.1f)
        {
            
            objetivo.gameObject.GetComponent<Enemigo>().RecibirAtaque(damage);
            Destroy(gameObject);
        }        
    }
}
