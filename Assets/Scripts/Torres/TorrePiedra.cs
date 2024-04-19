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
            velocidadBala = 5f;
            radio = 5f; 

            definirVariables(transform, _prefabBala, frecuencia, velocidadBala, radio);        
        }

        private void Update()
        {
            ejecutar(); 
        }
    }
}