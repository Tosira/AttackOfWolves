using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    [SerializeField] private Image barImage;

    public void actualizarBarraVida(float vidaMaxima, float vidaActual)
    {
        barImage.fillAmount = vidaActual / vidaMaxima;
    }
}
