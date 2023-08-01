using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Proto.Dialogue.Editor {
    public class DialogueEditor : EditorWindow {

        Dialogue selectedDialogue = null;
        GUIStyle nodeStyle;

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

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
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
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    OnGUINode(node);
                }
            }
            
        }

        private void OnGUINode(DialogueNode node)
        {
            GUILayout.BeginArea(node.position, nodeStyle);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Node:", EditorStyles.whiteLabel);

            string newText = EditorGUILayout.TextField(node.text);
            string newUniqueID = EditorGUILayout.TextField(node.uniqueID);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                node.text = newText;
                node.uniqueID = newUniqueID;
            }
            
            GUILayout.EndArea();
        }
    }
}

