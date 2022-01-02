using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BoatAi : Agent
{
    // Start is called before the first frame update
    //[SerializeField] private float jumpStrength = 5f;
    [SerializeField] private TextMeshPro scoreboard;

    private bool canMove = true;
    private Rigidbody rigidbody;

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
        
        transform.localPosition = new Vector3(7, 0.5f, 0);
        transform.localRotation = Quaternion.Euler(0, -90, 0f);
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorAction = actions.DiscreteActions;
        if (vectorAction[0] == 1)
        {
            //punish with small negative award to prevent jumping all the time
            AddReward(-0.01f);
            Jump();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //map actions to movement
        var jump = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            jump = 1;
        }
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = jump;
    }



    private void Jump()
    {
        if (canMove)
        {
            rigidbody.AddForce(new Vector3(0, jumpStrength, 0), ForceMode.VelocityChange);
            canMove = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Water"))
        {
            canMove = true;
        }

        if (collision.transform.CompareTag("Terrain") || collision.transform.CompareTag("Player"))
        {
            Debug.Log("collide with obstacle");
            Destroy(collision.gameObject);
            AddReward(-1f);
        }

        if (collision.transform.CompareTag("Checkpoint"))
        {
            Debug.Log("collide with good obstacle");
            Destroy(collision.gameObject);
            AddReward(1f);
        }
        /*if (collision.transform.CompareTag("HiddenCollider"))
        {
            Debug.Log("collide with hidden collider");
            AddReward(1f);
        }*/

    }
}
