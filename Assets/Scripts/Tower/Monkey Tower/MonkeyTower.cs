using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class MonkeyTower : StandardTower {


        // [SerializeField, Header("Monkey Tower Specifics:")]

        protected override void FixedUpdate() {
            base.FixedUpdate();

            var stats = GetStats<Tower.BaseTowerStats>();

            if (GameControl.WaveSpawner.controllerObject.waveActive) {
                GetStats<Tower.BaseTowerStats>().justShot -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                GetStats<Tower.BaseTowerStats>().firingCooldown -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                GetStats<Tower.BaseTowerStats>().aimCooldown -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;

                // Debug.Log("Cooldown: " + generalStats.firingCooldown + ". Speed: " + generalStats.attackSpeed);

                if (stats.justShot < 0) {

                    if (stats.aimCooldown < 0)
                        stats.aiming = true;
                    else
                        stats.aiming = false;

                    if (stats.aiming)
                        AttemptTargetting();
                }
            }

            else {
                GetStats<Tower.BaseTowerStats>().aiming = false;
                GetStats<Tower.BaseTowerStats>().justShot = -1;
                GetStats<Tower.BaseTowerStats>().firingCooldown = -1;
                GetStats<Tower.BaseTowerStats>().aimCooldown = -1;
            }
        }

        protected Collider2D SearchForEnemiesInRange() {

            Collider2D _target = null;

            var allCollisions = base.GetVisibleEnemiesInRange();

            if (allCollisions == null) {
                Debug.Log("No collisions");
                return null;
            }

            switch (GetStats<Tower.BaseTowerStats>().targettingState) {
                case TowerStats.TargettingStates.Close:
                    _target = SearchForClosestEnemy(allCollisions);
                    break;
                case TowerStats.TargettingStates.First:
                    _target = SearchForFirstEnemy(allCollisions);
                    break;
                case TowerStats.TargettingStates.Last:
                    _target = SearchForLastEnemy(allCollisions);
                    break;
                case TowerStats.TargettingStates.Strong:
                    _target = SearchForToughestEnemy(allCollisions);
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

                    int currentWayPoint = target.GetComponent<Bloon.WayPoints>().currentWayPointInt;

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

                    int currentWayPoint = target.GetComponent<Bloon.WayPoints>().currentWayPointInt;

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

            var stats = GetStats<Tower.BaseTowerStats>();

            if (stats.attackSpeed < GameControl.GameController.controllerObject.aimTimer)
                stats.aimCooldown = stats.attackSpeed * 2 / 3;
            else
                stats.aimCooldown = GameControl.GameController.controllerObject.aimTimer;

            stats.target = SearchForEnemiesInRange();

            if (stats.target != null) {
                AimAt(stats.target.gameObject);
            }
        }

        protected virtual void AimAt(GameObject target) {

            var stats = GetStats<Tower.BaseTowerStats>();

            var dir = target.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + stats.rotationOffset;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (stats.firingCooldown < 0) {
                Shoot();
            }
        }

        protected override void Shoot() {

            var stats = GetStats<Tower.BaseTowerStats>();
            stats.firingCooldown = stats.attackSpeed;

            base.Shoot();

            if (stats.attackSpeed < stats.RotationDurationAfterShooting)
                stats.justShot = stats.attackSpeed * 1 / 3;
            else
                stats.justShot = stats.RotationDurationAfterShooting;
        }


    }
}