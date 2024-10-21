Async File Reader with Progress Display
This project is a simple asynchronous file reader built in C#. It demonstrates how to read large files line-by-line without blocking the main thread, while displaying the progress in real-time as a percentage. Users can view the file contents upon completion and control whether the data is displayed.

Features
Asynchronous File Loading: The file reading process runs asynchronously, ensuring that the application remains responsive even for large files.
Progress Tracking: Real-time progress is displayed during the file reading operation, showing the percentage of completion based on the number of lines read.
User Interaction: After the file is read, the user can choose to display the contents of the file or skip it.
Error Handling: Basic exception handling ensures that any issues encountered during file reading or user input are handled gracefully.
How It Works
File Selection: The user specifies the file path, and the program calculates the total number of lines in the file.
Asynchronous Read: The file is read asynchronously using Task.Run(), ensuring the UI or console remains responsive.
Progress Monitoring: As the file is read line-by-line, the progress is calculated and updated in the console in percentage increments.
User Choice: Once reading is completed, the user is asked if they want to display the file contents.
Code Structure
Models:
FileModel: Stores the file path and lines of the file.
Controllers:
FileController: Manages the file reading process and interacts with the model.
LoadFileAsync(): Asynchronous method to load the file and monitor progress.
ReadFile(): Synchronously reads the file line-by-line and updates the progress.
CountTotalLines(): Counts the total number of lines in the file for accurate progress calculation.
ShowFileContents(): Displays the file contents in the console based on user choice.
