using Unity.VisualScripting;

namespace Hassa.DialogueSystem
{
    /// <summary>
    /// Called when the dialogue becomes enabled and active.
    /// Always set coroutine flag to true and hide it in inspector.
    /// </summary>
    [UnitCategory("Dialogue")]
    [UnitTitle("Dialogue Start")]
    [TypeIcon(typeof(GraphInput))]
    public class DialogueStart : MachineEventUnit<EmptyEventArgs>
    {
        protected override string hookName => EventHooks.OnEnable;

        public new bool coroutine { get; set; } = true;

        protected override void Definition()
        {
            base.Definition();
            coroutine = true;
        }

    }
}