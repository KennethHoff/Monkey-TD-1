using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile { 
    public class StandardProjectile : MonoBehaviour {

        public GameControl.PlacementController.Towers towerEnum;
        
        public Tower.StandardTower tower;
        public float despawnDistance;
        protected float distanceFromSpawn;
        protected Vector2 spawnPoint;

        public float moveSpd;

        public int totalPower;
        public int remainingPower;

        protected Rigidbody2D rbody;

        protected int projectileID;

        public enum DamageTypes {
            Sharp,
            Explosive,
            Both
        }
        public DamageTypes damageType;

        private float finalMoveSpd;

        protected virtual void Start() {
            spawnPoint = (Vector2)transform.position;
            projectileID = gameObject.GetInstanceID();

            tower = GameControl.DictionaryController.placementDictionary[towerEnum].towerPrefab;

            Tower.ParentController parent = GameControl.DictionaryController.placementDictionary[towerEnum].towerParentPrefab;

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
        public void OnDestroy() {
            // GameControl.DictionaryController.controllerObject.OnProjectileDestroyed(projectileID);
            GameControl.DictionaryController.controllerObject.OnProjectileDestroyedGameObject(gameObject);
        }
    }
}