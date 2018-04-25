using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class MonkeyTower : StandardTower {


        // [SerializeField, Header("Monkey Tower Specifics:")]

        protected override void FixedUpdate() {

            if (GameControl.WaveSpawner.controllerObject.waveActive) {
                GetStats<Tower.BaseTowerStats>().justShot -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                GetStats<Tower.BaseTowerStats>().firingCooldown -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                GetStats<Tower.BaseTowerStats>().aimCooldown -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;

                // Debug.Log("Cooldown: " + generalStats.firingCooldown + ". Speed: " + generalStats.attackSpeed);

                if (GetStats<Tower.BaseTowerStats>().justShot < 0) {

                    if (GetStats<Tower.BaseTowerStats>().aimCooldown < 0)
                        GetStats<Tower.BaseTowerStats>().aiming = true;
                    else
                        GetStats<Tower.BaseTowerStats>().aiming = false;

                    if (GetStats<Tower.BaseTowerStats>().aiming)
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

            switch (GetStats<Tower.BaseTowerStats>().targettingState) {
                case TowerStats.TargettingStates.Close:
                    _target = SearchForClosestEnemy(base.GetVisibleEnemiesInRange());
                    break;
                case TowerStats.TargettingStates.First:
                    _target = SearchForFirstEnemy(base.GetVisibleEnemiesInRange());
                    break;
                case TowerStats.TargettingStates.Last:
                    _target = SearchForLastEnemy(base.GetVisibleEnemiesInRange());
                    break;
                case TowerStats.TargettingStates.Strong:
                    _target = SearchForToughestEnemy(base.GetVisibleEnemiesInRange());
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

            if (GetStats<Tower.BaseTowerStats>().attackSpeed < GameControl.GameController.controllerObject.aimTimer)
                GetStats<Tower.BaseTowerStats>().aimCooldown = GetStats<Tower.BaseTowerStats>().attackSpeed * 2 / 3;
            else
                GetStats<Tower.BaseTowerStats>().aimCooldown = GameControl.GameController.controllerObject.aimTimer;

            GetStats<Tower.BaseTowerStats>().target = SearchForEnemiesInRange();

            if (GetStats<Tower.BaseTowerStats>().target != null) {
                if (GetStats<Tower.BaseTowerStats>().justShot <= 0)
                    AimAt(GetStats<Tower.BaseTowerStats>().target.gameObject);
            }
        }

        protected virtual void AimAt(GameObject target) {

            var dir = target.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + GetStats<Tower.BaseTowerStats>().rotationOffset;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (GetStats<Tower.BaseTowerStats>().firingCooldown < 0) {
                Shoot();
            }
        }

        protected override void Shoot() {
            GetStats<Tower.BaseTowerStats>().firingCooldown = GetStats<Tower.BaseTowerStats>().attackSpeed;

            base.Shoot();

            if (GetStats<Tower.BaseTowerStats>().attackSpeed < GetStats<Tower.BaseTowerStats>().RotationDurationAfterShooting)
                GetStats<Tower.BaseTowerStats>().justShot = GetStats<Tower.BaseTowerStats>().attackSpeed * 1 / 3;
            else
                GetStats<Tower.BaseTowerStats>().justShot = GetStats<Tower.BaseTowerStats>().RotationDurationAfterShooting;
        }


    }
}