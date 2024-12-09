using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Transform fader; //transform of the fade object
    public Transform Lock; //transform of the dimension lock
    public Animator fadeAnim; //animator of the fade object
    public Image HP; //image of the hp
    public Image currentD; //image of the current dimension
    public PlayerScript player; //script of the player movement
    public Sprite twoD, threeD, fullHP, midHP, lowHP, emptyHP; //sprites of all the hp states and dimensions

    public AudioSource fadeAudio;

    // Start is called before the first frame update
    void Start()
    {
        //sets fade active as soon as the scene loads to complete the transition effect
        fader.gameObject.SetActive(true);
    }



    // Update is called once per frame
    void Update()
    {
        setD();
        setHP();

        //sets state of the lock depending on the player DLock
        Lock.gameObject.SetActive(!player.DLock);
    }

    //sets hp properly
    void setHP()
    {
        //checks current hp value of the player and displays corresponding sprite
        switch (player.HP)
        {
            case 3:
                {
                    HP.sprite = fullHP;
                    break;
                }
            case 2:
                {
                    HP.sprite = midHP;
                    break;
                }
            case 1:
                {
                    HP.sprite = lowHP;
                    break;
                }
            default:
                {
                    HP.sprite = emptyHP;
                    break;
                }
        }
    }

    //sets current dimension sprite
    void setD()
    {
        //checks the current dimension from the player script and sets the corresponding sprite
        if (player.dimension)
        {
            currentD.sprite = twoD;
        }
        else
        {
            currentD.sprite = threeD;
        }
    }

    //scene transition to title screen
    public IEnumerator ToTitle()
    {
        fadeAudio.Play();
        fadeAnim.SetBool("Fading", true);

        yield return new WaitForSeconds(2);

        //loads title screen
        SceneManager.LoadScene(0);
    }

    //scene transition for scenes different from the title screen
    public IEnumerator ToLevel(int scene)
    {
        //plays fade animation
        fadeAudio.Play();
        fadeAnim.SetBool("Fading", true);

        //waits for the animation to play
        yield return new WaitForSecondsRealtime(2);

        //loads desired scene
        SceneManager.LoadScene(scene);
    }
}
