using System;
using System.Text.RegularExpressions;
using UnityEngine;

/**
 * 
 */
public class MathExpressionVariable: NumericVariable, VariableListener {

    private static readonly Regex placeholderRegex = new Regex("\\$[0-9]+");

    [SerializeField]
    private string expression;

    [SerializeField]
    private NumericVariable[] inputs;

    public void OnEnable() {
        foreach (Variable var in inputs) {
            var.AddListener(this);
        }
    }

    public void OnDestroy() {
        foreach (Variable var in inputs) {
            var.RemoveListener(this);
        }
    }

    public override float Get() {
        float value;
        string processedExpression = ReplacePlaceholders();
        if (!ExpressionEvaluator.Evaluate(processedExpression, out value)) {
            throw new ArgumentException($"Syntax error in expression: \"{processedExpression}\"");
        }
        return value;
    }

    public override void Set(float value) {
        // NOP...;
    }

    protected string ReplacePlaceholders() {
        return placeholderRegex.Replace(expression, new MatchEvaluator(PlaceholderMatcher));
    }

    protected string PlaceholderMatcher(Match m) {

        uint idx = uint.Parse(m.ToString().Substring(1));

        if (idx >= inputs.Length) {
            throw new ArgumentOutOfRangeException("Placeholder index out of range");
        }

        return inputs[idx].Get().ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ValueChanged() {
        NotifyListeners();
    }
}
