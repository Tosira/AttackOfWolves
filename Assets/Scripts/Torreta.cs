using Assets.src.Enemigos;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    public Transform target;
    public Transform originShot;
    private GameObject prefabBullet;
    public float frequency;
    public float originalFrequency;
    public float bulletSpeed;
    public float radio;
    public float damage;
    public SpriteRenderer spriteRend;
    private int precio; 

    private float disMin = 10000000f;    

    public void Update()
    {
        Defend();
    }

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
            if (e.CompareTag("Enemigo"))
            {
                Enemigo enemigo = e.GetComponent<Enemigo>();
                if (enemigo.esVisible && e != null) {

                    float distancia = Vector3.Distance(e.gameObject.transform.position, GameState.target.transform.position);
                    if (distancia < disMin)
                    {
                        //disMin = Vector3.Distance(e.gameObject.transform.position, Meta.insMeta.transform.position);
                        disMin = distancia;                        
                        target = e.transform;
                    }

                }
                            
            }
        }
    }        

    public virtual void Shoot()
    {
        if (!(prefabBullet != null && originShot != null && target != null && frequency <= 0)) return;
        
        GameObject bulletObject = Instantiate(prefabBullet, originShot.position, originShot.rotation);
        Bala bulletComponent = bulletObject.GetComponent<Bala>();
        if (bulletComponent != null) bulletComponent.Initialize(target, gameObject, bulletSpeed, damage);
        frequency = originalFrequency;
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
    
    public void Defend()
    {
        FindEnemy();
        Shoot();
        frequency -= Time.deltaTime;
    }

    public virtual void ImpactoBala() { }  

    public virtual int GetPrecio() { return precio; }
}
