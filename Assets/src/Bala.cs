using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidad = 1f;
    private Transform objetivo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetObjetivo(Transform _obejtivo)
    {
        objetivo = _obejtivo;
    }

    // Update is called once per frame
    void Update()
    {
        if (objetivo == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad*Time.deltaTime);

        if (Vector3.Distance(transform.position, objetivo.position) < 0.8f)
        {
            Destroy(gameObject);
        }
    }
}
