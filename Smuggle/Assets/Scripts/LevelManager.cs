using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Canvas alertCanvas; //worldview canvas for alert bars. 
    public Transform patrolPointParent;

    public float maxTime; //amount of time player has to complete level
    public float currentTime;
    public bool isTimed = true;
    public bool startLevel = false;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else Destroy(this.gameObject);
    }

    private void Start() {
        StartCoroutine(GameManager.instance.TriggerAnimationAndWait("Open", StartLevelTimer));

    }

    public void StartLevelTimer() {
        
        currentTime = maxTime;
        startLevel = true;
    }

    private void Update() {
        if (isTimed) {
            if (startLevel) {
                currentTime -= Time.deltaTime;

                TimeSpan time = TimeSpan.FromSeconds(currentTime);
                GameManager.instance.timerText.text = time.ToString(@"mm\:ss");

                if(currentTime <= 0) {
                    //level ends, you failed
                    FindObjectOfType<PlayerMovement>().canMove = false;
                    StartCoroutine(GameManager.instance.TriggerAnimationAndWait("Close", GameManager.instance.LostLevel));
                }
            }        
        }
    }
}
