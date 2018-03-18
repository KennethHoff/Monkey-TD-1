﻿using System.Collections;
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
        public float PoppingPower = 1;
        public int layersToPop = 1;

        [Header("Debugging:")]
        public bool drawGizmos;

        public bool targetting;
        protected float rotationDurationAfterShooting = 1 / 3f; // Time the Tower Stays aiming at the same spot after shooting (looks jiterry otherwise)

        protected float justShot;

        protected float firingCooldown;
        protected float aimCooldown;

        protected int totalPierces;

        [SerializeField]
        protected GameObject projectile;

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
            return Physics2D.OverlapCircleAll(transform.position, firingRange, GameControl.GameController.controllerObject.enemyLayer);
        }

        #region Targetting modes

        #endregion

        protected virtual void Shoot() {
            //Projectile.StandardProjectile shotProjectile = CreateProjectile();

            firingCooldown = attackSpeed;
        }

        protected virtual Projectile.StandardProjectile CreateProjectile(GameObject _projectile, Vector2 _position, Quaternion _rotation, Transform _parent) {

            GameObject shotProjectile = Instantiate(_projectile, _position, _rotation, _parent);

            Projectile.StandardProjectile shotProjectileScript = shotProjectile.GetComponent<Projectile.StandardProjectile>();

            totalPierces = Mathf.FloorToInt(PoppingPower);

            int pierceCheck = UnityEngine.Random.Range(0, 100);

            if (pierceCheck < (PoppingPower % Mathf.FloorToInt(PoppingPower) * 100)) {
                totalPierces++;
                Debug.Log("Added an extra pierce on a roll of " + pierceCheck + ".\nChance of " + PoppingPower % Mathf.FloorToInt(PoppingPower));
            }

            shotProjectileScript.totalPower = totalPierces;

            return shotProjectileScript;
        }
        
        protected virtual void OnDrawGizmos() {
            if (drawGizmos) Gizmos.DrawWireSphere(transform.position, firingRange);
        }

    }
}