using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoking : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerMovement;
    private float shrinkSpeed;

    public bool insideCapsule;
    private bool insideTorus;
    private CapsuleDetection capsuleDetection;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        shrinkSpeed = 0.8f;
        insideCapsule = false;
        insideTorus = false;
        capsuleDetection = GetComponentInChildren<CapsuleDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += Vector3.one * shrinkSpeed * Time.deltaTime;

        if(transform.localScale.x >= 4){
            DestroyTorus();
        }
    }

    void OnTriggerEnter (Collider collider){
        if(collider.gameObject.tag == "Player"){
            
            insideTorus = true;
            if (!insideCapsule){
                playerMovement.Death();
                DestroyTorus();

            }
            

        }
    }

    void OnTriggerExit (Collider collider){
        if(collider.gameObject.tag == "Player"){
            insideTorus = false;

        }
    }

    public void checkInside(){
        if (insideTorus){
            playerMovement.Death();
            DestroyTorus();
        }

    }

    void DestroyTorus(){
        Destroy(gameObject);
    }
}
