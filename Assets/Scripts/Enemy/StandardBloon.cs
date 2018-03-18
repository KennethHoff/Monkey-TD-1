using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Bloon {
    public class StandardBloon : MonoBehaviour {

        #region variables
        protected GameControl.BloonSpawner.Bloons bloonEnum;

        [Header("Child:")]

        protected GameControl.BloonSpawner.Bloons bloonChildToSpawn;
        public int amountOfBloonsToSpawn = 1;
        public float childSpawningSpacing = 0.25f;

        protected float regenerationCooldown;

        [SerializeField, Header("Bloon Stats:")]
        protected Sprite regrowthSprite;

        public int RBE;
        [Space(6f)]
        public bool regrowth;
        public bool camo;
        public bool immuneToSharpObjects;
        public bool immuneToExplosions;
        protected bool hitThisFrame;

        [HideInInspector]
        public bool damageable;

        [HideInInspector]
        public List<GameControl.BloonSpawner.Bloons> familyTree;

        [SerializeField]
        [Space]
        protected List<AudioClip> popSounds = new List<AudioClip>();

        protected AudioSource audioSource;

        #endregion

        protected virtual void Awake() {
            if (familyTree == null) familyTree = new List<GameControl.BloonSpawner.Bloons>();
        }

        protected virtual void Start() {
            regenerationCooldown = GameControl.GameController.controllerObject.regenerationTime;

            if (regrowth) {
                GetComponent<SpriteRenderer>().sprite = regrowthSprite; 
            }
            if (familyTree.LastOrDefault() != bloonEnum) { 
                familyTree.Add(bloonEnum);
            }
            audioSource = FindObjectOfType<AudioSource>();
            damageable = true;
            
        }

        protected virtual void Update() {
            hitThisFrame = false;
        }

        protected virtual void FixedUpdate() {
            if (regrowth) {
                Regrowth();
            }
        }


        protected virtual List<int> SpawnMultipleIDList(int amount, int layers) {
            Vector2 movingDir = Vector3.zero;
            List<int> childrenIDList = new List<int>();

            for (int i = 0; i < amount; i++) { // HACK : Implement less hacky multi-children direction support (for when they move diagonally / for when/if I make waypoints more smooth)
                switch (GetComponent<WayPoints>().dir) {
                    case WayPoints.Direction.Left:
                        movingDir = new Vector2(1, 0);
                        break;
                    case WayPoints.Direction.Right:
                        movingDir = new Vector2(-1, 0);
                        break;
                    case WayPoints.Direction.Up:
                        movingDir = new Vector2(0, 1);
                        break;
                    case WayPoints.Direction.Down:
                        movingDir = new Vector2(0, -1);
                        break;
                }

                Vector2 PosToSpawn = new Vector2(transform.position.x, transform.position.y) + (movingDir * i * childSpawningSpacing);
                int childID = SpawnSingleID(PosToSpawn, layers);
                childrenIDList.Add(childID);
            }
            return childrenIDList;
        }

        #region debug

        protected virtual void AddChildrenToDictionaryGameObject(GameObject projectile, List<GameObject> childrenListGameObject) {
            GameControl.DictionaryController.controllerObject.AddChildrenToDictionaryGO(gameObject, childrenListGameObject, projectile);
        }

        public virtual void PopBloonGameObject(Tower.StandardTower tower, GameObject projectile) { // When the bloon gets killed
            int soundToPlay = Mathf.RoundToInt(UnityEngine.Random.Range(0, popSounds.Count)); // tilfeldig tall mellom 0 og antallet lyder i listen av lyder.
            audioSource.clip = popSounds[soundToPlay];
            Destroy(gameObject);

            GameControl.WaveSpawner.controllerObject.RBEKilledThisWave++;
            GameControl.InventoryController.controllerObject.gold += 1 * GameControl.InventoryController.controllerObject.goldGainMultiplier;

            audioSource.Play();
            if (bloonChildToSpawn != GameControl.BloonSpawner.Bloons.Undefined) {
                if (GameControl.DictionaryController.bloonDictionary[bloonChildToSpawn].GetComponent<StandardBloon>().RBE < tower.layersToPop) {
                    List<GameObject> childrenList = SpawnMultipleListGameObject(amountOfBloonsToSpawn, 1);
                    GameControl.DictionaryController.controllerObject.AddChildrenToDictionaryGO(gameObject, childrenList, projectile);

                }
            }
        }

        protected virtual List<GameObject> SpawnMultipleListGameObject(int amount, int layers) {
            Vector2 movingDir = Vector3.zero;
            List<GameObject> childrenList = new List<GameObject>();

            for (int i = 0; i < amount; i++) {
                Vector2 PosToSpawn = new Vector2(transform.position.x, transform.position.y) + (movingDir * i * childSpawningSpacing);
                GameObject childID = SpawnSingleGameObject(PosToSpawn, layers);
                childrenList.Add(childID);
            }
            return childrenList;
        }

        protected virtual GameObject SpawnSingleGameObject(Vector2 posToSpawn, int layers) {
            
            StandardBloon childrenComponents = GetComponentInChildren<StandardBloon>(true);

            var childBloon = GameControl.BloonSpawner.SpawnBloon(GameControl.DictionaryController.bloonDictionary[bloonChildToSpawn], posToSpawn, Quaternion.identity, GetComponent<WayPoints>().currentWayPoint, regrowth, camo);
            childBloon.GetComponent<StandardBloon>().familyTree = familyTree;
            return childBloon;
        }


    #endregion

        protected virtual int SpawnSingleID(Vector2 posToSpawn, int layers) {

            StandardBloon childrenComponents = GetComponentInChildren<StandardBloon>(true);

            var childBloon = GameControl.BloonSpawner.SpawnBloon(GameControl.DictionaryController.bloonDictionary[bloonChildToSpawn], posToSpawn, Quaternion.identity, GetComponent<WayPoints>().currentWayPoint, regrowth, camo);
            childBloon.GetComponent<StandardBloon>().familyTree = familyTree;
            return childBloon.GetInstanceID();
            
        }

        public virtual void SetChildAttributes(GameObject child) {
        }

        public virtual  void SetFatherAttributes(GameObject father) {
            father.GetComponent<StandardBloon>().familyTree = familyTree;
        }

        public virtual void DamageBloon(Tower.StandardTower tower, GameObject projectileObject) { // When damage occurs.

            if (immuneToSharpObjects)
                if (projectileObject.GetComponent<Projectile.StandardProjectile>().damageType == Projectile.StandardProjectile.DamageTypes.Sharp)
                    damageable = false;

            if (immuneToExplosions)
                if (projectileObject.GetComponent<Projectile.DartMonkeyDart>().damageType == Projectile.StandardProjectile.DamageTypes.Explosive)
                    damageable = false;

            if (damageable)
                OnPopped(tower, projectileObject);
        }

        public virtual void PopBloon(Tower.StandardTower tower, int projectileID) { // When the bloon gets killed
            int soundToPlay = Mathf.RoundToInt(UnityEngine.Random.Range(0, popSounds.Count)); // tilfeldig tall mellom 0 og antallet lyder i listen av lyder.
            audioSource.clip = popSounds[soundToPlay];
            Destroy(gameObject);

            GameControl.WaveSpawner.controllerObject.RBEKilledThisWave++;
            GameControl.InventoryController.controllerObject.gold += 1 * GameControl.InventoryController.controllerObject.goldGainMultiplier;

            audioSource.Play();
            if (bloonChildToSpawn != GameControl.BloonSpawner.Bloons.Undefined) {
                if (GameControl.DictionaryController.bloonDictionary[bloonChildToSpawn].GetComponent<StandardBloon>().RBE < tower.layersToPop) { 
                    List<int> childrenIDList = SpawnMultipleIDList(amountOfBloonsToSpawn, tower.layersToPop);
                    AddChildrenToDictionary(projectileID, childrenIDList);

                }
            }
        }

        protected virtual void OnPopped(Tower.StandardTower tower, GameObject projectileObject) {
            // PopBloon(tower, projectileObject.GetInstanceID());
            PopBloonGameObject(tower, projectileObject);
        }

        protected virtual void AddChildrenToDictionary(int projectileID, List<int> childrenIDList) {
            GameControl.DictionaryController.controllerObject.AddChildrenToDictionary(GetInstanceID(), childrenIDList, projectileID);
        }

        public virtual void FinalDestinationReached() {
            GameControl.InventoryController.controllerObject.life -= RBE;
            GameControl.WaveSpawner.controllerObject.RBEReachedFinalDestinationThisWave += RBE;
            Debug.Log("Final destination reached. Lost " + RBE + " life. Currently on: " + GameControl.InventoryController.controllerObject.life);
            Destroy(gameObject);
            GameControl.WaveSpawner.controllerObject.bloonsReachedFinalDestination++;
        }

        public virtual void FinalPop() { // Complete removal off the bloon (loses all health)
            GameControl.WaveSpawner.controllerObject.bloonsKilledThisWave++;
        }

        protected virtual void Regenerate() {
            Debug.Log("Increase Bloon Tier by 1 (Red > Blue. Blue > Green ...");
            regenerationCooldown = GameControl.GameController.controllerObject.regenerationTime;
            RegenerateUpOneTier();
        }

        protected virtual void RegenerateUpOneTier() {
            Debug.Log("Regenerating to.. " + familyTree.Last());
            familyTree.Remove(familyTree.Last());
            GameObject prefabToSpawn = GameControl.DictionaryController.bloonDictionary[familyTree.Last()];
            GameObject spawnedObject = GameControl.BloonSpawner.SpawnBloon(prefabToSpawn, transform.position, transform.rotation, GetComponent<WayPoints>().currentWayPoint, regrowth, camo);
            StandardBloon spawnedScript = spawnedObject.GetComponent<StandardBloon>();
            int difference_RBE = (spawnedScript.RBE - RBE);
            GameControl.WaveSpawner.controllerObject.RBERegeneratedThisWave += difference_RBE;
            SetFatherAttributes(spawnedObject);
            Destroy(gameObject);
        }

        protected virtual void Regrowth() {
            if (familyTree[0] != bloonEnum || familyTree.Count > 1) {
                regenerationCooldown -= Time.fixedDeltaTime;
                if (regenerationCooldown < 0) {
                    Regenerate();
                }
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D collision) {
            if (hitThisFrame) return;
            if (collision.gameObject.tag == "Projectile") {
                Projectile.StandardProjectile projectileScript = collision.GetComponent<Projectile.StandardProjectile>();
                if (projectileScript.remainingPower >= 1) {
                    int projectileID = collision.GetInstanceID();
                    int bloonID = GetInstanceID();

                    if (GameControl.GameController.controllerObject.UseGameObjectBasedCollisionDictionary) {
                         if (GameControl.DictionaryController.controllerObject.CanCollideGameObject(projectileScript.gameObject, gameObject)) {
                            if (!immuneToSharpObjects) {
                                hitThisFrame = true;
                                PopBloonGameObject(projectileScript.tower, projectileScript.gameObject);
                                projectileScript.remainingPower--;
                                GameControl.DictionaryController.controllerObject.OnBloonHitGameObject(projectileScript.gameObject, gameObject);
                                if (projectileScript.remainingPower <= 0)
                                    Destroy(projectileScript.gameObject);
                            }
                        }
                    }
                    else {
                        if (GameControl.DictionaryController.controllerObject.CanCollide(projectileID, bloonID)) {
                            if (!immuneToSharpObjects) {
                                hitThisFrame = true;
                                PopBloon(projectileScript.tower, projectileID);
                                projectileScript.remainingPower--;
                                GameControl.DictionaryController.controllerObject.OnBloonHit(projectileID, bloonID);
                                if (projectileScript.remainingPower <= 0)
                                    Destroy(projectileScript.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }
}