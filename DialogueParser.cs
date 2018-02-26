using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Elm.Dialogue.Parser.Core
{
    public class DialogueParser : MonoBehaviour
    {
        private struct DialogueLine
        {
            public string id { get; set; }
            public string name { get; set; }
            public string content { get; set; }
            public string[] choices;
            public string[] choiceLead;

            public DialogueLine(string Id, string Name, string Content)
            {
                id = Id;
                name = Name;
                content = Content;
                choices = new string[0];
                choiceLead = new string[0];
            }
        }

        private List<DialogueLine> _lines;
        [Tooltip("File where dialogue is stored.")] [SerializeField] private string _dialogueFile;


        private void Start()
        {
            string file = string.Format("{0}/Dialogue/{1}", Application.streamingAssetsPath, _dialogueFile);

            _lines = new List<DialogueLine>();

            LoadDialogue(file);
        }

        public void LoadDialogue(string filename)
        {
            string line;
            StreamReader r = new StreamReader(filename);

            using (r)
            {
                do
                {
                    line = r.ReadLine();
                    if (line != null)
                    {
                        string[] lineData = line.Split(';');

                        DialogueLine lineEntry = new DialogueLine(lineData[0], lineData[1], lineData[2]);
                        lineEntry.choices = lineData[3].Split(':');
                        lineEntry.choiceLead = lineData[4].Split(':');
                        _lines.Add(lineEntry);
                    }
                }
                while (line != null);
                r.Close();
            }
        }

        public string GetName(int eventId)
        {
            return FilterLine(eventId).name;
        }

        public string GetContent(int eventId)
        {
            return FilterLine(eventId).content;
        }

        public string[] GetChoices(int eventId)
        {
            return FilterLine(eventId).choices;
        }

        public string[] GetChoiceLeads(int eventId)
        {
            return FilterLine(eventId).choiceLead;
        }

        private DialogueLine FilterLine(int eventId)
        {
            IEnumerable<DialogueLine> filteredLine = _lines.Where(user => (user.id == eventId.ToString()));
            List<DialogueLine> filteredLineList = filteredLine.ToList();
            return filteredLineList[0];
        }
    }

}
