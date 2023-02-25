using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{
    [SerializeField]
    private float speed = 5f;

    private Transform agentPos;
    private Vector2 startPos;

    private Rigidbody2D rb;

    public override void Initialize()
    {
        base.Initialize();
        rb = GetComponent<Rigidbody2D>();
        agentPos = transform;
        startPos = transform.position;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentPos.position);
        sensor.AddObservation(rb.velocity);
    }

    Vector3 vel = Vector3.zero;
    public override void OnActionReceived(ActionBuffers actionsBuffer)
    {
        AddReward(-0.01f);

        var actions = actionsBuffer.ContinuousActions;

        float moveY = Mathf.Clamp(actions[0], -1, 1f);
        float moveX = Mathf.Clamp(actions[1], -1, 1f);

        vel = new Vector3(moveX, moveY, 0).normalized;
        rb.velocity = vel * speed;
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        agentPos.position = startPos;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionOut = actionsOut.ContinuousActions;

        continuousActionOut[0] = Input.GetAxisRaw("Vertical");
        continuousActionOut[1] = Input.GetAxisRaw("Horizontal");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            SetReward(-1f);
            GameManager.Instance.death += 1;
            EndEpisode();
        }
        else if(collision.gameObject.CompareTag("Goal"))
        {
            AddReward(1f);
            GameManager.Instance.NextScene();
        }
    }

    public float DecisionWaitingTime = 5;
    float m_currentTime = 0;

    public void WaitTimeInference(int action)
    {
        if (Academy.Instance.IsCommunicatorOn)
        {
            RequestDecision();
        }
        else
        {
            if (m_currentTime >= DecisionWaitingTime)
            {
                m_currentTime = 0f;
                RequestDecision();
            }
            else
            {
                m_currentTime += Time.deltaTime;
            }
        }
    }
}
