using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    private float radio = 10f;
    [SerializeField] private GameObject prefabBala;
    private Transform objetivo;
    private Transform puntoDisparo;
    private float tiepoDisparo = 3f;
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
            Debug.Log("Entra foreach");
            if (e.CompareTag("Enemigo"))
            {
                Debug.Log("Detecta objetivo");
                objetivo = e.transform;
                return;
            }
        }
        objetivo = null;
    }

    void apuntar()
    {
        if(objetivo == null)
        {
            return;
        }
        Vector3 direccion = (objetivo.position - transform.position).normalized;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(Vector3.forward, direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 2);

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
            }
            tiepoDisparo = 3f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }

}
