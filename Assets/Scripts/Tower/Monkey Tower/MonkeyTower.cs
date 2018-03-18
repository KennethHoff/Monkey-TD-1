using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class MonkeyTower : StandardTower {


        protected enum TargettingStates {
            Closest,
            First,
            Last,
            Toughest
        }
        protected TargettingStates state;


        protected override void FixedUpdate() {

            if (GameControl.WaveSpawner.controllerObject.waveActive) {
                justShot -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                firingCooldown -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                aimCooldown -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                if (justShot < 0) {
                    if (aimCooldown < 0) {
                        targetting = true;
                        aimCooldown = GameControl.GameController.controllerObject.aimTimer;
                        SearchForEnemiesInRange();
                    }
                }
                else targetting = false;
            }
            else {
                targetting = false;
                justShot = -1;
                firingCooldown = -1;
                aimCooldown = -1;
            }
        }


        protected void SearchForEnemiesInRange() {

            Collider2D[] allCollisions = GetEnemiesInRange();

            switch (state) {
                case TargettingStates.Closest:
                    SearchForClosestEnemy(allCollisions);
                    break;
                case TargettingStates.First:
                    break;
                case TargettingStates.Last:
                    break;
                case TargettingStates.Toughest:
                    break;
            }
        }

        protected virtual void SearchForClosestEnemy(Collider2D[] allCollisions) {
            Collider2D closestEnemy = null;
            float closestEnemyDistance = float.MaxValue;

            foreach (Collider2D target in allCollisions) {
                if (target.tag == "Enemy") {

                    /*
                    float xDistToTower = target.transform.position.x - transform.position.x; // X axis
                    float yDistToTower = target.transform.position.y - transform.position.y; // Y Axis
                    float distToTower = Mathf.Pow(xDistToTower, 2) + Mathf.Pow(yDistToTower, 2);

                    float absDistToTower = Mathf.Abs(distToTower);
                    */

                    float absDist = Vector2.Distance(target.transform.position, transform.position);
                    if (absDist < closestEnemyDistance) {
                        closestEnemy = target;
                        closestEnemyDistance = absDist;
                    }
                }
                if (closestEnemy != null) {
                    if (justShot <= 0) AimAt(closestEnemy.gameObject);
                }
            }
        }


        protected virtual void AimAt(GameObject target) {

            var dir = target.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + rotationOffset;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (firingCooldown <= 0) Shoot();
        }

        protected override void Shoot() {
            base.Shoot();
            justShot = rotationDurationAfterShooting;
        }

    }

}