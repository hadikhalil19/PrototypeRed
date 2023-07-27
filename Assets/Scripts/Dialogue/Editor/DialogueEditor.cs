using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Proto.Dialogue.Editor {
    public class DialogueEditor : EditorWindow {

        [MenuItem("Window/Dialogue Editor")]
        private static void ShowEditorWindow() {
            var window = GetWindow<DialogueEditor>();
            window.titleContent = new GUIContent("Dialogue");
            window.Show();
        }

        private void OnGUI() {
            
        }
    }
}

