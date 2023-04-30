using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager : MonoBehaviour
{

    public List<Guard> guards = new List<Guard>();

    private float value;

    private void FixedUpdate() {

        foreach(Guard guard in guards) {
            value = guard.alertValue;
            if(guard.alertLevel != AlertLevel.none) {
                //guard is at least alert somewhat
                
            }
        }
        
    }

}
