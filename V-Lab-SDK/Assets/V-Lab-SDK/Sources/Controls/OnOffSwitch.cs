using System;

using UnityEngine;

/**
 *
 */
[RequireComponent(typeof(Collider))]
public class OnOffSwitch: Control {

    [SerializeField]
    private BooleanVariable state;

    public OnOffSwitch() {
        SetCapabilities(new Capability[] {Capability.PRESS});
    }

    public override void Press(float duration) {
        base.Press(duration);
        state.Toggle();
    }
} 