using System.Collections;
using UnityEngine;

namespace Assets.src.Torres
{
    public class TorrePiedra : Torreta
    {
        [SerializeField] private GameObject _prefabBala;                  

        private void Start()
        {
            frequency = 1f;
            bulletSpeed = 1f;
            radio = 8f;
            damage = 1f;
            SetTower(transform, _prefabBala, frequency, bulletSpeed, radio, damage);
        }

        private void Update()
        {
            Defender(); 
        }
    }
}