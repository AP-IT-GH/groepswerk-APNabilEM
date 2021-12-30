using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BoatMotion : MonoBehaviour
{
    // Start is called before the first frame update
   
    
   /* [SerializeField] private float speed = 1;
    public GameObject leftPlane;
    public GameObject rightPlane;
    public GameObject paddle;
    public GameObject playerBoat;
    public float yAngle;*/
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       /* if (speed >= 2)
        {
            speed -= 0.05F;
        }
        else if (speed <= 1)
        {
            speed = 1;
        }*/
    }

    private void OnCollisionEnter(Collision other)
    {
      
        
            //Check to see if the tag on the collider is equal to Enemy
            if (other.gameObject.tag == "LeftSide")
            {
                Debug.Log("Triggered by left side");
            } 

            else if (other.gameObject.tag == "RightSide")
            {
                
                Debug.Log("Triggered by right side");
            }
        

 /*       playerBoat.transform.Rotate(0, yAngle * 1.05F, 0, Space.Self);
        speed = speed * 1.1F;
        if (speed >= 2)
        {
            speed = 2;
        }
        transform.position += transform.forward * Time.deltaTime * speed;*/
    }
}

