using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordLinkOrder
{
    public class WordLinkOrder
    {

        private Application word = null; // Word application
        private string path;  // The path to the Word document file
        public bool Verbose { get; set; } = false;

        public WordLinkOrder(string path)
        {
            System.Windows.Forms.MessageBox.Show("WordLinkOrder получил строку");
            // Check if file exists
            if (!File.Exists(path))
                throw new FileNotFoundException("The specified file does not exist.", path);

            this.path = path;
        }

        public int[] Run()
        {
            System.Windows.Forms.MessageBox.Show("WordLinkOrder запущен");
            // Initialise word if not already present
            if (this.word == null)
            {
                if (this.Verbose)
                    Console.WriteLine("Opening Word...");
                this.word = new Application();
                this.word.Visible = false;
            }

            // Read document
            if (this.Verbose)
                Console.WriteLine($"Reading document {this.path}...");
            Document document = this.word.Documents.OpenNoRepairDialog(FileName: this.path, ReadOnly: true, Visible: false);
            string documentText = String.Join(Environment.NewLine, document.StoryRanges.OfType<Range>().SelectMany(r => r.Paragraphs.OfType<Paragraph>()).Select(p => p.Range.Text)).Replace("\v", Environment.NewLine);
            document.Close(SaveChanges: false);

            // Close word
            if (this.Verbose)
                Console.WriteLine("Closing Word...");
            this.word.Quit(SaveChanges: false);
            this.word = null;

            // Find all occurences of [n]
            if (this.Verbose)
                Console.WriteLine("Finding matches...");
            MatchCollection matches = Regex.Matches(documentText, @"\[(\d+)\]");
            if (this.Verbose)
                Console.WriteLine($"Found {matches.Count} references.");

            // Build reference list
            if (this.Verbose)
                Console.WriteLine("Building reference list...");
            List<int> referenceList = new List<int>();

            foreach (Match match in matches)
            {
                // Convert matched [n] to int n
                int referenceNumber = int.Parse(match.Groups[1].Value);

                if (this.Verbose)
                    Console.WriteLine($"Found reference {referenceNumber}");
                
                referenceList.Add(referenceNumber);
            }

            // Return the result
            return referenceList.ToArray();
        }
    }
}
