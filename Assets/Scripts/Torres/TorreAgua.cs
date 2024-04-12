using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.src.Torres
{
    public class TorreAgua : Torreta
    {
        [SerializeField] private GameObject _prefabBala;
        [SerializeField] private GameObject _prefabArea;
        Bala balaComponente;

        private Transform targetAgua;
        void Awake()
        {
            level=1;
            availableLevel=1;
            _name = "Torre de Agua";
            frequency = 1.2f;
            originalFrequency = frequency;
            bulletSpeed = 1f;
            radio = 3.0f;
            damage = 0.25f;
            price = 30;
            prefabBullet = _prefabBala;

            //  Estadisticas niveles
            statistics = new List<Statistics>();
            statistics.Add(new WaterTower2());
            statistics.Add(new WaterTower3());
        }

        public override void Shoot()
        {
            //  'origenDisparo' no se setea desde esta clase padre en el metodo Start.
            //  Debug.Log("prefabBala: " + prefabBala + "origenDisparo: " + origenDisparo + "objetivo: " + objetivo + "frecuencia: " + frecuencia); 
            if (_prefabBala != null && target != null && frequency <= 0)
            {
                if (targetAgua == target)
                {
                    damage = damage * 1.50f;
                }
                else
                {
                    damage = 0.25f;
                }
                targetAgua = target;

                GameObject bala = Instantiate(_prefabBala, transform.position, transform.rotation);
                balaComponente = bala.GetComponent<Bala>();
                if (balaComponente != null) balaComponente.Initialize(target, gameObject, bulletSpeed, damage);               
                frequency = originalFrequency;

            }
        }

        public override void ImpactoBala()
        {

            Instantiate(_prefabArea, balaComponente.transform.position, balaComponente.transform.rotation);
        }
    }
}