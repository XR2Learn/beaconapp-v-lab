using UnityEngine;

/**
 *
 */
public abstract class BoundedIntegerVariable: Variable<int> {

    [SerializeField]
    private int Min;

    [SerializeField]
    private int Max;

    [SerializeField]
    private bool cycle;

    protected void Awake() {
        int v = Get();
        if (v > Max) {
            Set(Max);
        }
        else if (v < Min) {
            Set(Min);
        }
    }

    public void Increase() {
        int v = Get();
        if (v++ > Max) {
            v = cycle ? Max : Min;
        }
        Set(v);
    }

    public void Decrease() {
        int v = Get();
        if (v-- < Min) {
            v = cycle ? Min : Max;
        }
        Set(v);
    }
}
