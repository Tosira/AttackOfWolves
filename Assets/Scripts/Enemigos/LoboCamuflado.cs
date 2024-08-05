using System.Collections;
using UnityEngine;

namespace Assets.src.Enemigos
{
    public class LoboCamuflado : Enemigo
    {
        float time = 6f;
        SpriteRenderer spriteRend;
        float transparencia = 0.5f;

        private void Awake()
        {
            vidaActual = vidaMaxima = 3;
            isAttackable = false;
            reward = 10;
            
            spriteRend = gameObject.GetComponent<SpriteRenderer>();
            Color color = spriteRend.color;
            color.a = transparencia;
            spriteRend.color = color;
        }    

        public override void MakeUnattackable()
        {
            if (isAttackable) return;
            
            if (time > 0) time -= Time.deltaTime;
            else
            {
                isAttackable = true;
                Color color = spriteRend.color;
                color.a = 1f;
                spriteRend.color = color;
            }
        }
    }
}