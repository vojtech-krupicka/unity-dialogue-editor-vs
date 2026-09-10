using System;
using UnityEngine;
using Unity.VisualScripting;

namespace Hassa.Editor.DialogueSystem
{
    using Hassa.DialogueSystem;
    using UnityEditor;

    [Inspector(typeof(PlayerChoice.ChoiceOption))]
    public sealed class ChoiceOptionInspector : Inspector
    {
        public ChoiceOptionInspector(Metadata metadata) : base(metadata) { }

        private Metadata textMetadata => metadata[nameof(PlayerChoice.ChoiceOption.Text)];
        private Metadata typeMetadata => metadata[nameof(PlayerChoice.ChoiceOption.Type)];

        protected override float GetHeight(float width, GUIContent label)
        {
            var height = 0f;

            height += LudiqGUI.GetInspectorHeight(this, typeMetadata, width, label);
            height += EditorGUIUtility.standardVerticalSpacing * 2.0f;
            height += LudiqGUI.GetInspectorHeight(this, textMetadata, width, label);

            return height;
        }

        protected override bool cacheHeight => false;

        protected override void OnGUI(Rect position, GUIContent label)
        {
            BeginLabeledBlock(metadata, position, label);

            EditorGUI.BeginChangeCheck();

            LudiqGUI.Inspector(typeMetadata, position.VerticalSection(ref y, LudiqGUI.GetInspectorHeight(this, typeMetadata, position.width)));

            y += EditorGUIUtility.standardVerticalSpacing * 2.0f;

            LudiqGUI.Inspector(textMetadata, position.VerticalSection(ref y, LudiqGUI.GetInspectorHeight(this, textMetadata, position.width)));

            if (EditorGUI.EndChangeCheck()) {

            }

            EndBlock(metadata);
        }

        public override float GetAdaptiveWidth()
        {
            return textMetadata.Inspector().GetAdaptiveWidth();
        }
    }
}