﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentController : MonoBehaviour {

    protected virtual void LateUpdate() {

        if (transform.childCount <= 0) 
            Destroy(gameObject);
    }
}