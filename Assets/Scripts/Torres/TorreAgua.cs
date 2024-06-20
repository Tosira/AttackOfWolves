using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.src.Torres
{ 
    public class TorreAgua : Torreta
    {
        [SerializeField] private GameObject _prefabBala;
        private Transform targetAgua;
        void Start()
        {
            frequency = 1f;
            bulletSpeed = 1f;
            radio = 50f;
            damage = 0.25f;
            SetTower(transform, _prefabBala, frequency, bulletSpeed, radio, damage);
        }

        public override void Shoot()
        {
            //  'origenDisparo' no se setea desde esta clase padre en el metodo Start.
            //  Debug.Log("prefabBala: " + prefabBala + "origenDisparo: " + origenDisparo + "objetivo: " + objetivo + "frecuencia: " + frecuencia); 
            if (_prefabBala != null && originShot != null && target != null && frequency <= 0)
            {
                if (targetAgua == target)
                {
                    damage = damage * 2;
                }
                else
                {
                    damage = 0.25f;
                }
                targetAgua = target;

                GameObject bala = Instantiate(_prefabBala, originShot.position, originShot.rotation);
                Bala balaComponente = bala.GetComponent<Bala>();
                if (balaComponente != null)
                {
                    balaComponente.SetTarget(target);
                    balaComponente.velocidad = bulletSpeed;
                    balaComponente.damage = damage;// Posible extensibilidad de codigo
                }
                frequency = originalFrequency;
                
            }
        }

        // Update is called once per frame
        void Update()
        {
            
            Defender();
            
        }
    }
}
