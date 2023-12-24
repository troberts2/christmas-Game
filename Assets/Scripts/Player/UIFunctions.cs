using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunctions : MonoBehaviour
{
    private PlayerMovement pm;
    [SerializeField] GameObject options;
    private void Start() {
        pm = FindObjectOfType<PlayerMovement>();
    }
    public void ResumeGame(){
        pm.pauseMenu.SetActive(false);
        Time.timeScale = 1;
        pm.isPaused = false;
    }
    public void MainMenu(){
        SceneManager.LoadScene("TitleScreen");
        Time.timeScale = 1;
    }
    public void LoadLevelByName(string name){
        SceneManager.LoadScene(name);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void Options(){
        options.SetActive(true);
    }
    public void OptionsBack(){
        options.SetActive(false);
    }
}
