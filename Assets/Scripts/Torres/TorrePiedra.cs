using System.Collections;
using UnityEngine;

namespace Assets.src.Torres
{
    public class TorrePiedra : Torreta
    {
        [SerializeField] private GameObject _prefabBala;
        private static int precio = 15;

        private void Start()
        {
            frequency = 1f;
            bulletSpeed = 1f;
            radio = 6f;
            damage = 1f;
            SetTower(transform, _prefabBala, frequency, bulletSpeed, radio, damage);
        }

        public override int GetPrecio()
        {
            return precio;
        }
    }
}