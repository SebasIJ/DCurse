using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Camera myCamera;
    public Rigidbody player;
    public Animator playerAnimator;
    public Transform sprite;
    public Transform direction;
    public float speed;
    public float gravityForce = 0;
    public float jumpHeight;
    private int currentJump = 0;
    private int playerHP = 3;
    private bool twoD = true;
    private bool jump = false;
    private bool grounded = true;
    private bool canSwitch = true;
    private Vector3 input;
    private float last3D = 0;
    private float Ztwo = 0;
    private Vector3 respawn = Vector3.zero;

    public float knockForce;
    private float knockTime = 0;
    public float knockTotal;
    private bool knockRight;   
    public float totaliFrames;
    private float iFrames = 0;


    public bool dimension
    {
        get
        {
            return twoD;
        }
    }

    public bool DLock
    {
        get
        {
            return canSwitch;
        }
    }

    public int HP
    {
        get
        {
            return playerHP;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sprite = transform.Find("Sprite");
        direction = transform.Find("Direction");
        player = GetComponent<Rigidbody>();
        player.velocity = new Vector3(0, 0, 0);
        myCamera.orthographic = true;
        twoD = true;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Sets y velocity to 0 when its greater than 0 due to a bug with the gravity
        if (player.velocity.y > 0 && jump == false)
        {

            player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);

        }

        //applys customizable gravity force
        Vector3 gravity = -gravityForce * Vector3.up;
        player.AddForce(gravity, ForceMode.Acceleration);

        //save movement input
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));



        //boolean twoD determines the current dimension
        if (twoD)
        {


            //sets 2D coordinates (Z should always be 0 in 2D)
            player.position = new Vector3(player.position.x, player.position.y, Ztwo);

            myCamera.transform.position = new Vector3(player.position.x, player.position.y + 3, Ztwo - 25);

            //checks for input to switch dimension to 3D
            if (Input.GetKeyUp(KeyCode.Z))
            {
                if (canSwitch)
                {
                    myCamera.orthographic = false; //sets camera type to perspective
                    twoD = false; //if twoD is false the current dimension is 3D

                    //Sets the camera transform apropiately for the current view
                    myCamera.transform.position = new Vector3(player.position.x - 10, player.position.y + 5, player.position.z - 10);
                    myCamera.transform.rotation = Quaternion.Euler(new Vector3(10, 45, 0));
                    myCamera.farClipPlane = 100;


                    //Rotates sprite to face camera
                    sprite.rotation = Quaternion.Euler(new Vector3(0, 45, 0));

                    //sets 3D coordinates
                    player.position = new Vector3(player.position.x, player.position.y, last3D);
                }
            }

            if (knockTime <= 0)
            {
                playerAnimator.SetBool("damage", false);
                Movement2D();
            }
            else
            {
                if (knockRight)
                {
                    player.velocity = new Vector3(knockForce, knockForce, 0);
                }
                else
                {
                    player.velocity = new Vector3(-knockForce, knockForce, 0);
                }

                knockTime -= Time.deltaTime;
            }
        }
        else if (!twoD)
        {

            myCamera.transform.position = new Vector3(player.position.x - 10, player.position.y + 5, player.position.z - 10);

            //checks for input to switch dimension to 2D
            if (Input.GetKeyUp(KeyCode.Z))
            {
                if (canSwitch)
                {
                    myCamera.orthographic = true; //sets camera type to orthographic
                    twoD = true; //if twoD is true current dimension is 2D

                    //Sets the camera transform apropiately for the current view
                    myCamera.transform.position = new Vector3(player.position.x, player.position.y + 3, Ztwo - 25);
                    myCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    myCamera.farClipPlane = 50;

                    //Rotates sprite to face camera
                    sprite.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                    //saves last Z position for next switch to 3D
                    last3D = player.position.z;

                    //sets 2D coordinates (Z should always be 0 in 2D)
                    player.position = new Vector3(player.position.x, player.position.y, Ztwo);
                }
            }

            if (knockTime <= 0)
            {
                playerAnimator.SetBool("damage", false);
                Movement3D();
            }
            else
            {
                if (knockRight)
                {
                    player.velocity = new Vector3(knockForce, knockForce, 0);
                }
                else
                {
                    player.velocity = new Vector3(-knockForce, knockForce, 0);
                }

                knockTime -= Time.deltaTime;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.X) && jump == false && grounded == true)
        {
            jump = true;
        }

        if(iFrames > 0)
        {
            iFrames--;
            sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        }
        else
        {
            sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }


        Jump();

        AnimatePlayer();

        if (player.position.y <= -50)
        {
            Failsafe();
        }
    }

    //Handles control in 2D state
    private void Movement2D()
    {

        player.velocity = new Vector3(input.x, player.velocity.y, 0).normalized * speed;


        //turns sprite in the proper direction
        if (input.x > 0)
        {
            sprite.localScale = Vector3.one;
        }
        else if (input.x < 0)
        {
            sprite.localScale = new Vector3(-1, 1, 1);
        }
    }

    //Handles control in 3D state
    private void Movement3D()
    {
        //multiplying the regular movement vector by a matrix skewed by 45 degrees achieves the proper isometric movement
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        player.velocity = isoMatrix.MultiplyPoint3x4(new Vector3(input.x, player.velocity.y, input.z).normalized * speed);


        //turns sprite in the proper direction
        if (input.x > 0)
        {
            sprite.localScale = Vector3.one;
        }
        else if (input.x < 0)
        {
            sprite.localScale = new Vector3(-1, 1, 1);
        }
    }


    private void AnimatePlayer()
    {
        //Animation parameters
        playerAnimator.SetBool("walk", (player.velocity.x != 0 || player.velocity.z != 0) && knockTime<=0);
        playerAnimator.SetBool("jump", jump);
        playerAnimator.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        if (jump == true && currentJump > 0 && Input.GetKey(KeyCode.X))
        {
            //player.AddForce(player.velocity.x*2, jumpHeight - (currentJump/3), player.velocity.z*2, ForceMode.Impulse);
            if (twoD)
            {
                //player.velocity = new Vector3(player.velocity.x, jumpHeight - (currentJump / 2), player.velocity.z);
                player.velocity = new Vector3(input.x * speed, jumpHeight * (currentJump / jumpHeight), 0);
            }
            else if (!twoD)
            {
                Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
                player.velocity = isoMatrix.MultiplyPoint3x4((new Vector3(input.x, 0, input.z).normalized * speed) + new Vector3(0, jumpHeight * (currentJump / jumpHeight), 0));
            }
            currentJump--;

            grounded = false;
        }
        else if (grounded == false)
        {
            currentJump = 0;

            if (currentJump < 1)
            {
                //player.AddForce(player.velocity.x*2 , 0, player.velocity.z*2, ForceMode.Impulse);
                //player.velocity = new Vector3(player.velocity.x, player.velocity.y, player.velocity.z);

                if (twoD)
                {
                    player.velocity = new Vector3(input.x * speed, player.velocity.y, 0);
                }
                else if (!twoD)
                {
                    Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
                    player.velocity = isoMatrix.MultiplyPoint3x4((new Vector3(input.x, 0, input.z).normalized * speed) + new Vector3(0, player.velocity.y, 0));
                }
            }

            jump = false;
        }
    }

    public void Failsafe()
    {
        Ztwo = respawn.z;
        player.position = respawn;
        player.velocity = Vector3.zero;
        playerHP = 3;
    }

    private void OnCollisionEnter(Collision collision)
    {

        jump = false;
        grounded = true;
        currentJump = (int)jumpHeight;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (player.velocity.y < 0)
        {
            jump = false;
            grounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NoSwitch"))
        {
            canSwitch = false;
        }
        else if (other.gameObject.CompareTag("SetZ"))
        {
            Ztwo = other.gameObject.transform.position.z;
        }
        else if (other.gameObject.CompareTag("Entity") || other.gameObject.CompareTag("Entity2D") || other.gameObject.CompareTag("Entity3D"))
        {
            if(iFrames <= 0)
            {
                if (playerHP > 1)
                {
                    playerAnimator.SetBool("damage", true);
                    playerHP--;
                    knockTime = knockTotal;
                    iFrames = totaliFrames;
                    if (other.transform.position.x >= player.position.x)
                    {
                        knockRight = false;
                    }
                    else
                    {
                        knockRight = true;
                    }
                }
                else if(playerHP == 1)
                {
                    playerHP--;
                    knockTime = knockTotal;
                    player.GetComponent<BoxCollider>().isTrigger = true;
                    playerAnimator.SetBool("dead", true);
                    StartCoroutine(GameOver());
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NoSwitch"))
        {
            canSwitch = true;
        }
    }

    public void Respawn()
    {
        player.position = new Vector3(0, -40, 0);
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1);

        playerAnimator.SetBool("dead", false);
        player.GetComponent<BoxCollider>().isTrigger = false;
        Failsafe();
    }
}
