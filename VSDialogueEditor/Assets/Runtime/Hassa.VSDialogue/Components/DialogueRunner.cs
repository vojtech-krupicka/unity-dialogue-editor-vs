// using Unity.VisualScripting;
// using UnityEngine;

// namespace Hassa.VSDialogue
// {
//     [AddComponentMenu("Hassa/Dialogue/Dialogue Runner")]
//     [RequireComponent(typeof(Variables))]
//     [DisableAnnotation]
//     public class DialogueRunner : EventMachine<FlowGraph, ScriptGraphAsset>
//     {
//         public override FlowGraph DefaultGraph()
//         {
//             return FlowGraph.WithStartUpdate();
//         }

//         protected override void OnEnable()
//         {
//             if (base.hasGraph) {
//                 base.graph.StartListening(base.reference);
//             }

//             base.OnEnable();
//         }

//         protected override void OnInstantiateWhileEnabled()
//         {
//             if (base.hasGraph) {
//                 base.graph.StartListening(base.reference);
//             }

//             base.OnInstantiateWhileEnabled();
//         }

//         protected override void OnUninstantiateWhileEnabled()
//         {
//             base.OnUninstantiateWhileEnabled();
//             if (base.hasGraph) {
//                 base.graph.StopListening(base.reference);
//             }
//         }

//         protected override void OnDisable()
//         {
//             base.OnDisable();
//             if (base.hasGraph) {
//                 base.graph.StopListening(base.reference);
//             }
//         }

//         [ContextMenu("Show Data...")]
//         protected override void ShowData()
//         {
//             base.ShowData();
//         }
//     }

// }