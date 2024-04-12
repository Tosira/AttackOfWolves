using System.Collections;
using UnityEngine;

namespace Assets.src.Enemigos
{
    public class Duque : Enemigo
    {
        private void Awake()
        {
            vidaActual = vidaMaxima = 50;
            reward = 5;
            speed = 1.0f;
        }      
    }
}