using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enfriamiento : MonoBehaviour
{
    public static Enfriamiento instancia;
    public Image circulo;
    public float duracion = 10f;

    public float circuloTiempo = 0f;

    private void Start()
    {
        instancia = this;
    }

    void Update()
    {
        if (circuloTiempo < duracion)
        {
            circuloTiempo += Time.deltaTime;
            float fillAmount = 1 - (circuloTiempo / duracion);
            circulo.fillAmount = fillAmount;
        }
        else
        {
            circulo.fillAmount = 0;
        }
    }
}
