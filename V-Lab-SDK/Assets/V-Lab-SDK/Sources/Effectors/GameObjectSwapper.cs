using System;
using UnityEngine;

/**
 * @todo Derive from Effector.
 */
public class GameObjectSwapper: MonoBehaviour, VariableListener {

    [SerializeField]
    private GameObject[] targets;

    [SerializeField]
    private IntegerVariable input;

    protected void Awake() {
        input.AddListener(this);
    }

    protected void Start() {
        if (enabled) {
            ActivateTarget(input.Get());
        }
    }

    public void ValueChanged() {
        if (enabled) {
            ActivateTarget(input.Get());
        }
    }

    public void ActivateTarget(int index) {
        if (index < 0 || index > targets.Length - 1) {
            throw new ArgumentException();
        }
        for (int i = 0; i != targets.Length; i++) {
            targets[i].SetActive(i == index);
        }
    }
}
