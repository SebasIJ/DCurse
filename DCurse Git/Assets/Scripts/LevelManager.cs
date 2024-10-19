using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerScript PScript;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDimension(PScript.dimension);
    }

    private void CheckDimension(bool currentD)
    {
        foreach(Transform levelObject in transform)
        {
            switch (currentD)
            {
                case true:
                    {
                        if (levelObject.CompareTag("Ground2D") || levelObject.CompareTag("Entity2D"))
                        {
                            levelObject.gameObject.SetActive(true);
                        }
                        else if (levelObject.CompareTag("Ground3D") || levelObject.CompareTag("Entity3D"))
                        {
                            levelObject.gameObject.SetActive(false);
                        }

                        break;
                    }
                case false:
                    {
                        if (levelObject.CompareTag("Ground2D") || levelObject.CompareTag("Entity2D"))
                        {
                            levelObject.gameObject.SetActive(false);
                        }
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
