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
            frecuencia = 5f;
            velocidadBala = 20f;
            radio = 50f;

            definirVariables(transform, _prefabBala, frecuencia, velocidadBala, radio);
        }

        // Update is called once per frame
        void Update()
        {
            ejecutar();
        }
    }
}
