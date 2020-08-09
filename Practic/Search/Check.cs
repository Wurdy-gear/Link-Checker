using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordLinkOrder;
using WordLinkOrderChecker;
using System.IO;
using Practic;

namespace WordLinkOrderChecker
{
    class Check
    {
        public void Checker (string path)
        {

            WordLinkOrder.WordLinkOrder wordLinkOrder = null;
            try
            {
                wordLinkOrder = new WordLinkOrder.WordLinkOrder(path);
            }
            catch (FileNotFoundException ex)
            {
                //Console.Error.WriteLine($"The file {ex.FileName} could not be found.");
                return;
            }

            //Running a check
            int[] references = wordLinkOrder.Run();
            System.Windows.Forms.MessageBox.Show("Check запущен и начал проверку");
            // Check reference order
            int firstNumber = 0;
            int lastNumber = 0;
            bool hasBadReferences = false;
                foreach (int reference in references)
                {
                    if (reference != firstNumber + 1)
                    {
                    System.Windows.Forms.MessageBox.Show($"Bad reference order found: Got [{reference}] but expected [{lastNumber + 1}]!");
                    //Console.WriteLine($"Bad reference order found: Got [{reference}] but expected [{lastNumber + 1}]!");
                    hasBadReferences = true;
                    }

                    firstNumber = reference;
                    lastNumber = lastNumber + 1;
                }
            //Тут должен быть вызов нового окна с результатами

           // new ResultWindow(references).ShowDialog(); // Show ResultsWindow modally
        }
    }
}