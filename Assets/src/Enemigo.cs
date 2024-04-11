using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class Enemigo : MonoBehaviour
{
    //[SerializeField] private float velocidad = 1.0f;

    [SerializeField] public Transform target; 

    NavMeshAgent agent;    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; 
        agent.updateUpAxis = false;      
    }


    private void Update()
    {
        agent.SetDestination(target.position); 
    }    
}
