using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class Enemigo : MonoBehaviour
{
    private List<GameObject> route = new List<GameObject>();
    private Transform target; 
    public NavMeshAgent agent;    

    // duda con el agente que esta en privado y aun asi los enemigos se mueven. 
    // implementar vida de los enemigos

    public float vidaActual; 
    public float vidaMaxima; 
    private float velocidad = 2.0f;
    public bool esVisible;

    public BarraDeVida barraV;

    private Animator animator;

    // Metodo llamado cuando la instancia del script se carga (llamado antes que Start). No importa si el objeto esta activo o inactivo. 
    private void Awake()
    {
        SetAgent();
    }

    public void Start()
    {
        animator = GetComponent<Animator>();
        SetEnemy(); 
    }

    private void FixedUpdate()
    {
        
        UpdateRoute(); 
    }

    private void SetAgent()
    {
        //Debug.Log("Agente Seteado");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = velocidad;

    }
    
    private void SetTarget(Transform target)
    {
        this.target = target;
        agent.SetDestination(this.target.position); 
    }

    public void SetRoute(List<GameObject> route)
    {
        if (route.Count == 0)
        {
            Debug.Log("Ruta vacia");
            return; 
        }
        
        foreach (GameObject nodo in route)
        {
            this.route.Add(nodo); 
        }         

        SetTarget(this.route[0].transform);  // Se define primer punto de la lista que debe seguir el enemigo
    }

    public bool CheckAgent()
    {
        return agent != null;   // Agente inicializado en Awake
    }

    private void UpdateRoute()
    {
        if (route.Count == 1) return;   // Este ultimo nodo es la meta, no existe nodo al que cambiar luego de este. 
         
        foreach(GameObject nodo in route)
        {
            if (Vector2.Distance(nodo.transform.position, this.transform.position) <= 1)
            {                
                var index = route.IndexOf(nodo);
                SetTarget(route[index + 1].transform);
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
            Destroy(gameObject); 
        }
        barraV.actualizarBarraVida(vidaMaxima, vidaActual);
    }
    public virtual void SetEnemy() {
    }
    public virtual void Atacar() { }
}
