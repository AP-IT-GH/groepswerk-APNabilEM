using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public GameObject leftSide;
    public GameObject rightSide;
    //public GameObject paddle;
    public GameObject boat;
    public Rigidbody boatsRigidbody;
    public float speed = 1f;
    public float rotationSpeed = 15f;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == leftSide.gameObject.name)
        {
            boatsRigidbody.velocity = transform.forward * speed;
            boat.transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime * speed, Space.World);
            //transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime * speed, Space.World);
            //transform.Rotate(0, speed, 0);
        }
        if (other.gameObject.name == rightSide.gameObject.name)
        {
            
            boatsRigidbody.velocity = transform.forward * speed;
            boat.transform.Rotate(new Vector3(0, -rotationSpeed, 0) * Time.deltaTime * speed, Space.World);
            //transform.Rotate(new Vector3(0, -speed, 0) * Time.deltaTime * speed, Space.World);
            //transform.Rotate(0, speed, 0);
        }
    }
    
}
