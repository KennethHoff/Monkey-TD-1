using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower { 
    public class StandardTower : MonoBehaviour {

        // LowPrio: Add animation (check original asset ingame.xml file for the directions)

        public GameControl.PlacementController.Towers towerEnum;


        public Transform[] projectileSpawnPoints;
        public int goldCost;
        public bool CamoDetection;
        public float attackSpeed = 0.25f;
        public float firingRange = 3f;
        public float projectilePoppingPower = 1; // How many bloons it pierces. // 1 = hit 1 bloon, then despawn, 2 = pierce 1, allowing it to hit 1 more before despawning.
        public int projectilePenetration = 1; // How many layers each bloon loses. (1 = pink(4) > yellow(3). 2 = pink(4) > green(2))
        
        public GameControl.GameController.DamageTypes damageType;

        [SerializeField]
        protected Projectile.StandardProjectile projectileToFire;

        [Header("Debugging:")]
        public bool drawGizmos;

        public bool aiming;
        protected float rotationDurationAfterShooting = 1 / 3f; // Time the Tower Stays aiming at the same spot after shooting (looks jiterry otherwise)

        protected float justShot;

        public float firingCooldown;
        protected float aimCooldown;

        protected int totalPierces;


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

        }

        protected virtual List<Projectile.StandardProjectile> CreateProjectiles(Projectile.StandardProjectile _projectile, Transform[] _spawnPoints, Quaternion _rotation, Transform _parent, int _amount) {
            List<Projectile.StandardProjectile> projectileList = new List<Projectile.StandardProjectile>();

            foreach (Transform spawnPoint in _spawnPoints) {
                CreateProjectile(_projectile, spawnPoint.position, _rotation, _parent);
            }
            return projectileList;
        }

        // TODO: Make a method that allows (info inside):
        // Some projectiles are meant to be mutually inclusive in the "CanCollide" dictionary (Tack Shooter tacks comes to mind - If multiples hit the same bloon, only one of them should be able to do damage)
        // Some projectiles comes from the same pool, but are *not* mutually inclusive in the dictionary. (Probably multi-throw monkeys)
        // Gigantic Bloons (MOAB-Class) are meant to be hit by the same tower. (Tack Shooter can hit them multiple times..)

        protected virtual Projectile.StandardProjectile CreateProjectile(Projectile.StandardProjectile _projectile, Vector2 _position, Quaternion _rotation, Transform _parent) {

            var projectile = Instantiate(_projectile, _position, _rotation, _parent);
            
            totalPierces = Mathf.FloorToInt(projectilePoppingPower);

            int pierceCheck = UnityEngine.Random.Range(0, 100);

            if (pierceCheck < (projectilePoppingPower % Mathf.FloorToInt(projectilePoppingPower) * 100)) {
                totalPierces++;
                Debug.Log("Added an extra pierce on a roll of " + pierceCheck + ".\nChance of " + projectilePoppingPower % Mathf.FloorToInt(projectilePoppingPower));
            }

            projectile.totalPower = totalPierces;
            projectile.penetration = projectilePenetration;
            projectile.damageType = damageType;

            return projectile;
        }

        protected virtual void OnDrawGizmos() {
            if (drawGizmos) Gizmos.DrawWireSphere(transform.position, firingRange);
        }
    }
}