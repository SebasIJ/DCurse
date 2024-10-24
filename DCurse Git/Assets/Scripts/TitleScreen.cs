using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class TitleScreen : MonoBehaviour
{
    public Animator playerAnimator; //animator of the player in the title screen
    public Rigidbody player; //rigidbody of the player in the title screen
    public float speed; //walk speed of the player in the title screen
    public Camera myCamera; //camera of the title screen
    public GameObject bridge; //bridge near the end of the title screen

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator.SetBool("walk", true); //sets player in walking animation
        myCamera.orthographic = true; //sets camera type to orthographic

        //Sets the camera transform apropiately for the current view
        myCamera.transform.position = new Vector3(player.position.x, player.position.y + 4, -25);
        myCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        myCamera.farClipPlane = 50;
    }

    // Update is called once per frame
    void Update()
    {
        //moves player forward constantly
        player.velocity = new Vector3(1, 0, 0).normalized * speed;
    }

    //trigger enter events
    private void OnTriggerEnter(Collider other)
    {
        //checks the tag of the trigger
        switch (other.tag)
        {
            //automatically flips to 3D when entering an entity 3D trigger
            case "Entity3D":
                {
                    myCamera.orthographic = false; //sets camera type to perspective

                    //Sets the camera transform apropiately for the current view
                    myCamera.transform.position = new Vector3(player.position.x - 10, player.position.y + 6, player.position.z - 10);
                    myCamera.transform.rotation = Quaternion.Euler(new Vector3(10, 45, 0));
                    myCamera.farClipPlane = 200;
                    //sets bridge to active
                    bridge.SetActive(true);
                    break;
                }
            //automatically flips to 2D when entering an entity 2D trigger
            case "Entity2D":
                {
                    myCamera.orthographic = true; //sets camera type to orthographic

                    //Sets the camera transform apropiately for the current view
                    myCamera.transform.position = new Vector3(player.position.x, player.position.y + 4, -25);
                    myCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    myCamera.farClipPlane = 50;
                    bridge.SetActive(false);
                    break;
                }
            //warps player back to the beginning of the title screen looping it indefinitely
            case "Warp":
                {
                    player.gameObject.transform.position = new Vector3(0, 5.15f, 0);
                    break;
                }
        }
    }
}
