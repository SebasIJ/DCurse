using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class TitleScreen : MonoBehaviour
{
    public Animator playerAnimator;
    public Rigidbody player;
    public float speed;
    public Camera myCamera;
    public GameObject bridge;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator.SetBool("walk", true);
        myCamera.orthographic = true; //sets camera type to orthographic

        //Sets the camera transform apropiately for the current view
        myCamera.transform.position = new Vector3(player.position.x, player.position.y + 4, -25);
        myCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        myCamera.farClipPlane = 50;
    }

    // Update is called once per frame
    void Update()
    {
        player.velocity = new Vector3(1, 0, 0).normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Entity3D"))
        {
            myCamera.orthographic = false; //sets camera type to perspective

            //Sets the camera transform apropiately for the current view
            myCamera.transform.position = new Vector3(player.position.x - 10, player.position.y + 6, player.position.z - 10);
            myCamera.transform.rotation = Quaternion.Euler(new Vector3(10, 45, 0));
            myCamera.farClipPlane = 200;
            bridge.SetActive(true);
        }
        else if (other.gameObject.CompareTag("Entity2D"))
        {
            myCamera.orthographic = true; //sets camera type to orthographic

            //Sets the camera transform apropiately for the current view
            myCamera.transform.position = new Vector3(player.position.x, player.position.y + 4, -25);
            myCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            myCamera.farClipPlane = 50;
            bridge.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Warp"))
        {
            player.gameObject.transform.position = new Vector3(0, 5.15f, 0);
        }
    }
}
