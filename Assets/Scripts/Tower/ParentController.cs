using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower { 

    public class ParentController : MonoBehaviour {

        private void LateUpdate() {

            if (transform.childCount <= 0) 
                Destroy(gameObject);

        }
    }

}