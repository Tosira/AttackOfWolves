using System.Collections;
using UnityEngine;

namespace Assets.src.Enemigos
{
    public class LoboGigante : Enemigo
    {
        public override void SetEnemy()
        {
            vidaActual = vidaMaxima = 5;
            esVisible = true;
            reward = 5;
        }

        public override void Atacar()
        {
            base.Atacar();
        }
    }
}
