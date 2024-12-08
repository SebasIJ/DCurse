using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostScript : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent ghost;

    // Start is called before the first frame update
    void Start()
    {
        ghost = GetComponent<NavMeshAgent>();        
    }

    // Update is called once per frame
    void Update()
    {
        ghost.destination = target.position;
    }
}
