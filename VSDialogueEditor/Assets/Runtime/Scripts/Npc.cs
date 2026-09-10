using UnityEngine;
using UnityEngine.InputSystem;
using Hassa.DialogueSystem;

public class NPC : MonoBehaviour
{
    [SerializeField] DialogueAdapter m_dialogueAdapter;

    InputAction m_interactAction;
    InputAction m_jumpAction;

    void Start()
    {
        m_interactAction = InputSystem.actions.FindAction("Interact");
        m_jumpAction = InputSystem.actions.FindAction("Jump");

        if (m_dialogueAdapter == null) {
            m_dialogueAdapter = GetComponent<DialogueAdapter>();
        }
    }

    void Update()
    {
        if (m_interactAction.WasPressedThisFrame()) {
            m_dialogueAdapter.StartDialogue();
        }
        if (m_jumpAction.WasPressedThisFrame()) {
            //m_dialogueAdapter.StartDialogue("Test Two");
        }
    }
}
