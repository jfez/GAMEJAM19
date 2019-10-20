using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTorus : MonoBehaviour
{
    public GameObject torus;
    public float maxtTime;
    
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;

        if(timer >= maxtTime){
            timer = 0f;
            Instantiate(torus, transform.parent);

        }
        
    }
}
