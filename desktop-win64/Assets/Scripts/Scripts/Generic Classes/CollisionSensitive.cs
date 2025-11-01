using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSensitive : MonoBehaviour {

    public List<GameObject> Collidables = new List<GameObject>();

    [HideInInspector]
    public bool CollisionDetected;

    // Start is called before the first frame update
    void Start() {
        CollisionDetected = false;
    }

    void OnCollisionEnter (Collision _Collision) {

        for (int i = 0; i < Collidables.Count; i++)
            if (_Collision.gameObject == Collidables[i]) {
                CollisionDetected = true;
                break;
            }

    }

    void OnCollisionExit (Collision _Collision) {

        for (int i = 0; i < Collidables.Count; i++)
            if (_Collision.gameObject == Collidables[i]) {
                CollisionDetected = false;
                break;
            }

    }

}
