using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unity.VisualScripting;

namespace Hassa.DialogueSystem
{

    using DialogueRunner = ScriptMachine;

    public class LineInfo
    {
        public string text;
        public GameObject actor;
        public System.Action onComplete;
    }

    public class OptionInfo
    {
        public string text = "";
        public bool enabled = false;
        public bool visited = false;
    }

    public class ChoiceInfo
    {
        public List<OptionInfo> options;
        public System.Action<int> onSelect;
    }

    public class DialogueController : MonoBehaviour
    {
        [SerializeField] DialogueView m_view;

        private DialogueRunner m_currentRunner = null;

        private LineInfo m_currentLineInfo = null;
        private LineInfo m_nextLineInfo = null;
        private ChoiceInfo m_currentChoiceInfo = null;

        private float m_typingSpeed = 0.05f;
        private float m_waitForLineEnd = 0.1f;


        private bool m_isTyping = false;
        private bool m_isDialogueActive = false;

        void Start()
        {
            m_view.OnSkipLineTyping += HandleSkipLineTyping;
            m_view.OnContinue += HandleContinue;
            m_view.OnOptionSelect += HandleOptionSelect;
        }

        public bool StartDialogue(DialogueRunner runner)
        {
            if (runner == null || m_currentRunner != null || m_isDialogueActive) {
                return false;
            }

            m_currentRunner = runner;
            m_isDialogueActive = true;

            //InputSystem.DisableAllEnabledActions();
            StartCoroutine(m_view.InitializeView());
            m_view.Show();

            Debug.Log("Dialogue Started");

            m_currentRunner.enabled = true;
            return true;
        }
        public void EndDialogue(bool immediate)
        {
            m_currentRunner.enabled = false;
            m_view.Hide();

            m_currentRunner = null;
            m_isDialogueActive = false;
            Debug.Log("Dialogue Ended");
        }


        public void RunLine(LineInfo info)
        {
            m_nextLineInfo = info;
            if (m_currentLineInfo == null) {
                // nemam aktualni radek (zacatek dialogu nebo po vyberu z choice)
                // prehodim nextLine do currentLine a currentLine zacnu vypisovat.
                m_currentLineInfo = m_nextLineInfo;
                m_nextLineInfo = null;
                DoRunLine();
            }
            else {
                // mam aktualni radek a dostavam novy, nic nedelam?
                m_view.ShowContinueButton();
            }
        }

        void DoRunLine()
        {
            StartCoroutine(TypeLine());
        }

        IEnumerator TypeLine()
        {
            // vycisti UI nebo prida do historie...
            m_view.StartLine();
            m_isTyping = true;

            // TODO: tady bude asi neco jako nastaveni noveho actora (jmeno a obrazek)
            // + rozhodnout se jestli ho nejak znam nebo jeste ne

            // TODO: taky tu bude preklad a formatovani textu
            var formattedLine = m_currentLineInfo.text;

            // Pokud mam nastavenou rychlost textu, tak iteruju nad textem, jinak vracim hned celou zpravu
            if (m_typingSpeed > 0.0f) {
                var sb = new StringBuilder();

                foreach (char c in formattedLine) {
                    sb.Append(c);

                    // TODO: tady zpracujeme vsechny HTML tagy a predame jako jeden string, proto pouzivame StringBuilder

                    // Nastavime aktualne precteny retezec (nebo jedno pismeno) z radky a predame do view
                    m_view.UpdateLine(sb.ToString());

                    // Pokud nam nekdo mezitim nastavil m_isTyping na false, koncime a posleme do view cely radek
                    if (!m_isTyping) {
                        m_view.UpdateLine(formattedLine);
                        break;
                    }

                    // Cekame pozadovany cas
                    yield return new WaitForSeconds(m_typingSpeed);
                }

                // Mame dopsano, aby bylo vzdy jiste ze mame cely radek vysazeny, aktualizujeme
                m_view.UpdateLine(formattedLine);
            }
            else {
                // Nesazime pismena, ale rovnou cely radek, protoze je zakazano sazeni
                m_view.UpdateLine(formattedLine);
            }

            // Radek jsem vypsaly cely, nepiseme
            m_isTyping = false;

            yield return new WaitForSeconds(m_waitForLineEnd);
            yield return new WaitForEndOfFrame();

            // Informujeme view, ze radek je kompletni, to by melo znamenat, ze nechceme dale poslouchat
            // na skip (space) sazeni radku do UI.
            m_view.EndLine();

            // Notifikujeme naprimo aktualni node, ze mame hotovo. To nam posle dalsi radek a by se v RunLine() 
            // rozhodneme, jestli zacneme hned sazet text nebo zobrazime tlacitko `Continue` nebo nam prijde
            // Choice s options. V kazdem pripade nic nedelame a nemame moznost jak reagovat v UI. Mezitim se
            // muze v dialogu dit spousta veci - cekame na animaci, zvuk, ...
            m_currentLineInfo.onComplete?.Invoke();
        }

        public void RunOptions(ChoiceInfo info)
        {
            m_currentChoiceInfo = info;

            m_view.StartOptions();

            for (var i = 0; i < info.options.Count; i++) {
                var option = info.options[i];
                Debug.Log($"Choice Option #{i}: {option.text}, {option.enabled}, {option.visited}");

                if (option.enabled) {
                    m_view.AddOption(i, option.text, option.visited);
                }
            }

            m_view.EndOptions();
        }

        private void HandleSkipLineTyping()
        {
            m_isTyping = false;
        }

        private void HandleContinue()
        {
            m_currentLineInfo = m_nextLineInfo;
            m_nextLineInfo = null;
            DoRunLine();
        }

        private void HandleOptionSelect(int index)
        {
            var choiceInfo = m_currentChoiceInfo;
            m_currentChoiceInfo = null;
            m_currentLineInfo = null;
            choiceInfo?.onSelect?.Invoke(index);
        }

    }
}