using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class Guard : MonoBehaviour
{
   
    private NavMeshAgent agent;
    private PlayerMovement player;
    [SerializeField] private float speed;
    public AlertLevel alertLevel = AlertLevel.none;
    public GuardState guardState = GuardState.none;

    private int patrolIndex = -1;
    public List<Transform> patrolLocations = new List<Transform>();

    [Range(0, 100)]
    public float alertValue = 0f;

    [Range(0, 100)]
    public float alertReductionMinimum = 0f;
    
    public float alertWaitingBuffer = 5f; //seconds for how long we wait to decrease alert value
    public bool guardIsAnxious; //when this is true, the guard will never go below medium alert
    
    private IEnumerator decreaseAlert, waitForAlert;
    private bool isPatroling, isChasing;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        ResetCoroutines();
    }

    private void Start() {
        player = PlayerMovement.instance;
    }

    public void FixedUpdate() {
        if(alertValue >= 50) {
            if(!isPatroling && !isChasing) {
                isPatroling = true;
                StartCoroutine(Patrol());
            }
            
        }
    }
    public void ResetCoroutines() {
        decreaseAlert = DecreaseAlertValue();
        waitForAlert = WaitForAlert();
    }
    public IEnumerator TriggerPatrolAndWait(Action action) {

        yield return null;

        if(action != null) {
            action();
        }
    }

    public IEnumerator WaitForAlert() {

        yield return new WaitForSeconds(alertWaitingBuffer);

        StartCoroutine(decreaseAlert);
    }

    public IEnumerator DecreaseAlertValue() {

        while(alertValue > alertReductionMinimum) {
            alertValue -= 1 * Time.deltaTime;
            yield return null;
        }

        alertValue = alertReductionMinimum;
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

    public IEnumerator Chase() {
        var i = 0f;
        while (i < 20f) {
            i += 1 * Time.deltaTime;
            agent.SetDestination(patrolLocations[patrolIndex].position);
            yield return null;
        }
        yield return new WaitForSeconds(5);
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


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            StopCoroutine(decreaseAlert);
            StopCoroutine(waitForAlert);
            ResetCoroutines();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            alertValue += IncreaseAlertValue() * Time.deltaTime;
            GetAlertLevel();

        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            StartCoroutine(waitForAlert);
            
        }
    }

    private float IncreaseAlertValue() {
        switch (player.moveType) {
            case movementType.walking:
                return 2f;
            case movementType.sprinting:
                return 4f;
            case movementType.crouching:
                return 1f;
            default:
                return 0f;
        }
    }

    public void GetAlertLevel() {
        if(alertValue >= 25 && alertValue < 50) {
            alertLevel = AlertLevel.minor;
        } else if (alertValue >= 50 && alertValue < 75) {
            alertLevel = AlertLevel.medium;
            alertReductionMinimum = 50;
            guardIsAnxious = true;
        } else if (alertValue >= 75) {
            alertLevel = AlertLevel.moderate;
            guardIsAnxious = true;
        }
    }
}

public enum AlertLevel {
    none,
    minor, 
    medium, 
    moderate
}

public enum GuardState {
    none,
    patrol, 
    chase, 
}