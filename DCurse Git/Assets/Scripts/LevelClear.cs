using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClear : MonoBehaviour
{
    public Animator playerAnim; //the animator component of the player
    public Animator clearUI; //the animator component of the ui
    public UI levelLoad; //the ui script
    public Rigidbody player; //the rigidbody component of the player

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //trigger enter events
    private void OnTriggerEnter(Collider other)
    {
        //when entering the goal object forces player down and sets victory animation
        if (other.gameObject.CompareTag("Goal"))
        {
            player.AddForce(Vector3.down, ForceMode.Impulse);
            playerAnim.SetBool("victory", true);
        }
    }

    //trigger exit event
    private void OnTriggerExit(Collider other)
    {
        //when leaving the goal object destroys it and shows the cleared text ui
        if (other.gameObject.CompareTag("Goal"))
        {
            Destroy(other.gameObject);
            clearUI.gameObject.SetActive(true);
            StartCoroutine(waitLand());
            
        }
    }

    //freezes time to prevent further player movement and waits before starting load level fuction of the ui script
    //that way all animations have time to play properly
    IEnumerator waitLand()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0;
        StartCoroutine(levelLoad.ToLevel(1));
    }
}
