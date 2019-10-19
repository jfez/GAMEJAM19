using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesResize : MonoBehaviour
{
    public TimerNight timerNight;

    private float originalSizeX;
    
    // Start is called before the first frame update
    void Start()
    {
       originalSizeX = transform.localScale.x; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(originalSizeX * (timerNight.timeCounter/timerNight.standarTime), transform.localScale.y, transform.localScale.z);

        }
        
        
        
}
