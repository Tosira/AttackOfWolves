using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TorreFinal : MonoBehaviour
{

    public Transform target;
    private float disMin = 10000000f;
    public Transform stretchObject;

    void Update()
    {
        FindEnemy();
        Shoot();
        Attack();
        if (target == null)
        {
            stretchObject.localScale = new Vector3(0, stretchObject.localScale.y, stretchObject.localScale.z);
        }
    }

    void Attack()
    {
        if (target == null) return;

        if (target.CompareTag("Enemigo")){
            target.gameObject.GetComponent<Enemigo>().GetAttack(0.01f);
        }
    }

    void FindEnemy()
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(transform.position, 1000000);
        foreach (Collider2D e in enemigos)
        {
            //  'objetivo' igual al primer enemigo captado dentro del area.
            if (target == e.transform)
            {
                return;
            }
        }

        target = null;
        disMin = 10000000f;
        foreach (Collider2D e in enemigos)
        {
            if (e.CompareTag("Enemigo"))
            {
                Enemigo enemigo = e.GetComponent<Enemigo>();
                if (enemigo.IsAttackable && e != null)
                {

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

    void Shoot()
    {
        if (target != null && stretchObject != null)
        {
            // Obtener el vector desde el objeto actual al objetivo
            Vector3 directionToTarget = target.position - transform.position;

            // Calcular la distancia al objetivo
            float distance = directionToTarget.magnitude*0.85f;

            // Calcular el �ngulo de rotaci�n hacia el objetivo en 2D
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Asignar la rotaci�n al objeto que se va a estirar
            stretchObject.rotation = Quaternion.Euler(0, 0, angle);

            // Ajustar la escala del objeto estirado (asumiendo que se estira en el eje X)
            stretchObject.localScale = new Vector3(distance, stretchObject.localScale.y, stretchObject.localScale.z);

            // Ajustar la posici�n del objeto estirado para que comience en el objeto original y termine en el objetivo
            stretchObject.position = (transform.position + target.position) / 2;
        }
    }
}
