using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spider : MonoBehaviour
{
    public float speed;
    public float maxDistance;
    public Rigidbody body;
    public Transform sprite;
    private bool walkRight = true;
    private float currentDistance = 0;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        currentDistance = maxDistance / 2;
    }

    

    // Update is called once per frame
    void Update()
    {
        Walk();

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
        if (walkRight)
        {
            body.position = new Vector3(body.position.x + (speed / 20), body.position.y, body.position.z);
            sprite.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            body.position = new Vector3(body.position.x - (speed / 20), body.position.y, body.position.z);
            sprite.localScale = new Vector3(1, 1, 1);
        }
        currentDistance++;
    }
}
