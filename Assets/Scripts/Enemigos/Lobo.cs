using System.Collections;
using UnityEngine;

namespace Assets.src.Enemigos
{
    public class Lobo : Enemigo
    {
        public override void SetEnemy()
        {
            vidaActual = vidaMaxima = 5;
            esVisible = true;
        }

        public override void Atacar()
        {
            base.Atacar();
        }
    }
}