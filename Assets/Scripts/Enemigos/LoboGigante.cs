using System.Collections;
using UnityEngine;

namespace Assets.src.Enemigos
{
    public class LoboGigante : Enemigo
    {
        private void Awake()
        {
            vidaActual = vidaMaxima = 7;
            reward = 5;
        }
    }
}
