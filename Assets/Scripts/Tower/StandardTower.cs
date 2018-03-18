using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower { 
    public class StandardTower : MonoBehaviour {

        // LowPrio: Add animation (check original asset ingame.xml file for the directions)

        public GameControl.PlacementController.Towers towerEnum;

        public int goldCost;
        public bool CamoDetection;
        public float attackSpeed = 0.25f;
        public float firingRange = 3f;
        public float projectilePoppingPower = 1; // How many bloons it pierces. // 1 = hit 1 bloon, then despawn, 2 = pierce 1, allowing it to hit 1 more before despawning.
        public int projectilePenetration = 1; // How many layers each bloon loses. (1 = pink(4) > yellow(3). 2 = pink(4) > green(2))
        
        public GameControl.GameController.DamageTypes damageType;

        [Header("Debugging:")]
        public bool drawGizmos;

        public bool targetting;
        protected float rotationDurationAfterShooting = 1 / 3f; // Time the Tower Stays aiming at the same spot after shooting (looks jiterry otherwise)

        protected float justShot;

        protected float firingCooldown;
        protected float aimCooldown;

        protected int totalPierces;

        [SerializeField]
        protected Projectile.StandardProjectile projectileToFire;

        protected float rotationOffset = -90f; // Sprite is 90° "skewed"

        protected virtual void Start() {
            firingCooldown = 0f;
        }

        protected virtual void FixedUpdate() {
            // Barebones implementation -- Override at your leasure
            // Everything else depends on the Tower. (Nova Towers do not aim, therefore all of that is unnecessary. 
            // The only thing that is universal for every kind of tower, is that all of them does something on a regular basis ("firing cooldown"), be it shooting, freezing etc..
            // ... Except for Monkey Village, which is just a Area-of-Effect Buff (will do that if/when I get there..)

            if (GameControl.WaveSpawner.controllerObject.waveActive) {
                firingCooldown -= Time.fixedDeltaTime;
            }
            else firingCooldown = -1;
        }

        protected virtual Collider2D[] GetEnemiesInRange() {
            if (firingRange != -1)
            return Physics2D.OverlapCircleAll(transform.position, firingRange, GameControl.GameController.controllerObject.enemyLayer);
            else return Physics2D.OverlapBoxAll(transform.position, Vector2.one * 100, GameControl.GameController.controllerObject.enemyLayer);
        }
        
        protected virtual void Shoot() {
            //Projectile.StandardProjectile shotProjectile = CreateProjectile();

            firingCooldown = attackSpeed;
        }

        protected virtual List<Projectile.StandardProjectile> CreateProjectiles(Projectile.StandardProjectile _projectile, Vector2 _position, Quaternion _rotation, Transform _parent, int _amount) {

            List<Projectile.StandardProjectile> projectileList = GameControl.GameController.controllerObject.CreateProjectileFamilyTree(_projectile, _position, _rotation, this, _amount);

            foreach (Projectile.StandardProjectile projectile in projectileList) {
                totalPierces = Mathf.FloorToInt(projectilePoppingPower);

                int pierceCheck = UnityEngine.Random.Range(0, 100);

                if (pierceCheck < (projectilePoppingPower % Mathf.FloorToInt(projectilePoppingPower) * 100)) {
                    totalPierces++;
                    Debug.Log("Added an extra pierce on a roll of " + pierceCheck + ".\nChance of " + projectilePoppingPower % Mathf.FloorToInt(projectilePoppingPower));
                }

                projectile.totalPower = totalPierces;
                projectile.penetration = projectilePenetration;
            }
            return projectileList;
        }
        
        protected virtual void OnDrawGizmos() {
            if (drawGizmos) Gizmos.DrawWireSphere(transform.position, firingRange);
        }
    }
}