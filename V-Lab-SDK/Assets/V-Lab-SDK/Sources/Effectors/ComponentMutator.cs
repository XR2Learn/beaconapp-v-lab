using System;
using System.Reflection;
using UnityEngine;

/**
 * @todo Derive from Effector.
 */
public class ComponentMutator: Effector, VariableListener {

    [SerializeField]
    private Variable input;

    [SerializeField]
    private Component target;

    [SerializeField]
    private string field;
    
    public void OnEnable() {
        input.AddListener(this);
    }

    public void OnDestroy() {
        input.RemoveListener(this);
    }

    public void Run() {

        PropertyInfo pi = target.GetType().GetProperty(field, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);
        FieldInfo fi = target.GetType().GetField(field, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField);

        // Debug.Log(input);
        // Debug.Log(input.AsObject());

        if (pi != null) {
            // Debug.Log($"{pi.Name}: {pi.PropertyType}");
            // Debug.Log(pi.GetValue(target));
            pi.SetValue(target, input.AsObject());
        }
        else if (fi != null) {
            // Debug.Log($"{fi.Name}: {fi.FieldType}");
            // Debug.Log(fi.GetValue(target));
            fi.SetValue(target, input.AsObject());
        }
    }

    public override void Trigger() {
        Run();
    }

    public void ValueChanged() {
        Run();
    }
}
