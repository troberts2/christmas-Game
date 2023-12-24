using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SantaBehavior : MonoBehaviour
{
    public float timerLength;
    private float timeMax;
    public bool stopTimer = false;
    [SerializeField] private Gradient _gradient = null;
    [SerializeField] private Image _image;
    internal Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        timeMax = timerLength;
        animator = GetComponent<Animator>();
        StartTimer();
    }

    public void StartTimer(){
        StartCoroutine(StartTheTimer());
        StartCoroutine(AnimateSanta());
    }

    IEnumerator StartTheTimer(){
        while(!stopTimer){
            timerLength -= Time.deltaTime;
            yield return new WaitForSeconds(.001f);

            if(timerLength <= 0){
                stopTimer = true;
                SceneManager.LoadScene("DiedScreen");
            }

            if(!stopTimer){
                _image.color = _gradient.Evaluate(_image.fillAmount);
                _image.fillAmount = timerLength/timeMax;
            }
        }
    }

    IEnumerator AnimateSanta(){
        while(!stopTimer){
            if(timerLength/timeMax > .66f){
                animator.SetTrigger("idle1");
                yield return null;
            }
            else if(timerLength/timeMax > .33f){
                animator.SetTrigger("idle2");
                yield return null;
            }
            else if(timerLength/timeMax > 0f){
                animator.SetBool("idle3", true);
                yield return new WaitForSeconds(3f);
                animator.SetTrigger("eat");
                yield return new WaitForSeconds(3f);
            }
        }
        yield return null;
    }

    public IEnumerator GrabUpgrade(){
        timerLength += 60;
        animator.SetTrigger("walk");
        yield return new WaitForSeconds(4f);
        //trigger ui for upgrade
    }

}
