using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float damage = 1;
    private float alturaArco = 10f;
    public float velocidad;
    private Transform target;

    private Vector3 startPos;
    private float startTime;

    public void SetTarget(Transform target)
    {
        this.target = target;
        startPos = transform.position;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //  Previene referencia a enemigo destruido
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        float t = (Time.time - startTime) * velocidad;

        //Calcula la posición en función del tiempo y la altura del arco
        float x = Mathf.Lerp(startPos.x, target.position.x, t);

        // Por favor, explique que es lo que sucede aqui. 
        float y = startPos.y + (target.position.y - startPos.y) * t - (t * (t - 1)) * alturaArco;
        
        //Crea el vector de posición
        Vector3 newPos = new Vector3(x, y, 0);

        //Mueve la bala hacia la nueva posición
        transform.position = newPos;

        //transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad*Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 1.1f)
        {
            
            target.gameObject.GetComponent<Enemigo>().GetAttack(damage);
            Destroy(gameObject);
        }        
    }
}
