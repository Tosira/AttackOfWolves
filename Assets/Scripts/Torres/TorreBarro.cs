using System.Collections;
using UnityEngine;

namespace Assets.src.Torres
{
    public class TorreBarro : Torreta
    {
        [SerializeField] private GameObject _prefabBala;
        private Enemigo enemigoActual;
        private static int precio = 20;

        // Use this for initialization
        void Start()
        {
            frequency = 1.5f;
            bulletSpeed = 1.5f;
            radio = 4f;
            damage = 1.4f;
            SetTower(transform, _prefabBala, frequency, bulletSpeed, radio, damage);
        }

        // Update is called once per frame
        void Update()
        {
            Defend();
        }

        public override void Shoot()
        {
            //  'origenDisparo' no se setea desde esta clase padre en el metodo Start.
            //  Debug.Log("prefabBala: " + prefabBala + "origenDisparo: " + origenDisparo + "objetivo: " + objetivo + "frecuencia: " + frecuencia); 
            
            

            if (_prefabBala != null && originShot != null && target != null && frequency <= 0)
            {
                GameObject bala = Instantiate(_prefabBala, originShot.position, originShot.rotation);
                Bala balaComponente = bala.GetComponent<Bala>();
                if (balaComponente != null) balaComponente.Initialize(target, gameObject, bulletSpeed, damage);                
                frequency = originalFrequency;
            }
        }

        public override void ImpactoBala()
        {
            Enemigo e = target.GetComponent<Enemigo>();

            if (e == null || e == enemigoActual) return;                         
            e.agent.speed = e.agent.speed * 0.5f;
            e.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.15f, 0.05f);
            enemigoActual = e;
        }

        public override int GetPrecio()
        {
            return precio; 
        }

    }
}