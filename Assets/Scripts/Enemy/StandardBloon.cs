using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Bloon {

    public class StandardBloon : MonoBehaviour {


        // TODO: Create a "BloonStats" Script
        #region variables
            
        public GameControl.DictionaryController.Bloons bloonEnum;

        public float moveSpd = 1f;

        public int startArmor;

        public int childrenAmount;
        public float childSpawningSpacing = 0.25f;
        public int RBE;

        protected float regenerationCooldown;

        [SerializeField]
        protected Sprite regrowthSprite;
        [SerializeField]
        protected List<AudioClip> popSounds;
        
        [Header("Bloon Stats:")]
        public bool regrowth;
        public bool camo;
        
        [SerializeField]
        protected int currArmor;
        [SerializeField]
        protected bool immuneToSharpObjects, immuneToExplosions;
        protected bool hitThisFrame;

        public int currentFamilyTreeIndex;

        public int originalFamilyTreeIndex;

        protected AudioSource audioSource;

        #endregion

        protected virtual void Start() {
            currArmor = startArmor;

            SetFamilyTreeIndex();

            // Remove this, and make it into a dictionary (or some other method that is not called upon every creation
            for (int i = currentFamilyTreeIndex; i >= 0; i--) {
                // int startArmor =  GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[i]].startArmor;
                int startArmor = GameControl.DictionaryController.RetrieveBloonFromBloonDictionary_Index(i).startArmor;
                RBE += 1 + startArmor;
            }
            
            regenerationCooldown = GameControl.GameController.controllerObject.regenerationTime;

            if (regrowth) {
                GetComponent<SpriteRenderer>().sprite = regrowthSprite;
            }
            audioSource = FindObjectOfType<AudioSource>();

        }

        private void SetFamilyTreeIndex() {
            for (int i = 0; i < GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray.Length; i++) {

                if (GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[i] == bloonEnum) {
                    currentFamilyTreeIndex = i;
                    break;
                }
            }
            if (originalFamilyTreeIndex == -1)
                originalFamilyTreeIndex = currentFamilyTreeIndex;
        }

        protected virtual void FixedUpdate() {

            hitThisFrame = false;

            if (regrowth) {
                Regrowth();
            }
        }

        protected virtual void AddChildrenToDictionary(ParentController _projectileParent, List<Bloon.StandardBloon> _childrenList) {
            GameControl.DictionaryController.controllerObject.AddChildrenToCollisionDictionary(_childrenList, _projectileParent);
        }

        protected virtual List<Bloon.StandardBloon> SpawnMultipleList(Bloon.StandardBloon _bloonToSpawn, int _amount, float _spacing) {
            Vector2 movingDir = Vector2.one;
            List<Bloon.StandardBloon> childrenList = new List<Bloon.StandardBloon>();

            for (int i = 0; i < _amount; i++) {
                Vector2 PosToSpawn = new Vector2(transform.position.x, transform.position.y) + (movingDir * i * _spacing);
                Bloon.StandardBloon child = SpawnSingle(PosToSpawn, _bloonToSpawn);
                childrenList.Add(child);
            }
            return childrenList;
        }

        protected virtual Bloon.StandardBloon SpawnSingle(Vector2 _posToSpawn, Bloon.StandardBloon _bloonToSpawn) {
            
            StandardBloon childrenComponents = GetComponentInChildren<StandardBloon>(true);

            Bloon.StandardBloon childBloon = GameControl.BloonSpawner.SpawnBloon(_bloonToSpawn, _posToSpawn, Quaternion.identity, GetComponent<WayPoints>().currentWayPoint, regrowth, camo);

            childBloon.originalFamilyTreeIndex = originalFamilyTreeIndex;

            return childBloon;
        }
          
        private void PopBloon(int _Overkill, params GameObject[] theObjects) {
            int soundToPlay = Mathf.RoundToInt(UnityEngine.Random.Range(0, popSounds.Count)); // tilfeldig tall mellom 0 og antallet lyder i listen av lyder.
            List<StandardBloon> childrenList = null;
            audioSource.clip = popSounds[soundToPlay];
            Destroy(gameObject);

            audioSource.Play();

            Tower.StandardTower _Tower = null;
            Projectile.StandardProjectile _Proj = null;
            foreach (GameObject obj in theObjects) {
                if (obj.GetComponent<Projectile.StandardProjectile>() != null) {
                    _Proj = obj.GetComponent<Projectile.StandardProjectile>();
                    _Tower = _Proj.tower;
                    
                }
                else if (obj.GetComponent<Tower.StandardTower>() != null) {
                    _Tower = obj.GetComponent<Tower.StandardTower>();
                }
                else {
                    Debug.LogError("Not a Projectile or Tower as parameters.");
                    return;
                }
                
                if (_Tower != null && _Overkill > 0)
                    childrenList = CreateChildren(_Overkill, _Tower);

                if (_Proj != null) {
                    if (childrenList != null) {
                        AddChildrenToDictionary(_Proj.parent, childrenList);
                    }
                }
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
            GameControl.InventoryController.ChangeLife(-RBE);
            GameControl.WaveSpawner.ChangeRBE_ReachedFinalDestination(RBE);
            GameControl.WaveSpawner.ChangeBloons_ReachedFinalDestination(1);
            Destroy(gameObject);
        }

        public virtual void FinalPop(Tower.StandardTower _Tower) { // Complete removal off the bloon (loses all health)
            GameControl.WaveSpawner.ChangeBloons_Killed(1);
            _Tower.ChangePop(1, true);
        }

        protected virtual void Regenerate() {
            regenerationCooldown = GameControl.GameController.controllerObject.regenerationTime;
            RegenerateUpOneTier();
        }

        protected virtual void RegenerateUpOneTier() {
            Bloon.StandardBloon prefabToSpawn = new StandardBloon();
            Bloon.StandardBloon spawnedBloon = new StandardBloon();

            var _Index = currentFamilyTreeIndex + 1;

            var _Prefab = GameControl.DictionaryController.RetrieveBloonFromBloonDictionary_Index(_Index);

            spawnedBloon = GameControl.BloonSpawner.SpawnBloon(prefabToSpawn, transform.position, transform.rotation, GetComponent<WayPoints>().currentWayPoint, regrowth, camo);

            spawnedBloon.originalFamilyTreeIndex = originalFamilyTreeIndex;

            int difference_RBE = (spawnedBloon.RBE - RBE);
            GameControl.WaveSpawner.ChangeRBE_Regenerated(difference_RBE);
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


        #region Collision Detection

        protected virtual void OnTriggerStay2D(Collider2D collision) {
            if (hitThisFrame) return;
            if (collision.gameObject.tag == "Projectile") {
                Projectile.StandardProjectile projectile = collision.GetComponent<Projectile.StandardProjectile>();
                if (projectile.remainingPower >= 1) {
                    // Debug.Log("can collide?");

                    if (GameControl.DictionaryController.controllerObject.CanCollide(projectile.parent, this)) {
                        // Debug.Log("Monkey:" + projectile.parent.GetInstanceID().ToString() + " can hit bloon: " + GetInstanceID().ToString());
                        CollidedWithProjectile(projectile);
                    }
                }
            }
        }

        protected virtual void CollidedWithProjectile(Projectile.StandardProjectile _Projectile) {
            GameControl.DictionaryController.controllerObject.OnBloonHit(_Projectile.parent, this);
            _Projectile.remainingPower--;
            if (_Projectile.remainingPower <= 0)
                Destroy(_Projectile.gameObject);
            hitThisFrame = true;

            if (Damageable(_Projectile.damageType))
                DamageBloon(_Projectile.gameObject);
        }

        #endregion

        #region Hitscan Detection

        public virtual void HitScanShot(Tower.StandardTower _Tower) {
            if (Damageable(_Tower.generalStats.damageType)) {
                DamageBloon(_Tower.gameObject);
            }
        }
        
        protected virtual void DamageBloon(params GameObject[] theObjects) {
            foreach(GameObject obj in theObjects) {

                if (obj.GetComponent<Projectile.StandardProjectile>() != null) {
                    var _Proj = obj.GetComponent<Projectile.StandardProjectile>();
                    currArmor -= _Proj.tower.generalStats.penetration;

                    if (currArmor < 0) {
                        PopBloon(-currArmor, _Proj.gameObject);
                    }
                }
                else if (obj.GetComponent<Tower.StandardTower>() != null) {
                    var _Tower = obj.GetComponent<Tower.StandardTower>();
                    currArmor -= _Tower.generalStats.penetration;

                    if (currArmor < 0) {
                        PopBloon(-currArmor, _Tower.gameObject);
                    }
                }
                else {
                    Debug.LogError("Not a Projectile or Tower as parameters.");
                    return;
                }
            }
        }

        #endregion

        #region Child Creation

        private List<Bloon.StandardBloon> CreateChildren(int _overkill, Tower.StandardTower _Tower) {

            List<Bloon.StandardBloon> childrenList = new List<Bloon.StandardBloon>();

            int layersToGoThrough = CalculateLayers(ref _overkill, _Tower);
            if (layersToGoThrough <= 0) return null;

            int _amountOfChildren = 0;
            float _spacing = 0;

            CalculateNumberOfChildren(layersToGoThrough, ref _amountOfChildren, ref _spacing, ref currentFamilyTreeIndex);

            var _Index = currentFamilyTreeIndex - layersToGoThrough;
            StandardBloon bloonToSpawn = GameControl.DictionaryController.RetrieveBloonFromBloonDictionary_Index(_Index);
            childrenList = SpawnMultipleList(bloonToSpawn, _amountOfChildren, _spacing);

            return childrenList;
        }

        private static void CalculateNumberOfChildren(int layersToGoThrough, ref int _amountOfChildren, ref float _spacing, ref int _CurrentIndex) {

            int _highestChildrenAmount = 0;
            for (int i = 0; i < layersToGoThrough; i++) {

                var _Index = _CurrentIndex - i;
                var _Prefab = GameControl.DictionaryController.RetrieveBloonFromBloonDictionary_Index(_Index);

                _amountOfChildren += _Prefab.childrenAmount;
                if (_Prefab.childrenAmount > _highestChildrenAmount)
                    _highestChildrenAmount = _amountOfChildren;

                if (_Prefab.childSpawningSpacing != 0)
                    _spacing = _Prefab.childSpawningSpacing;
            }

            if (_amountOfChildren > 1 && _highestChildrenAmount >= 2) {
                _amountOfChildren = 0;
                for (int i = 0; i < layersToGoThrough; i++) {

                    var _Index = _CurrentIndex - i;
                    var _Prefab = GameControl.DictionaryController.RetrieveBloonFromBloonDictionary_Index(_Index);

                    if (_Prefab.childrenAmount > 1) {
                        _amountOfChildren += _Prefab.childrenAmount;

                        if (_Prefab.childSpawningSpacing != 0) {

                            var _IndexNew = _Index - i;
                            var _PrefabNew = GameControl.DictionaryController.RetrieveBloonFromBloonDictionary_Index(_IndexNew);
                            _spacing = _PrefabNew.childSpawningSpacing;
                        }
                    }
                }
            }
            else if (_amountOfChildren > 1 && _highestChildrenAmount == 1) {
                _amountOfChildren = 1;
            }
        }

        private int CalculateLayers(ref int _overkill, Tower.StandardTower _Tower) {
            int layersToGoThrough = 0;

            if (currentFamilyTreeIndex < GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray.Length - 1) {
                for (int i = 0; i < _overkill; i++) {
                    int _Index = currentFamilyTreeIndex + i;
                    if (GameControl.DictionaryController.RetrieveBloonFromBloonDictionary_Index(_Index).startArmor > 0) {
                        int _IndexNew = currentFamilyTreeIndex + i;
                        _overkill -= GameControl.DictionaryController.RetrieveBloonFromBloonDictionary_Index(_IndexNew).startArmor;
                    }
                    else layersToGoThrough++;
                }
            }
            else layersToGoThrough = 1;
            
            if (layersToGoThrough > currentFamilyTreeIndex) {
                FinalPop(_Tower);
                int count = currentFamilyTreeIndex + 1;
                GameControl.InventoryController.ChangeGold(count);

                GameControl.WaveSpawner.ChangeRBE_Killed(count);
                GameControl.WaveSpawner.ChangeBloons_Killed(1);
                return 0;
            }
            else {
                GameControl.InventoryController.ChangeGold(layersToGoThrough);
                GameControl.WaveSpawner.ChangeRBE_Killed(layersToGoThrough);
                return layersToGoThrough;
            }
        }
        #endregion
    }
}