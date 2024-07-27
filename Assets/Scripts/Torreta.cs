using Assets.src.Enemigos;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    public Transform target;
    public Transform originShot;
    private GameObject prefabBullet;
    public float frequency;
    public float originalFrequency;
    public float bulletSpeed;
    public float radio;
    public float damage;
    public SpriteRenderer spriteRend;

    protected int price;
    protected short level;
    protected short availableLevel;
    public short AvailableLevel { get { return availableLevel; } }
    protected string _name;
    protected List<Statistics> statistics;
    private float disMin = 10000000f;

    public void Update()
    {
        Defend();
    }

    void FindEnemy()
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(transform.position, radio);
        foreach(Collider2D e in enemigos)
        { 
            //  'objetivo' igual al primer enemigo captado dentro del area.
            if(target == e.transform)
            {
                return;
            }
        }

        target = null;
        disMin = 10000000f;
        foreach(Collider2D e in enemigos)
        {             
            if (e.CompareTag("Enemigo"))
            {
                Enemigo enemigo = e.GetComponent<Enemigo>();
                if (enemigo.esVisible && e != null) {

                    float distancia = Vector3.Distance(e.gameObject.transform.position, GameState.target.transform.position);
                    if (distancia < disMin)
                    {
                        //disMin = Vector3.Distance(e.gameObject.transform.position, Meta.insMeta.transform.position);
                        disMin = distancia;                        
                        target = e.transform;
                    }

                }
                            
            }
        }
    }        

    public virtual void Shoot()
    {
        if (!(prefabBullet != null && originShot != null && target != null && frequency <= 0)) return;
        
        GameObject bulletObject = Instantiate(prefabBullet, originShot.position, originShot.rotation);
        Bala bulletComponent = bulletObject.GetComponent<Bala>();
        if (bulletComponent != null) bulletComponent.Initialize(target, gameObject, bulletSpeed, damage);
        frequency = originalFrequency;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }

    public void SetTower(Transform origenDisparo, 
                                GameObject prefabBala, 
                                float tipoDisparo, 
                                float velocidadBala,
                                float radio,
                                float damage)
    {

        this.originShot = origenDisparo; 
        this.prefabBullet = prefabBala;  
        this.frequency = tipoDisparo;
        this.originalFrequency = tipoDisparo;
        this.bulletSpeed = velocidadBala;
        this.radio = radio;
        this.damage = damage;
    }
    
    public void Defend()
    {
        if (DialogsManager.dm.isDialogueInProgress()) return;
        FindEnemy();
        Shoot();
        frequency -= Time.deltaTime;
    }

    private Statistics SearchStatistics()
    {
        if (statistics.Count <= 0) { Debug.Log("Sin Estadisticas para mejorar"); return null; }
        foreach (Statistics st in statistics)
        {
            if (st.Level > level) return st;            
        }
        return null;
    }

    public bool Upgrade()
    {
        if (statistics.Count <= 0) { Debug.Log("Sin Estadisticas para mejorar"); return false; }
        
        Statistics st = SearchStatistics();
        if (st == null) return false;
        
        Debug.Log("Mejora de nivel " + level);
        if (!GameState.gs.Buy(st.Precio)) return false;
        level = st.Level;
        damage = st.Damage;
        originalFrequency = st.Frequency;
        radio = st.Radio;
        bulletSpeed = st.BulletSpeed;
        price = st.Precio;
        Debug.Log(" a nivel " + level);
        return true;
    }

    public bool Buy()
    {
        Debug.Log("Compra " + price);
        return GameState.gs.Buy(price);
    }

    public void Sell()
    {
        GameState.gs.AddMoney(CalculateProfit());
        // Destruccion hecha por Buttons::FinishSellTower
    }

    public int CalculateProfit()
    {
        return  (int)(price*0.6);
    }

    public string GetDetailsUpgrade()
    {
        Statistics st = SearchStatistics();
        if (st == null) return "Nivel Maximo Alcanzado";
        string details = _name + " Nivel " + st.Level + "\n\n" +
                         "Daño: " + damage + "   +" + (st.Damage-damage).ToString("F2") + "\n" +
                         "Frecuencia: " + originalFrequency + "   +" + (st.Frequency-originalFrequency).ToString("F2") + "\n" +
                         "Velocidad Bala: " + bulletSpeed + "   +" + (st.BulletSpeed-bulletSpeed).ToString("F2") + "\n" +
                         "Radio: " + radio + "   +" + (st.Radio-radio).ToString("F2") + "\n\n" +
                         "Precio Mejora: " + st.Precio;

        return details;
    }

    public string GetDetailsTower()
    {
        string details = _name + " Nivel " + level + "\n\n" +
                         "Daño: " + damage + "\n" +
                         "Frecuencia: " + originalFrequency + "\n" +
                         "Velocidad Bala: " + bulletSpeed + "\n" +
                         "Radio: " + radio + "\n" +
                         "Precio: " + price;
        return details;
    }

    public virtual void ImpactoBala() { }  

    public virtual int GetPrecio() { return price; }
}
