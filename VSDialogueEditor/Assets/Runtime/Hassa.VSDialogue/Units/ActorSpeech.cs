using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using System.Collections;


namespace Hassa.DialogueSystem
{

    /// <summary>
    /// T
    /// </summary>
    [UnitCategory("Dialogue")]
    [UnitTitle("Actor Speech")]
    [TypeIcon(typeof(IList))]
    public class ActorSpeech : DialogueUnit
    {


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
        public ControlOutput After { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [DoNotSerialize]
        public ValueInput Condition { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [DoNotSerialize]
        public List<ValueInput> LineInputs { get; private set; }

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
        [InspectorLabel("Lines")]
        [InspectorTextArea(minLines = 5)]
        public List<string> Lines { get; set; } = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        [Inspectable]
        public List<AudioClip> AudioClips { get; private set; }

        [DoNotSerialize]
        private bool m_isLineComplete = false;

        #endregion

        #region IUnit Interface

        public override bool canDefine => Lines != null;

        protected override void Definition()
        {
            Enter = ControlInputCoroutine(nameof(Enter), OnEnter);
            After = ControlOutput(nameof(After));

            Condition = ValueInput(nameof(Condition), true);

            LineInputs = new List<ValueInput>();

            for (var i = 0; i < Lines.Count; i++) {
                var input = ValueInput<object>($"line_{i + 1}", Lines[i]).AllowsNull();
                LineInputs.Add(input);
            }

            Requirement(Condition, Enter);
            Succession(Enter, After);
        }

        public void CopyFrom(ActorSpeech source)
        {
            base.CopyFrom(source);
            Actor = source.Actor;
        }

        #endregion

        #region OnEnter

        private IEnumerator OnEnter(Flow flow)
        {
            var adapter = GetDialogueAdapter(flow);

            var isValid = flow.GetValue<bool>(Condition);
            if (isValid) {
                for (var i = 0; i < Lines.Count; i++) {
                    m_isLineComplete = false;

                    var line = Lines[i];
                    var input = LineInputs[i];
                    if (input.connectedPorts.ToArray().Length > 0) {
                        var inputValue = flow.GetValue<object>(input);
                        line = inputValue.ToString();
                    }

                    if (string.IsNullOrEmpty(line)) {
                        continue;
                    }

                    var info = new LineInfo { text = line, actor = Actor, onComplete = CompleteLine };
                    adapter.HandleLine(info);
                    Debug.Log($"Input {input.key}: {line}");

                    while (!m_isLineComplete) {
                        yield return null;
                    }
                }
            }

            yield return After;
        }

        public void CompleteLine() => m_isLineComplete = true;

        #endregion

    }
}