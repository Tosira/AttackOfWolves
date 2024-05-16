using System.Collections;
using UnityEngine;

namespace Assets.src.Torres
{
    public class TorreBarro : Torreta
    {
        [SerializeField] private GameObject _prefabBala;        

        // Use this for initialization
        void Start()
        {
            frequency = 1f;
            bulletSpeed = 1.5f;
            radio = 5f;
            damage = 1f;
            SetTower(transform, _prefabBala, frequency, bulletSpeed, radio, damage);
        }

        // Update is called once per frame
        void Update()
        {
            Defender();
        }
    }
}