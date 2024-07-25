using Assets.src.Torres;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        float dist = 0.5f;

        Vector2 target2D = new Vector2(target.position.x, target.position.y);
        Vector2 my2D = new Vector2(transform.position.x, transform.position.y);

        //transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad*Time.deltaTime);
        if (Vector2.Distance(target2D, my2D) < dist)
        {
            myTower.GetComponent<Torreta>().ImpactoBala();
            target.gameObject.GetComponent<Enemigo>().GetAttack(damage);
            Destroy(gameObject);
        }
    }
}