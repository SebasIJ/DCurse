using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Spider : MonoBehaviour
{
    //this script is currently under review.

    public float speed; //walk speed of the spider
    
    public Rigidbody body; //rigidbody component of the spider
    public Transform sprite; //transform of the sprite child object
    private bool walkRight = true; //determines if the spider is walking to the right or not
    

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>(); //gets the rigidbody in its needed variable
    }

    

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    private void Walk()
    {
        //checks walking direction
        if (walkRight)
        {
            //moves spider and sets direction it faces right
            body.position = new Vector3(body.position.x + (speed / 20), body.position.y, body.position.z);
            sprite.localScale = new Vector3(-1, 1, 1);

            // Edge dectection
            var rayHitWall = Physics.Raycast(body.position, Vector3.right, 1f);
            var rayCliff = Physics.Raycast(body.position + Vector3.right, Vector3.down, 2f);
            if (rayHitWall || !rayCliff)
            {
                walkRight = false;
            }
        }
        else
        {
            //moves spider and sets direction it faces left
            body.position = new Vector3(body.position.x - (speed / 20), body.position.y, body.position.z);
            sprite.localScale = new Vector3(1, 1, 1);

            // Edge detection
            var rayHitWall = Physics.Raycast(body.position, Vector3.left, 1f);
            var rayCliff = Physics.Raycast(body.position + Vector3.left, Vector3.down, 2f);
            if (rayHitWall || !rayCliff)
            {
                walkRight = true;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (Physics.Raycast(transform.position, Vector3.up, 5f) && collision.rigidbody.position.y > transform.position.y + 2.5)
                Destroy(this.gameObject);
        }
    }

}
