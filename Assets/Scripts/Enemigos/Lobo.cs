using System.Collections;
using UnityEngine;

namespace Assets.src.Enemigos
{
    public class Lobo : Enemigo
    {
        // Use this for initialization
        void Start()
        {
            SetAgent(); 
            vidaActual = vidaMaxima = 5;            
        }

        private void Update()
        {
            moverAgenteNavMesh(); 
        }

        public override void Atacar()
        {
            base.Atacar();
        }
    }
}