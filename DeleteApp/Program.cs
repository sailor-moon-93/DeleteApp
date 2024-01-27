using System;
using System.IO;

namespace DeleteApp
{

    internal class Program
    {
        static void DeleteFiles(string directoryPath, string key)
        {
            try
            {
                if (Directory.Exists(directoryPath)) // Проверим, существует ли каталог по данному пути 
                {
                    string[] files = Directory.GetFiles(directoryPath);

                    var counter = 0; //счетчик подходящих файлов

                    using (StreamWriter sw = File.CreateText(directoryPath + "\\log.txt")) //пишем лог
                    {
                        foreach (var file in files) //проверяем каждый файл в каталоге
                        {
                            if (file.Contains(key)) //если имя файла содержит ключ, удаляем
                            {
                                counter++;
                                File.Delete(file);
                                Console.WriteLine($"Удалил файл {file}, т.к. его название содержит \"{key}\"");
                                sw.WriteLine($"Удалил файл {file} в {DateTime.Now}, т.к. его название содержит \"{key}\"");
                            }
                        }
                    }
                    Console.WriteLine("\nВсе файлы проверены");
                    if (files.Length == 0)
                    {
                        Console.WriteLine("\nПапка пуста");
                    }
                    if (counter == 0)
                        Console.WriteLine("\nПодходящих для удаления файлов нет");
                    else
                    {
                        Console.WriteLine($"\nВсе удаленные файлы записаны в {directoryPath}\\log.txt");
                    }
                }
                else
                {
                    Console.WriteLine("Такого каталога не существует");
                }
            }
            //проверка доступа к папке
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Отсутствует доступ к папке");
            }
        }

        static void GetInfo(string dirName)
        {
            if (Directory.Exists(dirName)) // Проверим, существует ли каталог по данному пути 
            {
                string[] dirs = Directory.GetDirectories(dirName);  // создаем массив для всех подпапок

                Console.WriteLine($"\nПапок в каталоге {dirName}: {dirs.Length}");
                foreach (string d in dirs) // выводим их все
                    Console.WriteLine(d);


                string[] files = Directory.GetFiles(dirName);// создаем массив под файлы
                Console.WriteLine($"\nФайлов в каталоге {dirName}: {files.Length}");

                long bytesSum = 0;
                foreach (string file in files)
                {
                    Console.WriteLine(Path.GetFileName(file));
                    bytesSum += new FileInfo(file).Length;
                }


                Console.WriteLine($"Их общий размер в байтах: {bytesSum}");

                if (dirs.Length != 0) //если есть вложенные папки
                {
                    foreach (string d in dirs)
                        GetInfo(d);
                }
            }
            else
            {
                Console.WriteLine("Невозможно вывести инфо о файлах");
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Введите адрес папки:");
            string? directoryPath = Console.ReadLine();

            Console.WriteLine("Хотите ли увидеть инфо о папке до удаления?");
            string? answer = Console.ReadLine();
            if (answer.ToLower() == "yes" || answer.ToLower() == "да")
            {
                GetInfo(directoryPath);
            }
            else 
            {
                Console.WriteLine("Инфо о папке можно будет увидеть после удаления");
            }
            

            Console.WriteLine("\nВведите часть имени файла, согласно которому они будут удалены:");
            string? key = Console.ReadLine();


            DeleteFiles(directoryPath, key);
            Console.WriteLine("\nВывести инфо о папке после удаления?");
            answer = Console.ReadLine();
            if (answer.ToLower() == "yes" || answer.ToLower() == "да")
            {
                GetInfo(directoryPath);
            }
        }
    }
}
