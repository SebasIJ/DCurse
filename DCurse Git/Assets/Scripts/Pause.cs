using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject toHide; //ui game object that must be hidden while paused
    public GameObject pauseMenu; //ui game object that contains the pause menu
    private bool isPaused = false; //is the game currently paused or not
    public Animator pauseAnim; //animator of the pause menu
    public PlayerScript player; //movement script of the player
    public UI ui; //main ui script

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //checks for pause button input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //pauses if the game is not currently paused
            //resumes if the game is currently paused
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }

    public void Paused()
    {
        pauseMenu.SetActive(true); //sets pause menu as active
        pauseAnim.SetBool("Paused", true); //plays the pause animation of the puse menu
        toHide.SetActive(false); //deactivates the rest of the ui
        Time.timeScale = 0f; //sets timescale to 0 so the player and other level objects stop moving
        isPaused = true; //game is currently paused
    }

    public void Resume()
    {
        pauseAnim.SetBool("Paused", false); //plays the unpaused animation for the pause menu
        StartCoroutine(animExit()); //starts animexit ienumerator function       
        isPaused = false; //game is no longer paused
    }

    public void Restart()
    {
        pauseAnim.SetBool("Paused", false); //plays the unpaused animation for the pause menu
        StartCoroutine(ExitAndRespawn()); //starts exit and respawn ienumerator function
        isPaused = false; //game is no longer paused
    }

    public void Exit()
    {
        Time.timeScale = 1f; //sets time scale back to normal
        isPaused = false; //game is no longer paused
        StartCoroutine(ui.ToTitle()); //calls totitle function of the main ui script
    }

    //waits for 1 second before unpausing so the unpausing animation can play
    IEnumerator animExit()
    {
        yield return new WaitForSecondsRealtime(1f);        

        //sets ui active and inactive as needed
        pauseMenu.SetActive(false);
        toHide.SetActive(true);

        Time.timeScale = 1f; //time scale is back to normal
    }

    //same as animExit but also calls the respawn function from the player movement script
    IEnumerator ExitAndRespawn()
    {
        yield return new WaitForSecondsRealtime(1f);

        pauseMenu.SetActive(false);

        toHide.SetActive(true);

        Time.timeScale = 1f;

        player.Respawn();
    }
}
