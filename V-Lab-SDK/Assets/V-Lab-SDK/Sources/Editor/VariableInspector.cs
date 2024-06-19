using UnityEditor;
using UnityEngine;

public class VariableInspector: EditorWindow, VariableListener {

    private Variable[] vars;

    private bool dirty = false;

    // private bool isActualPlaymode = false;

    private GUIStyle labelStyle;

    [SerializeField]
    private bool realtimeRefresh;

    public void Refresh() {
        lock(typeof(VariableInspector)) {
            if (vars == null) {
                Debug.Log("Refresh");
                vars = FindObjectsByType<Variable>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                foreach (Variable var in vars) {
                    var.AddListener(this);
                }
            }
        }
    }

    public void UnWatch() {
        lock (typeof(VariableInspector)) {
            if (vars != null) {
                Debug.Log("Unwatch");
                foreach (Variable var in vars) {
                    var.RemoveListener(this);
                }
                vars = null;
            }
        }
    }

    [MenuItem("V-Lab/SDK/Variable Inspector")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(VariableInspector));
    }

    public void Awake() {
        Debug.Log("Awake");
    }
    
    public void PlayModeStateChangeHandler(PlayModeStateChange playModeStateChange) {
        // Debug.Log(playModeStateChange.ToString());
        switch (playModeStateChange) {
            case PlayModeStateChange.EnteredPlayMode:
                Refresh();
                // isActualPlaymode = true;
                break;
            case PlayModeStateChange.ExitingPlayMode:
                UnWatch();
                // isActualPlaymode = false;
                break;
        }
    }

    public void OnEnable() {
        // Debug.Log("OnEnable");
        UnWatch();
        EditorApplication.playModeStateChanged += PlayModeStateChangeHandler;
    }

    public void OnDestroy() {
        // Debug.Log("OnDestroy");
        UnWatch();
        EditorApplication.playModeStateChanged -= PlayModeStateChangeHandler;
    }

    public void CreateGUI() {
        // Debug.Log("CreateGUI");
        if (EditorApplication.isPlaying) {
            UnWatch();
            Refresh();
        }
    }

    public void OnBecameVisible() {
        // Debug.Log("OnBecameVisible");
    }

    public void OnBecameInvisible() {
        // Debug.Log("OnBecameInvisible");
    }

    public void OnSelectionChange() {
        Debug.Log("OnSelectionChange");
        dirty = true;
    }

    public void OnHierarchyChange() {
        Debug.Log("OnHierarchyChange");
        dirty = true;
    }

    public void OnProjectChange() {
        Debug.Log("OnProjectChange");
        dirty = true;
    }

    public void OnInspectorUpdate() {
        // Debug.Log("OnInspectorUpdate");
        if (dirty || realtimeRefresh) {
            dirty = false;
            Repaint();
        }
    }

    public void OnGUI() {

        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.wordWrap = true;

        // Debug.Log("OnGUI");

        float w = EditorGUIUtility.currentViewWidth;
        float wHalf = w * 0.5f;
        float wThird = w * 0.333f;
        GUILayout.BeginVertical();

        if (vars != null) {
            if (GUILayout.Button("Refresh list")) {
                Refresh();
            }
            realtimeRefresh = GUILayout.Toggle(realtimeRefresh, "Realtime refresh");
            EditorGUILayout.Separator();
            GUILayout.BeginVertical();
            foreach (Variable var in vars) {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{var.name} ({var.GetType().Name})", GUILayout.Width(wHalf));
                GUILayout.Label(var.ToString(), GUILayout.Width(wHalf));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        else {
            GUILayout.Label("No vars to show. Enter play mode to refresh.", labelStyle, GUILayout.Width(w));
        }
        GUILayout.EndVertical();
    }

    public void ValueChanged() {
        dirty = true;
    }
}
