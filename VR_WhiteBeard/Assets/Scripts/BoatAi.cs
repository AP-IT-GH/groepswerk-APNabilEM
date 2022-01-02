using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.MLAgents;
//using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;

public class BoatAi : Agent
{
    // Start is called before the first frame update
    //[SerializeField] private float jumpStrength = 5f;
    public Text scoreboard;
    //[SerializeField] private float speed = 1f;
    private bool canMove = true;
    private Rigidbody rigidbody;
    Paddle paddle = new Paddle();

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
       
    }

    // Update is called once per frame
    void Update()
    {
        scoreboard.text = GetCumulativeReward().ToString("f4");
    }

    public override void OnEpisodeBegin()
    {
        
      //  transform.localPosition = new Vector3(7, 0.5f, 0);
       // transform.localRotation = Quaternion.Euler(0, -90, 0f);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //base.OnActionReceived(vectorAction);
        if (vectorAction[0] == 1)
        {
            //punish with small negative award to prevent jumping all the time
            AddReward(-0.01f);
            Move();
        }

    }

    public override void Heuristic(float[] actionsOut)
    {
        //base.Heuristic(actionsOut);
        var move = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            move = 1;
        }
        
        actionsOut[0] = move;
    }


    private void Move()
    {
        if (canMove)
        {
            rigidbody.velocity = transform.forward * (paddle.getSpeed() / 5);
            canMove = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Water"))
        {
            canMove = true;
        }

        if (collision.transform.CompareTag("Terrain") || collision.transform.CompareTag("Boat"))
        {
            transform.localPosition = new Vector3(-120f, -215, -150f);
            transform.localRotation = Quaternion.Euler(-90f, 180f, 0f);
            Debug.Log("collided with obstacle (terrain and/or another boat)");
            AddReward(-1f);
        }

        if (collision.transform.CompareTag("Checkpoint"))
        {
            Debug.Log("collided with checkpoint");
            AddReward(1f);
        }
        /*if (collision.transform.CompareTag("HiddenCollider"))
        {
            Debug.Log("collide with hidden collider");
            AddReward(1f);
        }*/

    }
}
