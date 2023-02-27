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
    private Transform goalPos;
    private Vector2 startPos;

    private float preDist;

    private Rigidbody2D rb;

    public override void Initialize()
    {
        base.Initialize();
        rb = GetComponent<Rigidbody2D>();
        goalPos = GameObject.Find("Goal").transform;
        agentPos = transform;
        startPos = transform.position;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentPos.position);
        sensor.AddObservation(goalPos.position);
        sensor.AddObservation(rb.velocity);
    }

    public override void OnActionReceived(ActionBuffers actionsBuffer)
    {
        AddReward(-0.01f);

        var actions = actionsBuffer.ContinuousActions;

        float moveY = Mathf.Clamp(actions[0], -1, 1f);
        float moveX = Mathf.Clamp(actions[1], -1, 1f);

        Vector3 vel = new Vector3(moveX, moveY, 0).normalized;
        rb.velocity = vel * speed;

        float distance = Vector3.Magnitude(goalPos.position - agentPos.position);
        if (distance <= 0.5f)
        {
            SetReward(10f);
            EndEpisode();
        }
        else
        {
            float reward = preDist - distance;
            AddReward(reward);
            preDist = distance;
        }
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        GameManager.Instance.SetDeathText();
        agentPos.position = startPos;
        preDist = Vector3.Magnitude(goalPos.position - agentPos.position);
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
