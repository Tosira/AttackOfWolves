using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.src.Torres
{
    public class TorreRepeticionMultiple : Torreta
    {
        [SerializeField] private GameObject _prefabBala;
        // private static int precio = 25;

        private void Awake()
        {
            level=1;
            _name = "Torre de Rápida";
            frequency = 0.3f;
            bulletSpeed = 1f;
            radio = 5f;
            damage = 0.45f;
            statistics = new List<Statistics>();
            statistics.Add(new FastTower2());
            statistics.Add(new FastTower3());
            price = 25;
            SetTower(transform, _prefabBala, frequency, bulletSpeed, radio, damage);
        }

        public override int GetPrecio()
        {
            return price; 
        }
    }
}