using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 checkpoint;

    public TimerNight timerNight;

    private Vector3 moveDirection = Vector3.zero;

    public Transform pivot;
    public float rotateSpeed;

    public Material dayPlayerNightcap;
    public Material dayPlayerNightgown;

    public Material nightPlayerNightcap;
    public Material nightPlayerNightgown;

    public Material dayGround;
    public Material nightGround;

    public Material dayForeground;
    public Material nightForeground;
    public GameObject foreground;

    public GameObject playerNightgown;
    public GameObject playerNightcap;

    //public GameObject ground;
    private GameObject[] grounds;
    private bool resetJump;
    private bool canSwitch;

    [HideInInspector]public bool isDay;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        isDay = true;
        resetJump = true;
        canSwitch = true;

        grounds = GameObject.FindGameObjectsWithTag("Ground");
        checkpoint = transform.position;
    }

    void FixedUpdate()
    {   
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            resetJump = true;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                
                
            }
        }

        else{
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.z = Input.GetAxis("Vertical");
            moveDirection.x *= speed*3/4;
            moveDirection.z *= speed*3/4;

            if((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton5)) && resetJump && canSwitch){
                moveDirection.y = jumpSpeed*0.6f;
                resetJump = false;
            }
        }
        

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        //Rotation of the player
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotateSpeed*Time.deltaTime);
        }
    }

    void Update(){
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton2)){
            speed = 10.0f;

        }
        else{
            speed = 6.0f;
        }
        
        if((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton5)) && canSwitch){
            if (isDay){
                isDay = false;
                changeMaterialNight();
                timerNight.startNight();
            }

            else{
                isDay = true;
                changeMaterialDay();
                timerNight.finishNight();

            }

        }
    }

    void changeMaterialNight(){
        playerNightgown.GetComponent<SkinnedMeshRenderer>().material = nightPlayerNightgown;
        playerNightcap.GetComponent<SkinnedMeshRenderer>().material = nightPlayerNightcap;
        foreground.GetComponent<MeshRenderer>().material = nightForeground;
        foreach (GameObject ground in grounds){
            ground.GetComponent<MeshRenderer>().material = nightGround;
        }
        

    }

    public void changeMaterialDay(){
        playerNightgown.GetComponent<SkinnedMeshRenderer>().material = dayPlayerNightgown;
        playerNightcap.GetComponent<SkinnedMeshRenderer>().material = dayPlayerNightcap;
        foreground.GetComponent<MeshRenderer>().material = dayForeground;
        foreach (GameObject ground in grounds){
            ground.GetComponent<MeshRenderer>().material = dayGround;
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Death"){
            Death();

        }

        if(collider.gameObject.tag == "ColliderObstacle"){
            canSwitch=false;

        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "ColliderObstacle"){
            canSwitch=true;

        }
    }

    public void Death(){
        if (!isDay){
            isDay = true;
            changeMaterialDay();
            timerNight.finishNight();
        }

        characterController.enabled = false;
        characterController.transform.position = checkpoint;
        characterController.enabled = true;

    }
    
}
