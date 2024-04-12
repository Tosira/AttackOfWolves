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

    private AudioSource audio_;
    private bool isPlayingSound = false;

    private void Awake()
    {
        // Debug.Log("Sonido");
        audio_ = GetComponent<AudioSource>();
    }

    private IEnumerator SonidoImpacto(float clipLength)
    {
        isPlayingSound = true;
        gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(clipLength);
        Destroy(gameObject);
    }

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
        if (isPlayingSound) return;
        //  Previene referencia a enemigo destruido
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        float t = (Time.time - startTime) * speed;

        //Calcula la posici�n en funci�n del tiempo y la altura del arco
        float x = Mathf.Lerp(startPos.x, target.position.x, t);

        // Por favor, explique que es lo que sucede aqui. 
        float y = startPos.y + (target.position.y - startPos.y) * t - (t * (t - 1)) * alturaArco;

        //Crea el vector de posici�n
        Vector3 newPos = new Vector3(x, y, 0);

        //Mueve la bala hacia la nueva posici�n
        transform.position = newPos;
        float dist = 0.5f;

        Vector2 target2D = new Vector2(target.position.x, target.position.y);
        Vector2 my2D = new Vector2(transform.position.x, transform.position.y);

        //transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad*Time.deltaTime);
        if (Vector2.Distance(target2D, my2D) < dist)
        {
            myTower.GetComponent<Torreta>().ImpactoBala();
            target.gameObject.GetComponent<Enemigo>().GetAttack(damage);
            audio_.Play();
            StartCoroutine(SonidoImpacto(audio_.clip.length));
        }
    }
}