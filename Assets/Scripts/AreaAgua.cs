using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAgua : MonoBehaviour
{
    // Start is called before the first frame update
    private float radio;
    private float tick = 0.0f;
    private float tick2 = 2.0f;
    void Start()
    {
        radio = transform.localScale.x/2;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(transform.position, radio);
        if(tick2 <= 0.0f)
        {
            Destroy(gameObject);
        }
        if(tick > 0.0f)
        {
            
        }
        else
        {
            tick= 1.0f;
            foreach (Collider2D e in enemigos)
            {
                Enemigo enemigo = e.GetComponent<Enemigo>();
                if (enemigo != null)
                {
                    enemigo.GetAttack(0.5f);
                }
            }
        }

        

        tick -= Time.deltaTime;
        tick2 -= Time.deltaTime;
    }
}
