using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject toHide;
    public GameObject pauseMenu;
    private bool isPaused = false;
    public Animator pauseAnim;
    public PlayerScript player;
    public UI ui;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        pauseMenu.SetActive(true);
        pauseAnim.SetBool("Paused", true);
        toHide.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseAnim.SetBool("Paused", false);
        StartCoroutine(animExit());       
        isPaused = false;
    }

    public void Restart()
    {
        pauseAnim.SetBool("Paused", false);
        StartCoroutine(ExitAndRespawn());
        isPaused = false;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        isPaused = false;
        StartCoroutine(ui.ToTitle());
    }

    IEnumerator animExit()
    {
        yield return new WaitForSecondsRealtime(1f);        

        pauseMenu.SetActive(false);

        toHide.SetActive(true);

        Time.timeScale = 1f;
    }

    IEnumerator ExitAndRespawn()
    {
        yield return new WaitForSecondsRealtime(1f);

        pauseMenu.SetActive(false);

        toHide.SetActive(true);

        Time.timeScale = 1f;

        player.Respawn();
    }
}
