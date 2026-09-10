
using System;
using System.Collections;
using System.Collections.Generic;
using Hassa.Essentials;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hassa.DialogueSystem
{

    public class DialogueView : MonoBehaviour
    {
        [SerializeField] protected UIDocument m_document;
        [SerializeField] protected StyleSheet m_styleSheet;

        protected VisualElement m_root;
        protected VisualElement m_container;
        protected Image m_actorIcon;
        protected Label m_actorName;
        protected VisualElement m_dialogueTextBox;
        protected Label m_dialogueText;
        protected VisualElement m_dialogueOptionBox;

        protected Button m_continueButton;

        protected Sprite ActorIconSprite;

        public event Action OnSkipLineTyping = delegate { };
        public event Action OnContinue = delegate { };
        public event Action<int> OnOptionSelect = delegate { };

        public IEnumerator InitializeView()
        {
            m_root = m_document.rootVisualElement;
            m_root.Clear();
            m_root.styleSheets.Add(m_styleSheet);

            m_container = m_root.CreateChild("container");
            m_container.focusable = true;
            m_container.Focus();
            m_container.RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.NoTrickleDown);

            var dialogue = m_container.CreateChild("dialogue-container");
            var header = dialogue.CreateChild("header");
            m_actorIcon = header.CreateChild<Image>("actor-icon");
            m_actorName = header.CreateChild<Label>("actor-name");
            m_actorName.text = "<Actor name>";

            m_dialogueTextBox = dialogue.CreateChild("dialogue-text-box");
            m_dialogueText = m_dialogueTextBox.CreateChild<Label>();
            m_dialogueText.text = "<Dialogue line>";
            // var text = m_dialogueTextBox.CreateChild<Label>();
            // text.text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis arcu odio, hendrerit eu diam sit amet, pharetra vulputate enim. Etiam tortor magna, semper sed dui vel, dignissim pretium justo. Maecenas posuere sem in faucibus viverra. Aliquam erat volutpat.";
            // text = m_dialogueTextBox.CreateChild<Label>();
            // text.text = "Aenean eu tortor mattis elit feugiat convallis eu sit amet sem. Donec ut dolor sollicitudin, aliquet justo eu, volutpat elit. Donec pretium mauris fermentum scelerisque laoreet. Etiam id ipsum non odio dictum luctus. Curabitur non lectus purus. Sed non ante sit amet sem hendrerit consectetur ut sit amet risus. Nunc rhoncus libero nisl, in venenatis nulla maximus vel. Suspendisse blandit laoreet sagittis. Suspendisse porta, arcu vitae vulputate tincidunt, sapien purus tincidunt nisl, a vehicula enim mi quis nibh. ";

            m_dialogueOptionBox = dialogue.CreateChild("dialogue-option-box");
            m_dialogueOptionBox.style.display = DisplayStyle.None;

            // for (var i = 0; i < 20; i++) {
            //     var option = m_dialogueOptionBox.CreateChild<Label>();
            //     option.text = $"Option {i}";
            //     option.style.display = DisplayStyle.None;

            //     m_optionButtonList.Add(option);
            // }

            m_continueButton = dialogue.CreateChild<Button>("dialogue-continue-button");
            m_continueButton.text = "Continue";
            m_continueButton.style.display = DisplayStyle.None;
            m_continueButton.RegisterCallback<ClickEvent>(OnContinueClicked);

            // var option = m_dialogueOptionBox.CreateChild<Label>();
            // option.text = "1. Maecenas egestas ornare neque ac pulvinar.";
            // option = m_dialogueOptionBox.CreateChild<Label>();
            // option.text = "2. Cras in eros semper, posuere erat nec, pretium turpis.";
            // option = m_dialogueOptionBox.CreateChild<Label>();
            // option.text = "3. Cras nisl ante, aliquet eget dapibus at, ultrices a arcu.";
            // option = m_dialogueOptionBox.CreateChild<Label>();
            // option.text = "4. Ut tincidunt arcu eros, a fringilla purus aliquet nec.";

            Hide();
            yield return null;
        }

        public void Show()
        {
            m_container.style.display = DisplayStyle.Flex;
        }
        public void Hide()
        {
            m_container.style.display = DisplayStyle.None;
        }

        public void UpdateLine(string line) => m_dialogueText.text = line;

        public void StartLine()
        {
            // Zaciname psat novy radek, nastavime container aby mohl prijimat callback na skipTyping
            // m_container.focusable = true;
            // m_container.Focus();
            // m_container.RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.NoTrickleDown);

            // Schovame Continue button
            m_continueButton.style.display = DisplayStyle.None;

            // Vymazeme text
            // TODO: presun do historie?
            m_dialogueText.text = "";
        }

        public void EndLine()
        {
            // Radek je vypsany, zrusime containeru moznost prijimani callbacku na skipTyping
            // m_container.focusable = false;
            // m_container.Blur();
            // m_container.UnregisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.NoTrickleDown);
        }

        public void StartOptions()
        {
            m_dialogueOptionBox.Clear();
            m_dialogueOptionBox.style.display = DisplayStyle.None;
        }

        public void EndOptions()
        {
            m_dialogueOptionBox.style.display = DisplayStyle.Flex;
        }

        public void AddOption(int index, string text, bool visited)
        {
            var option = m_dialogueOptionBox.CreateChild<Label>();

            option.text = text;
            option.style.display = DisplayStyle.Flex;
            option.RegisterCallback<PointerDownEvent>(evt => OnOptionClicked(index, text));
        }

        public void ShowContinueButton()
        {
            Debug.Log("Show Continue Button");
            m_continueButton.style.display = DisplayStyle.Flex;
        }

        void OnKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Space) {
                if (m_continueButton.style.display == DisplayStyle.Flex) {
                    Debug.Log("Continue button clicked...");
                    OnContinue?.Invoke();
                }
                else {
                    Debug.Log("Skiping line typing...");
                    OnSkipLineTyping?.Invoke();
                }
            }
            else if (evt.keyCode == KeyCode.Alpha1) {
                var index = evt.shiftKey ? 11 : 1;
                OnOptionClicked(index, "");
            }
        }

        void OnContinueClicked(ClickEvent evt)
        {
            Debug.Log("Continue button clicked...");
            OnContinue?.Invoke();
        }

        void OnOptionClicked(int index, string text)
        {
            Debug.Log($"Option {index} selected...");

            m_dialogueOptionBox.Clear();
            m_dialogueOptionBox.style.display = DisplayStyle.None;

            OnOptionSelect?.Invoke(index);
        }

    }
}