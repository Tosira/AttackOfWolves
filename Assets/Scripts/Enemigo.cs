using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    private List<GameObject> route = new List<GameObject>();
    private Transform currentTarget; 
    public int reward;

    public float vidaActual; 
    public float vidaMaxima; 
    public float speed = 2.0f;
    public bool esVisible;

    public BarraDeVida barraV;
    private Animator animator;    

    public void Start()
    {
        animator = GetComponent<Animator>();
        SetEnemy();
    }

    private void Update()
    {
        if (isInTheTarget())
        {
            GameState.gs.ReceiveEnemyAttack();
            Destroy(gameObject);
        }
        Attack();
    }

    private void FixedUpdate()
    {        
        UpdateRoute();
        if (this != null && currentTarget != null) transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
    }

    private bool isInTheTarget()
    {
        return currentTarget == null;                       
    }          

    public void SetRoute(List<GameObject> route)
    {
        if (route.Count == 0)
        {
            Debug.LogError("Ruta vacia");
            return;
        }
        this.route = route.ToList();
        currentTarget = route[0].transform;
    }

    private void UpdateRoute()
    {
        if (route.Count == 0) return;
         
        foreach(GameObject nodo in route)
        {
            if (Vector2.Distance(nodo.transform.position, this.transform.position) <= 1)
            {                
                var index = route.IndexOf(nodo);
                if (route.Count == 1)
                {
                    currentTarget = null;
                }
                else
                {
                    currentTarget = route[index + 1].transform;
                }
                route.Remove(nodo);
                //Debug.Log("Ruta actulizada");
                return; 
            }            
        }        
    }

    public void GetAttack(float damage)
    {
        if ((vidaActual -= damage) <= 0)
        {
            GameState.gs.AddMoney(reward); 
            Destroy(gameObject); 
        }
        barraV.actualizarBarraVida(vidaMaxima, vidaActual);
    }
    public virtual void SetEnemy() { }
    public virtual void Attack() { }
}// Enemigo
