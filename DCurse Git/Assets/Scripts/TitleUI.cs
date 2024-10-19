using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    private Transform logo;
    private Transform pressText;
    private Transform btnPlay;
    private Transform btnQuit;
    public Transform fader;
    public Animator fadeAnim;
    private bool started = false;
    private bool faded;
    private float currentMovement = 0;

    // Start is called before the first frame update
    void Start()
    {
        logo = transform.Find("Logo");
        pressText = transform.Find("PressStart");
        btnPlay = transform.Find("PlayGame");
        btnQuit = transform.Find("Quit");

        pressText.gameObject.SetActive(false);
        logo.position = new Vector3(logo.position.x, 2000, logo.position.z);
        btnPlay.gameObject.SetActive(false);
        btnPlay.position = new Vector3(btnPlay.position.x + 550, btnPlay.position.y, btnPlay.position.z);
        btnQuit.position = new Vector3(btnQuit.position.x + 550, btnQuit.position.y, btnQuit.position.z);
        fader.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
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
            if (logo.position.y > 1150)
            {
                logo.position = new Vector3(logo.position.x, logo.position.y - 2, logo.position.z);
            }
            else
            {
                logo.position = new Vector3(logo.position.x, 1150, logo.position.z);
                pressText.gameObject.SetActive(true);
            }
        }
    }

    public void Started()
    {
        started = true;
        pressText.gameObject.SetActive(false);
        btnPlay.gameObject.SetActive(true);
    }

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

    IEnumerator PlayGame()
    {
        fadeAnim.SetBool("Fading", true);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(1);
    }

    IEnumerator QuitGame()
    {
        fadeAnim.SetBool("Fading", true);

        yield return new WaitForSeconds(2);

        Application.Quit();
    }
}
