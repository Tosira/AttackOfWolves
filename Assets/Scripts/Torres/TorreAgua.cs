using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.src.Torres
{ 
    public class TorreAgua : Torreta
    {
        [SerializeField] private GameObject _prefabBala;
        void Start()
        {
            frequency = 1f;
            bulletSpeed = 1f;
            radio = 50f;
            damage = 0.5f;
            SetTower(transform, _prefabBala, frequency, bulletSpeed, radio, damage);
        }

        // Update is called once per frame
        void Update()
        {
            Defender();
        }
    }
}
