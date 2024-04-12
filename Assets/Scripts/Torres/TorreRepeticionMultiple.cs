using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.src.Torres
{
    public class TorreRepeticionMultiple : Torreta
    {
        [SerializeField] private GameObject _prefabBala;

        private void Awake()
        {
            level=1;
            availableLevel = 1;
            _name = "Torre de Rápida";
            frequency = 0.3f;
            originalFrequency = frequency;
            bulletSpeed = 1f;
            radio = 5f;
            damage = 0.45f;
            prefabBullet = _prefabBala;

            // Estadisticas niveles
            statistics = new List<Statistics>();
            statistics.Add(new FastTower2());
            statistics.Add(new FastTower3());
            price = 25;
        }
    }
}