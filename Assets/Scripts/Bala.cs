using Assets.src.Torres;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float damage = 1;
    private float alturaArco = 10f;
    public float speed;
    private Transform target;

    private Vector3 startPos;
    private float startTime;

    public GameObject myTower;

    public void Initialize(Transform target, GameObject tower, float speed, float damage)
    {
        this.target = target;
        startPos = transform.position;
        startTime = Time.time;

        myTower = tower;
        this.speed = speed;
        this.damage = damage;
    }

    void FixedUpdate()
    {
        //  Previene referencia a enemigo destruido
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        float t = (Time.time - startTime) * speed;

        //Calcula la posición en función del tiempo y la altura del arco
        float x = Mathf.Lerp(startPos.x, target.position.x, t);

        // Por favor, explique que es lo que sucede aqui. 
        float y = startPos.y + (target.position.y - startPos.y) * t - (t * (t - 1)) * alturaArco;

        //Crea el vector de posición
        Vector3 newPos = new Vector3(x, y, 0);

        //Mueve la bala hacia la nueva posición
        transform.position = newPos;
        float dist = 1.1f;
        //transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad*Time.deltaTime);
        
        if (Vector3.Distance(transform.position, target.position) < dist)
        {
            myTower.GetComponent<Torreta>().ImpactoBala();
            target.gameObject.GetComponent<Enemigo>().GetAttack(damage);
            Destroy(gameObject);
        }
    }
}