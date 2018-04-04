using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Bloon {
    public class StandardBloon : MonoBehaviour {

        #region variables
        protected GameControl.BloonSpawner.Bloons bloonEnum;

        [SerializeField]
        protected int childrenAmount = 1;
        [SerializeField]
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

        [SerializeField]
        protected int startArmor;
        protected int currArmor;
        [SerializeField]
        protected bool immuneToSharpObjects, immuneToExplosions;
        protected bool hitThisFrame;
        
        public List<GameControl.BloonSpawner.Bloons> completeFamilyTree;

        public int currentFamilyTreeIndex = 0;
        

        protected AudioSource audioSource;

        #endregion

        protected virtual void Start() {
            currArmor = startArmor;

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
            childBloon.completeFamilyTree = completeFamilyTree;
            
            return childBloon;
        }

        protected virtual void DamageBloonGameObject(Projectile.StandardProjectile projectile) {
            currArmor -= projectile.penetration;
            if (currArmor < 0) {
                PopBloonGameObject(projectile.tower, projectile.parent, 0-currArmor);
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


        #region collision detection

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

            if (currentFamilyTreeIndex < completeFamilyTree.Count - 1) { 
                for (int i = 0; i < _overkill; i++) {
                    if (GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + i]].startArmor > 0) {
                        _overkill -= GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + i]].startArmor;
                    }
                    else layersToGoThrough++;
                }
            }
            else layersToGoThrough = 1;

            if (layersToGoThrough >= completeFamilyTree.Count - currentFamilyTreeIndex) {
                FinalPop();
                int count = completeFamilyTree.Count - currentFamilyTreeIndex;
                GameControl.WaveSpawner.controllerObject.RBEKilledThisWave += count;
                GameControl.InventoryController.controllerObject.gold += count * GameControl.InventoryController.controllerObject.goldGainMultiplier;
                GameControl.WaveSpawner.controllerObject.bloonsKilledThisWave++;
                return null;
            }
            else {
                GameControl.WaveSpawner.controllerObject.RBEKilledThisWave += layersToGoThrough;
                GameControl.InventoryController.controllerObject.gold += layersToGoThrough * GameControl.InventoryController.controllerObject.goldGainMultiplier;
            }


            // TODO: Make Children Spawn Amount work correctly. (See under for details)

            // Currently:
            // 2 + 2         = 4 § Riktig  (4)
            // 1 Lead > 2 Black. 1 Black > 2 Pink. Therefore 1 Lead > 4 Pink.
            // 1 + 1 + 1 + 1 = 4 § Feil    (1)
            // 1 Pink > 1 Yellow. 1 Yellow > 1 Green. 1 Green > 1 Blue. 1 Blue > 1 Red. Therefore 1 Pink > 1 Red.
            // 2 + 2 + 1 + 1 = 6 § Feil (4)
            // 1 Lead > 2 Black. 1 Black > 2 Pink. 1 Pink > 1 Yellow. 1 Yellow > 1 Green. Therefore 1 Lead > 4 Yellow.



            // What I want:
            // 2 + 2 = 4
            // 1 + 1 + 1 + 1 = 1
            // 2 + 2 + 1 + 1 = 4
            // 1 + 1 + 1 + 0 = 0 (burde aldri komme hit, da en tidligere sjekk burde allerede fjernet objektet.
            // 4 + 4 + 2 + 2 + 1 + 1 + 1 = 12



            // If (EndResult > 1) 1 = 0

            // 2 + 2 + 1(0) = 4 § Korrekt
            // 2 + 1(0) + 1(0) + 1(0) = 2 § Korrekt
            // 1(0) + 1(0) + 1(0) + 1(0) + 1(0) = 0 § Feil

            // If (EndResult > 1 && highestChildAmount == 2+) 1 = 0
            // Else If (EndResult > 1 && highestChildAmount == 1) EndResult = 1

            // 2 + 2 + 1(0) = 4 § Korrekt
            // 2 + 1(0) + 1(0) + 1(0) = 2 § Korrekt
            // 1 + 1 + 1 + 1 = 4(1) § Korrekt
            // 4 + 4 + 3 + 2 + 1(0) + 1(0) = 13 § Korrekt

            // Seems like it should work.

            // It looks like it does :D

            int _amountOfChildren = 0;
            int _highestChildrenAmount = 0;
            float _spacing = 0;


            for (int i = 0; i < layersToGoThrough; i++) {
                _amountOfChildren += GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + i]].childrenAmount;
                if (GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + i]].childrenAmount > _highestChildrenAmount)
                    _highestChildrenAmount = _amountOfChildren;

                if (GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + i]].childSpawningSpacing != 0)
                    _spacing = GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + i]].childSpawningSpacing;
            }

            if (_amountOfChildren > 1 && _highestChildrenAmount >= 2) {
                _amountOfChildren = 0;
                for (int i = 0; i < layersToGoThrough; i++) {
                    if (GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + i]].childrenAmount > 1) {
                        _amountOfChildren += GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + i]].childrenAmount;

                        if (GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + i]].childSpawningSpacing != 0) {
                            _spacing = GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + i]].childSpawningSpacing;
                        }

                    }
                }
            }
            else if (_amountOfChildren > 1 && _highestChildrenAmount == 1)
                _amountOfChildren = 1;


            StandardBloon bloonToSpawn = GameControl.DictionaryController.bloonDictionary[completeFamilyTree[currentFamilyTreeIndex + layersToGoThrough]];
            childrenList = SpawnMultipleListGameObject(bloonToSpawn, _amountOfChildren, _spacing);

            return childrenList;
        }
    }
}