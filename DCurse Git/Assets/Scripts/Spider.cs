using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spider : MonoBehaviour
{
    //this script is currently incomplete
    //the spider should walk nonstop and turn around on its own when encountering a wall or cliff
    //currently it walks a set distance and turns around after that set distance

    public float speed; //walk speed of the spider
    public float maxDistance; //should remove this variable eventually
    public Rigidbody body; //rigidbody component of the spider
    public Transform sprite; //transform of the sprite child object
    private bool walkRight = true; //determines if the spider is walking to the right or not
    private float currentDistance = 0; //should remove this variable eventually

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>(); //gets the rigidbody in its needed variable
        currentDistance = maxDistance / 2; //sets an initial current distance
    }

    

    // Update is called once per frame
    void Update()
    {
        Walk();

        //when current distance reaches max distance turns around and resets current distance
        if(currentDistance >= maxDistance)
        {
            if (walkRight)
            {
                walkRight = false;
            }
            else
            {
                walkRight = true;
            }
            currentDistance = 0;
        }
    }

    private void Walk()
    {
        //checks walking direction
        if (walkRight)
        {
            //moves spider and sets direction it faces
            body.position = new Vector3(body.position.x + (speed / 20), body.position.y, body.position.z);
            sprite.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            //moves spider and sets direction it faces
            body.position = new Vector3(body.position.x - (speed / 20), body.position.y, body.position.z);
            sprite.localScale = new Vector3(1, 1, 1);
        }
        //increases current distance
        currentDistance++;
    }
}
