using UnityEngine;

public abstract class Effector: MonoBehaviour {

    [SerializeField]
    protected BooleanVariable preconditions;

    public abstract void Trigger();
}
