using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsyncProgramming.Controllers;

namespace AsyncFileReader
{
    class Program
    {
        [STAThread] // STA-Thread-Attribut für Windows Forms benötigt
        static void Main(string[] args)
        {
            // Verschiebe den OpenFileDialog in eine eigene Methode
            string filePath = FileController.SelectFile();

            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("No file selected.");
                return; // Beende das Programm, wenn keine Datei ausgewählt wurde
            }

            // Erstelle die Instanz von FileController für asynchrone Operationen
            var controller = new FileController();

            // Starte den asynchronen Leseprozess nach der Dateiauswahl
            Task.Run(async () => await controller.LoadFileAsync(filePath)).GetAwaiter().GetResult();

            Console.WriteLine("Drücken Sie eine beliebige Taste, um das Programm zu beenden...");
            Console.ReadKey();  // Wartet, bis eine Taste gedrückt wird

        }
    }
}
