using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : Vessel {
    
    public GameObject CapOn;

    [HideInInspector]
    public Vector3 LocalPosition_for_Cap;
    [HideInInspector]
    public Vector3 LocalRotation_for_Cap;

    public void setCapOn (GameObject _Cap) {
        CapOn = _Cap;
    }


    // Start is called before the first frame update
    public override void Start () {
        
        base.Start ();

        LocalPosition_for_Cap = Vector3.zero;
        LocalRotation_for_Cap = Vector3.zero;

    }


    public void placeOn (GameObject _Cap) {
        _Cap.transform.parent = transform;
        _Cap.transform.localPosition = new Vector3 (0F, 0.0225F, 0F);
        _Cap.transform.localEulerAngles = new Vector3 (0F, 0F, 0F);
    }

    public override void evacuate (GameObject _Object) {

        setCapOn (null);

        _Object.GetComponent<MouseUI> ().Place = null;
        
        _Object.transform.parent = null;

    }

}
