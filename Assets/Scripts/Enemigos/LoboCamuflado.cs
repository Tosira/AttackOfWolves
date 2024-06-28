using System.Collections;
using UnityEngine;

namespace Assets.src.Enemigos
{
    public class LoboCamuflado : Enemigo
    {
        float contador = 8f;
        int cont = 2;
        SpriteRenderer spriteRend;
        public float transparencia = 0.5f;

        public override void SetEnemy()
        {
            vidaActual = vidaMaxima = 3;
            esVisible = false;
            reward = 10; 
        }        

        public override void Attack()
        {
            base.Attack();
            if (cont == 2)
            {
                cont = 1;
                spriteRend = this.GetComponent<SpriteRenderer>();
                Color color = spriteRend.color;
                color.a = transparencia;
                spriteRend.color = color;
            }
            if (!esVisible && contador > 0)
            {
                contador -= Time.deltaTime;
            }
            else
            {
                if (cont == 1)
                {
                    cont = 0;
                    esVisible = true;
                    Color color = spriteRend.color;
                    color.a = 1f;
                    spriteRend.color = color;
                }
            }
        }
    }
}