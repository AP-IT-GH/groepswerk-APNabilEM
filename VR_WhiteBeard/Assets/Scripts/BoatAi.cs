using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.MLAgents;
//using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;
using Unity.MLAgents.Actuators;
using System.Linq;

public class BoatAi : Agent
{
    // Start is called before the first frame update
    public Text scoreboard;
    private bool canMove = true;
    private bool canTurn = true;
    private Rigidbody rigidbody;
    Paddle paddle = new Paddle();
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private int score = 0;
    public float speed = 1f;
    [SerializeField] private Transform position;
    private bool collided = false;
    private int aantalCheckpoints = 0;
    private List<string> checkpoints = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //startingPosition = new Vector3(-124.42f, -215, -100f); //transform.position;
        //startingRotation = Quaternion.Euler(-0f, -90f, 0f);//transform.rotation;

        startingPosition = position.localPosition;
        startingRotation = position.localRotation;
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
        if (aantalCheckpoints > 4)
        {
            AddReward(-5f);
            EndEpisode();
            aantalCheckpoints = 0;
        }
        /*if (checkpoints.Count != checkpoints.Distinct().Count())
        {
            // Duplicates exist
            AddReward(-2f);
            EndEpisode();
            checkpoints.Clear();
            transform.localPosition = startingPosition;
            transform.localRotation = startingRotation;
        }*/
        /*if (checkpoints.Contains(gameObject.name))
        {
            AddReward(-2f);
            EndEpisode();
            checkpoints.Clear();
            transform.localPosition = startingPosition;
            transform.localRotation = startingRotation;
        }*/

       /* if (checkpoints.Count >= 2)
        {
            for (int i = 0; i < checkpoints.Count; i++)
            {
                if (checkpoints[i] == checkpoints[checkpoints.Count -1])
                {
                    AddReward(-5f);
                    EndEpisode();
                    transform.localPosition = startingPosition;
                    transform.localRotation = startingRotation;
                }
            }
        }*/

        /*if (position.rotation.y >= 180 || position.rotation.y <= -180)
        {
            AddReward(-3f);
            EndEpisode();
            transform.localPosition = startingPosition;
            transform.localRotation = startingRotation;
        }*/
        scoreboard.text = GetCumulativeReward().ToString("f4");
    }

    public override void OnEpisodeBegin()
    {
        collided = false;
        // transform.localPosition = new Vector3(7, 0.5f, 0);
        // transform.localRotation = Quaternion.Euler(0, -90, 0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorAction = actions.DiscreteActions;
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
    

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //base.Heuristic(actionsOut);
        var move = 0;
        var movementRotation = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        { 
            move = 1;           
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementRotation = 1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movementRotation = 2;
        }

        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = move;
        discreteActionsOut[1] = movementRotation;
    }


    private void Move()
    {
        if (canMove)
        {
            rigidbody.velocity = transform.forward * paddle.getSpeed() * speed;
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
            collided = true;
            //checkpoints.Clear();
            transform.localPosition = startingPosition;
            transform.localRotation = startingRotation;
            Debug.Log("collided with obstacle (terrain and/or another boat)");
            AddReward(-1f);
            EndEpisode();
        }
        

    }

    private void OnTriggerEnter(Collider collidedObj)
    {
        if (collidedObj.gameObject.CompareTag("Checkpoint"))
        {
            checkpoints.Add(gameObject.name);
            AddReward(1f);
            aantalCheckpoints++;
            Debug.Log("Went through the checkpoint");
            score++;
            scoreboard.text = score.ToString();
        }
        
        /**/
        if (collidedObj.gameObject.CompareTag("Finish"))
        {
            checkpoints.Add(gameObject.name);
            aantalCheckpoints++;
            transform.localPosition = startingPosition;
            transform.localRotation = startingRotation;
            score++;
            scoreboard.text = score.ToString();
            Debug.Log("Went through the finishline");
            AddReward(10f);
            EndEpisode();
        }
        /**/

    }


    public bool getCollided()
    {
        return collided;
    }
}
