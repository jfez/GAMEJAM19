using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour
{
    public GameObject canvasPause;
    
    [HideInInspector]
    public bool paused;
    
    // Start is called before the first frame update
    void Start()
    {
        canvasPause.SetActive(false);
        Time.timeScale = 1;
        paused = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)){
            
            if (paused){
                canvasPause.SetActive(false);
                paused = false;
                Time.timeScale = 1;

            }

            else{
                canvasPause.SetActive(true);
                paused = true;
                Time.timeScale = 0;

            }
            
            

        }
    }

    public void PlayGame(){
        canvasPause.SetActive(false);
        paused = false;
        Time.timeScale = 1;
    }

    public void ExitGame(){
        SceneManager.LoadScene("Initial");

    }
}
