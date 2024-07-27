using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Barricada : MonoBehaviour
{
    private bool colocar = true;
    private float rotacion = 100f;
    private float duracion = 10f;
    private Collider2D col;
    private List<Enemigo> enemigos0;
    private Vector2 tamanio;
    private List<float> velocidades;

    private void Start()
    {
        velocidades = new List<float>();
        enemigos0 = new List<Enemigo>();
        col = GetComponent<Collider2D>();
        tamanio = col.bounds.size;
        tamanio.x *= 0.88f;
        tamanio.y *= 0.65f;
    }

    void Update()
    {

        Debug.Log(enemigos0.Count);
        foreach (Enemigo enemigo in enemigos0)
        {
            
            enemigo.speed = velocidades[enemigos0.IndexOf(enemigo)];
        }

        //col = GetComponent<Collider2D>();
        if (col != null )
        {
            
            float angulo = transform.eulerAngles.z;

            Collider2D[] enemigos = Physics2D.OverlapBoxAll(transform.position, tamanio, angulo);
            foreach(Collider2D e in enemigos )
            {
                if (e.CompareTag("Enemigo"))
                {
                    Enemigo enemigo = e.GetComponent<Enemigo>();
                    if(enemigo.speed != 0f)
                    {

                        
                        if (!enemigos0.Contains(enemigo))
                        {
                            velocidades.Add(enemigo.speed);
                            enemigos0.Add(enemigo);
                        }
                        enemigo.speed = 0f;
                    }
                    
                }
            }
        }
        


        if ( colocar)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                transform.Rotate(Vector3.forward, scroll * rotacion);
            }
        }
        if (Input.GetMouseButtonDown(0) && colocar)
        {
            dejar();
        }

        duracion -= Time.deltaTime;
        if (duracion <= 0f)
        {
            foreach (Enemigo enemigo in enemigos0)
            {
                enemigo.speed = 2f;
            }
            Destroy(gameObject);
        }


    }
    
    private void dejar()
    {
        if (BtnBarricada.Instance.enPausa == false) return;
        colocar = false;
        BtnBarricada.Instance.cambiarPausa();
    }


}
