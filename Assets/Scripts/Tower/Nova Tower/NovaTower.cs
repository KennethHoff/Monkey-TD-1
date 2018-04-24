using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower { 

    public class NovaTower : StandardTower {

        protected override void Start() {
            base.Start();
        }

        protected override void FixedUpdate() {

            if (GameControl.WaveSpawner.controllerObject.waveActive)
                generalStats.firingCooldown -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                if (generalStats.firingCooldown < 0) 
                    if (IsEnemyInRange()) 
                        Shoot();
            else
                    generalStats.firingCooldown = -1;
            
        }

        protected virtual bool IsEnemyInRange() {

            Collider2D[] allCollisions = Physics2D.OverlapCircleAll(transform.position, generalStats.firingRange, GameControl.PlacementController.controllerObject.enemyLayer);

            foreach (Collider2D collision in allCollisions) 
                if (!collision.gameObject.GetComponent<Bloon.StandardBloon>().camo) 
                    return true;

            return false;
        }

        protected override void Shoot() {
            // Different from Ice Tower and Tack Shooter - therefore nothing here.
            generalStats.firingCooldown = generalStats.attackSpeed;

            base.Shoot();
        }
    }

}