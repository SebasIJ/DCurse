using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpwardPlatform : MonoBehaviour
{
    private Vector3 startingPos;
    private bool goUp;
    private bool upReached;
    public Vector3 destination;
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (goUp && transform.position.y < destination.y && !upReached)
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        else if(transform.position.y > startingPos.y || upReached)
        {
            transform.position -= new Vector3(0, speed/3 * Time.deltaTime, 0);
        }

        if(transform.position.y >= destination.y)
        {
            upReached = true;
        }
        else if(transform.position.y <= startingPos.y)
        {
            upReached = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = this.transform;
            goUp = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
            goUp = false;
        }
    }
}
