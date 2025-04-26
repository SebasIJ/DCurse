using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostTree : MonoBehaviour
{
    public class ghostBehavior
    {
        public int beID;
        public ghostBehavior left;
        public ghostBehavior right;
    }

    //IDs
    //0 no behavior
    //1 attack
    //2 move
    //11 shoot
    //12 tackle
    //21 player level
    //22 above player

    private ghostBehavior root;
    private ghostBehavior currentBehavior;
    private Vector3 target;
    private int moveCooldown = 0;
    System.Random RNG = new System.Random();

    public PlayerScript player;
    public Transform sprite;
    public GameObject fireBall;


    // Start is called before the first frame update
    void Start()
    {
        root = new ghostBehavior();
        root.beID = 0;

        ghostBehavior temp = root;
        if (temp.left == null)
        {
            ghostBehavior left1 = new ghostBehavior();
            left1.beID = 1;

            temp.left = left1;

            ghostBehavior temptemp = temp.left;
            if (temptemp.left == null)
            {
                ghostBehavior left11 = new ghostBehavior();
                left11.beID = 11;

                temptemp.left = left11;
            }

            if (temptemp.right == null)
            {
                ghostBehavior right12 = new ghostBehavior();
                right12.beID = 12;

                temptemp.right = right12;
            }
        }

        if (temp.right == null)
        {
            ghostBehavior right2 = new ghostBehavior();
            right2.beID = 2;

            temp.right = right2;

            ghostBehavior temptemp = temp.right;
            if (temptemp.left == null)
            {
                ghostBehavior left21 = new ghostBehavior();
                left21.beID = 21;

                temptemp.left = left21;
            }

            if (temptemp.right == null)
            {
                ghostBehavior right22 = new ghostBehavior();
                right22.beID = 22;

                temptemp.right = right22;
            }
        }

        currentBehavior = root;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > transform.position.x)
        {
            sprite.transform.localScale = new Vector3(-1, 1, 1); ;
        }
        else
        {
            sprite.transform.localScale = new Vector3(1, 1, 1);
        }

        switch (currentBehavior.beID)
        {
            case 0:

                if (RNG.Next(2) == 0)
                {
                    currentBehavior = currentBehavior.left;
                }
                else
                {
                    currentBehavior = currentBehavior.right;
                }

                break;

            case 1:

                if (RNG.Next(2) == 0)
                {
                    currentBehavior = currentBehavior.left;
                }
                else
                {
                    currentBehavior = currentBehavior.right;
                }

                break;

            case 2:

                if (RNG.Next(2) == 0)
                {
                    currentBehavior = currentBehavior.left;
                }
                else
                {
                    currentBehavior = currentBehavior.right;
                }

                break;

            case 11:

                GameObject newFire = Instantiate(fireBall, player.transform.position + new Vector3(0,10,0), player.transform.rotation);
                newFire.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                currentBehavior = root;
                break;

            case 12:

                if (moveCooldown == 0)
                {
                    target = player.transform.position;
                    moveCooldown = 1;
                }

                if (transform.position != target)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, 5 * Time.deltaTime);
                }
                else
                {
                    moveCooldown = 0;
                    currentBehavior = root;
                }

                break;

            case 21:

                if (moveCooldown == 0)
                {
                    if (RNG.Next(2) == 0)
                    {
                        transform.position = new Vector3(player.transform.position.x - 10, player.transform.position.y, player.transform.position.z);
                    }
                    else
                    {
                        transform.position = new Vector3(player.transform.position.x + 10, player.transform.position.y, player.transform.position.z);
                    }
                    moveCooldown = 120;
                }
                else
                {
                    moveCooldown--;

                    if (moveCooldown == 0)
                    {
                        currentBehavior = root;
                    }

                }

                break;

            case 22:

                if (moveCooldown == 0)
                {
                    transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 10, player.transform.position.z);
                    moveCooldown = 120;
                }
                else
                {
                    moveCooldown--;

                    if (moveCooldown == 0)
                    {
                        currentBehavior = root;
                    }

                }

                break;
        }
    }
}
