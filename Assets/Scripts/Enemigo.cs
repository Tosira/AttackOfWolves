using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    private List<GameObject> route = new List<GameObject>();
    private Transform currentTarget; 
    public int reward;
    protected bool isAttackable = true;
    public bool IsAttackable { get { return isAttackable; } }

    public float vidaActual; 
    public float vidaMaxima; 
    public float speed = 2.0f;

    public BarraDeVida barraV;
    private Animator animator;    

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (DialogsManager.Instance.DialogueInProgress) return;

        if (isInTheTarget())
        {
            GameState.gs.ReceiveEnemyAttack();
            Destroy(gameObject);
        }
        Attack();
        MakeUnattackable();
    }

    private void FixedUpdate()
    {
        if (DialogsManager.Instance.DialogueInProgress) return;

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

    public virtual void MakeUnattackable()
    {
        // Implementar como o cuando un enemigo no es atacable
    }

    public virtual void Attack() { }
}// Enemigo
