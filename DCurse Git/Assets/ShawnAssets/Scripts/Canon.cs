using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{

    public float m_Blammo = 20f;
    public float thrust;
    public GameObject Rocket;
    public Transform RocketSpawn;
    public Rigidbody rb;
    public float ShotFrequency = 2f;
    public AudioSource shootSound;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(ShootRoutine());
        rb = GetComponent<Rigidbody>();
        //rb = GetComponent<Rigidbody>();
        // Rocket rb = GetComponent("Rocket") as Rocket;
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            ShootRocket();
            yield return new WaitForSeconds(ShotFrequency);
        }
    }

    private void ShootRocket()
    {
        GameObject RocketInstance = Instantiate(Rocket, RocketSpawn.transform.position, transform.rotation);//, Quaternion.identity);
        Rigidbody RocketRb = RocketInstance.GetComponent<Rigidbody>();

        if (RocketRb != null)
        {
            RocketRb.AddForce(RocketSpawn.forward * m_Blammo, ForceMode.Impulse);
        }
        shootSound.Play();

        Destroy(RocketRb.gameObject, 5);
    }
    // Update is called once per frame
    void Update()
    {
      
    }

   
}
