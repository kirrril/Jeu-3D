using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbCopy : MonoBehaviour
{
    // public void Climb()
	// {
	// 	float maxHeight = GetMaxClimbHeight();

	// 	spotLight.transform.LookAt(transform.position);
	// 	spotLight.transform.position = spotLightPosition.position;



	// 	if (Input.GetKey(KeyCode.Space))
	// 	{
	// 		canWalk = false;
	// 		isOnTheFloor = false;

	// 		isClimbing = true;
	// 		isClimbingUp = true;

	// 		if (!isInitializedAtClimbingPosition)
	// 		{
	// 			rb.MovePosition(climbingPosition.transform.position);
	// 			rb.MoveRotation(climbingPosition.transform.rotation);
	// 			isInitializedAtClimbingPosition = true;
	// 		}


	// 		if (transform.position.y >= maxHeight)
	// 		{
	// 			maxwHeightReached = true;
	// 		}

	// 		if (maxwHeightReached)
	// 		{
	// 			climbSpeed = 1f;

	// 			rb.velocity = -Vector3.up * climbSpeed;

	// 			isClimbing = true;
	// 			isClimbingUp = false;
	// 		}
	// 		else
	// 		{
	// 			climbSpeed = 1.5f;

	// 			rb.velocity = Vector3.up * climbSpeed;

	// 			isClimbing = true;
	// 			isClimbingUp = true;
	// 		}
	// 	}
	// 	else
	// 	{
	// 		isClimbing = true;
	// 		isClimbingUp = false;

	// 		maxwHeightReached = false;

	// 		climbSpeed = 3f;

	// 		if (transform.position.y > climbGroundY)
	// 		{
	// 			rb.velocity = -Vector3.up * climbSpeed;
	// 		}
	// 		else
	// 		{
	// 			GetReadyToClimb();
	// 		}
	// 	}
	// }

	// float GetMaxClimbHeight()
	// {
	// 	float backTraining = GameManager.instance.currentPlayer.backTraining;
	// 	if (backTraining < 0.2f) return -2.5f;
	// 	if (backTraining < 0.5f) return -2.5f;
	// 	if (backTraining < 0.8f) return -2.5f;
	// 	return float.MaxValue;
	// }

	// void GetReadyToClimb()
	// {
	// 	isInClimbingZone = true;

	// 	if (!isOnTheFloor)
	// 	{
	// 		rb.MovePosition(stopClimbingPosition.transform.position);
	// 		rb.MoveRotation(stopClimbingPosition.transform.rotation);
	// 		isOnTheFloor = true;
	// 	}

	// 	canWalk = false;
	// 	isClimbing = false;
	// 	isClimbingUp = false;

	// 	maxwHeightReached = false;

	// 	isInitializedAtClimbingPosition = false;

	// 	StartCoroutine(CanWalkCorout());
	// }

}
