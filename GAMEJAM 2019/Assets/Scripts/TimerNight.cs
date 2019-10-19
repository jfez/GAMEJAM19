using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerNight : MonoBehaviour
{
    public PlayerMovement playerMovement;
    
    [HideInInspector]public float timeCounter;
    public bool isNight;
    public Image timeBar;
    [HideInInspector]public float standarTime;

    private GameObject[] obstaclesNight;
    

    private float resizeSpeed;
    private float originalSizeX;
    
    
    // Start is called before the first frame update
    void Start()
    {
        standarTime = 5f;
        timeCounter = standarTime;
        isNight = false;
        resizeSpeed = 2f;
        

        obstaclesNight = GameObject.FindGameObjectsWithTag("Night");
        

        foreach (GameObject obstacle in obstaclesNight){
            obstacle.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //print(timeCounter);
        timeBar.fillAmount = timeCounter/standarTime;
        
        if (isNight){
            timeCounter -= Time.deltaTime;
        }

        else if (!isNight && timeCounter < standarTime){
            timeCounter += Time.deltaTime;
        }

        if (timeCounter <= 0){
            finishNight();
            playerMovement.changeMaterialDay();
            playerMovement.isDay =true;

        }
    }

    public void startNight(){
        isNight = true;
        foreach (GameObject obstacle in obstaclesNight){
            obstacle.SetActive(true);
        }
    }

    public void finishNight(){
        isNight = false;
        foreach (GameObject obstacle in obstaclesNight){
            obstacle.SetActive(false);
        }
    }
}
