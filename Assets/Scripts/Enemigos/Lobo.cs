﻿using System.Collections;
using UnityEngine;

namespace Assets.src.Enemigos
{
    public class Lobo : Enemigo
    {
        public override void SetEnemy()
        {
            vidaActual = vidaMaxima = 4;
            esVisible = true;
            reward = 5; 
        }        
    }
}