using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.UI;

namespace Hassa.DialogueSystem
{

    public enum ChoiceType
    {
        Normal,
        Exit,
        Fight,
        Quest
    }

    /// <summary>
    /// T
    /// </summary>
    [UnitCategory("Dialogue")]
    [UnitTitle("Player Choice")]
    [TypeIcon(typeof(IBranchUnit))]
    public class PlayerChoice : DialogueUnit
    {

        public class ChoiceOption
        {
            [Serialize]
            [Inspectable]
            [InspectorWide]
            [InspectorLabel("Text")]
            [InspectorTextArea(minLines = 2)]
            public string Text { get; set; }

            [Serialize]
            public ChoiceType Type { get; set; }
        }


        #region Inputs & Outputs

        /// <summary>
        /// A
        /// </summary>
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput Enter { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput Default { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        [DoNotSerialize]
        public List<ValueInput> ChoiceConditions { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        [DoNotSerialize]
        public List<ControlOutput> ChoiceOutputs { get; private set; }

        #endregion

        #region Fields

        /// <summary>
        /// 
        /// </summary>
        [Inspectable]
        [InspectorLabel("Actor")]
        [UnitHeaderInspectable("Actor")]
        public GameObject Actor { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        [Serialize]
        [Inspectable]
        [InspectorWide]
        [InspectorLabel("Options")]
        [InspectorTextArea(minLines = 2)]
        public List<ChoiceOption> Options { get; set; } = new List<ChoiceOption>();

        [DoNotSerialize]
        private int m_selectedIndex = -1;

        #endregion

        #region IUnit Interface

        public override bool canDefine => Options != null;

        protected override void Definition()
        {
            Enter = ControlInputCoroutine(nameof(Enter), OnEnter);
            Default = ControlOutput(nameof(Default));

            ChoiceConditions = new List<ValueInput>();
            ChoiceOutputs = new List<ControlOutput>();

            for (var i = 0; i < Options.Count; i++) {
                var option = Options[i];

                var condition = ValueInput($"condition_{i + 1}", true);
                ChoiceConditions.Add(condition);

                var choice = ControlOutput($"option_{i + 1}");
                ChoiceOutputs.Add(choice);

                Requirement(condition, Enter);
                Succession(Enter, choice);
            }
        }

        public void CopyFrom(ActorSpeech source)
        {
            base.CopyFrom(source);
            Actor = source.Actor;
        }

        #endregion

        #region OnEnter

        public IEnumerator OnEnter(Flow flow)
        {
            var adapter = GetDialogueAdapter(flow);

            var validOptions = new List<int>();
            var choiceInfo = new ChoiceInfo { options = new(), onSelect = OptionSelected };

            for (var i = 0; i < Options.Count; i++) {
                var option = Options[i];

                // If text is valid
                if (string.IsNullOrEmpty(option.Text)) {
                    continue;
                }

                // Evaluate condition
                var choice = ChoiceOutputs[i];
                var condition = ChoiceConditions[i];
                var isValid = flow.GetValue<bool>(condition);
                if (isValid) {
                    validOptions.Add(i);
                }

                var optionInfo = new OptionInfo { text = option.Text, enabled = isValid, visited = false };
                choiceInfo.options.Add(optionInfo);

                Debug.Log($"Option<{option.Type}>: '{option.Text}' (valid: {isValid}, target: {choice.key})");
            }

            Debug.Log($"Valid Options: {string.Join(", ", validOptions)}");

            if (validOptions.Count == 0) {
                Debug.Log($"No valid options, run Default output");
                yield return Default;
            }

            adapter.HandleChoice(choiceInfo);

            while (m_selectedIndex < 0) {
                yield return null;
            }

            var selectedIndex = validOptions[m_selectedIndex];

            Debug.Log($"Selected<{m_selectedIndex} -> {selectedIndex}>: option<{Options[selectedIndex].Type}>: '{Options[selectedIndex].Text}' (target: {ChoiceOutputs[selectedIndex].key})");
            yield return ChoiceOutputs[selectedIndex];
        }

        public void OptionSelected(int index) => m_selectedIndex = index;

        #endregion
    }

}