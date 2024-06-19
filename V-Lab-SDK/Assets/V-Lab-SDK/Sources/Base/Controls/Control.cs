using System;

using UnityEngine;

/**
 * Represents a physical control that can be manipulated in physical space.
 */
public abstract class Control: MonoBehaviour {

    public enum Capability {
        MOVE,
        TURN,
        PRESS
    }

    private static float DEFAULT_MOVE_STEP = 0.001f;
    private static float DEFAULT_TURN_STEP = 0.1f;

    [SerializeField]
    private BooleanVariable Preconditions;

    private bool isTouching = false;

    private Capability[] capabilities = new Capability[] { };

    protected void SetCapabilities(Capability[] capabilities) {
        this.capabilities = capabilities;
    }

    public int GetCapabilitiesCount() {
        return capabilities.Length;
    }

    public int FindCapability(Capability capability) {
        for (int i = 0; i != capabilities.Length; i++) {
            if (capabilities[i].Equals(capability)) {
                return i;
            }
        }
        return -1;
    }

    public bool HasCapability(Capability capability) {
        return FindCapability(capability) != -1;
    }

    public Capability GetCapability(int index) {
        if (index < 0 || index >= capabilities.Length) {
            throw new IndexOutOfRangeException();
        }
        return capabilities[index];
    }

    public virtual void OnValueChanged() {
    }

    public virtual void Reset() {
    }

    public virtual void Press(float duration) {
    }

    public virtual void Move(Vector3 offset, Vector3 destination) {
    }

    public virtual void Turn(Vector3 newOrbitVector, Vector3 oldOrbitVector) {
    }

    public virtual void TouchStart() {
        isTouching = true;
    }

    public virtual void TouchEnd() {
        isTouching = false;
    }

    public virtual bool IsTouching() {
        return isTouching;
    }

    public virtual float GetMoveStep() {
        return DEFAULT_MOVE_STEP;
    }

    public virtual float GetTurnStep() {
        return DEFAULT_TURN_STEP;
    }
}
