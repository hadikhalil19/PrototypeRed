using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Proto.Dialogue.Editor {
    public class DialogueEditor : EditorWindow {

        Dialogue selectedDialogue = null;

        [MenuItem("Window/Dialogue Editor")]
        private static void ShowEditorWindow() {
            var window = GetWindow<DialogueEditor>();
            window.titleContent = new GUIContent("Dialogue Editor");
            window.Show();
        }

        

        [OnOpenAsset(1)]
        public static bool OnOpenDialogueAsset(int instanceID, int line) {
            
           if(EditorUtility.InstanceIDToObject(instanceID) is Dialogue dialogue){
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable() {
            Selection.selectionChanged += OnSelectionChanged;
        }
        private void OnSelectionChanged() {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if (newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }

        }
        


        private void OnGUI() {
            if (selectedDialogue == null){
                EditorGUILayout.LabelField("No Dialogue Selected.");
            } else {
                foreach (DialogueNode node in selectedDialogue.GetAllNodes()) {
                    EditorGUILayout.LabelField(node.text);
                }
            }
            
        }
    }
}

