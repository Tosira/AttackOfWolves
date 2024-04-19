using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    //  Variables que implican extensibilidad de codigo. Valores distintos dadas las mejoras de torretas. 
    private Transform objetivo;
    private Transform origenDisparo;
    private GameObject prefabBala;
    public float frecuencia;
    public float frecuenciaOriginal;
    public float velocidadBala;
    public float radio; 

    private float disMin = 10000000f;

    void buscarEnemigo()
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(transform.position, radio);
        foreach(Collider2D e in enemigos)
        { 
            //  'objetivo' igual al primer enemigo captado dentro del area.
            if(objetivo == e.transform)
            {
                return;
            }
        }

        objetivo = null;
        disMin = 10000000f;
        foreach(Collider2D e in enemigos)
        {
            //  Codigo extendible para dar prioridad a enemigos. 
            if (e.CompareTag("Enemigo"))
            {
                float distancia = Vector3.Distance(e.gameObject.transform.position, Meta.insMeta.transform.position);   // ?? 
                if (distancia < disMin)
                {
                    //disMin = Vector3.Distance(e.gameObject.transform.position, Meta.insMeta.transform.position);
                    disMin = distancia; 

                    //  Se define el objetivo
                    objetivo = e.transform;
                }                
            }
        }
    }

    void apuntar()
    {
        if(objetivo == null)
        {
            return;
        }

        Vector3 direccion = (objetivo.position - transform.position).normalized;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(Vector3.forward, direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 3f);

        disparar();
    }

    void disparar()
    {
        //  'origenDisparo' no se setea desde esta clase padre en el metodo Start.
        //  Debug.Log("prefabBala: " + prefabBala + "origenDisparo: " + origenDisparo + "objetivo: " + objetivo + "frecuencia: " + frecuencia); 
        if (prefabBala != null && origenDisparo != null && objetivo != null && frecuencia <= 0)
        {            
            GameObject bala = Instantiate(prefabBala, origenDisparo.position, origenDisparo.rotation);
            Bala balaComponente = bala.GetComponent<Bala>();
            if (balaComponente != null)
            {
                balaComponente.SetObjetivo(objetivo);
                balaComponente.velocidad = velocidadBala;     // Posible extensibilidad de codigo
            }
            frecuencia = frecuenciaOriginal;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }

    //  Mejorar nombre funcion
    public void definirVariables(Transform origenDisparo, 
                                GameObject prefabBala, 
                                float tipoDisparo, 
                                float velocidadBala,
                                float radio)
    {

        this.origenDisparo = origenDisparo; 
        this.prefabBala = prefabBala;  
        this.frecuencia = tipoDisparo;
        this.frecuenciaOriginal = tipoDisparo;
        this.velocidadBala = velocidadBala;
        this.radio = radio; 
    }

    //  Cambiar nombre funcion
    public void ejecutar()
    {
        buscarEnemigo();
        apuntar();
        frecuencia -= Time.deltaTime;
    }
}
