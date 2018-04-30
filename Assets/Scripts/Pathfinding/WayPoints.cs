using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bloon { 
    public class WayPoints : MonoBehaviour {
        // TODO: Find a way to check if Transform is under an "Underpass" Collider (but not on the Path above it), and if it is, make it invisible, untargettable and invincible. (Layer a photo of the underpass over them.

        public Bloon.StandardBloon bloon;
        public int currentWayPointInt = 0;
        public float distanceToPreviousWaypoint = 0;
        Transform targetWayPoint;

        private float moveSpdOffSet; // In order to have the "MoveSpd" variable be 1 = Red bloon Speed (easier to adjust it that way)
        private float actualMoveSpd; // Base + offset

        public float spdMultiplier = 1f; // In cases where the bloons are slowed by various in-game means (ice etc...)

        [SerializeField]
        private float finalMoveSpd;

        public enum Direction { Left, Right, Up, Down }

        public Direction dir;
    
        void Start() {
            bloon = GetComponent<Bloon.StandardBloon>();
            GameControl.PathController.controllerObject = FindObjectOfType<GameControl.PathController>();
            moveSpdOffSet = 2f; // Do not change
            actualMoveSpd = bloon.moveSpd * moveSpdOffSet;
        }
    
        void FixedUpdate() {
            finalMoveSpd = actualMoveSpd * spdMultiplier * GameControl.GameController.controllerObject.currentGameSpeed * Time.fixedDeltaTime;
            StartMoving();

        }

        void Walk() {

            if (currentWayPointInt > 0)
                distanceToPreviousWaypoint = Vector2.Distance(transform.position, GameControl.PathController.controllerObject.wayPointList[currentWayPointInt-1].position);

            Vector3 newMove = MoveTowardsNew(transform.position, targetWayPoint.position, finalMoveSpd);
            //if (newMove == Vector3.zero) {
            //    newMove = MoveTowardsNew(transform.position, GameControl.PathController.controllerObject.wayPointList[currentWayPoint + 1].position, finalMoveSpd);
            //}
            transform.position = newMove;

            /*
            float xDist = targetWayPoint.position.x - transform.position.x; // X axis
            float yDist = targetWayPoint.position.y - transform.position.y; // Y Axis
            float dist = Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2);
            */

            float dist = Vector2.Distance(targetWayPoint.position, transform.position);

            if (dist < finalMoveSpd) {
                currentWayPointInt++;
                targetWayPoint = null;
            }
        }


        private void FinalDestinationReached() {
            GetComponent<Bloon.StandardBloon>().FinalDestinationReached();
        }

        private void StartMoving() {

            if (currentWayPointInt < GameControl.PathController.controllerObject.wayPointList.Count) {
                if (targetWayPoint == null) { 
                    targetWayPoint = GameControl.PathController.controllerObject.wayPointList[currentWayPointInt];
                }
                Walk();
            }
            else {
                FinalDestinationReached();
            }
        }


        public Vector3 MoveTowardsNew(Vector3 current, Vector3 target, float maxDistanceDelta) {
            Vector3 a = target - current;
            float magnitude = a.magnitude;
            if (magnitude <= maxDistanceDelta || magnitude == 0f) {
                return MoveTowardsNew(transform.position, GameControl.PathController.controllerObject.wayPointList[currentWayPointInt + 1].position, finalMoveSpd);
            }
            return current + a / magnitude * maxDistanceDelta;
        }
    }
}