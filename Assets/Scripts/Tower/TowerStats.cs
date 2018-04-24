using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower { 
    [System.Serializable]
    public class TowerStats {

        /// <summary>
        /// How quickly a Tower will shoot (in seconds).
        /// </summary>
        [Header("Balancing stats:")]
        public float attackSpeed = 0.25f;
        
        /// <summary>
        /// Base Cost of the Tower.
        /// </summary>
        public int goldCost;
        
        /// <summary>
        /// How far a tower is able to see, and by extension, how far it is able to damage. (Some exceptions to this rule, mainly Sniper Monkey).
        /// <para>This is directly altered by <see cref="EffectiveFiringRange"/>.</para>
        /// </summary>
        public float firingRange = 3f;
    
        /// <summary>
        /// Whether a Tower can see Camo Bloons. If a projectile goes through a Camo bloon, it will be hit, however.
        /// </summary>
        public bool CamoDetection;


        /// <summary>
        /// Total number of pops one projectile can do before the projectile dissapears. This has no effect on some towers (primarily the Sniper Monkey)
        /// <para>A popping power of 10 will pop 10 different bloons before being removed from the game.</para>
        /// </summary>
        public float poppingPower = 1;

        /// <summary>
        /// Total number of layers each 'pop' will do to each bloon.
        /// <para>A penetration of 3 will completely remove 3 layers off of a bloon.</para>
        /// </summary>
        public int penetration = 1;

        /// <summary>
        /// There are 3 types of DamageTypes in this game. ('Sharp', 'Explosive' and 'Both). This determines which bloons the different Towers can damage.
        /// </summary>
        public GameControl.GameController.DamageTypes damageType;

        /// <summary>
        /// Which Projectile to Fire. 
        /// See: <see cref="projectileObject"/>
        /// </summary>
        public GameControl.DictionaryController.Projectiles projectileEnum; // LowPrio: Remove the serialization, and make it dynamic (based on Upgrades).



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
        /// Icon that shows when targetted.
        /// </summary>
        public Sprite towerIcon;

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
        public List<Transform> projectileSpawnPoints;

        /// <summary>
        /// Due to the way targetting works, if two colliders collide, it will target the Bloon. I want to use Round Numbers, and to do so, I have to adjust it so it scales to only target if the Collider is halfway in.
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
        public float rotationDurationAfterShooting = 1 / 3f; // Time the Tower Stays aiming at the same spot after shooting (looks jiterry otherwise)

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
    }
}