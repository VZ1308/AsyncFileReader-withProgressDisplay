using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsyncProgramming.Controllers;

namespace AsyncFileReader
{
    class Program
    {
        [STAThread] // STA-Thread-Attribut hinzufügen
        static void Main(string[] args) // Entferne async
        {
            string filePath = null;

            // OpenFileDialog muss in einem synchronen STA-Thread aufgerufen werden
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.Filter = "Textdateien (*.txt)|*.txt|Alle Dateien (*.*)|*.*";
                openFileDialog.Title = "Wählen Sie eine Textdatei aus";

                // Datei-Auswahl über OpenFileDialog
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
                else
                {
                    Console.WriteLine("Keine Datei ausgewählt.");
                    return; // Beende, wenn keine Datei ausgewählt wurde
                }
            }

            // Starte den asynchronen Leseprozess nach der Dateiauswahl
            var controller = new FileController();
            Task.Run(async () => await controller.LoadFileAsync(filePath)).GetAwaiter().GetResult();
        }
    }
}
