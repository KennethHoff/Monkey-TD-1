using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile { 
    public class StandardProjectile : MonoBehaviour {

        public GameControl.PlacementController.Towers towerEnum;
        
        [HideInInspector]
        public Tower.StandardTower tower;
        public float despawnDistance;
        protected float distanceFromSpawn;
        protected Vector2 spawnPoint;

        [HideInInspector]
        public ParentController parent;

        public float moveSpd;

        [HideInInspector]
        public int penetration; // How many layers it pops.

        [HideInInspector]
        public int totalPower;
        
        [HideInInspector]
        public int remainingPower;

        protected Rigidbody2D rbody;

        protected int projectileID;
        
        public GameControl.GameController.DamageTypes damageType;

        private float finalMoveSpd;

        protected virtual void Start() {
            parent = transform.parent.GetComponent<ParentController>();
            spawnPoint = (Vector2)transform.position;
            projectileID = gameObject.GetInstanceID();

            tower = GameControl.DictionaryController.placementDictionary[towerEnum].towerPrefab;

            remainingPower = totalPower;
            rbody = GetComponent<Rigidbody2D>();

            finalMoveSpd = moveSpd * Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
            rbody.velocity = transform.up * finalMoveSpd;
        }

        protected virtual void FixedUpdate() {
            distanceFromSpawn = Vector2.Distance(transform.position, spawnPoint);
            if (distanceFromSpawn > despawnDistance) {
                Destroy(gameObject);
            }
        }
    }
}