using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public UI gameOver;
    public Rigidbody player;        //player rigidbody component
    public Animator playerAnimator;     //player animator component
    public Transform sprite;        //transform of the sprite child object of the player
    public Transform shadowSprite; //transform of the shadow sprite
    public Transform cameraTf; //transform of the camera
    public float speed;     //player movement speed
    public float gravityForce = 0;      //gravity force
    public float jumpHeight;        //max jump value
    private int currentJump = 0;        //jump value of the current jump
    private int playerHP = 3;       //player hp
    private bool twoD = true;       //2d is true or false
    private bool jump = false;      //can jump at the moment or not
    private bool grounded = true;       //is the player on the ground
    private bool canSwitch = true;      //can gravity be flipped or not
    private Vector3 input;      //vector 3 that contains the directional input
    private float last3D = 0;       //z position of the last flip
    private float Ztwo = 0;     //z position in 2d
    private Vector3 respawn = Vector3.zero;     //vector that defines the respawn location

    public float knockForce;       //force of the knockout when taking damage
    private float knockTime = 0;    //time of the current knockback
    public float knockTotal;        //time a knockback lasts
    private bool knockRight;        //is the knockback coming from the right
    public float totaliFrames;      //time invincibility lasts
    private float iFrames = 0;      //time of the current invincibility


    //Getters for the variables the ui needs access to
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

    public float ZTWO
    {
        get
        {
            return Ztwo;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //initializes needed variables
        sprite = transform.Find("Sprite");
        player = GetComponent<Rigidbody>();
        player.velocity = new Vector3(0, 0, 0);
        twoD = true;
        Time.timeScale = 1;
        last3D = player.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //save movement input
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //boolean twoD determines the current dimension
        if (twoD)
        {
            //sets 2D coordinates (Z should always be 0 in 2D)
            player.position = new Vector3(player.position.x, player.position.y, Ztwo);

            //checks for input to switch dimension to 3D
            if (Input.GetKeyUp(KeyCode.Z))
            {
                if (canSwitch)
                {
                    twoD = false; //if twoD is false the current dimension is 3D               

                    //sets 3D coordinates
                    player.position = new Vector3(player.position.x, player.position.y, last3D);
                }
            }

            
        }
        else if (!twoD) //not twoD means current dimension is 3D
        {
            //Rotates sprite to face camera
            sprite.rotation = Quaternion.Euler(new Vector3(0, cameraTf.rotation.eulerAngles.y, 0));

            //checks for input to switch dimension to 2D
            if (Input.GetKeyUp(KeyCode.Z))
            {
                if (canSwitch)
                {
                    twoD = true; //if twoD is true current dimension is 2D

                    //Rotates sprite to face camera
                    sprite.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                    //saves last Z position for next switch to 3D
                    last3D = player.position.z;

                    //sets 2D coordinates (Z should always be 0 in 2D)
                    player.position = new Vector3(player.position.x, player.position.y, Ztwo);
                }
            }

            
            
        }

        //getting jump input and checking conditions for jumping
        if (Input.GetKeyDown(KeyCode.X) && jump == false && grounded == true)
        {
            jump = true;
        }

        //decreases invincibility time and sets the transparency effect during invincibility
        if(iFrames > 0)
        {
            iFrames--;
            sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        }
        else
        {
            sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }


        PlaceShadow();

        AnimatePlayer();

        //when the player falls below 50 on y calls failsafe
        if (player.position.y <= -50)
        {
            Failsafe();
        }
    }


    //player movement is in fixed update to remain similar despite performance
    private void FixedUpdate()
    {
        //Sets y velocity to 0 when its greater than 0 due to a bug with the gravity
        if (player.velocity.y > 0 && jump == false)
        {

            player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);

        }

        //applys customizable gravity force because rigidbody 3d has no custom gravity
        Vector3 gravity = -gravityForce * Vector3.up;
        player.AddForce(gravity, ForceMode.Acceleration);

        //applies movement in 2D
        if (twoD)
        {
            //checks if there is a current knockback
            if (knockTime <= 0)
            {
                playerAnimator.SetBool("damage", false);
                Movement2D();
            }
            else //applies knockback
            {
                //checks direction to apply knockback in
                if (knockRight)
                {
                    player.velocity = new Vector3(knockForce, knockForce, 0);
                }
                else
                {
                    player.velocity = new Vector3(-knockForce, knockForce, 0);
                }

                //decreases knockback time
                knockTime -= Time.deltaTime;
            }
        }
        else //applies movement in 3D
        {
            if (knockTime <= 0)
            {
                playerAnimator.SetBool("damage", false);
                Movement3D();
            }
            else
            {
                //checks direction to apply knockback in
                if (knockRight)
                {
                    player.velocity = new Vector3(knockForce, knockForce, 0);
                }
                else
                {
                    player.velocity = new Vector3(-knockForce, knockForce, 0);
                }
                //decreases knockback time
                knockTime -= Time.deltaTime;
            }
        }

        Jump();
    }

    //Handles control in 2D state
    private void Movement2D()
    {
        //moves player using the velocity of the rigidbody
        player.velocity = new Vector3(input.x, player.velocity.y, 0).normalized * speed;


        //turns sprite in the proper direction
        if (input.x > 0)
        {
            sprite.localScale = Vector3.one;
        }
        else if (input.x < 0)
        {
            //setting the scale of x to -1 flips the sprite
            sprite.localScale = new Vector3(-1, 1, 1);
        }
    }

    //Handles control in 3D state
    private void Movement3D()
    {
        //multiplying the regular movement vector by a matrix skewed by 45 degrees achieves the proper isometric movement
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, cameraTf.rotation.eulerAngles.y, 0));
        player.velocity = isoMatrix.MultiplyPoint3x4(new Vector3(input.x, player.velocity.y, input.z).normalized * speed);


        //turns sprite in the proper direction
        if (input.x > 0)
        {
            sprite.localScale = Vector3.one;
        }
        else if (input.x < 0)
        {
            //setting the scale of x to -1 flips the sprite
            sprite.localScale = new Vector3(-1, 1, 1);
        }
    }

    //handles animation parameters that depend on what the player is doing at the moment
    private void AnimatePlayer()
    {
        //Animation parameters
        playerAnimator.SetBool("walk", (player.velocity.x != 0 || player.velocity.z != 0) && knockTime<=0);
        playerAnimator.SetBool("jump", jump);
        playerAnimator.SetBool("grounded", grounded);
    }

    //player jumping
    private void Jump()
    {
        //checks if the key is still pressed to make the jump longer
        if (jump == true && currentJump > 0 && Input.GetKey(KeyCode.X))
        {
            //Checks current dimension
            if (twoD)
            {
                //applies jump force in 2D movement
                player.velocity = new Vector3(input.x * speed, jumpHeight * (currentJump / jumpHeight) *2, 0);
            }
            else if (!twoD)
            {
                //applies jump force in 3D movement
                Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, cameraTf.rotation.eulerAngles.y, 0));
                player.velocity = isoMatrix.MultiplyPoint3x4((new Vector3(input.x, 0, input.z).normalized * speed) + new Vector3(0, jumpHeight * (currentJump / jumpHeight) *2, 0));
            }
            //decreases current jump until it reaches 0
            currentJump--;

            //after jump grounded is set to false
            grounded = false;
        }
        else if (grounded == false) //handles falling after jumping
        {
            //sets current jump to 0 to avoid unintended double jumps
            currentJump = 0;

            //checks dimension to apply fall in current dimension
            if (twoD)
            {
                player.velocity = new Vector3(input.x * speed, player.velocity.y, 0);
            }
            else if (!twoD)
            {
                Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, cameraTf.rotation.eulerAngles.y, 0));
                player.velocity = isoMatrix.MultiplyPoint3x4((new Vector3(input.x, 0, input.z).normalized * speed) + new Vector3(0, player.velocity.y, 0));
            }

            //keeps jump as false while landing
            jump = false;
        }
    }

    //Failsafe respawns the player in a determined location that can be altered if checkpoints are implemented
    public void Failsafe()
    {
        Ztwo = respawn.z;
        player.position = respawn;
        player.velocity = Vector3.zero;
        playerHP = 3;
    }

    //collision enter events
    private void OnCollisionEnter(Collision collision)
    {
        //what happens after landing from a jump
        //raycast checks if the collision is below the player
        if (Physics.Raycast(player.position, Vector3.down, this.GetComponent<BoxCollider>().size.y /2 + 0.1f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            jump = false;
            grounded = true;
            currentJump = (int)jumpHeight;
        }
        else if (Physics.Raycast(player.position, Vector3.up, this.GetComponent<BoxCollider>().size.y / 2 + 0.1f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            jump = false;
            grounded = true;
            currentJump = (int)jumpHeight;
        }
    }

    //collision exit events
    private void OnCollisionExit(Collision collision)
    {
        //leaving a collision after jumping
        if (player.velocity.y < 0)
        {
            jump = false;
            grounded = false;
        }
    }

    //trigger enter events
    private void OnTriggerStay(Collider other)
    {
        //event depends on trigger object tag
        switch (other.gameObject.tag)
        {
            //no switch area trigger preventing dimension switching
            case "NoSwitch":
                {
                    canSwitch = false;
                    break;
                }
            //trigger area that defines a new default 2d z axis, allowing for 3d depth
            case "SetZ":
                {
                    Ztwo = other.gameObject.transform.position.z;
                    if (twoD)
                    {
                        last3D = Ztwo;
                    }
                    break;
                }
            //entity triggers that damage the player
            case "Entity":
            case "Entity2D":
            case "Entity3D":
                {
                    if (iFrames <= 0)
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
                        else if (playerHP == 1)
                        {
                            playerHP--;
                            knockTime = knockTotal;
                            player.GetComponent<BoxCollider>().isTrigger = true;
                            playerAnimator.SetBool("dead", true);
                            StartCoroutine(GameOver());
                        }
                    }
                    break;
                }
            case "Respawn":
                {
                    respawn = other.transform.position;
                    break;
                }
        }
        
    }

    //trigger exit events
    private void OnTriggerExit(Collider other)
    {
        //leaving a no switch area allowing dimension switch again
        if (other.gameObject.CompareTag("NoSwitch"))
        {
            canSwitch = true;
        }
    }

    //teleports the player close to the failsafe area for the restart option in the pause menu
    public void Respawn()
    {
        player.position = new Vector3(0, -40, 0);
    }

    //game over event, ienumerator allows to delay code for some time
    //that way the game over animation can play before respawning
    private IEnumerator GameOver()
    {
        //wait for one second
        yield return new WaitForSeconds(1);

        //sets player collider to is trigger allowing to fall through the level in game over state
        playerAnimator.SetBool("dead", false);
        player.GetComponent<BoxCollider>().isTrigger = false;
        StartCoroutine(gameOver.ToLevel(SceneManager.GetActiveScene().buildIndex));
    }

    private void PlaceShadow()
    {
        //A ray is sent downwards placing the shadow where it collides with an object
        if(Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hitInfo, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            shadowSprite.gameObject.SetActive(true);
            shadowSprite.position = hitInfo.point + new Vector3(0, 0.1f, 0);
        }
        else
        {
            shadowSprite.gameObject.SetActive(false);
        }
    }
}
