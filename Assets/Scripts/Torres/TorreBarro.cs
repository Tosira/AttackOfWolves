using System.Collections.Generic;
using UnityEngine;

namespace Assets.src.Torres
{
    public class TorreBarro : Torreta
    {
        [SerializeField] private GameObject _prefabBala;
        private Enemigo currentEnemy;
        // private static int precio = 20;

        // Use this for initialization
        void Awake()
        {
            level = 1;
            _name = "Torre de Barro";
            frequency = 1.5f;
            bulletSpeed = 1.5f;
            radio = 4f;
            damage = 1.4f;
            price = 20;
            statistics = new List<Statistics>();
            statistics.Add(new MudTower2());
            statistics.Add(new MudTower3());
            SetTower(transform, _prefabBala, frequency, bulletSpeed, radio, damage);
        }

        public override void Shoot()
        {            
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
            if (target == null) return; // Parece ser que da error de referencia nula debido a que otra torre destruye al enemigo objetivo
            Enemigo e = target.GetComponent<Enemigo>();

            if (e == null || e == currentEnemy) return;                        
            e.speed *= 0.5f;
            e.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.15f, 0.05f);
            currentEnemy = e;
        }

        public override int GetPrecio()
        {
            return price; 
        }

    }
}