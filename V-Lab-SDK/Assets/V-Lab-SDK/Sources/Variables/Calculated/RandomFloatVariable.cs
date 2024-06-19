using UnityEngine;

public class RandomFloatVariable: NumericVariable {

    [SerializeField]
    private float lowerBound;

    [SerializeField]
    private float upperBound;

    [SerializeField]
    private int seed;

    private System.Random rnd;

    public void Start() {
        if (seed == 0) {
            rnd = new System.Random();
        }
        else {
            rnd = new System.Random(seed);
        }
    }

    public override float Get() {
        return (float) (lowerBound + rnd.NextDouble() * (upperBound - lowerBound));
    }

    public override void Set(float value) {
        // NOP...
    }
}
