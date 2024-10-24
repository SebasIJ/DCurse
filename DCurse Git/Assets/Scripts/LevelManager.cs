using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerScript PScript; //player script to get access to the current dimension boolean


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //loads the level objects depending on the current dimension
        CheckDimension(PScript.dimension);
    }

    private void CheckDimension(bool currentD)
    {
        //checks all child objects of the object the script is attached to 
        foreach(Transform levelObject in transform)
        {
            switch (currentD)
            {
                //when the current dimension is 2D
                case true:
                    {
                        //sets objects tagged as 2d only active
                        if (levelObject.CompareTag("Ground2D") || levelObject.CompareTag("Entity2D"))
                        {
                            levelObject.gameObject.SetActive(true);
                        }
                        //sets objects tagged as 3d only inactive
                        else if (levelObject.CompareTag("Ground3D") || levelObject.CompareTag("Entity3D"))
                        {
                            levelObject.gameObject.SetActive(false);
                        }

                        break;
                    }
                //When the current dimension is 3D
                case false:
                    {
                        //sets all objects tagged as 2d only inactive
                        if (levelObject.CompareTag("Ground2D") || levelObject.CompareTag("Entity2D"))
                        {
                            levelObject.gameObject.SetActive(false);
                        }
                        //sets objects tagged as 3d only active
                        else if (levelObject.CompareTag("Ground3D") || levelObject.CompareTag("Entity3D"))
                        {
                            levelObject.gameObject.SetActive(true);
                        }

                        break;
                    }
            }
        }
    }
}
