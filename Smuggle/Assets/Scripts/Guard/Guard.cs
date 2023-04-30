using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class Guard : MonoBehaviour
{
   
    private NavMeshAgent agent;
    private int patrolIndex = -1;
    public List<Transform> patrolLocations = new List<Transform>();
    public AlertLevel alertLevel = AlertLevel.none;
    [SerializeField] private float speed;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

   
    public Quaternion GetCurrentRotation() {
        return transform.rotation;
    }
    public IEnumerator TriggerPatrolAndWait(Action action) {

        yield return null;

        if(action != null) {
            action();
        }
    }

    public IEnumerator Patrol() {
        if (patrolIndex >= patrolLocations.Count - 1) {
            patrolIndex = -1;
        }
        patrolIndex++;
        
        agent.SetDestination(patrolLocations[patrolIndex].position);
        float dist = Vector3.Distance(this.transform.position, patrolLocations[patrolIndex].position);
        while (dist >= 5f ) {
            dist = Vector3.Distance(this.transform.position, patrolLocations[patrolIndex].position);
            yield return null;
        }
        Debug.Log("HEHOEIRHOAISH");
        yield return new WaitForSeconds(5f);
        StartCoroutine(Patrol());
    }
    public IEnumerator LookAtTarget(Transform target, Action action = null) {
        var i = 0f;
        float speed = 2f;
        Quaternion toRotation = Quaternion.LookRotation(target.position - transform.position);
        while(i < 1f) {
            i += Time.deltaTime * speed;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, i);
            yield return null;
        }

        if(action != null) {
            action();
        }
    }


}

public enum AlertLevel {
    none,
    minor, 
    medium, 
    moderate
}