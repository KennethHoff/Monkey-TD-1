using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class MonkeyTower : StandardTower {
        
        
        // [SerializeField, Header("Monkey Tower Specifics:")]

        protected override void FixedUpdate() {

            if (GameControl.WaveSpawner.controllerObject.waveActive) {
                generalStats.justShot -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                generalStats.firingCooldown -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                generalStats.aimCooldown -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;

                if (generalStats.justShot < 0) {

                    if (generalStats.aimCooldown < 0)
                        generalStats.aiming = true;
                    else
                        generalStats.aiming = false;

                    if (generalStats.aiming)
                        AttemptTargetting();
                }
            }

            else {
                generalStats.aiming = false;
                generalStats.justShot = -1;
                generalStats.firingCooldown = -1;
                generalStats.aimCooldown = -1;
            }
        }

        protected Collider2D SearchForEnemiesInRange() {

            Collider2D _target = null;

            switch (generalStats.targettingState) {
                case TowerStats.TargettingStates.Close:
                    _target = SearchForClosestEnemy(base.GetEnemiesInRange());
                    break;
                case TowerStats.TargettingStates.First:
                    _target = SearchForFirstEnemy(base.GetEnemiesInRange());
                    break;
                case TowerStats.TargettingStates.Last:
                    _target = SearchForLastEnemy(base.GetEnemiesInRange());
                    break;
                case TowerStats.TargettingStates.Strong:
                    _target = SearchForToughestEnemy(base.GetEnemiesInRange());
                    break;
            }
            return _target;
        }

        protected virtual Collider2D SearchForClosestEnemy(Collider2D[] allCollisions) {
            Collider2D enemy = null;
            float closestEnemyDistance = float.MaxValue;

            foreach (Collider2D target in allCollisions) {
                if (target.tag == "Enemy") {

                    float absDist = Vector2.Distance(target.transform.position, transform.position);
                    if (absDist < closestEnemyDistance) {
                        enemy = target;
                        closestEnemyDistance = absDist;
                    }
                }
            }
            return enemy;
        }

        protected virtual Collider2D SearchForFirstEnemy(Collider2D[] allCollisions) {
            return GetFirstEnemy(allCollisions);
        }

        protected virtual Collider2D SearchForLastEnemy(Collider2D[] allCollisions) {
            return GetLastEnemy(allCollisions);
        }

        private Collider2D GetLastEnemy(Collider2D[] allCollisions) {
            Collider2D enemy = null;
            float lastEnemyDistance = float.MaxValue;
            int lastEnemyWayPoint = int.MaxValue;

            foreach (Collider2D target in allCollisions) {
                if (target.tag == "Enemy") {

                    int currentWayPoint = target.GetComponent<Bloon.WayPoints>().currentWayPoint;

                    Vector2 currentWayPointPos = GameControl.PathController.controllerObject.wayPointList[currentWayPoint].position;

                    float absDist = Vector2.Distance(currentWayPointPos, transform.position);

                    if (currentWayPoint <= lastEnemyWayPoint) {
                        if (absDist <= lastEnemyWayPoint) {
                            lastEnemyWayPoint = currentWayPoint;
                            lastEnemyDistance = absDist;
                            enemy = target;
                        }
                    }
                }
            }
            return enemy;
        }
        protected static Collider2D GetFirstEnemy(Collider2D[] allCollisions) {
            Collider2D enemy = null;
            float firstEnemyDistance = 0;
            int firstEnemyWaypoint = 0;

            foreach (Collider2D target in allCollisions) {
                if (target.tag == "Enemy") {

                    int currentWayPoint = target.GetComponent<Bloon.WayPoints>().currentWayPoint;

                    float distanceToPreviousWaypoint = target.GetComponent<Bloon.WayPoints>().distanceToPreviousWaypoint;

                    if (currentWayPoint > firstEnemyWaypoint) {
                        enemy = target;
                        firstEnemyWaypoint = currentWayPoint;
                        firstEnemyDistance = distanceToPreviousWaypoint;
                    }
                    else if (currentWayPoint == firstEnemyWaypoint) {
                        if (distanceToPreviousWaypoint > firstEnemyDistance) {
                            enemy = target;
                            firstEnemyWaypoint = currentWayPoint;
                            firstEnemyDistance = distanceToPreviousWaypoint;
                        }
                    }
                }
            }
            return enemy;
        }

        protected virtual Collider2D SearchForToughestEnemy(Collider2D[] allCollisions) {
            
            List<Collider2D> toughestBloons = new List<Collider2D>();
            int toughestEnemyHealth = 0;

            foreach (Collider2D target in allCollisions) {
                if (target.tag == "Enemy") {
                    int currentEnemyHealth = target.GetComponent<Bloon.StandardBloon>().RBE;
                    if (toughestEnemyHealth < currentEnemyHealth) {
                        toughestEnemyHealth = currentEnemyHealth;
                        toughestBloons = new List<Collider2D>();
                    }
                    if (toughestEnemyHealth == currentEnemyHealth) {
                        toughestBloons.Add(target);
                    }
                }
            }

            return GetFirstEnemy(toughestBloons.ToArray());
        }

        protected void AttemptTargetting() {

            if (generalStats.attackSpeed < GameControl.GameController.controllerObject.aimTimer)
                generalStats.aimCooldown = generalStats.attackSpeed * 2 / 3;
            else
                generalStats.aimCooldown = GameControl.GameController.controllerObject.aimTimer;

            generalStats.target = SearchForEnemiesInRange();

            if (generalStats.target != null) {
                if (generalStats.justShot <= 0)
                    AimAt(generalStats.target.gameObject);
            }
        }

        protected virtual void AimAt(GameObject target) {

            var dir = target.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + generalStats.rotationOffset;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (generalStats.firingCooldown < 0) {
                Shoot();
            }
        }

        protected override void Shoot() {
            generalStats.firingCooldown = generalStats.attackSpeed;

            base.Shoot();

            if (generalStats.attackSpeed < generalStats.rotationDurationAfterShooting)
                generalStats.justShot = generalStats.attackSpeed * 1 / 3;
            else
                generalStats.justShot = generalStats.rotationDurationAfterShooting;
        }

        
    }
}