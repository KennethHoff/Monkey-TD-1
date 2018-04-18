using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControl;

namespace GameControl {

    //LowPrio: Add BTD5 Font

    public class GameController : MonoBehaviour {

        public enum Difficulties {
            Easy,
            Normal,
            Hard,
            Impoppable
        }

        public enum DamageTypes {
            Sharp,
            Explosive,
            Both
        }

        [Header("Universal Game Information")]
        public LayerMask enemyLayer;
        public LayerMask environmentLayer;
        public LayerMask waterLayer;
        public LayerMask towerLayer;
        public bool UseGameObjectBasedCollisionDictionary;

        [SerializeField]
        private ProjectileParent ProjectileContainer;
        [SerializeField]

        private TowerParent towerContainer;

        [Header("Current Game Information:")]
        public Difficulties difficulty;

        public bool AutoStartNextWave = false;

        public bool fastForward = false;

        public float currentGameSpeed;

        public static GameController controllerObject;

        public static GameObject towerParent, enemyParent;
        
        public float regenerationTime = 1/1.2f; // How long it takes to go back up a tier of Bloon

        public float aimTimer = 1/30f; // (When Tower is unable to aim, how long to wait between each aim (performance).

        private void Awake() {
            controllerObject = GetComponent<GameController>();
        }

        void Start() {

            Debug.Log("Started game.");
            enemyParent = GameObject.Find("Enemy");
            towerParent = GameObject.Find("Tower");
        }

        // Update is called once per frame
        void Update() {
            if (fastForward) {
                Time.fixedDeltaTime = 0.005f;
                currentGameSpeed = 2f;
            }
            else {
                Time.fixedDeltaTime = 0.01f;
                currentGameSpeed = 1f;
            }
        }

        public ParentController CreateTowerFamilyTree(Tower.StandardTower _tower, Vector3 _position, Quaternion _rotation) {

            ParentController parentContainer = Instantiate(towerContainer, _position, _rotation, towerParent.transform);
            parentContainer.name = "TowerParent_" + _tower.name;
            Instantiate(_tower, _position, _rotation, parentContainer.transform);

            return parentContainer;
        }

        public List<Projectile.StandardProjectile> CreateProjectileFamilyTree(Projectile.StandardProjectile _projectile, Vector3 _position, Quaternion _rotation, Tower.StandardTower _tower, int _amount) {

            ParentController parentContainer = Instantiate(towerContainer, _position, _rotation, _tower.transform.parent.transform);
            parentContainer.name = "ProjectileParent_" + _projectile.name;
            List<Projectile.StandardProjectile> projectileList = new List<Projectile.StandardProjectile>();

            for (int i = 0; i < _amount; i++) {
                projectileList.Add(Instantiate(_projectile, _position, _rotation, parentContainer.transform));
            }
            return projectileList;
        }

        public static bool WithinMapPosition(Vector2 worldPos, float outsideRadius) {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(worldPos);
            if (WithinMapWorldPoint(worldPoint, outsideRadius))
                return true;
            else return false;

        }
        public static bool WithinMapWorldPoint(Vector2 worldPoint, float outsideRadius) {
            float leftSide = -9.05f - outsideRadius;
            float rightSide = 6.6f + outsideRadius;
            float topSide = 6.8f + outsideRadius;
            float bottomSide = -4.5f - outsideRadius;
            if (rightSide > worldPoint.x && worldPoint.x > leftSide + outsideRadius && topSide + outsideRadius > worldPoint.y && worldPoint.y > bottomSide + outsideRadius) 
                return true;
            else return false;
        }

        public void EndGame() {
            WaveSpawner.controllerObject.state = WaveSpawner.SpawnState.GameOver;
        }
    }
}