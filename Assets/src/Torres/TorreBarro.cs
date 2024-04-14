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
            frecuencia = 1.5f;
            velocidadBala = 10f;
            radio = 3f; 

            definirVariables(transform, _prefabBala, frecuencia, velocidadBala, radio); 
        }

        // Update is called once per frame
        void Update()
        {
            ejecutar();
        }
    }
}