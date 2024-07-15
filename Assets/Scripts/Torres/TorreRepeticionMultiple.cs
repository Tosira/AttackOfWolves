using System.Collections;
using UnityEngine;

namespace Assets.src.Torres
{
    public class TorreRepeticionMultiple : Torreta
    {
        [SerializeField] private GameObject _prefabBala;
        private static int precio = 25;

        private void Start()
        {
            frequency = 0.3f;
            bulletSpeed = 1f;
            radio = 5f;
            damage = 0.45f;
            SetTower(transform, _prefabBala, frequency, bulletSpeed, radio, damage);
        }

        public override int GetPrecio()
        {
            return precio; 
        }
    }
}