using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 checkpoint;

    public TimerNight timerNight;

    public GameObject canvasTuto;

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
    [HideInInspector]
    public GameObject [] foregrounds;

    public GameObject obstacle1;
    public GameObject obstacle2;
    public GameObject obstacle3;

    public GameObject playerNightgown;
    public GameObject playerNightcap;

    public GameObject lightObject;

    
    public GameObject flag;
    private Light lightComponent;

    private Color lightDay;
    private Color lightNight;

    //public GameObject ground;
    private GameObject[] grounds;
    private bool resetJump;
    private bool canSwitch;
    private bool walking;
    private Animator animator;

    [HideInInspector]public bool isDay;

    public AudioClip jumpSound;
    public AudioClip switchSound;
    private AudioSource amaranthAudio;

    private ExitManager exitManager;

    private int checkpointIndex;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        isDay = true;
        resetJump = true;
        canSwitch = true;

        grounds = GameObject.FindGameObjectsWithTag("Ground");
        foregrounds = GameObject.FindGameObjectsWithTag("Foreground");
        checkpoint = transform.position;
        walking = false;
        animator = GetComponent<Animator>();
        lightDay = new Color(0.94f,0.92f,0.2f);
        lightNight = new Color(0.2f, 0.35f, 0.94f);
        lightComponent = lightObject.GetComponent<Light>();
        amaranthAudio = GetComponent<AudioSource>();
        exitManager = GameObject.FindGameObjectWithTag("ExitManager").GetComponent<ExitManager>();
        checkpointIndex = 0;
        canvasTuto.SetActive(true);

    }

    void FixedUpdate()
    {   
        if (exitManager.paused){
            return;
        }
        
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            resetJump = true;

            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
                walking = true;

            }

            else{
                walking = false;
            }

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                animator.SetTrigger("Jump");
                amaranthAudio.clip = jumpSound;
                amaranthAudio.Play();
                
                
            }
        }

        else{
            walking = false;
            
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
        if (exitManager.paused){
            return;
        }
        
        animator.SetBool("Walking", walking);
        
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("Run") != 0){
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

            amaranthAudio.clip = switchSound;
            amaranthAudio.Play();

        }
    }

    void changeMaterialNight(){
        playerNightgown.GetComponent<SkinnedMeshRenderer>().material = nightPlayerNightgown;
        playerNightcap.GetComponent<SkinnedMeshRenderer>().material = nightPlayerNightcap;
        foreach (GameObject foreground in foregrounds){
            foreground.GetComponent<MeshRenderer>().material = nightForeground;
        }
        foreach (GameObject ground in grounds){
            ground.GetComponent<MeshRenderer>().material = nightGround;
        }
        lightComponent.color = lightNight;
        
        

    }

    public void changeMaterialDay(){
        playerNightgown.GetComponent<SkinnedMeshRenderer>().material = dayPlayerNightgown;
        playerNightcap.GetComponent<SkinnedMeshRenderer>().material = dayPlayerNightcap;
        foreach (GameObject foreground in foregrounds){
            foreground.GetComponent<MeshRenderer>().material = dayForeground;
        }
        foreach (GameObject ground in grounds){
            ground.GetComponent<MeshRenderer>().material = dayGround;
        }
        lightComponent.color = lightDay;

    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Death"){
            Death();

        }

        if(collider.gameObject.tag == "ColliderObstacle"){
            canSwitch=false;

        }

        if(collider.gameObject.tag == "Checkpoint1" && checkpointIndex < 1){
            checkpoint = transform.position;
            
            Instantiate(flag, checkpoint, flag.transform.rotation);
            checkpointIndex++;

            canvasTuto.SetActive(false);

        }

        if(collider.gameObject.tag == "Checkpoint2" && checkpointIndex < 2){
            checkpoint = transform.position;
            
            Instantiate(flag, checkpoint, flag.transform.rotation);
            checkpointIndex++;

        }

        if(collider.gameObject.tag == "Checkpoint3" && checkpointIndex < 3){
            checkpoint = transform.position;
            
            Instantiate(flag, checkpoint, flag.transform.rotation);
            checkpointIndex++;

        }

        if(collider.gameObject.tag == "Checkpoint4" && checkpointIndex < 4){
            checkpoint = transform.position;
            
            Instantiate(flag, checkpoint, flag.transform.rotation);
            checkpointIndex++;

        }

        if(collider.gameObject.tag == "Checkpoint6" && checkpointIndex < 6){
            checkpoint = transform.position;
            
            Instantiate(flag, checkpoint, flag.transform.rotation);
            checkpointIndex++;

        }

        

        if(collider.gameObject.tag == "SpecialCP" && checkpointIndex < 5){
            checkpoint = transform.position;
            obstacle1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            obstacle1.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            obstacle2.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            obstacle2.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            obstacle3.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            obstacle3.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;

            
            Instantiate(flag, checkpoint, flag.transform.rotation);

            checkpointIndex++;

            

        }

        if(collider.gameObject.tag == "End"){
            SceneManager.LoadScene("Initial");

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
