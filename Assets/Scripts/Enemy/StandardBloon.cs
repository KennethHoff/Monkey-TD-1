using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Bloon {
    public class StandardBloon : MonoBehaviour {

        #region variables
        public GameControl.BloonSpawner.Bloons bloonEnum;

        [SerializeField]
        protected int childrenAmount = 1;
        [SerializeField]
        protected float childSpawningSpacing = 0.25f;

        protected float regenerationCooldown;

        [SerializeField]
        protected Sprite regrowthSprite;
        [SerializeField]
        protected List<AudioClip> popSounds;


        public int RBE;
        [Header("Bloon Stats:")]
        public bool regrowth;
        public bool camo;

        [SerializeField]
        protected int startArmor;
        [SerializeField]
        protected int currArmor;
        [SerializeField]
        protected bool immuneToSharpObjects, immuneToExplosions;
        protected bool hitThisFrame;

        public int originalFamilyTreeIndex;

        public int currentFamilyTreeIndex;

        protected AudioSource audioSource;

        #endregion

        protected virtual void Start() {
            currArmor = startArmor;

            for (int i = 0; i < GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray.Length; i++) {
                if (GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[i].bloonEnum == bloonEnum) {
                    currentFamilyTreeIndex = i;
                    break;
                }
            }
            if (originalFamilyTreeIndex == -1)
                originalFamilyTreeIndex = currentFamilyTreeIndex;

            for (int i = currentFamilyTreeIndex; i >= 0; i--) {
                int startArmor = GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[i].startArmor;
                RBE += 1 + startArmor;
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

        protected virtual void AddChildrenToDictionaryGameObject(ParentController _projectileParent, List<Bloon.StandardBloon> _childrenListGameObject) {
            GameControl.DictionaryController.controllerObject.AddChildrenToDictionaryGameObject(_childrenListGameObject, _projectileParent);
        }

        public virtual void PopBloonGameObject(Tower.StandardTower _tower, ParentController _projectileParent, int _overkill) {

            int soundToPlay = Mathf.RoundToInt(UnityEngine.Random.Range(0, popSounds.Count)); // tilfeldig tall mellom 0 og antallet lyder i listen av lyder.
            audioSource.clip = popSounds[soundToPlay];
            Destroy(gameObject);

            audioSource.Play();
            List<StandardBloon> childrenList;

            if (_projectileParent != null && _overkill > 0)
                childrenList = CreateChildren(_overkill);

            else
                childrenList = null;

            if (childrenList != null)
                GameControl.DictionaryController.controllerObject.AddChildrenToDictionaryGameObject(childrenList, _projectileParent);
        }

        protected virtual List<Bloon.StandardBloon> SpawnMultipleListGameObject(Bloon.StandardBloon _bloonToSpawn, int _amount, float _spacing) {
            Vector2 movingDir = Vector2.one;
            List<Bloon.StandardBloon> childrenList = new List<Bloon.StandardBloon>();

            for (int i = 0; i < _amount; i++) {
                Vector2 PosToSpawn = new Vector2(transform.position.x, transform.position.y) + (movingDir * i * _spacing);
                Bloon.StandardBloon child = SpawnSingleGameObject(PosToSpawn, _bloonToSpawn);
                childrenList.Add(child);
            }
            return childrenList;
        }

        protected virtual Bloon.StandardBloon SpawnSingleGameObject(Vector2 _posToSpawn, Bloon.StandardBloon _bloonToSpawn) {
            
            StandardBloon childrenComponents = GetComponentInChildren<StandardBloon>(true);

            Bloon.StandardBloon childBloon = GameControl.BloonSpawner.SpawnBloon(_bloonToSpawn, _posToSpawn, Quaternion.identity, GetComponent<WayPoints>().currentWayPoint, regrowth, camo);

            childBloon.originalFamilyTreeIndex = originalFamilyTreeIndex;

            return childBloon;
        }

        protected virtual void DamageBloonGameObject(Projectile.StandardProjectile projectile) {
            currArmor -= projectile.penetration;
            if (currArmor < 0) {
                PopBloonGameObject(projectile.tower, projectile.parent, 0-currArmor);
            }
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
            regenerationCooldown = GameControl.GameController.controllerObject.regenerationTime;
            RegenerateUpOneTier();
        }

        protected virtual void RegenerateUpOneTier() {
            Bloon.StandardBloon prefabToSpawn = new StandardBloon();
            Bloon.StandardBloon spawnedBloon = new StandardBloon();
            prefabToSpawn = GameControl.DictionaryController.bloonDictionary[GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[currentFamilyTreeIndex + 1].bloonEnum];

            spawnedBloon = GameControl.BloonSpawner.SpawnBloon(prefabToSpawn, transform.position, transform.rotation, GetComponent<WayPoints>().currentWayPoint, regrowth, camo);

            spawnedBloon.originalFamilyTreeIndex = originalFamilyTreeIndex;

            int difference_RBE = (spawnedBloon.RBE - RBE);
            GameControl.WaveSpawner.controllerObject.RBERegeneratedThisWave += difference_RBE;
            Destroy(gameObject);
        }

        protected virtual void Regrowth() {
            if (currentFamilyTreeIndex != originalFamilyTreeIndex) {
                regenerationCooldown -= Time.fixedDeltaTime;
                if (regenerationCooldown < 0) {
                    Regenerate();
                }
            }
        }


        #region collision detection

        protected virtual void OnTriggerStay2D(Collider2D collision) {
            if (hitThisFrame) return;
            if (collision.gameObject.tag == "Projectile") {
                Projectile.StandardProjectile projectile = collision.GetComponent<Projectile.StandardProjectile>();
                if (projectile.remainingPower >= 1) {
                    Debug.Log("can collide?");

                    if (GameControl.DictionaryController.controllerObject.CanCollideGameObject(projectile.parent, this)) {
                        Debug.Log("Monkey:" + projectile.parent.GetInstanceID().ToString() + " can hit bloon: " + GetInstanceID().ToString());
                        CollidedWithProjectile(projectile);
                    }
                }
            }
        }

        protected virtual void CollidedWithProjectile(Projectile.StandardProjectile projectile) {
            GameControl.DictionaryController.controllerObject.OnBloonHitGameObject(projectile.parent, this);
            projectile.remainingPower--;
            if (projectile.remainingPower <= 0)
                Destroy(projectile.gameObject);
            hitThisFrame = true;
            
            if (Damageable(projectile.damageType)) DamageBloonGameObject(projectile);
        }

        #endregion

        #region hitscan detection

        public virtual void HitScanShot(Tower.StandardTower _tower) {
            if (Damageable(_tower.damageType)) {
                DamageBloonHitscanGameObject(_tower);
            }
        }


        protected virtual void DamageBloonHitscanGameObject(Tower.StandardTower _tower) {
            currArmor -= _tower.projectilePenetration;
            if (currArmor < 0) {
                PopBloonHitscanGameObject(_tower, -currArmor);
            }
        }

        private void PopBloonHitscanGameObject(Tower.StandardTower tower, int _overkill) {
            int soundToPlay = Mathf.RoundToInt(UnityEngine.Random.Range(0, popSounds.Count)); // tilfeldig tall mellom 0 og antallet lyder i listen av lyder.
            audioSource.clip = popSounds[soundToPlay];
            Destroy(gameObject);
            
            audioSource.Play();
            if (tower != null && _overkill > 0)
                CreateChildren(_overkill);
        }


    #endregion

        private List<Bloon.StandardBloon> CreateChildren(int _overkill) {

            List<Bloon.StandardBloon> childrenList = new List<Bloon.StandardBloon>();
            
            int layersToGoThrough = 0;

            if (currentFamilyTreeIndex < GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray.Length - 1) { 
                for (int i = 0; i < _overkill; i++) {
                    if (GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[currentFamilyTreeIndex + i].startArmor > 0) {
                        _overkill -= GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[currentFamilyTreeIndex + i].startArmor;
                    }
                    else layersToGoThrough++;
                }
            }
            else layersToGoThrough = 1;

            if (layersToGoThrough >= currentFamilyTreeIndex) {
                FinalPop();
                int count = GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray.Length - currentFamilyTreeIndex;
                GameControl.WaveSpawner.controllerObject.RBEKilledThisWave += count;
                GameControl.InventoryController.controllerObject.gold += count * GameControl.InventoryController.controllerObject.goldGainMultiplier;
                GameControl.WaveSpawner.controllerObject.bloonsKilledThisWave++;
                return null;
            }
            else {
                GameControl.WaveSpawner.controllerObject.RBEKilledThisWave += layersToGoThrough;
                GameControl.InventoryController.controllerObject.gold += layersToGoThrough * GameControl.InventoryController.controllerObject.goldGainMultiplier;
            }

            int _amountOfChildren = 0;
            int _highestChildrenAmount = 0;
            float _spacing = 0;


            for (int i = 0; i < layersToGoThrough; i++) {
                var index = currentFamilyTreeIndex - i;
                var arrayIndex = GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[index];

                _amountOfChildren += arrayIndex.childrenAmount;
                if (arrayIndex.childrenAmount > _highestChildrenAmount)
                    _highestChildrenAmount = _amountOfChildren;

                if (arrayIndex.childSpawningSpacing != 0)
                    _spacing = arrayIndex.childSpawningSpacing;
            }

            if (_amountOfChildren > 1 && _highestChildrenAmount >= 2) {
                _amountOfChildren = 0;
                for (int i = 0; i < layersToGoThrough; i++) {
                    var index = currentFamilyTreeIndex - i;
                    var arrayIndex = GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[index];
                    if (arrayIndex.childrenAmount > 1) {
                        _amountOfChildren += arrayIndex.childrenAmount;

                        if (arrayIndex.childSpawningSpacing != 0) {
                            _spacing = GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[currentFamilyTreeIndex + i].childSpawningSpacing;
                        }

                    }
                }
            }
            else if (_amountOfChildren > 1 && _highestChildrenAmount == 1)
                _amountOfChildren = 1;


            StandardBloon bloonToSpawn = GameControl.DictionaryController.bloonDictionary[GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[currentFamilyTreeIndex - layersToGoThrough].bloonEnum];
            childrenList = SpawnMultipleListGameObject(bloonToSpawn, _amountOfChildren, _spacing);

            return childrenList;
        }
    }
}