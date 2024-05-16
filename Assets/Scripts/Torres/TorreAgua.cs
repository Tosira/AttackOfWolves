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
            frecuencia = 1f;
            velocidadBala = 1f;
            radio = 50f;
            damage = 0.5f;
            definirVariables(transform, _prefabBala, frecuencia, velocidadBala, radio, damage);
        }

        // Update is called once per frame
        void Update()
        {
            ejecutar();
        }
    }
}
