using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardVision : MonoBehaviour
{

    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if(PlayerMovement.instance.isCaught == false && PlayerMovement.instance.hasWon == false) {
                PlayerMovement.instance.isCaught = true;
                //player was found by guard
                Debug.LogError("You have been caught!");
                FindObjectOfType<PlayerMovement>().canMove = false;
                StartCoroutine(GameManager.instance.TriggerAnimationAndWait("Close", GameManager.instance.LostLevel));
            }
            
            
        }
    }

}
