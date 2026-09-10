using System.Linq;
using Hassa.DialogueSystem;
using Unity.VisualScripting;

namespace Hassa.Editor.DialogueSystem
{
    [Descriptor(typeof(PlayerChoice))]
    public class PlayerChoiceDescriptor : UnitDescriptor<PlayerChoice>
    {
        public PlayerChoiceDescriptor(PlayerChoice unit) : base(unit) { }


        protected virtual string GetLabelForOption(PlayerChoice.ChoiceOption option)
        {
            if (string.IsNullOrEmpty(option.Text)) {
                return "<SKIPPED>";
            }

            var formattedOption = option.Text;

            var newLineIndex = formattedOption.IndexOf('\n');
            if (newLineIndex >= 0) {
                formattedOption = formattedOption[..newLineIndex];
            }
            if (formattedOption.Length > 24) {
                formattedOption = formattedOption[..24] + "...";
            }

            return formattedOption;
        }

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);

            switch (port.key) {
                case "Enter":
                    description.summary = "Trigger the concatenation of two strings, myValueA and myValueB, and return the result string on the Result port.";
                    break;
                case "Default":
                    description.summary = "Execute the next action in the Script Graph after concatenating myValueA and myValueB.";
                    break;
            }

            for (var i = 0; i < unit.Options.Count; i++) {
                var option = unit.Options[i];
                var condition = unit.ChoiceConditions[i];
                var choice = unit.ChoiceOutputs[i];
                var label = GetLabelForOption(option);

                if (condition == port) {
                    description.showLabel = false;
                    description.summary = $"The value to return if the enum has the value {label}.";
                }
                if (choice == port) {
                    description.label = label;
                    description.summary = $"The value to return if the enum has the value {label}.";
                }
            }
        }
    }
}