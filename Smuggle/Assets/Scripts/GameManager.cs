using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    //needs to hold onto time, and whatnot
    public static GameManager instance;
    public Animator animator;
    public int levelIndex;
    public TextMeshProUGUI timerText;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator TriggerAnimationAndWait(string animationName, Action action = null) {
        animator.SetTrigger(animationName);

        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 0.5f);

        if(action != null) {
            action();
        }
    }

    public void LostLevel() {
        Debug.Log("YOU LOST THE LEVEL");
        SceneManager.LoadScene(levelIndex);
    }

    public void VictoryLevel() {
        Debug.Log("YOU WON THE LEVEL");
        levelIndex++;
        SceneManager.LoadScene(levelIndex);
    }
}
