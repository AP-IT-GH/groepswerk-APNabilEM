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
    private bool canTurn = true;
    private Rigidbody rigidbody;
    Paddle paddle = new Paddle();
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private int score = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        startingPosition = new Vector3(-124f, -215, -150f); //transform.position;
        startingRotation = Quaternion.Euler(-0f, 0f, 0f);//transform.rotation;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            RequestDecision();
        }
        else if (canTurn)
        {
            RequestDecision();
        }
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
            //AddReward(-0.01f);
            Move();
        }
        else if (vectorAction[0] == 0)
        {
            AddReward(-0.01f);
        }

        if (vectorAction[1] == 1)
        {
            TurnLeft();
        }
        if (vectorAction[1] == 2)
        {
            TurnRight();
        }

    }

    public override void Heuristic(float[] actionsOut)
    {
        //base.Heuristic(actionsOut);
        var move = 0;
        var movementRotation = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        { 
            move = 1;
            //Move();
           
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementRotation = 1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movementRotation = 2;
        }
       
        actionsOut[0] = move;
        actionsOut[1] = movementRotation;
    }


    private void Move()
    {
        if (canMove)
        {
            rigidbody.velocity = transform.forward * (paddle.getSpeed() / 1);
            //transform.position += Vector3.forward * Time.deltaTime * paddle.getSpeed();
            //canMove = false;
        }
    }

    private void TurnLeft()
    {
        float rotationSpeed = paddle.getRotationSpeed();
        float speed = paddle.getSpeed();
        if (canTurn)
        {
            transform.Rotate(new Vector3(0, -rotationSpeed, 0) * Time.deltaTime * speed, Space.World);
        }
    }

    private void TurnRight()
    {
        float rotationSpeed = paddle.getRotationSpeed();
        float speed = paddle.getSpeed();
        if (canTurn)
        {
            transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime * speed, Space.World);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            canMove = true;
        }

        if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Boat"))
        {
            transform.localPosition = startingPosition;//new Vector3(-120f, -215, -150f);
            transform.localRotation = startingRotation;//Quaternion.Euler(-90f, 180f, 0f);
            Debug.Log("collided with obstacle (terrain and/or another boat)");
            AddReward(-1f);
            EndEpisode();
        }
/*
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            Debug.Log("collided with checkpoint");
            AddReward(1f);
        }*/
        /*if (collision.transform.CompareTag("HiddenCollider"))
        {
            Debug.Log("collide with hidden collider");
            AddReward(1f);
        }*/

    }

    private void OnTriggerEnter(Collider collidedObj)
    {
        if (collidedObj.gameObject.CompareTag("Checkpoint"))
        {
            AddReward(1f);
            Debug.Log("Went through the checkpoint");
            score++;
            scoreboard.text = score.ToString();
        }
    }
}
