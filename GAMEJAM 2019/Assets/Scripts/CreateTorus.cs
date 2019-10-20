using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTorus : MonoBehaviour
{
    public GameObject torus;
    public float maxTime;
    
    private float timer;
    private AudioSource smokingSounds;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        smokingSounds = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;

        if(timer >= maxTime){
            timer = 0f;
            Instantiate(torus, transform.parent);
            smokingSounds.Play();


        }
        
    }
}
