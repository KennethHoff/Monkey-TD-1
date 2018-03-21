using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Projectile;
using System;
using Tower;

namespace Bloon {
    public class StandardBloon : MonoBehaviour {

        #region variables
        protected GameControl.BloonSpawner.Bloons bloonEnum;

        protected int amountOfBloonsToSpawn = 1;
        protected float childSpawningSpacing = 0.25f;

        protected float regenerationCooldown;

        [SerializeField]
        protected Sprite regrowthSprite;
        [SerializeField]
        protected List<AudioClip> popSounds;


        [HideInInspector]
        public int RBE;
        [Header("Bloon Stats:")]
        public bool regrowth;
        public bool camo;

        protected int maxHealth;
        protected int currHealth;
        [SerializeField]
        protected bool immuneToSharpObjects, immuneToExplosions;
        protected bool hitThisFrame;
        
        public List<GameControl.BloonSpawner.Bloons> completeFamilyTree;

        public int currentFamilyTreeIndex = 0;
        

        protected AudioSource audioSource;

        #endregion

        protected virtual void Start() {

            for (int i = 0; i < completeFamilyTree.Count; i++) {
                if (completeFamilyTree[i] == bloonEnum) {
                    currentFamilyTreeIndex = i;
                    break;
                }
            }

            childSpawningSpacing = 0.25f;
            regenerationCooldown = GameControl.GameController.controllerObject.regenerationTime;

            if (regrowth) {
                GetComponent<SpriteRenderer>().sprite = regrowthSprite; 
            }
            audioSource = FindObjectOfType<AudioSource>();
            
        }

        protected virtual void Update() {
            hitThisFrame = false;
        }

        protected virtual void FixedUpdate() {
            if (regrowth) {
                Regrowth();
            }
        }


       #region debug

        protected virtual void AddChildrenToDictionaryGameObject(ParentController _projectileParent, List<Bloon.StandardBloon> _childrenListGameObject) {
            GameControl.DictionaryController.controllerObject.AddChildrenToDictionaryGameObject(_childrenListGameObject, _projectileParent);
        }

        public virtual void PopBloonGameObject(Tower.StandardTower _tower, ParentController _projectileParent, int _penetration) { // When the bloon gets killed

            // COMPLETED: Implement penetration (Currently no idea how I would go about doing it.)
            // Was done by changing "regrowthFamilyTree"(a list that showed the order of which bloons would be regrown) to a "completeFamilyTree" (a list that shows the order of the children. 
            // So: Pink > Yellow > Green > Blue > Red.
            // When regrow: Go up. 
            // When pop: Go down by the _penetration value

            int soundToPlay = Mathf.RoundToInt(UnityEngine.Random.Range(0, popSounds.Count)); // tilfeldig tall mellom 0 og antallet lyder i listen av lyder.
            audioSource.clip = popSounds[soundToPlay];
            Destroy(gameObject);



            audioSource.Play();

            if (_penetration >= RBE) { 
                FinalPop();
                GameControl.WaveSpawner.controllerObject.RBEKilledThisWave++;
                GameControl.InventoryController.controllerObject.gold += 1 * GameControl.InventoryController.controllerObject.goldGainMultiplier;
            }

            else if (completeFamilyTree[1] != GameControl.BloonSpawner.Bloons.Undefined) {
                Bloon.StandardBloon bloonToSpawn = GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + _penetration]];
                List<Bloon.StandardBloon> childrenList = SpawnMultipleListGameObject(bloonToSpawn, amountOfBloonsToSpawn);
                GameControl.DictionaryController.controllerObject.AddChildrenToDictionaryGameObject(childrenList, _projectileParent);

                GameControl.WaveSpawner.controllerObject.RBEKilledThisWave += _penetration;
                GameControl.InventoryController.controllerObject.gold += _penetration * GameControl.InventoryController.controllerObject.goldGainMultiplier;
            }
        }

        protected virtual List<Bloon.StandardBloon> SpawnMultipleListGameObject(Bloon.StandardBloon _bloonToSpawn, int _amount) {
            Vector2 movingDir = Vector3.zero;
            List<Bloon.StandardBloon> childrenList = new List<Bloon.StandardBloon>();

            for (int i = 0; i < _amount; i++) {
                Vector2 PosToSpawn = new Vector2(transform.position.x, transform.position.y) + (movingDir * i * childSpawningSpacing);
                Bloon.StandardBloon child = SpawnSingleGameObject(PosToSpawn, _bloonToSpawn);
                childrenList.Add(child);
            }
            return childrenList;
        }

        protected virtual Bloon.StandardBloon SpawnSingleGameObject(Vector2 _posToSpawn, Bloon.StandardBloon _bloonToSpawn) {
            
            StandardBloon childrenComponents = GetComponentInChildren<StandardBloon>(true);

            Bloon.StandardBloon childBloon = GameControl.BloonSpawner.SpawnBloon(_bloonToSpawn, _posToSpawn, Quaternion.identity, GetComponent<WayPoints>().currentWayPoint, regrowth, camo);
            childBloon.completeFamilyTree = completeFamilyTree;
            
            return childBloon;
        }

        protected virtual void DamageBloonGameObject(StandardProjectile projectile) {
            currHealth -= projectile.penetration;
            if (currHealth < 0) {
                PopBloonGameObject(projectile.tower, projectile.parent, -currHealth);
            }
        }

        #endregion

        public virtual  void SetFatherAttributes(Bloon.StandardBloon father) {
            father.GetComponent<StandardBloon>().completeFamilyTree = completeFamilyTree;
        }

        public virtual bool Damageable(GameControl.GameController.DamageTypes _damageType) { // Check if bloon is damageable.

            if (immuneToSharpObjects)
                if (_damageType == GameControl.GameController.DamageTypes.Sharp)
                    return false;

            if (immuneToExplosions)
                if (_damageType == GameControl.GameController.DamageTypes.Explosive)
                    return false;

            return true;
        }
        
        public virtual void FinalDestinationReached() {
            GameControl.InventoryController.controllerObject.life -= RBE;
            GameControl.WaveSpawner.controllerObject.RBEReachedFinalDestinationThisWave += RBE;
            Debug.Log("Final destination reached. Lost " + RBE + " life. Currently on: " + GameControl.InventoryController.controllerObject.life);
            Destroy(gameObject);
            GameControl.WaveSpawner.controllerObject.RBEReachedFinalDestinationThisWave++;
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
            Bloon.StandardBloon prefabToSpawn = new StandardBloon();
            Bloon.StandardBloon spawnedBloon = new StandardBloon();
            prefabToSpawn = GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex - 1]];

            spawnedBloon = GameControl.BloonSpawner.SpawnBloon(prefabToSpawn, transform.position, transform.rotation, GetComponent<WayPoints>().currentWayPoint, regrowth, camo);
            int difference_RBE = (spawnedBloon.RBE - RBE);
            GameControl.WaveSpawner.controllerObject.RBERegeneratedThisWave += difference_RBE;
            SetFatherAttributes(spawnedBloon);
            Destroy(gameObject);
        }

        protected virtual void Regrowth() {
            if (completeFamilyTree.First() != bloonEnum) {
                regenerationCooldown -= Time.fixedDeltaTime;
                if (regenerationCooldown < 0) {
                    Regenerate();
                }
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D collision) {
            if (hitThisFrame) return;
            if (collision.gameObject.tag == "Projectile") {
                Projectile.StandardProjectile projectile = collision.GetComponent<Projectile.StandardProjectile>();
                if (projectile.remainingPower >= 1) {
                    int projectileID = collision.GetInstanceID();
                    int bloonID = GetInstanceID();

                    if (GameControl.DictionaryController.controllerObject.CanCollideGameObject(projectile.parent, this)) {
                        if (Damageable(projectile.damageType)) {
                            hitThisFrame = true;
                            DamageBloonGameObject(projectile);
                            projectile.remainingPower--;
                            GameControl.DictionaryController.controllerObject.OnBloonHitGameObject(projectile.parent, this);
                            if (projectile.remainingPower <= 0)
                                Destroy(projectile.gameObject);
                        }
                    }
                }
            }
        }


        #region hitscan detection

        private void PopBloonHitscanGameObject(StandardTower tower, int _overkill) {
            int soundToPlay = Mathf.RoundToInt(UnityEngine.Random.Range(0, popSounds.Count)); // tilfeldig tall mellom 0 og antallet lyder i listen av lyder.
            audioSource.clip = popSounds[soundToPlay];
            Destroy(gameObject);



            audioSource.Play();

            if (_overkill >= RBE) {
                FinalPop();
                GameControl.WaveSpawner.controllerObject.RBEKilledThisWave++;
                GameControl.InventoryController.controllerObject.gold += 1 * GameControl.InventoryController.controllerObject.goldGainMultiplier;
            }

            else if (completeFamilyTree[1] != GameControl.BloonSpawner.Bloons.Undefined) {
                Bloon.StandardBloon bloonToSpawn = GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + _overkill]];
                List<Bloon.StandardBloon> childrenList = SpawnMultipleListGameObject(bloonToSpawn, amountOfBloonsToSpawn);

                GameControl.WaveSpawner.controllerObject.RBEKilledThisWave += _overkill;
                GameControl.InventoryController.controllerObject.gold += _overkill * GameControl.InventoryController.controllerObject.goldGainMultiplier;
            }
        }


        public virtual void HitScanShot(Tower.StandardTower _tower) {
            if (Damageable(_tower.damageType)) {
                DamageBloonHitscanGameObject(_tower);
            }
        }


        protected virtual void DamageBloonHitscanGameObject(Tower.StandardTower _tower) {
            currHealth -= _tower.projectilePenetration;
            if (currHealth < 0) {
                PopBloonHitscanGameObject(_tower, -currHealth);
            }
        }

        #endregion
    }

}