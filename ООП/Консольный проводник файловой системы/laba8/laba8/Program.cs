using System;
using System.IO;
using System.Text;

class FileExplorer
{
    static string currentDirectory = Directory.GetCurrentDirectory();

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            DisplayDirectoryContents(currentDirectory);

            Console.WriteLine("\nДоступные команды:");
            Console.WriteLine("1. Перейти в каталог");
            Console.WriteLine("2. Вернуться в родительский каталог");
            Console.WriteLine("3. Просмотреть файл");
            Console.WriteLine("4. Создать каталог");
            Console.WriteLine("5. Создать файл");
            Console.WriteLine("6. Удалить файл/каталог");
            Console.WriteLine("7. Выход");
            Console.Write("\nВыберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    EnterDirectory();
                    break;
                case "2":
                    GoToParentDirectory();
                    break;
                case "3":
                    ViewFile();
                    break;
                case "4":
                    CreateDirectory();
                    break;
                case "5":
                    CreateFile();
                    break;
                case "6":
                    DeleteItem();
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void DisplayDirectoryContents(string path)
    {
        Console.WriteLine($"Текущий каталог: {path}\n");
        Console.WriteLine("{0,-5} {1,-40} {2,-10}", "№", "Имя", "Тип");
        Console.WriteLine(new string('-', 60));

        try
        {
            // Отображаем подкаталоги
            string[] directories = Directory.GetDirectories(path);
            for (int i = 0; i < directories.Length; i++)
            {
                string dirName = Path.GetFileName(directories[i]);
                Console.WriteLine("{0,-5} {1,-40} {2,-10}", i + 1, dirName, "Каталог");
            }

            // Отображаем файлы
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileName(files[i]);
                Console.WriteLine("{0,-5} {1,-40} {2,-10}", i + directories.Length + 1, fileName, "Файл");
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Нет доступа к каталогу!");
        }
    }

    static void EnterDirectory()
    {
        Console.Write("Введите номер каталога: ");
        if (int.TryParse(Console.ReadLine(), out int dirNumber))
        {
            try
            {
                string[] directories = Directory.GetDirectories(currentDirectory);
                if (dirNumber > 0 && dirNumber <= directories.Length)
                {
                    currentDirectory = directories[dirNumber - 1];
                }
                else
                {
                    Console.WriteLine("Неверный номер каталога!");
                    Console.ReadKey();
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Нет доступа к каталогу!");
                Console.ReadKey();
            }
        }
        else
        {
            Console.WriteLine("Неверный ввод!");
            Console.ReadKey();
        }
    }

    static void GoToParentDirectory()
    {
        DirectoryInfo parentDir = Directory.GetParent(currentDirectory);
        if (parentDir != null)
        {
            currentDirectory = parentDir.FullName;
        }
        else
        {
            Console.WriteLine("Вы в корневом каталоге!");
            Console.ReadKey();
        }
    }

    static void ViewFile()
    {
        Console.Write("Введите номер файла: ");
        if (int.TryParse(Console.ReadLine(), out int fileNumber))
        {
            try
            {
                string[] directories = Directory.GetDirectories(currentDirectory);
                string[] files = Directory.GetFiles(currentDirectory);

                if (fileNumber > directories.Length && fileNumber <= directories.Length + files.Length)
                {
                    string filePath = files[fileNumber - directories.Length - 1];
                    Console.WriteLine($"\nСодержимое файла {Path.GetFileName(filePath)}:\n");
                    Console.WriteLine(File.ReadAllText(filePath));
                }
                else
                {
                    Console.WriteLine("Неверный номер файла!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Неверный ввод!");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void CreateDirectory()
    {
        Console.Write("Введите имя нового каталога: ");
        string dirName = Console.ReadLine();
        string newDirPath = Path.Combine(currentDirectory, dirName);

        try
        {
            Directory.CreateDirectory(newDirPath);
            Console.WriteLine($"Каталог {dirName} успешно создан!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void CreateFile()
    {
        Console.Write("Введите имя нового файла: ");
        string fileName = Console.ReadLine();
        string newFilePath = Path.Combine(currentDirectory, fileName);

        Console.WriteLine("Введите содержимое файла (для завершения ввода нажмите Enter):");
        string content = Console.ReadLine();

        try
        {
            File.WriteAllText(newFilePath, content);
            Console.WriteLine($"Файл {fileName} успешно создан!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void DeleteItem()
    {
        Console.Write("Введите номер файла/каталога для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int itemNumber))
        {
            try
            {
                string[] directories = Directory.GetDirectories(currentDirectory);
                string[] files = Directory.GetFiles(currentDirectory);

                if (itemNumber > 0 && itemNumber <= directories.Length + files.Length)
                {
                    string itemPath;
                    string itemName;
                    bool isDirectory;

                    if (itemNumber <= directories.Length)
                    {
                        itemPath = directories[itemNumber - 1];
                        itemName = Path.GetFileName(itemPath);
                        isDirectory = true;
                    }
                    else
                    {
                        itemPath = files[itemNumber - directories.Length - 1];
                        itemName = Path.GetFileName(itemPath);
                        isDirectory = false;
                    }

                    Console.Write($"Вы уверены, что хотите удалить {(isDirectory ? "каталог" : "файл")} {itemName}? (y/n): ");
                    string confirmation = Console.ReadLine().ToLower();

                    if (confirmation == "y")
                    {
                        if (isDirectory)
                        {
                            Directory.Delete(itemPath, true);
                        }
                        else
                        {
                            File.Delete(itemPath);
                        }
                        Console.WriteLine($"{itemName} успешно удален!");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный номер!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Неверный ввод!");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}