using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class Enemigo : MonoBehaviour
{
    public Transform target; 
    NavMeshAgent agent;    

    // duda con el agente que esta en privado y aun asi los enemigos se mueven. 
    // implementar vida de los enemigos

    public float vidaActual; 
    public float vidaMaxima; 
    public Vector3 velocidad;

    public BarraDeVida barraV;

    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();    
    }
    public void SetAgent()
    {
        //Debug.Log("Agente Seteado");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public virtual void moverAgenteNavMesh()
    {
        agent.SetDestination(target.position);
    }

    public virtual void Atacar() { }

    public void RecibirAtaque(float damage)
    {
        if ((vidaActual -= damage) <= 0)
        {
            Destroy(gameObject); 
        }
        barraV.actualizarBarraVida(vidaMaxima, vidaActual);
    }
}
