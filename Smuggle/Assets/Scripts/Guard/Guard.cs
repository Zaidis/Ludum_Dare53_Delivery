using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class Guard : MonoBehaviour
{
   
    private NavMeshAgent agent;
    public List<Transform> patrolLocations = new List<Transform>();
    public AlertLevel alertLevel = AlertLevel.none;
    

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
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

    public IEnumerator LookAtTarget(Quaternion startRotation, Transform target) {
        var i = 0f;
        float speed = 2f;
        //Quaternion toRotation = Quaternion.FromToRotation(transform.forward, target.position);
        Quaternion toRotation = Quaternion.LookRotation(target.position - transform.position);
        while(i < 1f) {
            i += Time.deltaTime * speed;
            // transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, i);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, i);
            yield return null;
        }

    }


}

public enum AlertLevel {
    none,
    minor, 
    medium, 
    moderate
}