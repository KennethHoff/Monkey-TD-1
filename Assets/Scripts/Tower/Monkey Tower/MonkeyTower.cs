using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class MonkeyTower : StandardTower {

        protected Collider2D target;

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
                        target = SearchForEnemiesInRange();

                        if (target != null) {
                            if (justShot <= 0) AimAt(target.gameObject);
                        }
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


        protected Collider2D SearchForEnemiesInRange() {

            Collider2D[] allCollisions = GetEnemiesInRange();
            Collider2D _target = null;

            switch (state) {
                case TargettingStates.Closest:
                    _target = SearchForClosestEnemy(allCollisions);

                    break;
                case TargettingStates.First:
                    break;
                case TargettingStates.Last:
                    break;
                case TargettingStates.Toughest:
                    break;
            }
            return _target;
        }

        protected virtual Collider2D SearchForClosestEnemy(Collider2D[] allCollisions) {
            Collider2D closestEnemy = null;
            float closestEnemyDistance = float.MaxValue;

            foreach (Collider2D target in allCollisions) {
                if (target.tag == "Enemy") {

                    float absDist = Vector2.Distance(target.transform.position, transform.position);
                    if (absDist < closestEnemyDistance) {
                        closestEnemy = target;
                        closestEnemyDistance = absDist;
                    }
                }
            }
            return closestEnemy;
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