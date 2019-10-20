using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleDetection : MonoBehaviour
{
    private Smoking smoking;
    // Start is called before the first frame update
    void Start()
    {
        smoking = GetComponentInParent<Smoking>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider collider){
        if(collider.gameObject.tag == "Player"){
            smoking.insideCapsule = true;

        }
    }

    void OnTriggerExit (Collider collider){
        if(collider.gameObject.tag == "Player"){
            smoking.insideCapsule = false;
            smoking.checkInside();

        }
    }
    
}
