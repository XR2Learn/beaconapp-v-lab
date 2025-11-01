using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingKnob : Knob {

    // Start is called before the first frame update
    public override void Start () {
    }


    public override void applyRotation (int _Direction) {

        setRotation (_Direction * da);

    }

}
