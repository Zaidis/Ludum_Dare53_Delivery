using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class Guard : MonoBehaviour
{
   
    private NavMeshAgent agent;
    [SerializeField] private GameObject body;
    private Animator animator;
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
    
    private IEnumerator decreaseAlert, waitForAlert, lookForTarget, patrol, chase;
    private bool isPatroling, isChasing;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = speed;

        lookForTarget = Look(null, null);
        patrol = Patrol();
        chase = Chase();
        ResetAlertCoroutines();
    }

    private void Start() {
        player = PlayerMovement.instance;
    }

    public void FixedUpdate() {
        if(alertValue >= 50) {
            if (!isPatroling && !isChasing) {
                isPatroling = true;
                ResetPatrol(true);
            } else if (!isChasing && alertValue > 75) {
                ResetPatrol();
                isChasing = true;
                ResetChase(true);
            }
            
        }
    }
    
    public void LookAtTarget(Transform target, Action action = null) {
        StopCoroutine(lookForTarget);
        lookForTarget = Look(target, action);
        StartCoroutine(lookForTarget);
    }
    public void ResetAlertCoroutines() {
        
        decreaseAlert = DecreaseAlertValue();
        waitForAlert = WaitForAlert();
    }

    public void ResetPatrol(bool on = false) {
        StopCoroutine(patrol);
        patrol = Patrol();
        if(on) StartCoroutine(patrol);
    }

    public void ResetChase(bool on = false) {
        StopCoroutine(chase);
        chase = Chase();
        if (on) StartCoroutine(chase);
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
        yield return new WaitForSeconds(5f);
        ResetPatrol(true);
    }

    public IEnumerator Chase() {
        isPatroling = false;
        Vector3 pos = player.transform.position;
        float dist = Vector3.Distance(this.transform.position, pos);
        agent.SetDestination(pos);
        while (dist >= 5f) {
            dist = Vector3.Distance(this.transform.position, pos);
            yield return null;
        }
        Debug.Log("CHASING ");
        yield return new WaitForSeconds(2f);
        ResetChase(true);
        
    }
    private IEnumerator Look(Transform target, Action action = null) {
        animator.SetBool("Idle", false);
        animator.enabled = false;
        var i = 0f;
        float speed = 2f;
        Quaternion toRotation = Quaternion.LookRotation(target.position - transform.position);
        while(i < 2f) {
            i += Time.deltaTime * speed;
            body.transform.localRotation = Quaternion.RotateTowards(body.transform.localRotation, Quaternion.Euler(0,90,0), i);
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
            ResetAlertCoroutines();
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