using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointDestroy : MonoBehaviour
{
    private BoatAi boatAi = new BoatAi();
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (boatAi.getCollided())
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
