using System.Collections;
using UnityEngine;

namespace Assets.src.Enemigos
{
    public class Lobo : Enemigo
    {
        private void Awake()
        {
            vidaActual = vidaMaxima = 4;
            reward = 5;
        }
    }
}