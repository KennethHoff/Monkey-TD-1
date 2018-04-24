using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile {
    public class Boomerang : StandardProjectile {

        public int currentSpins;
        public bool spinning;
        public int spinSpeed;

        public int totalSpins;

        private float timeSinceStartOfSpin;

        protected override void Start() {
            base.Start();

            timeSinceStartOfSpin = 0;
        }
        protected override void FixedUpdate() {
            base.FixedUpdate();

            timeSinceStartOfSpin += Time.fixedDeltaTime;

            Curve();
            CheckSpins();
            DestroyAfterSpins(totalSpins);
        }
        
        
        protected void CurveProjectile() {
            float timeBeforeStartCurving = 0.2f;

            if (false) {
                Debug.Log("Tower in front.");
                //rbody.MoveRotation(rbody.rotation + spinSpeed * Time.fixedDeltaTime);
            }

            else { 
                // rbody.MoveRotation(rbody.rotation + boomerangSpin * Time.fixedDeltaTime);
                // For first 1/4 of a second: go straight. Next 1/4 of a second: curve twice as fast.
                if (timeSinceStartOfSpin < timeBeforeStartCurving) {
                    // go forward
                    // Debug.Log("Not curving.");
                }
                else if (timeSinceStartOfSpin > timeBeforeStartCurving && timeSinceStartOfSpin < timeBeforeStartCurving * 2) {
                    // Curve at twice speed.
                    // Debug.Log("Curving at twice speed.");
                    rbody.MoveRotation(rbody.rotation + spinSpeed * Time.fixedDeltaTime * 2);
                }
                /*
                else if (timeSinceStartOfSpin > 0.5f) {
                    rbody.MoveRotation(rbody.rotation + spinSpeed * Time.fixedDeltaTime * 3);
                }
                */
                else {
                    // Curve at normal speed
                    // Debug.Log("Curving at normal speed.");
                    rbody.MoveRotation(rbody.rotation + spinSpeed * Time.fixedDeltaTime);
                }
            }
        }
        
        
        private IEnumerator CurveProjectile_IEnum() {

            while (true) {
                if (distanceFromSpawn > despawnDistance) { 

                    if (timeSinceStartOfSpin < 0.1f) {
                        rbody.MoveRotation(rbody.rotation + spinSpeed * Time.fixedDeltaTime * 2.5f);
                        Debug.Log("Curving at higher speed");
                        // yield return new WaitForFixedUpdate();
                    }
                    

                    rbody.MoveRotation(rbody.rotation + spinSpeed * Time.fixedDeltaTime);
                    // Debug.Log("Curving: " + timeSinceStartOfSpin);
                    yield return new WaitForFixedUpdate();
                }
                else {
                    // Debug.Log("Not curving: " + timeSinceStartOfSpin);
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        /*
        private void CurveProjectile() {

            if (distanceFromSpawn > despawnDistance) {
                rbody.MoveRotation(rbody.rotation + spinSpeed * Time.fixedDeltaTime);
                Debug.Log(despawnDistance);
            }
            else {
                Debug.Log("Not Curving.");
            }

        }
        */

        protected void Curve() {
            CurveProjectile();
            //StartCoroutine(CurveProjectile_IEnum());
        }
        protected void DestroyAfterSpins(int _amount) {
            if (currentSpins >= _amount) {
                if (distanceFromSpawn < 1) {
                    Destroy(gameObject);
                }
            }
        }
        protected void CheckSpins() {
            if (distanceFromSpawn > 1 && !spinning) {
                spinning = true;
            }
            if (distanceFromSpawn < 1 && spinning) {
                currentSpins++;
                timeSinceStartOfSpin = 0;
                transform.position = parent.transform.position;
                transform.rotation = parent.transform.rotation;
                spinning = false;
                Debug.Log("Full spin complete");
            }
        }
    }
}