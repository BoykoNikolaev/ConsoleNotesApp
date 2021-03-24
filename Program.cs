using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CRUDconsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string notesPath = Directory.GetCurrentDirectory();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("||======================================||");
            Console.WriteLine("||            Welcome, User!            ||");
            Console.WriteLine("||======================================||");
            Console.WriteLine();
            Console.ResetColor();

            bool running = true;

            while (running)
            {
                MainMenu();
                Console.Write("\nUser choice: ");

                string userChoice = Console.ReadLine();
                int choice;
                string input = "";

                switch (userChoice) 
                {
                    case "1":
                        if (NotesCount(notesPath) > 0)
                        {
                            DisplayNotes(notesPath);
                            Console.Write("\nChoose a note: ");
                            input = Console.ReadLine();

                            while(input.Length != 1)
                            {
                                Console.Write("\nInvalid choice. Choose again: ");
                                input = Console.ReadLine();
                            }
                            choice = int.Parse(input);

                            ReadNote(ChooseNote(notesPath, choice));

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nPress ENTER to get back to main menu...\n");
                            Console.ResetColor();
                            Console.ReadLine();
                        }
                        else
                        {
                            NoNotes();

                        }
                        break;
                    case "2":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("\nEnter the new note name here: ");
                        string newNoteName2 = Console.ReadLine();
                        Console.ResetColor();

                        CreateNote(notesPath, newNoteName2);

                        break;
                    case "3":
                        if(NotesCount(notesPath) > 0)
                        {
                            DeleteNote(notesPath);
                        }
                        else
                        {
                            NoNotes();
                        }
                        break;
                    case "4":
                        if(NotesCount(notesPath) > 0)
                        {
                            DisplayNotes(notesPath);
                            Console.Write("\nChoose a note: ");
                            input = Console.ReadLine();

                            while (input.Length != 1)
                            {
                                Console.Write("\nInvalid choice. Choose again: ");
                                input = Console.ReadLine();
                            }
                            choice = int.Parse(input);

                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("\nAdd to note or Overwrite the current text:");
                            Console.WriteLine("1. Add text.");
                            Console.WriteLine("2. Overwrite.\n");
                            Console.Write("Choose 1 or 2: ");

                            string addOrOvrwrt = Console.ReadLine();
                            bool add = false;
                            bool validChoice = false;

                            while (!validChoice)
                            {
                                if (addOrOvrwrt == "1")
                                {
                                    add = true;
                                    validChoice = true;
                                }
                                else if (addOrOvrwrt == "2")
                                {
                                    add = false;
                                    validChoice = true;
                                }
                                else
                                {
                                    Console.Write("\nInvalid choice. Choose 1 or 2:");
                                    addOrOvrwrt = Console.ReadLine();
                                }
                            }

                            UpdateNote(ChooseNote(notesPath, choice), add);
                        }
                        else
                        {
                            NoNotes();
                        }

                        break;
                    case "5":
                        Console.Clear();
                        break;
                    case "6":
                        running = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInvalid choice. Select again.\n");
                        Console.ResetColor();
                        break;
                }
            }

        }
        //main menu s opciite na prilojenieto.
        //Main Menu with options
        static void MainMenu()
        {
            Console.WriteLine("|==========================|");
            Console.WriteLine("|     Choose an option     |");
            Console.WriteLine("|--------------------------|");
            Console.WriteLine("| 1. Read existing notes.  |");
            Console.WriteLine("| 2. Create a new note.    |");
            Console.WriteLine("| 3. Delete existing note. |");
            Console.WriteLine("| 4. Update existing note. |");
            Console.WriteLine("| 5. Console clear.        |");
            Console.WriteLine("| 6. Quit.                 |");
            Console.WriteLine("|==========================|");

        }

        //pokazva spisyka s belejki
        //shows the list of notes
        static void DisplayNotes(string path)
        {
            string[] notes = Directory.GetFiles(path, "*.txt");
            int i = 1;

            Console.WriteLine("\nNotes:");
            foreach (string note in notes)
            {
                Console.WriteLine(i + "." + Path.GetFileName(note));
                i++;
            }
        }

        //izbira dadena belejka. Izpolzva se glavno kato parametyr za drugite metodi.
        //chooses a note. Used as a parameter for other methods
        static string ChooseNote(string notesPath, int choice)
        {

            string[] notes = Directory.GetFiles(notesPath, "*.txt");

            while(choice > notes.Length)
            {
                Console.Write("\nInvalid choice. Choose again: ");
                choice = int.Parse(Console.ReadLine());
            }

            return notes[choice - 1];
        }

        //pokazva izbranata belejka na konzolata.
        //shows the selected note
        static void ReadNote(string path)
        {
            StreamReader chetec = new StreamReader(path);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("||======================================||");
            Console.WriteLine("||        Note is displayed below.      ||");
            Console.WriteLine("||======================================||\n");
            Console.ResetColor();

            using (chetec)
            {
                Console.WriteLine(chetec.ReadToEnd());
            }

        }

        //syzdava nova belejka
        //creates new note
        static void CreateNote(string path, string newName)
        {
            int noteNum = 1;
            string fileName = $@"\{newName}.txt";

            if(File.Exists(path + fileName))
            {
                fileName = $@"\{newName}_{noteNum}.txt";
                noteNum++;
            }

            StreamWriter writer = new StreamWriter(path + fileName, false);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("|---------------------------------------|");
            Console.WriteLine(@"|  Enter text to write in the new note  |");
            Console.WriteLine("|---------------------------------------|");
            Console.ResetColor();
            Console.Write("\nWrite here: ");

            using (writer)
            {
                writer.WriteLine(Console.ReadLine());
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nNew note succesfully saved!");
            Console.WriteLine("Press ENTER to get back to main menu...\n");
            Console.ResetColor();
            Console.ReadLine();
        }

        //iztriva izbrana belejka (deletes the selected note)
        static void DeleteNote(string path)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nChoose a note to delete:\n");
            DisplayNotes(path);
            Console.Write("\nEnter the number of the note to DELETE: ");

            int delete = int.Parse(Console.ReadLine());
            File.Delete(ChooseNote(path, delete));

            Console.WriteLine("\nNote deleted.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nPress ENTER to get back to main menu...\n");
            Console.ResetColor();
            Console.ReadLine();
        }

        //dobavq text v dadena belejka ili iztriva vsichko i slaga noviq text.
        //adds text in the selected note or overwrites  it
        static void UpdateNote(string path, bool update)
        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nCurrent text:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            ReadNote(path);

            Console.ForegroundColor = ConsoleColor.Green;
            if (update)
            {
                Console.WriteLine("\nEnter text below and press ENTER to add it to the note:");
                StreamWriter writer = new StreamWriter(path, update);

                Console.ForegroundColor = ConsoleColor.Cyan;
                using (writer)
                {
                    writer.Write($"\n{Console.ReadLine()}");
                }
            }
            else
            {
                Console.WriteLine("\nEnter text below and press ENTER to overwrite the current note:");
                StreamWriter writer = new StreamWriter(path, update);

                Console.ForegroundColor = ConsoleColor.Cyan;
                using (writer)
                {
                    writer.Write($"{Console.ReadLine()}");
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nNote updated succesfully!");
            Console.WriteLine("\nPress ENTER to get back to main menu...\n");
            Console.ResetColor();
            Console.ReadLine();
        }

        //izpolzva se kato orientir dali ima syshtestvuvashti belejki
        //tracks the number of the notes in case there aren't any
        static int NotesCount(string path)
        {
            int x = 0;

            foreach(string note in Directory.GetFiles(path, "*.txt"))
            {
                x++;
            }

            return x;
        }

        //ako nqma syshtestvuvashti 
        //if there aren't any notes
        static void NoNotes()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nThere are no current notes.");
            Console.WriteLine("\nPress ENTER to get back to main menu...\n");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
