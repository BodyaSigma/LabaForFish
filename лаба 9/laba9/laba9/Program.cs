// See httpsusing System;
using System.IO;
using System.Linq;

class DiskManager
{
    static DriveInfo[] allDrives;
    static string currentPath;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        allDrives = DriveInfo.GetDrives();
        currentPath = null;

        while (true)
        {
            Console.Clear();
            DisplayMenu();

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListAllDrives();
                    break;
                case "2":
                    ShowDriveInfo();
                    break;
                case "3":
                    BrowseDrive();
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

    static void DisplayMenu()
    {
        Console.WriteLine("=== Управление дисками ===");
        Console.WriteLine("1. Просмотр доступных дисков");
        Console.WriteLine("2. Информация о диске");
        Console.WriteLine("3. Просмотр содержимого диска");
        Console.WriteLine("4. Создать каталог");
        Console.WriteLine("5. Создать файл");
        Console.WriteLine("6. Удалить файл/каталог");
        Console.WriteLine("7. Выход");
        Console.Write("\nВыберите действие: ");
    }

    static void ListAllDrives()
    {
        Console.WriteLine("\nДоступные диски:");
        for (int i = 0; i < allDrives.Length; i++)
        {
            var drive = allDrives[i];
            Console.WriteLine($"{i + 1}. {drive.Name} ({drive.DriveType})");
        }
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void ShowDriveInfo()
    {
        Console.Write("Введите номер диска: ");
        if (int.TryParse(Console.ReadLine(), out int driveNumber) && driveNumber > 0 && driveNumber <= allDrives.Length)
        {
            var drive = allDrives[driveNumber - 1];
            Console.WriteLine($"\nИнформация о диске {drive.Name}:");
            Console.WriteLine($"Тип: {drive.DriveType}");

            if (drive.IsReady)
            {
                Console.WriteLine($"Метка тома: {drive.VolumeLabel}");
                Console.WriteLine($"Файловая система: {drive.DriveFormat}");
                Console.WriteLine($"Общий размер: {drive.TotalSize / (1024 * 1024 * 1024)} GB");
                Console.WriteLine($"Доступно: {drive.TotalFreeSpace / (1024 * 1024 * 1024)} GB");
                Console.WriteLine($"Занято: {(drive.TotalSize - drive.TotalFreeSpace) / (1024 * 1024 * 1024)} GB");
            }
            else
            {
                Console.WriteLine("Диск не готов к использованию");
            }
        }
        else
        {
            Console.WriteLine("Неверный номер диска");
        }
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void BrowseDrive()
    {
        Console.Write("Введите номер диска: ");
        if (int.TryParse(Console.ReadLine(), out int driveNumber) && driveNumber > 0 && driveNumber <= allDrives.Length)
        {
            var drive = allDrives[driveNumber - 1];
            if (!drive.IsReady)
            {
                Console.WriteLine("Диск не готов к использованию");
                Console.ReadKey();
                return;
            }

            currentPath = drive.RootDirectory.FullName;
            BrowseDirectory(currentPath);
        }
        else
        {
            Console.WriteLine("Неверный номер диска");
            Console.ReadKey();
        }
    }

    static void BrowseDirectory(string path)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Текущий путь: {path}\n");

            try
            {
                var directories = Directory.GetDirectories(path);
                var files = Directory.GetFiles(path);

                Console.WriteLine("{0,-5} {1,-40} {2,-10}", "№", "Имя", "Тип");
                Console.WriteLine(new string('-', 60));

                // Родительская директория
                if (path != Path.GetPathRoot(path))
                {
                    Console.WriteLine("{0,-5} {1,-40} {2,-10}", "0", "[..]", "Каталог");
                }

                // Подкаталоги
                for (int i = 0; i < directories.Length; i++)
                {
                    string dirName = Path.GetFileName(directories[i]);
                    Console.WriteLine("{0,-5} {1,-40} {2,-10}", i + 1, dirName, "Каталог");
                }

                // Файлы
                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = Path.GetFileName(files[i]);
                    Console.WriteLine("{0,-5} {1,-40} {2,-10}", i + directories.Length + 1, fileName, "Файл");
                }

                Console.WriteLine("\nВыберите элемент для навигации (0 - назад, 'exit' - выход):");
                var input = Console.ReadLine();

                if (input == "exit")
                {
                    currentPath = null;
                    return;
                }

                if (int.TryParse(input, out int choice))
                {
                    if (choice == 0 && path != Path.GetPathRoot(path))
                    {
                        path = Directory.GetParent(path).FullName;
                    }
                    else if (choice > 0 && choice <= directories.Length)
                    {
                        path = directories[choice - 1];
                    }
                    else if (choice > directories.Length && choice <= directories.Length + files.Length)
                    {
                        ViewFile(files[choice - directories.Length - 1]);
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу...");
                    Console.ReadKey();
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Ошибка доступа!");
                Console.ReadKey();
                return;
            }
        }
    }

    static void ViewFile(string filePath)
    {
        Console.Clear();
        Console.WriteLine($"Содержимое файла {Path.GetFileName(filePath)}:\n");

        try
        {
            string content = File.ReadAllText(filePath);
            Console.WriteLine(content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void CreateDirectory()
    {
        if (currentPath == null)
        {
            Console.WriteLine("Сначала выберите диск (опция 3)");
            Console.ReadKey();
            return;
        }

        Console.Write("Введите имя нового каталога: ");
        string dirName = Console.ReadLine();
        string newDirPath = Path.Combine(currentPath, dirName);

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
        if (currentPath == null)
        {
            Console.WriteLine("Сначала выберите диск (опция 3)");
            Console.ReadKey();
            return;
        }

        Console.Write("Введите имя нового файла: ");
        string fileName = Console.ReadLine();
        string newFilePath = Path.Combine(currentPath, fileName);

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
        if (currentPath == null)
        {
            Console.WriteLine("Сначала выберите диск (опция 3)");
            Console.ReadKey();
            return;
        }

        Console.Write("Введите полный путь к файлу/каталогу для удаления: ");
        string itemPath = Console.ReadLine();

        try
        {
            if (Directory.Exists(itemPath))
            {
                Console.Write($"Вы уверены, что хотите удалить каталог {itemPath}? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    Directory.Delete(itemPath, true);
                    Console.WriteLine("Каталог успешно удален!");
                }
            }
            else if (File.Exists(itemPath))
            {
                Console.Write($"Вы уверены, что хотите удалить файл {itemPath}? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    File.Delete(itemPath);
                    Console.WriteLine("Файл успешно удален!");
                }
            }
            else
            {
                Console.WriteLine("Указанный путь не существует");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}