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

    public int vida; 
    public Vector3 velocidad;     
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

    public void RecibirAtaque(int damage)
    {
        if ((vida -= damage) <= 0)
        {
            Destroy(gameObject); 
        }       
    }
}
