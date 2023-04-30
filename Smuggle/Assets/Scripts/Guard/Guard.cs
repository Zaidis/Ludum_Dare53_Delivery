using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    private NavMeshAgent agent;
    public List<Transform> patrolLocations = new List<Transform>();
    public AlertLevel alertLevel = AlertLevel.none;
    

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start() {
        
    }


    
}





public enum AlertLevel {
    none,
    minor, 
    medium, 
    moderate
}