using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MoveToGoalAgent : Agent
{
    public float moveSpeed = 1f;
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = actions.ContinuousActions[0];
        float rotateY_Direction = actions.ContinuousActions[1];
        float rotateX_Tilt = actions.ContinuousActions[2];

        transform.position += new Vector3(move, 0, 0) * Time.deltaTime * moveSpeed;
    }
}
