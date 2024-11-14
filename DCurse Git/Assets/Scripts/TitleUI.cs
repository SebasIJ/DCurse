using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    private Transform logo; //transform of the logo
    private Transform pressText; //transform of the start text
    private Transform btnPlay; //transform of the play button
    private Transform btnQuit; //transform of the quit button
    public Transform fader; //transform of the scene transition effect
    public Animator fadeAnim; //animator of the scene transition
    private bool started = false; //was the start button pressed yet
    private float currentMovement = 0; //current movement of the objects after starting

    // Start is called before the first frame update
    void Start()
    {
        //finds all transforms on the ui and assigns them to their variables
        logo = transform.Find("Logo");
        pressText = transform.Find("PressStart");
        btnPlay = transform.Find("PlayGame");
        btnQuit = transform.Find("Quit");

        //sets initial posittion of all ui objects
        pressText.gameObject.SetActive(false);
        logo.position = new Vector3(logo.position.x, 2000, logo.position.z);
        btnPlay.gameObject.SetActive(false);
        btnPlay.position = new Vector3(btnPlay.position.x + 550, btnPlay.position.y, btnPlay.position.z);
        btnQuit.position = new Vector3(btnQuit.position.x + 550, btnQuit.position.y, btnQuit.position.z);
        fader.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //checks if the start button has been pressed or not
        if (started)
        {
            //after start button is pressed moves buttons into screen
            if(currentMovement < 550)
            {
                logo.position = new Vector3(logo.position.x - 5, logo.position.y, logo.position.z);
                btnPlay.position = new Vector3(btnPlay.position.x - 5, btnPlay.position.y, btnPlay.position.z);
                btnQuit.position = new Vector3(btnQuit.position.x - 5, btnQuit.position.y, btnQuit.position.z);
                currentMovement += 5;
            }
        }
        else
        {
            //moves logo into visible centered position
            if (logo.position.y > 1150)
            {
                logo.position = new Vector3(logo.position.x, logo.position.y - 6, logo.position.z);
            }
            else
            {
                logo.position = new Vector3(logo.position.x, 1150, logo.position.z);
                pressText.gameObject.SetActive(true);
            }
        }
    }

    //start button pressed event
    public void Started()
    {
        //sets started to true and activates the othe buttons
        started = true;
        pressText.gameObject.SetActive(false);
        btnPlay.gameObject.SetActive(true);
    }

    //pressed event for the other buttons
    public void ButtonPress(string Button)
    {
        switch (Button)
        {
            case "PlayGame":
                {
                    StartCoroutine(PlayGame());
                    break;
                }
            case "Quit":
                {
                    StartCoroutine(QuitGame());
                    break;
                }
        }
    }

    //plays fade animation and transitions scene
    IEnumerator PlayGame()
    {
        fadeAnim.SetBool("Fading", true);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(1);
    }

    //plays fade animation and exits game
    IEnumerator QuitGame()
    {
        fadeAnim.SetBool("Fading", true);

        yield return new WaitForSeconds(2);

        Application.Quit();
    }
}
