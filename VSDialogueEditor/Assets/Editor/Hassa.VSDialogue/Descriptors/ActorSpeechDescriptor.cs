using System.Linq;
using Hassa.DialogueSystem;
using Unity.VisualScripting;

namespace Hassa.Editor.DialogueSystem
{
    [Descriptor(typeof(ActorSpeech))]
    public class ActorSpeechDescriptor : UnitDescriptor<ActorSpeech>
    {
        public ActorSpeechDescriptor(ActorSpeech unit) : base(unit) { }


        protected virtual string GetLabelForLine(string line)
        {
            if (string.IsNullOrEmpty(line)) {
                return "<SKIPPED>";
            }

            var formattedLine = line;

            var newLineIndex = formattedLine.IndexOf('\n');
            if (newLineIndex >= 0) {
                formattedLine = formattedLine[..newLineIndex];
            }
            if (formattedLine.Length > 24) {
                formattedLine = formattedLine[..24] + "...";
            }


            return formattedLine;
        }

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);

            switch (port.key) {
                case "Enter":
                    description.summary = "Trigger the concatenation of two strings, myValueA and myValueB, and return the result string on the Result port.";
                    break;
                case "Condition":
                    description.summary = "First string value.";
                    break;
                case "After":
                    description.summary = "Execute the next action in the Script Graph after concatenating myValueA and myValueB.";
                    break;
            }

            // if (port.key.StartsWith("line_")) {
            //     description.showLabel = false;
            //     description.summary = "Line of dialogue.";
            // }

            for (var i = 0; i < unit.Lines.Count; i++) {
                var line = unit.Lines[i];
                var input = unit.LineInputs[i];

                if (input == port) {
                    var label = GetLabelForLine(line);

                    description.label = label;
                    description.summary = $"The value to return if the enum has the value {label}.";
                }
            }
        }
    }
}