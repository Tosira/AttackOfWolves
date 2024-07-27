using System.Collections;
using UnityEngine;

namespace Assets.src.Enemigos
{
    public class Duque : Enemigo
    {
        public override void SetEnemy()
        {
            vidaActual = vidaMaxima = 50;
            esVisible = true;
            reward = 5;
            speed = 1.0f;
        }        
    }
}