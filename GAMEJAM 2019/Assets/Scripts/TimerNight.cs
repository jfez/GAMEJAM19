using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerNight : MonoBehaviour
{
    public PlayerMovement playerMovement;
    
    [HideInInspector]public float timeCounter;
    private bool isNight;
    public Image timeBar;
    [HideInInspector]public float standarTime;

    private GameObject[] obstaclesNight;
    private GameObject[] obstaclesDay;
    
    private Color barColor;

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
        obstaclesDay = GameObject.FindGameObjectsWithTag("Day");
        

        foreach (GameObject obstacle in obstaclesNight){
            obstacle.SetActive(false);
        }

        foreach (GameObject obstacle in obstaclesDay){
            obstacle.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //print(timeCounter);
        timeBar.fillAmount = timeCounter/standarTime;
        barColor = new Color(timeCounter/standarTime, 0, 1 - (timeCounter/standarTime));
        timeBar.color = barColor;
        
        
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
        foreach (GameObject obstacle in obstaclesDay){
            obstacle.SetActive(false);
        }
    }

    public void finishNight(){
        isNight = false;
        foreach (GameObject obstacle in obstaclesNight){
            obstacle.SetActive(false);
        }
        foreach (GameObject obstacle in obstaclesDay){
            obstacle.SetActive(true);
        }
    }
}
