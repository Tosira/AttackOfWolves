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
            frecuencia = 1f;
            velocidadBala = 1.5f;
            radio = 5f;
            damage = 1f;
            definirVariables(transform, _prefabBala, frecuencia, velocidadBala, radio, damage);
        }

        // Update is called once per frame
        void Update()
        {
            ejecutar();
        }
    }
}