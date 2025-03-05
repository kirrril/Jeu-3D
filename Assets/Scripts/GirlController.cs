using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GirlController : AgentController
{
    private float fleeDistance = 2f;


    protected override IEnumerator ChaseFleePlayer()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 fleeDirection = transform.position - player.position;

        fleeDirection = fleeDirection.normalized * fleeDistance;

        Vector3 fleePosition = transform.position + fleeDirection;


        agent.SetDestination(fleePosition);
    }


    protected override void AttackPlayer()
    {
        SayStopHarrasingMe();
    }

    void SayStopHarrasingMe()
    {
        Debug.Log("Stop Harrasing Me! Help!!");
    }
}