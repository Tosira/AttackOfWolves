using Assets.src.Enemigos;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    //  Variables que implican extensibilidad de codigo. Valores distintos dadas las mejoras de torretas. 
    private Transform target;
    private Transform originShot;
    private GameObject prefabBullet;
    public float frequency;
    public float originalFrequency;
    public float bulletSpeed;
    public float radio;
    public float damage;

    private float disMin = 10000000f;

    void FindEnemy()
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(transform.position, radio);
        foreach(Collider2D e in enemigos)
        { 
            //  'objetivo' igual al primer enemigo captado dentro del area.
            if(target == e.transform)
            {
                return;
            }
        }

        target = null;
        disMin = 10000000f;
        foreach(Collider2D e in enemigos)
        {
            //  Codigo extendible para dar prioridad a enemigos. 
            if (e.CompareTag("Enemigo"))
            {
                Enemigo enemigo = e.GetComponent<Enemigo>();
                if (enemigo.esVisible) {

                    float distancia = Vector3.Distance(e.gameObject.transform.position, Meta.insMeta.transform.position);   // ?? 
                    if (distancia < disMin)
                    {
                        //disMin = Vector3.Distance(e.gameObject.transform.position, Meta.insMeta.transform.position);
                        disMin = distancia;

                        //  Se define el objetivo
                        target = e.transform;
                    }

                }
                            
            }
        }
    }

    void Aim()
    {
        if(target == null)
        {
            return;
        }

        Vector3 direccion = (target.position - transform.position).normalized;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(Vector3.forward, direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 3f);

        Shoot();
    }

    void Shoot()
    {
        //  'origenDisparo' no se setea desde esta clase padre en el metodo Start.
        //  Debug.Log("prefabBala: " + prefabBala + "origenDisparo: " + origenDisparo + "objetivo: " + objetivo + "frecuencia: " + frecuencia); 
        if (prefabBullet != null && originShot != null && target != null && frequency <= 0)
        {            
            GameObject bala = Instantiate(prefabBullet, originShot.position, originShot.rotation);
            Bala balaComponente = bala.GetComponent<Bala>();
            if (balaComponente != null)
            {
                balaComponente.SetTarget(target);
                balaComponente.velocidad = bulletSpeed;
                balaComponente.damage = damage;// Posible extensibilidad de codigo
            }
            frequency = originalFrequency;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }

    public void SetTower(Transform origenDisparo, 
                                GameObject prefabBala, 
                                float tipoDisparo, 
                                float velocidadBala,
                                float radio,
                                float damage)
    {

        this.originShot = origenDisparo; 
        this.prefabBullet = prefabBala;  
        this.frequency = tipoDisparo;
        this.originalFrequency = tipoDisparo;
        this.bulletSpeed = velocidadBala;
        this.radio = radio;
        this.damage = damage;
    }
    
    public void Defender()
    {
        FindEnemy();
        Aim();
        frequency -= Time.deltaTime;
    }
}
