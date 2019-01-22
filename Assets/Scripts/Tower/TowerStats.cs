using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Powerups;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace Tower {

    public class BaseTowerStats : TowerStats, Powerups.IPowerupable<BaseTowerStats> {

        public BaseTowerStats() {
            SetDefaultStats();
            
        }

        protected void SetProjectile() {
            if (projectileEnum != GameControl.DictionaryController.Projectiles.Undefined) { 
                projectileObject = GameControl.DictionaryController.RetrieveProjectileFromProjectileDictionary_Enum(projectileEnum).projectileObject;
            }
            else Debug.LogWarning("No Projectile set for: " + towerEnum);
        }

        public void AddPowerup(PowerupBase<BaseTowerStats> pu) {
            this.Powerups.Add(pu);
        }

        public virtual void OnStart() {
            DebugPrintStats();
        }

        public virtual TowerUpgrade GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades _enum) {
            return GameControl.DictionaryController.RetrieveTowerUpgradeFromTowerUpgradeDictionary_Enum(_enum);
        }

        protected virtual void SetUpgradePaths() {

        }
    }

    public class DartMonkeyTowerStats : BaseTowerStats, Powerups.IPowerupable<DartMonkeyTowerStats> {
        
        public DartMonkeyTowerStats() {
            this.towerEnum = GameControl.DictionaryController.Towers.Dart_Monkey;
            this.projectileEnum = GameControl.DictionaryController.Projectiles.Dart_Monkey_Default;
            
            SetStats(_GoldCost: 200, _attackSpeed: 1 / 1.03f, _DamageType: GameControl.GameController.DamageTypes.Sharp, _firingRange: 3, _poppingPower: 1, _penetration: 1, _projectileSpeed: 2500);

            SetDescription("Dart Monkey", Key.Q, "Medium Range.\nMedium Firing Speed\nPops 1 bloon");

        }
        public void AddPowerup(PowerupBase<DartMonkeyTowerStats> _Powerup) {
            this.Powerups.Add(_Powerup);
        }

        public override void OnStart() {
            SetProjectile();
            SetUpgradePaths();
            base.OnStart();
        }
        
        protected override void SetUpgradePaths() {

            var upgrade_Left_1 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Long_Range_Darts);
            var upgrade_Left_2 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Enhanced_Eyesight);
            var upgrade_Left_3 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Spike_O_Pult);
            var upgrade_Left_4 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Juggernaut);

            
            var upgrade_Right_1 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Sharp_Shots);
            var upgrade_Right_2 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Razor_Sharp_Shots);
            var upgrade_Right_3 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Triple_Darts);
            var upgrade_Right_4 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Super_Monkey_Fan_Club);


            List<TowerUpgrade> leftUpgradePath = new List<TowerUpgrade> {
                upgrade_Left_1,
                upgrade_Left_2,
                //upgrade_Left_3,
                //upgrade_Left_4
            };
            List<TowerUpgrade> rightUpgradePath = new List<TowerUpgrade> {
                upgrade_Right_1,
                upgrade_Right_2,
                //upgrade_Right_3,
                //upgrade_Right_4
            };
            SetUpgradePaths(leftUpgradePath.ToArray(), rightUpgradePath.ToArray());
        }
    }

    public class TackShooterTowerStats : BaseTowerStats, Powerups.IPowerupable<TackShooterTowerStats> {
        public TackShooterTowerStats() {
            this.towerEnum = GameControl.DictionaryController.Towers.Tack_Shooter;
            this.projectileEnum = GameControl.DictionaryController.Projectiles.Tack_Shooter_Default;

            SetStats(_GoldCost: 360, _attackSpeed: 1 / 0.6f, _DamageType: GameControl.GameController.DamageTypes.Sharp, _firingRange: 2, _poppingPower: 1, _penetration: 1, _projectileSpeed: 1500);

            SetDescription("Tack Shooter", Key.W, "Short Range\nSlow Firing Speed\n\nShoots 8 tacks in a circle around the Tower");
        }
        public void AddPowerup(PowerupBase<TackShooterTowerStats> pu) {
            this.Powerups.Add(pu);
        }

        public override void OnStart() {
            SetProjectile();
            SetUpgradePaths();
            base.OnStart();
        }

        protected override void SetUpgradePaths() {

            var upgrade_Left_1 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Faster_Shooting);
            var upgrade_Left_2 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Even_Faster_Shooting);
            var upgrade_Left_3 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Tack_Sprayer);
            var upgrade_Left_4 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Ring_Of_Fire);


            var upgrade_Right_1 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Extra_Range_Tacks);
            var upgrade_Right_2 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Super_Range_Tacks);
            var upgrade_Right_3 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Blade_Shooter);
            var upgrade_Right_4 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Blade_Maelstrom);


            List<TowerUpgrade> leftUpgradePath = new List<TowerUpgrade> {
                upgrade_Left_1,
                upgrade_Left_2,
                //upgrade_Left_3,
                //upgrade_Left_4
            };
            List<TowerUpgrade> rightUpgradePath = new List<TowerUpgrade> {
                upgrade_Right_1,
                upgrade_Right_2,
                //upgrade_Right_3,
                //upgrade_Right_4
            };
            SetUpgradePaths(leftUpgradePath.ToArray(), rightUpgradePath.ToArray());
        }
    }

    public class SniperMonkeyTowerStats : BaseTowerStats, Powerups.IPowerupable<SniperMonkeyTowerStats> {
        public SniperMonkeyTowerStats() {
            this.towerEnum = GameControl.DictionaryController.Towers.Sniper_Monkey;

            SetStats(_GoldCost: 350, _attackSpeed: 1 / 0.45f, _DamageType: GameControl.GameController.DamageTypes.Sharp, _firingRange: -1, _poppingPower: 1, _penetration: 2, _projectileSpeed: -1);

            SetDescription("Sniper Monkey", Key.E, "Unlimited Range\nPenetrates 2 layers\nSlow firing speed");
        }
        public void AddPowerup(PowerupBase<SniperMonkeyTowerStats> pu) {
            this.Powerups.Add(pu);
        }

        public override void OnStart() {
            SetUpgradePaths();
            base.OnStart();
        }

        protected override void SetUpgradePaths() {

            var upgrade_Left_1 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Full_Metal_Jacket);
            var upgrade_Left_2 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Point_Five_Oh);
            var upgrade_Left_3 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Deadly_Precision);
            var upgrade_Left_4 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Cripple_MOAB);


            var upgrade_Right_1 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Faster_Firing);
            var upgrade_Right_2 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Night_Vision_Goggles);
            var upgrade_Right_3 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Semi_Automatic_Rifle);
            var upgrade_Right_4 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Supply_Drop);


            List<TowerUpgrade> leftUpgradePath = new List<TowerUpgrade> {
                upgrade_Left_1,
                upgrade_Left_2,
                //upgrade_Left_3,
                //upgrade_Left_4
            };
            List<TowerUpgrade> rightUpgradePath = new List<TowerUpgrade> {
                upgrade_Right_1,
                upgrade_Right_2,
                //upgrade_Right_3,
                //upgrade_Right_4
            };
            SetUpgradePaths(leftUpgradePath.ToArray(), rightUpgradePath.ToArray());
        }
    }

    public class BoomerangThrowerTowerStats : BaseTowerStats, Powerups.IPowerupable<BoomerangThrowerTowerStats> {
        public BoomerangThrowerTowerStats() {
            this.towerEnum = GameControl.DictionaryController.Towers.Boomerang_Thrower;
            this.projectileEnum = GameControl.DictionaryController.Projectiles.Boomerang_Thrower_Default;

            SetStats(_GoldCost: 400, _attackSpeed: 1/0.75f, _DamageType: GameControl.GameController.DamageTypes.Sharp, _firingRange: 3, _poppingPower: 3, _penetration: 1, _projectileSpeed: 1000);

            SetDescription("Boomerang Thrower", Key.R, "Medium Range\nMedium Firing Speed\n\nShoots a Boomerang that returns back to the Tower");
        }

        public void AddPowerup(PowerupBase<BoomerangThrowerTowerStats> pu) {
            this.Powerups.Add(pu);
        }

        public override void OnStart() {
            SetProjectile();
            SetUpgradePaths();
            base.OnStart();
        }

        protected override void SetUpgradePaths() {

            var upgrade_Left_1 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Multi_Target);
            var upgrade_Left_2 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Glaive_Thrower);
            var upgrade_Left_3 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Glaive_Ricochet);
            var upgrade_Left_4 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Glaive_Lord);


            var upgrade_Right_1 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Sonic_Boom);
            var upgrade_Right_2 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Red_Hot_Rangs);
            var upgrade_Right_3 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Bionic_Boomer);
            var upgrade_Right_4 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Turbo_Charge);


            List<TowerUpgrade> leftUpgradePath = new List<TowerUpgrade> {
                upgrade_Left_1,
                upgrade_Left_2,
                //upgrade_Left_3,
                //upgrade_Left_4
            };
            List<TowerUpgrade> rightUpgradePath = new List<TowerUpgrade> {
                upgrade_Right_1,
                upgrade_Right_2,
                //upgrade_Right_3,
                //upgrade_Right_4
            };
            SetUpgradePaths(leftUpgradePath.ToArray(), rightUpgradePath.ToArray());
        }
    }

    public class NinjaMonkeyTowerStats : BaseTowerStats, Powerups.IPowerupable<NinjaMonkeyTowerStats> {
        public NinjaMonkeyTowerStats() {
            this.towerEnum = GameControl.DictionaryController.Towers.Ninja_Monkey;
            this.projectileEnum = GameControl.DictionaryController.Projectiles.Ninja_Monkey_Default;

            SetStats(_GoldCost: 500, _attackSpeed: 1 / 1.67f, _DamageType: GameControl.GameController.DamageTypes.Sharp, _firingRange: 3, _poppingPower: 2, _penetration: 1, _projectileSpeed: 4000);

            SetDescription("Ninja Monkey", Key.T, "Medium Range\nFast Firing Speed");
        }
        public void AddPowerup(PowerupBase<NinjaMonkeyTowerStats> pu) {
            this.Powerups.Add(pu);
        }

        public override void OnStart() {
            SetProjectile();
            SetUpgradePaths();
            base.OnStart();
        }

        protected override void SetUpgradePaths() {

            var upgrade_Left_1 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Ninja_Discipline);
            var upgrade_Left_2 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Sharp_Shuriken);
            var upgrade_Left_3 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Double_Shot);
            var upgrade_Left_4 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Bloonjitsu);


            var upgrade_Right_1 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Seeking_Shuriken);
            var upgrade_Right_2 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Distraction);
            var upgrade_Right_3 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Flash_Bomb);
            var upgrade_Right_4 = GetTowerUpgrade(GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Sabotage_Supply_Lines);


            List<TowerUpgrade> leftUpgradePath = new List<TowerUpgrade> {
                upgrade_Left_1,
                upgrade_Left_2,
                //upgrade_Left_3,
                //upgrade_Left_4
            };
            List<TowerUpgrade> rightUpgradePath = new List<TowerUpgrade> {
                upgrade_Right_1,
                upgrade_Right_2,
                //upgrade_Right_3,
                //upgrade_Right_4
            };
            SetUpgradePaths(leftUpgradePath.ToArray(), rightUpgradePath.ToArray());
        }

    }


    
    public class TowerStats : Powerups.ITowerBase {

        public TowerStats() {

            Powerups = new List<IPowerup>();
            projectileSpawnPoints = new List<Transform>();

            }

        public void SetDefaultStats() {

            this.rotationOffset = -90f;
            this.goldCost = 100;
            this.attackSpeed = 0.5f;
            this.damageType = GameControl.GameController.DamageTypes.Sharp;
            this.firingRange = 3;
            this.CamoDetection = false;
            this.poppingPower = 1;
            this.penetration = 1;
            this.priceModifier = 1;
            this.sellValueModifier = 0.8f;
            this.upgradePaths = new TowerUpgradePath();

        }

        public void SetStats(int _GoldCost, float _attackSpeed, GameControl.GameController.DamageTypes _DamageType, float _firingRange, float _poppingPower, int _penetration, int _projectileSpeed) {
            this.goldCost = _GoldCost;
            this.attackSpeed = _attackSpeed;
            this.damageType = _DamageType;
            this.firingRange = _firingRange;
            this.poppingPower = _poppingPower;
            this.penetration = _penetration;
            this.projectileSpeed = _projectileSpeed;
        }

        public void SetDescription(string _name, Key _keyCode, string _description) {
            this.towerName = _name;
            this.hotkey = _keyCode;
            this.towerDescription = _description;
        }

        public void SetUpgradePaths(TowerUpgrade[] _leftUpgrades, TowerUpgrade[] _rightUpgrades) {
            for (int i = 0; i < _leftUpgrades.Length; i++) {
                if (_leftUpgrades[i] == null) return;
                upgradePaths.leftUpgradePath[i] = _leftUpgrades[i];
            }

            for (int i = 0; i < _rightUpgrades.Length; i++) {
                if (_rightUpgrades[i] == null) return;
                upgradePaths.rightUpgradePath[i] = _rightUpgrades[i];
            }
            
        }

        public void SetUpgradeIndex() {

            currentLeftUpgrade = upgradePaths.leftUpgradePath[upgradePaths.currentLeftUpgrade];
            currentLeftUpgradeInt = upgradePaths.currentLeftUpgrade;

            currentRightUpgrade = upgradePaths.rightUpgradePath[upgradePaths.currentRightUpgrade];
            currentRightUpgradeInt = upgradePaths.currentRightUpgrade;

        }

        public void DebugPrintStats() {
            Debug.Log(this.towerEnum + ": Range: " + firingRange + ". Attack Speed: " + attackSpeed + ". Projectile: " + projectileEnum);
        }

        public Powerups.IPowerup GetPowerup(GameControl.DictionaryController.TowerUpgrades i) {
            
            foreach (Powerups.IPowerup _powerup in Powerups) {
                if (_powerup.GetType() == i) {
                    return _powerup;
                }
            }
            return null;
            // return Powerups.First(pu => pu.GetType() == i);
        }

        protected List<Powerups.IPowerup> Powerups { get; set; }
        


        /// <summary>
        /// How quickly a Tower will shoot (in seconds).
        /// </summary>
        [Header("Balancing stats:")]
        public float attackSpeed;
        
        /// <summary>
        /// Base Cost of the Tower.
        /// </summary>
        public int goldCost;

        /// <summary>
        /// How far a tower is able to see, and by extension, how far it is able to damage. (Some exceptions to this rule, mainly Sniper Monkey).
        /// <para>This is directly altered by <see cref="EffectiveFiringRange"/>.</para>
        /// </summary>
        public float firingRange;


        public float despawnDistance;

        public int projectileSpeed;
        

    
        /// <summary>
        /// Whether a Tower can see Camo Bloons. If a projectile goes through a Camo bloon, it will be hit, however.
        /// </summary>
        public bool CamoDetection;


        /// <summary>
        /// Total number of pops one projectile can do before the projectile dissapears. This has no effect on some towers (primarily the Sniper Monkey)
        /// <para>A popping power of 10 will pop 10 different bloons before being removed from the game.</para>
        /// </summary>
        public float poppingPower;

        /// <summary>
        /// Total number of layers each 'pop' will do to each bloon.
        /// <para>A penetration of 3 will completely remove 3 layers off of a bloon.</para>
        /// </summary>
        public int penetration;

        /// <summary>
        /// There are 3 types of DamageTypes in this game. ('Sharp', 'Explosive' and 'Both). This determines which bloons the different Towers can damage.
        /// </summary>
        public GameControl.GameController.DamageTypes damageType;

        /// <summary>
        /// Which Projectile to Fire. 
        /// See: <see cref="projectileObject"/>
        /// </summary>
        public GameControl.DictionaryController.Projectiles projectileEnum;
        
        /// <summary>
        /// Total number of pops this instance of the Tower has done.
        /// </summary>
        [Header("Active Instance:")]
        [ReadOnly] public Collider2D target;

        public int towerPops = 0;

        /// <summary>
        /// The total sellValue (This is different to BuyValue, as the SellValue can change dynamically based on various external sources. (Mainly the Monkey Village 10% cost redution).
        /// <para>See: <see cref="sellValueModifier"/></para>
        /// </summary>
        public float sellValue;

        /// <summary>
        /// There are 4 targetting states. They represent what target the Tower will choose when it aims. They will only aim on bloons within their 'AttackRange'
        /// <para>'Close' targets the Bloon closest to the tower.</para>
        /// <para>'Strong' targets the strongest Bloon.</para>
        /// <para>'First' targets the Bloon that is the furthest into the battlefield.</para>
        /// <para>'Last' targets the Bloon that last entered the battlefield.</para>
        /// <para>See also: <seealso cref=" TargettingStates"/>, <seealso cref="firingRange"/></para>
        /// </summary>
        public TargettingStates targettingState;



        /// <summary>
        /// Which Tower this GameObject represents.
        /// </summary>
        [Space(10), Header("Implementation Stats")]
        public GameControl.DictionaryController.Towers towerEnum;

        /// <summary>
        /// An Array of Right- and Left-Hand side upgrades.
        /// </summary>
        public TowerUpgradePath upgradePaths;

        /// <summary>
        /// If I want some things to reduce the overall cost to purchasing things (let's say the Monkey Village). This is meant to facilitate that.
        /// </summary>
        public float priceModifier;

        /// <summary>
        /// Default resell Value is 80% of the cost. I could want to change this in the future.
        /// <para>See: <see cref="sellValue"/></para>
        /// </summary>
        public float sellValueModifier;

        /// <summary>
        /// Pre-placed Transforms that represent where to spawn Projectiles, and which direction.
        /// </summary>
        // public PositionRotationList projectileSpawnPoints;

        public List<Transform> projectileSpawnPoints;

        /// <summary>
        /// Due to the way targetting works, if two colliders collide, it will target the Bloon. Round Numbers are used for consistency, and to do so, it needs to be adjusted so it scales to only target if the Collider is halfway in.
        /// <para>See: <see cref="firingRange"/></para>
        /// </summary>
        public float EffectiveFiringRange {
            get {
                return firingRange - 0.375f;
            }
        }
    
        /// <summary>
        /// The Sprites are at a 90° skew, meaning they aim 90° to the left of the target.
        /// </summary>
        [HideInInspector]
        public float rotationOffset = -90f; // Sprite is 90° "skewed"


        /// <summary>
        /// See: <see cref="targettingState"></see>
        /// </summary>
        public enum TargettingStates {
            First,
            Last,
            Close,
            Strong
        }

        /// <summary>
        /// Which Projectile to Fire.
        /// See: <see cref="projectileEnum"/>
        /// </summary>
        [ReadOnly] public Projectile.StandardProjectile projectileObject;

        /// <summary>
        /// Set to True if it is actively aiming this frame.
        /// <para>See: <see cref="justShot"/>, <see cref="rotationDurationAfterShooting"/></para>
        /// </summary>
        public bool aiming;

        /// <summary>
        /// <para>For some time after shooting, The Tower will not change 'look direction'. This is to easily remember which Target it hit. </para>
        /// <para>See: <see cref="justShot"/></para>
        /// <para>See also: <seealso cref="aiming"/></para>
        /// </summary>
        public float RotationDurationAfterShooting { // Time the Tower Stays aiming at the same spot after shooting (looks jiterry otherwise)
            get {
                if (attackSpeed > 1) {
                    return 1 / 3;
                }
                else {
                    return attackSpeed * 0.5f;
                }
                
            }
        }

        /// <summary>
        /// <para>For some time after shooting, The Tower will not change 'look direction'. This is to easily remember which Target it hit. </para>
        /// <para>This time is set via the <see cref="rotationDurationAfterShooting"/> attribute.</para>
        /// <para>See also: <seealso cref="aiming"/></para>
        /// </summary>
        /// 
        public float justShot;

        /// <summary>
        /// Only fires under 0.
        /// </summary>
        public float firingCooldown;

        /// <summary>
        /// Only aims under 0 (Looks sharper and performs a lot better)
        /// </summary>
        public float aimCooldown;

        public TowerUpgrade currentLeftUpgrade;
        public int currentLeftUpgradeInt;

        public TowerUpgrade currentRightUpgrade;
        public int currentRightUpgradeInt;

        public Key hotkey; // Change this to use InputMaster

        public string towerName;

        public string towerDescription;


    }
}