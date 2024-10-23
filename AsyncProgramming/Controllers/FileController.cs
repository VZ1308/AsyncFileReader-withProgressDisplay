using AsyncProgramming.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncProgramming.Controllers
{
    /// <summary>
    /// Klasse die für das Laden von Dateien und das Verwalten des Dateiinhalts zuständig ist
    /// </summary>
    public class FileController
    {
        private FileModel _fileModel; // speichert Instanz des FileModel Objekts

        // Konstruktor, um FileModel zu initialisieren
        public FileController()
        {
            _fileModel = new FileModel();
        }

        // Asynchrone Methode zum Laden der Datei
        public async Task LoadFileAsync(string filePath) // Task steht für eine asynchrone Operation:
                                                         // Es kann z.B. das Einlesen einer Datei oder das Abrufen von Daten aus dem Internet darstellen
        {
            _fileModel.FilePath = filePath; //Pfad wird in _fileModel gespeichert

            // Berechne die Gesamtzahl der Zeilen vorab, um den Fortschritt zu berechnen
            int totalLines = CountTotalLines(filePath);
            Console.WriteLine($"Total lines in file: {totalLines}");

            Console.WriteLine("Data is loading...");

            // Starten des Tasks zum Einlesen der Datei
            var readingTask = Task.Run(() => ReadFile(filePath, totalLines));

            // Fortschritt wird überwacht
            while (!readingTask.IsCompleted)
            {
                await Task.Delay(500); // Warte 500 ms
            }

            // ToDo: do while
            // Warten auf den Abschluss des Lesevorgangs
            await readingTask;
            Console.WriteLine("Reading finished.");
            
            string choice;
            bool isValidInput = false; // Variable, um zu überprüfen, ob die Eingabe gültig ist

            do
            {
                Console.WriteLine("Do you want to show the data? (Yes/No):");

                try
                {
                    choice = Console.ReadLine();

                    // Entferne Leerzeichen und konvertiere in Kleinbuchstaben
                    choice = choice.Trim().ToLower();

                    if (choice == "yes")
                    {
                        ShowFileContents(); // Zeigt die Datei an
                        isValidInput = true; // Gültige Eingabe -> Schleife beenden
                    }
                    else if (choice == "no")
                    {
                        Console.WriteLine("Contents will not be displayed.");
                        isValidInput = true; // Gültige Eingabe -> Schleife beenden
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'Yes' or 'No'.");
                        // isValidInput bleibt "false", daher läuft die Schleife weiter
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    // Bei einer Exception kann die Schleife erneut die Eingabe anfordern
                }

            } while (!isValidInput); // Schleife wiederholen, solange die Eingabe ungültig ist
        }

            // Methode zur Anzeige des Dateiinhalts
            private void ShowFileContents()
        {
            Console.WriteLine("Contents of the file:");
            foreach (var line in _fileModel.Lines)
            {
                Console.WriteLine(line); // Jede Zeile wird in Konsole angezeigt
            }
        }

        // Methode zum Zählen der Gesamtanzahl der Zeilen
        private int CountTotalLines(string filePath)
        {
            int lineCount = 0;
            using (var reader = new StreamReader(filePath)) // StreamReader wird verwendet, um die Datei zeilenweise zu lesen. Der using-Block stellt sicher, dass der StreamReader
                                                            // ordnungsgemäß geschlossen wird, sobald der Block verlassen wird
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++; // Erhöht die Zeilenanzahl
                }
            } // Hier wird der StreamReader automatisch geschlossen, wenn der Block verlassen wird (wegen using)
            return lineCount;
        }

        // Methode zum Lesen der Datei und Fortschritt in % anzeigen
        private void ReadFile(string filePath, int totalLines)
        {
            int linesRead = 0;
            int lastProgress = 0; // Speichert den zuletzt angezeigten Fortschritt

            using (var reader = new StreamReader(filePath))
            {
                string line;
                // Diese Schleife liest so lange ein und gibt den Fortschritt aus bis Ende der Datei
                while ((line = reader.ReadLine()) != null)
                {
                    _fileModel.Lines.Add(line); // Füge die Zeile zur Liste hinzu
                    linesRead++; // Erhöhe die Anzahl der gelesenen Zeilen

                    // Berechne den Fortschritt in Prozent
                    int progress = (int)((double)linesRead / totalLines * 100);

                    // Nur anzeigen, wenn sich der Fortschritt geändert hat
                    if (progress > lastProgress)
                    {
                        Console.WriteLine($"Progress: {progress}%");
                        lastProgress = progress; // Aktualisiere den letzten Fortschrittswert
                    }
                }
            }
        }
        /// <summary>
        /// Zeigt den OpenFileDialog an und gibt den Pfad der ausgewählten Datei zurück.
        /// </summary>
        /// <returns>Der Dateipfad oder null, wenn keine Datei ausgewählt wurde.</returns>
        public static string SelectFile()
        {
            // OpenFileDialog muss in einem synchronen STA-Thread aufgerufen werden
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Select a text file";

                // Datei-Auswahl über OpenFileDialog
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName; // Rückgabe des Dateipfads
                }
                else
                {
                    return null; // Rückgabe von null, wenn keine Datei ausgewählt wurde
                }
            }
        }
    }
}
