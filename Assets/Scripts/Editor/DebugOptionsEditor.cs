#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// A custom editor for the DebugOptions class, providing additional 
/// inspector buttons for triggering immediate game over or game win.
/// </summary>
[CustomEditor(typeof(DebugOptions))]
public class DebugOptionsEditor : Editor
{
    /// <summary>
    /// Overrides the default inspector GUI to include custom buttons 
    /// that call the DebugOptions script's methods for game over/win.
    /// </summary>
    public override void OnInspectorGUI()
    {
        // Draws default Inspector fields (toggles, etc.)
        base.OnInspectorGUI();

        // Retrieve the target script instance
        DebugOptions debugOptions = (DebugOptions)target;

        // Adds a button in the Inspector to trigger game over
        if (GUILayout.Button("Trigger Game Over"))
        {
            debugOptions.TriggerImmediateGameOver();
        }

        // Adds a button in the Inspector to trigger game win
        if (GUILayout.Button("Trigger Game Win"))
        {
            debugOptions.TriggerImmediateGameWin();
        }
    }
}
#endif