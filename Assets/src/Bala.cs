using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidad = 5f;
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

        Vector3 direccion = (objetivo.position - transform.position).normalized;
        transform.Translate(direccion * velocidad * Time.deltaTime);

        if (Vector3.Distance(transform.position, objetivo.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
