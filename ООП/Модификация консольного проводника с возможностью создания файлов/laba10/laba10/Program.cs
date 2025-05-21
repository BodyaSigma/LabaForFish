using System;
using System.IO;
using System.Text;

namespace EnhancedFileExplorer
{
    class Program
    {
        private static DriveInfo[] allDrives;
        private static string currentDirectory;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            allDrives = DriveInfo.GetDrives();
            currentDirectory = null;

            bool exitRequested = false;
            while (!exitRequested)
            {
                Console.Clear();
                DisplayMainMenu();

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
                        CreateNewDirectory();
                        break;
                    case "5":
                        CreateTextFileWithContent();
                        break;
                    case "6":
                        DeleteFileOrDirectory();
                        break;
                    case "7":
                        exitRequested = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("=== УСОВЕРШЕНСТВОВАННЫЙ КОНСОЛЬНЫЙ ПРОВОДНИК ===");
            Console.WriteLine("1. Просмотр доступных дисков");
            Console.WriteLine("2. Информация о диске");
            Console.WriteLine("3. Просмотр содержимого диска");
            Console.WriteLine("4. Создать каталог");
            Console.WriteLine("5. Создать текстовый файл");
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
            if (int.TryParse(Console.ReadLine(), out int driveNumber) &&
                driveNumber > 0 && driveNumber <= allDrives.Length)
            {
                var drive = allDrives[driveNumber - 1];
                Console.WriteLine($"\nИнформация о диске {drive.Name}:");
                Console.WriteLine($"Тип: {drive.DriveType}");

                if (drive.IsReady)
                {
                    Console.WriteLine($"Метка тома: {drive.VolumeLabel}");
                    Console.WriteLine($"Файловая система: {drive.DriveFormat}");
                    Console.WriteLine($"Общий размер: {FormatBytes(drive.TotalSize)}");
                    Console.WriteLine($"Доступно: {FormatBytes(drive.TotalFreeSpace)}");
                    Console.WriteLine($"Занято: {FormatBytes(drive.TotalSize - drive.TotalFreeSpace)}");
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

        static string FormatBytes(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int suffixIndex = 0;
            double size = bytes;

            while (size >= 1024 && suffixIndex < suffixes.Length - 1)
            {
                size /= 1024;
                suffixIndex++;
            }

            return $"{size:0.##} {suffixes[suffixIndex]}";
        }

        static void BrowseDrive()
        {
            Console.Write("Введите номер диска: ");
            if (int.TryParse(Console.ReadLine(), out int driveNumber) &&
                driveNumber > 0 && driveNumber <= allDrives.Length)
            {
                var drive = allDrives[driveNumber - 1];
                if (!drive.IsReady)
                {
                    Console.WriteLine("Диск не готов к использованию");
                    Console.ReadKey();
                    return;
                }

                currentDirectory = drive.RootDirectory.FullName;
                BrowseDirectory(currentDirectory);
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
                Console.WriteLine($"Текущая директория: {path}\n");

                try
                {
                    var directories = Directory.GetDirectories(path);
                    var files = Directory.GetFiles(path);

                    Console.WriteLine("{0,-5} {1,-40} {2,-15} {3,-10}", "№", "Имя", "Тип", "Размер");
                    Console.WriteLine(new string('-', 80));

                    // Родительская директория
                    if (path != Path.GetPathRoot(path))
                    {
                        Console.WriteLine("{0,-5} {1,-40} {2,-15} {3,-10}",
                            "0", "[..]", "Каталог", "");
                    }

                    // Подкаталоги
                    for (int i = 0; i < directories.Length; i++)
                    {
                        string dirName = Path.GetFileName(directories[i]);
                        Console.WriteLine("{0,-5} {1,-40} {2,-15} {3,-10}",
                            i + 1, dirName, "Каталог", "");
                    }

                    // Файлы
                    for (int i = 0; i < files.Length; i++)
                    {
                        string fileName = Path.GetFileName(files[i]);
                        FileInfo fileInfo = new FileInfo(files[i]);
                        Console.WriteLine("{0,-5} {1,-40} {2,-15} {3,-10}",
                            i + directories.Length + 1, fileName, "Файл",
                            FormatBytes(fileInfo.Length));
                    }

                    Console.WriteLine("\nВыберите элемент (0 - назад, 'm' - меню):");
                    var input = Console.ReadLine();

                    if (input.ToLower() == "m")
                    {
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
                            ViewFileContent(files[choice - directories.Length - 1]);
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    Console.ReadKey();
                    return;
                }
            }
        }

        static void ViewFileContent(string filePath)
        {
            Console.Clear();
            Console.WriteLine($"Содержимое файла {Path.GetFileName(filePath)}:\n");
            Console.WriteLine(new string('-', 80));

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    string line;
                    int lineCount = 0;
                    while ((line = reader.ReadLine()) != null && lineCount < 20)
                    {
                        Console.WriteLine(line);
                        lineCount++;
                    }

                    if (line != null)
                    {
                        Console.WriteLine("\n... (файл слишком большой, показаны первые 20 строк)");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void CreateNewDirectory()
        {
            if (currentDirectory == null)
            {
                Console.WriteLine("Сначала выберите диск (опция 3)");
                Console.ReadKey();
                return;
            }

            Console.Write("Введите имя нового каталога: ");
            string dirName = Console.ReadLine();
            string newDirPath = Path.Combine(currentDirectory, dirName);

            try
            {
                Directory.CreateDirectory(newDirPath);
                Console.WriteLine($"Каталог '{dirName}' успешно создан!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании каталога: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void CreateTextFileWithContent()
        {
            if (currentDirectory == null)
            {
                Console.WriteLine("Сначала выберите диск (опция 3)");
                Console.ReadKey();
                return;
            }

            Console.Write("Введите имя нового файла (с расширением .txt): ");
            string fileName = Console.ReadLine();

            // Добавляем расширение .txt, если его нет
            if (!fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                fileName += ".txt";
            }

            string newFilePath = Path.Combine(currentDirectory, fileName);

            if (File.Exists(newFilePath))
            {
                Console.Write("Файл уже существует. Перезаписать? (y/n): ");
                if (Console.ReadLine().ToLower() != "y")
                {
                    Console.WriteLine("Создание файла отменено.");
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine("\nВведите содержимое файла (для завершения введите пустую строку):\n");

            try
            {
                using (var writer = new StreamWriter(newFilePath))
                {
                    string line;
                    int lineNumber = 1;
                    while (true)
                    {
                        Console.Write($"{lineNumber}: ");
                        line = Console.ReadLine();
                        if (string.IsNullOrEmpty(line))
                            break;
                        writer.WriteLine(line);
                        lineNumber++;
                    }
                }

                Console.WriteLine($"\nФайл '{fileName}' успешно создан!");
                FileInfo fileInfo = new FileInfo(newFilePath);
                Console.WriteLine($"Размер файла: {FormatBytes(fileInfo.Length)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании файла: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void DeleteFileOrDirectory()
        {
            if (currentDirectory == null)
            {
                Console.WriteLine("Сначала выберите диск (опция 3)");
                Console.ReadKey();
                return;
            }

            Console.Write("Введите имя файла/каталога для удаления: ");
            string itemName = Console.ReadLine();
            string itemPath = Path.Combine(currentDirectory, itemName);

            try
            {
                if (Directory.Exists(itemPath))
                {
                    Console.Write($"Вы уверены, что хотите удалить каталог '{itemName}'? (y/n): ");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        Directory.Delete(itemPath, true);
                        Console.WriteLine("Каталог успешно удален!");
                    }
                    else
                    {
                        Console.WriteLine("Удаление отменено.");
                    }
                }
                else if (File.Exists(itemPath))
                {
                    Console.Write($"Вы уверены, что хотите удалить файл '{itemName}'? (y/n): ");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        File.Delete(itemPath);
                        Console.WriteLine("Файл успешно удален!");
                    }
                    else
                    {
                        Console.WriteLine("Удаление отменено.");
                    }
                }
                else
                {
                    Console.WriteLine("Указанный файл/каталог не существует");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}