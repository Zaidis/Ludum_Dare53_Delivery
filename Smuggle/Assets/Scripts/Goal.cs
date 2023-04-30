using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            //level complete, you delivered. 
            Debug.Log("Victory");
        }
    }


}
