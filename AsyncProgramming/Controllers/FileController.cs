using AsyncProgramming.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AsyncProgramming.Controllers
{
    /// <summary>
    /// Klasse die für das Laden von dateien und das Verwalten des Dateiinhalts zuständig ist
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
        public async Task LoadFileAsync(string filePath)
        {
            _fileModel.FilePath = filePath;

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

            // Warten auf den Abschluss des Lesevorgangs
            await readingTask;
            Console.WriteLine("Reading finished.");
            Console.WriteLine("Do you want to show the data? (Yes/No):");

            try
            {
                string choice = Console.ReadLine();

                choice = choice.Trim().ToLower(); // Entfernt Leerzeichen und konvertiert in Kleinbuchstaben

                if (choice == "yes")
                {
                    ShowFileContents(); // Zeigt die Datei an
                }
                else if (choice == "no")
                {
                    Console.WriteLine("Contents will not be displayed.");
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'Yes' or 'No'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
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
            }
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
    }
}
