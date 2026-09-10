using UnityEngine;
using Unity.VisualScripting;


namespace Hassa.DialogueSystem
{

    /// <summary>
    /// T
    /// </summary>
    [UnitCategory("Dialogue")]
    [UnitTitle("Dialogue End")]
    [TypeIcon(typeof(GraphOutput))]
    public class DialogueEnd : DialogueUnit
    {

        /// <summary>
        /// A
        /// </summary>
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput Enter { get; private set; }

        protected override void Definition()
        {
            Enter = ControlInput(nameof(Enter), OnEnter);
        }

        private ControlOutput OnEnter(Flow flow)
        {
            var adapter = GetDialogueAdapter(flow);

            Debug.Log($"Dialogue '{flow.stack.self.name}' ended!");
            adapter.HandleDialogueEnd(false);

            return null;
        }
    }

}