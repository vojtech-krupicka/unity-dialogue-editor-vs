using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace Hassa.DialogueSystem
{

    using DialogueRunner = ScriptMachine;

    public class DialogueAdapter : MonoBehaviour
    {
        [SerializeField] DialogueController m_controller;

        [SerializeField] DialogueRunner m_mainRunner;

        public SerializableDictionary<string, DialogueRunner> m_namedRunners = new();

        void Awake()
        {
            Assert.IsNotNull(m_controller, "");

            if (m_mainRunner) {
                m_mainRunner.enabled = false;
            }
            foreach (var item in m_namedRunners) {
                if (item.Value == null) {
                    continue;
                }

                item.Value.enabled = false;

                if (string.IsNullOrEmpty(item.Key)) {
                    if (m_mainRunner != null) {
                        Debug.LogWarning("Main dialogue runner is already set!");
                    }
                    else {
                        m_mainRunner = item.Value;
                    }
                }
            }
        }

        public void StartDialogue(string name = "")
        {
            if (string.IsNullOrEmpty(name)) {
                m_controller.StartDialogue(m_mainRunner);
            }
            else if (m_namedRunners.ContainsKey(name)) {
                m_controller.StartDialogue(m_namedRunners[name]);
            }
            else {
                Debug.LogWarning($"Dialogue with name '{name}' not registered within this adapter!");
            }
        }

        public void HandleLine(LineInfo info) => m_controller.RunLine(info);
        public void HandleChoice(ChoiceInfo info) => m_controller.RunOptions(info);
        public void HandleDialogueEnd(bool immediate) => m_controller.EndDialogue(immediate);

    }
}