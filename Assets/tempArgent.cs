using UnityEngine;
using UnityEngine.AI;

public class tempArgent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    NavMeshAgent agent;
    
    void Start()
    
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(new Vector3(0, 0, transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
