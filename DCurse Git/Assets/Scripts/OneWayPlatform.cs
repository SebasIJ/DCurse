using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public Collider platformCollider; //the solid collider of the platform

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        //when the player enters the trigger below the platform the collision between them gets ignored
        if(other.tag == "Player")
        {
            Physics.IgnoreCollision(platformCollider, other, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //when the player exits the trigger collision goes back to normal
        if (other.tag == "Player")
        {
            Physics.IgnoreCollision(platformCollider, other, false);
        }
    }
}
