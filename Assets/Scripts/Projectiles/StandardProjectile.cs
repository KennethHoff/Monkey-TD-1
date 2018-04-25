using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile {
    public class StandardProjectile : MonoBehaviour {

        [ReadOnly, HideInInspector] public Tower.StandardTower tower;
        [ReadOnly] public float despawnDistance;
        [ReadOnly, SerializeField] protected float distanceFromSpawn;
        protected Vector2 spawnPoint;
        
        [HideInInspector]
        public ParentController parent;

        [ReadOnly] public float moveSpd;
        
        [ReadOnly] public int penetration; // How many layers it pops.

        [ReadOnly] public int totalPower;
        
        [ReadOnly] public int remainingPower;

        [ReadOnly] protected Rigidbody2D rbody;

        [ReadOnly] public GameControl.GameController.DamageTypes damageType;

        private float finalMoveSpd;

        protected virtual void Start() {

            rbody = GetComponent<Rigidbody2D>();
            parent = transform.parent.GetComponent<ParentController>();

            spawnPoint = (Vector2)transform.position;

            SetAttributes();
        }
        
        protected void SetAttributes() {
            remainingPower = totalPower;
            SetMovementAttributes();
        }
        protected void SetMovementAttributes() {
            finalMoveSpd = moveSpd * Time.fixedDeltaTime;
            rbody.velocity = transform.up * finalMoveSpd;

        }

        protected virtual void FixedUpdate() {
            SetMovementAttributes();
            distanceFromSpawn = Vector2.Distance(transform.position, spawnPoint);
        }

        protected void DestroyAtDespawnDistance() {
            if (distanceFromSpawn > despawnDistance) {
                Destroy(gameObject);
            }
        }
        protected void CheckCollisionBetweenLastAndCurrentPosition() { // TODO: Collision checking between last and current position.

        }
        
    }
}