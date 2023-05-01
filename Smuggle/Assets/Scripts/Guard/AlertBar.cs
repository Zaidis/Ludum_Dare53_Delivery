using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertBar : MonoBehaviour
{
    public Transform guardTransform;
   
    [SerializeField] private Image m_barImage;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Vector3 barOffset; //so its not directly inside the guard. 
    private IEnumerator alertAnimation;

    private void Start() {
        alertAnimation = null;
    }
    private void Update() {
        transform.LookAt(Camera.main.transform, Vector3.up);
        if(guardTransform != null)
            transform.position = guardTransform.position + barOffset;
    }


    public void SetAlertBarFillValue(float alertValue) {
        float num = alertValue / 100;
        if (alertAnimation != null) StopCoroutine(alertAnimation);

        if(m_barImage.fillAmount != num) {
            alertAnimation = AnimateAlertBar(num);
            StartCoroutine(alertAnimation);
        }
    }

    private IEnumerator AnimateAlertBar(float alertValue = 0) {
        float i = 0f;
        float speed = 3;
        float initialFill = m_barImage.fillAmount;
        while(i < 1f) {
            i += speed * Time.deltaTime;
            m_barImage.fillAmount = Mathf.Lerp(initialFill, alertValue, i);

            m_barImage.color = gradient.Evaluate(1 - m_barImage.fillAmount);
            
            yield return null;
        }

        m_barImage.fillAmount = alertValue;
        m_barImage.color = gradient.Evaluate(1 - m_barImage.fillAmount);
    }
}
