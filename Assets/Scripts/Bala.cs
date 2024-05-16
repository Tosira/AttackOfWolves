using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float damage = 1;
    private float alturaArco = 10f;
    public float velocidad;
    private Transform objetivo;

    private Vector3 startPos;
    private float startTime;

    public void SetObjetivo(Transform _obejtivo)
    {
        objetivo = _obejtivo;
        startPos = transform.position;
        startTime = Time.time;
    }

    private void Start()
    {;
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

        float t = (Time.time - startTime) * velocidad;

        //Calcula la posición en función del tiempo y la altura del arco
        float x = Mathf.Lerp(startPos.x, objetivo.position.x, t);

        // Por favor, explique que es lo que sucede aqui. 
        float y = startPos.y + (objetivo.position.y - startPos.y) * t - (t * (t - 1)) * alturaArco;
        
        //Crea el vector de posición
        Vector3 newPos = new Vector3(x, y, 0);

        //Mueve la bala hacia la nueva posición
        transform.position = newPos;

        //transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad*Time.deltaTime);

        if (Vector3.Distance(transform.position, objetivo.position) < 1.1f)
        {
            
            objetivo.gameObject.GetComponent<Enemigo>().RecibirAtaque(damage);
            Destroy(gameObject);
        }        
    }
}
