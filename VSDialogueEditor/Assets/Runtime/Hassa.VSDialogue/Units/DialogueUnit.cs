using UnityEngine;
using Unity.VisualScripting;

namespace Hassa.DialogueSystem
{

    /// <summary>
    /// T
    /// </summary>
    [UnitCategory("Dialogue")]
    public abstract class DialogueUnit : Unit
    {

        private DialogueAdapter m_dialogueAdapter;
        protected DialogueAdapter GetDialogueAdapter(Flow flow)
        {
            if (m_dialogueAdapter == null) {
                m_dialogueAdapter = flow.stack.self.GetComponentInParent<DialogueAdapter>();
            }

            return m_dialogueAdapter;
        }
    }
}