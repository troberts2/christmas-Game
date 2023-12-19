using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SantaBehavior : MonoBehaviour
{
    public float timerLength;
    private float timeMax;
    public bool stopTimer = false;
    [SerializeField] private Gradient _gradient = null;
    [SerializeField] private Image _image;
    // Start is called before the first frame update
    void Start()
    {
        timeMax = timerLength;
        StartTimer();
    }

    public void StartTimer(){
        StartCoroutine(StartTheTimer());
    }

    IEnumerator StartTheTimer(){
        while(!stopTimer){
            timerLength -= Time.deltaTime;
            yield return new WaitForSeconds(.001f);

            if(timerLength <= 0){
                stopTimer = true;
            }

            if(!stopTimer){
                _image.color = _gradient.Evaluate(_image.fillAmount);
                _image.fillAmount = timerLength/timeMax;
            }
        }
    }

}
