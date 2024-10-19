using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClear : MonoBehaviour
{
    public Animator playerAnim;
    public Animator clearUI;
    public UI levelLoad;
    public Rigidbody player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            player.AddForce(Vector3.down, ForceMode.Impulse);
            playerAnim.SetBool("victory", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            Destroy(other.gameObject);
            clearUI.gameObject.SetActive(true);
            StartCoroutine(waitLand());
            
        }
    }

    IEnumerator waitLand()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0;
        StartCoroutine(levelLoad.ToLevel());
    }
}
