// using System;
// using UnityEngine;
// using Unity.VisualScripting;

// namespace Hassa.Editor.DialogueSystem
// {
//     using Hassa.DialogueSystem;

//     [Inspector(typeof(ActorSpeech))]
//     public sealed class ActorSpeechInspector : Inspector
//     {
//         public ActorSpeechInspector(Metadata metadata) : base(metadata) { }

//         private Metadata lineCountMetadata => metadata[nameof(ActorSpeech.LineCount)];

//         protected override float GetHeight(float width, GUIContent label)
//         {
//             return LudiqGUI.GetInspectorHeight(this, lineCountMetadata, width, label);
//         }

//         protected override bool cacheHeight => false;

//         protected override void OnGUI(Rect position, GUIContent label)
//         {
//             LudiqGUI.Inspector(lineCountMetadata, position, label);
//         }

//         public override float GetAdaptiveWidth()
//         {
//             return lineCountMetadata.Inspector().GetAdaptiveWidth();
//         }
//     }
// }