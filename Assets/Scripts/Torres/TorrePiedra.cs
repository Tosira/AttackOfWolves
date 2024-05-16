using System.Collections;
using UnityEngine;

namespace Assets.src.Torres
{
    public class TorrePiedra : Torreta
    {
        [SerializeField] private GameObject _prefabBala;                  

        private void Start()
        {
            frecuencia = 1f;
            velocidadBala = 1f;
            radio = 8f;
            damage = 1f;
            definirVariables(transform, _prefabBala, frecuencia, velocidadBala, radio, damage);
        }

        private void Update()
        {
            ejecutar(); 
        }
    }
}