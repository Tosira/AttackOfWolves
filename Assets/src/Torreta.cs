using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    private float radio = 5f;
    [SerializeField] private GameObject prefabBala;
    private Transform objetivo;
    private Transform puntoDisparo;
    private float tiepoDisparo = 1f;
    private float velBala = 5f;
    private float disMin = 10000000f;
    // Start is called before the first frame update
    void Start()
    {
        puntoDisparo = transform;
    }

    // Update is called once per frame
    void Update()
    {
        buscarEnemigo();
        apuntar();
        tiepoDisparo -= Time.deltaTime;
    }

    void buscarEnemigo()
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(transform.position, radio);
        foreach(Collider2D e in enemigos)
        {
            if(objetivo == e.transform)
            {
                return;
            }
        }
        objetivo = null;
        disMin = 10000000f;
        foreach(Collider2D e in enemigos)
        {
            if (e.CompareTag("Enemigo"))
            {
                if(Vector3.Distance(e.gameObject.transform.position, Meta.insMeta.transform.position)  < disMin)
                {
                    disMin = Vector3.Distance(e.gameObject.transform.position, Meta.insMeta.transform.position);
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
        if (prefabBala != null && puntoDisparo != null && objetivo != null && tiepoDisparo <= 0)
        {
            GameObject bala = Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
            Bala balaComponente = bala.GetComponent<Bala>();
            if (balaComponente != null)
            {
                balaComponente.SetObjetivo(objetivo);
                balaComponente.velocidad = velBala;
            }
            tiepoDisparo = 1f;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }


}
