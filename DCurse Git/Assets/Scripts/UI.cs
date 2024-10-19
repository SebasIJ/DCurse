using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Transform fader;
    public Transform Lock;
    public Animator fadeAnim;
    public Image HP;
    public Image currentD;
    public PlayerScript player;
    public Sprite twoD, threeD, fullHP, midHP, lowHP, emptyHP;

    // Start is called before the first frame update
    void Start()
    {
        fader.gameObject.SetActive(true);
    }



    // Update is called once per frame
    void Update()
    {
        setD();
        setHP();

        Lock.gameObject.SetActive(!player.DLock);
    }

    void setHP()
    {
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

    void setD()
    {
        if (player.dimension)
        {
            currentD.sprite = twoD;
        }
        else
        {
            currentD.sprite = threeD;
        }
    }

    public IEnumerator ToTitle()
    {
        fadeAnim.SetBool("Fading", true);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(0);
    }

    public IEnumerator ToLevel()
    {
        fadeAnim.SetBool("Fading", true);

        yield return new WaitForSecondsRealtime(2);

        SceneManager.LoadScene(1);
    }
}
