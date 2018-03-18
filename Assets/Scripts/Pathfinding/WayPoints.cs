using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour {
    // TODO: Find a way to check if Transform is under an "Underpass" Collider (but not on the Path above it), and if it is, make it invisible, untargettable and invincible.

    private GameControl.PathController pc;
        
    public int currentWayPoint = 0;
    Transform targetWayPoint;

    [SerializeField]
    private float moveSpd = 1f; // Base movespeed
    private float moveSpdOffSet; // In order to have the "MoveSpd" variable be 1 = Red bloon Speed (easier to adjust it that way)
    private float actualMoveSpd; // Base + offset

    public float spdMultiplier = 1f; // In cases where the bloons are slowed by various in-game means (ice etc...)

    [SerializeField]
    private float finalMoveSpd;

    public enum Direction { Left, Right, Up, Down }

    public Direction dir;
    
    void Start() {
        pc = FindObjectOfType<GameControl.PathController>();
        moveSpdOffSet = 2f; // Do not change
        actualMoveSpd = moveSpd * moveSpdOffSet;

        finalMoveSpd = actualMoveSpd * spdMultiplier * GameControl.GameController.controllerObject.currentGameSpeed * Time.fixedDeltaTime;
        StartMoving();
    }
    
    void FixedUpdate() {
        finalMoveSpd = actualMoveSpd * spdMultiplier * GameControl.GameController.controllerObject.currentGameSpeed * Time.fixedDeltaTime;
        StartMoving();

    }

    void NewWalk() {

        Vector3 thing = MoveTowardsNew(transform.position, targetWayPoint.position, finalMoveSpd);
        if (thing == Vector3.zero) {
            thing = MoveTowardsNew(transform.position, pc.wayPointList[currentWayPoint + 1].position, finalMoveSpd);
        }
        transform.position = thing;


        float xDist = targetWayPoint.position.x - transform.position.x; // X axis
        float yDist = targetWayPoint.position.y - transform.position.y; // Y Axis
        float dist = Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2);

        if (dist < finalMoveSpd) {
            currentWayPoint++;
            targetWayPoint = pc.wayPointList[currentWayPoint];
        }
    }

    void Walk() {
        Debug.Log("Moving...");
        // move towards the target
        // transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, finalMoveSpd);


        if (targetWayPoint.position.x - transform.position.x > 0) {
            dir = Direction.Right;
        }
        else if (targetWayPoint.position.x - transform.position.x < 0) {
            dir = Direction.Left;
        }
        else if (targetWayPoint.position.y - transform.position.y > 0) {
            dir = Direction.Down;
        }
        else if (targetWayPoint.position.y- transform.position.y < 0) {
            dir = Direction.Up;
        }

        if (transform.position == targetWayPoint.position) {
            currentWayPoint++;
            targetWayPoint = pc.wayPointList[currentWayPoint];
        }
    }

    private void FinalDestinationReached() {
        GetComponent<Bloon.StandardBloon>().FinalDestinationReached();
    }

    private void StartMoving() {
        // Check if we have somewhere to walk

        if (currentWayPoint < pc.wayPointList.Count - 1) {
            if (targetWayPoint == null)
                targetWayPoint = pc.wayPointList[currentWayPoint];
            // Walk();
            NewWalk();
        }
        else
            FinalDestinationReached();
    }



    public Vector3 MoveTowardsNew(Vector3 current, Vector3 target, float maxDistanceDelta) {
        Vector3 a = target - current;
        float magnitude = a.magnitude;
        if (magnitude <= maxDistanceDelta || magnitude == 0f) {
            return Vector3.zero;
        }
        return current + a / magnitude * maxDistanceDelta;
    }
}