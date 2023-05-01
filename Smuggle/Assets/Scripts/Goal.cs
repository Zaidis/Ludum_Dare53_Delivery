using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Goal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            //level complete, you delivered. 
            Debug.Log("Victory");
            PlayerMovement.instance.hasWon = true;
            StartCoroutine(GameManager.instance.TriggerAnimationAndWait("Close", GameManager.instance.VictoryLevel));

        }
    }


}
