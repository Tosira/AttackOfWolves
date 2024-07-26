using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.src.Torres
{
    public class TorrePiedra : Torreta
    {
        [SerializeField] private GameObject _prefabBala;
        // private static int precio = 15;

        private void Awake()
        {
            level=1;
            availableLevel = 1;
            _name = "Torre de Piedra";
            frequency = 1f;
            bulletSpeed = 1f;
            radio = 6f;
            damage = 1f;
            price = 15;
            statistics = new List<Statistics>();
            statistics.Add(new StoneTower2());
            statistics.Add(new StoneTower3());
            SetTower(transform, _prefabBala, frequency, bulletSpeed, radio, damage);
        }

        public override int GetPrecio()
        {
            return price;
        }
    }
}