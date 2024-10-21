using System.Collections.Generic;

namespace AsyncProgramming.Models
{
    /// <summary>
    /// definiert ein Modell, dass den Pfad einer Datei und deren Inhalt in eine Liste speichert.
    /// </summary>
    public class FileModel
    {
        //Pfad zur Datei
        public string FilePath { get; set; }
        // Liste zur Speicherung der Zeilen der Datei
        public List<string> Lines { get; set; }

        // Konstruktor, um Liste der Zeilen zu initialisieren
        public FileModel()
        {
            Lines = new List<string>();
        }
    }
}
