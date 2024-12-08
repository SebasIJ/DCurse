using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneLoader : MonoBehaviour
{
    public float sceneDuration;
    public int loadScene;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        sceneDuration -= Time.deltaTime;

        if(sceneDuration <= 0)
        {
            SceneManager.LoadScene(loadScene);
        }
    }
}
