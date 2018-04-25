using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class NinjaMonkey : MonkeyTower {
        public override T GetStats<T>() {
            return ninjaMonkeyTowerStats as T;
        }
        public NinjaMonkeyTowerStats ninjaMonkeyTowerStats = new NinjaMonkeyTowerStats();

        protected override void Start() {
            ninjaMonkeyTowerStats = new NinjaMonkeyTowerStats();
            Debug.Log(ninjaMonkeyTowerStats.attackSpeed);
            base.Start();
        }

        protected override void Shoot() {

            List<Projectile.StandardProjectile> shotProjectileList = CreateProjectileFamilyTree(GetStats<Tower.BaseTowerStats>().projectileObject, transform.position, transform.rotation);

            foreach (Projectile.StandardProjectile projectile in shotProjectileList) {
                projectile.despawnDistance = GetStats<Tower.BaseTowerStats>().firingRange * 10f;
            }

            Debug.Log("Ninja Monkey #" + GetInstanceID() + " shot!");
            base.Shoot();
        }
    }
}