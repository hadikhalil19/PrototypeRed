using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Proto.Dialogue.Editor {
    public class DialogueEditor : EditorWindow {

        [MenuItem("Window/Dialogue Editor")]
        private static void ShowEditorWindow() {
            var window = GetWindow<DialogueEditor>();
            window.titleContent = new GUIContent("Dialogue Editor");
            window.Show();
        }

        private void OnGUI() {
            EditorGUI.LabelField(new Rect(10,10,200,200), "Hello World");
        }

        [OnOpenAssetAttribute(1)]
        public static bool OnOpenDialogueAsset(int instanceID, int line) {
            
           if(EditorUtility.InstanceIDToObject(instanceID) is Dialogue dialogue){
                ShowEditorWindow();
                return true;
            }
            return false;
        }
    }
}

