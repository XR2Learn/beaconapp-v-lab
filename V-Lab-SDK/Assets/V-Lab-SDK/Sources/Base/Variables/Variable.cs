using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Variable: MonoBehaviour {

    private List<VariableListener> listeners;

    public abstract object AsObject();

    protected Variable() {
        listeners = new List<VariableListener>();
    }

    public void AddListener(VariableListener listener) {
        listeners.Add(listener);
    }

    public void RemoveListener(VariableListener listener) {
        listeners.Add(listener);
    }

    protected void NotifyListeners() {
        foreach (VariableListener listener in listeners) {
            listener.ValueChanged();
        }
    }
}

/**
 *
 */
public abstract class Variable<T>: Variable {

    public abstract T Get();

    public abstract void Set(T value);

    public override object AsObject() {
        return Get();
    }

    public override string ToString() {
        return Get().ToString();
    }
}