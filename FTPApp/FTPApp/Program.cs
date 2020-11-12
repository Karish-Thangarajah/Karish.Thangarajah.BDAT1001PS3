using FTPApp.Models;
using FTPApp.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;

namespace FTPApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Question 1
            List<string> directories = FTP.GetDirectory(Constants.FTP.BaseUrl);
            foreach (var entry in directories)
            {
                Console.WriteLine(entry);
            }

            Console.WriteLine("\n");
            //Question 2.1
            if (FTP.FileExists(Constants.FTP.MyUrl + "/" + Constants.Student.InfoCSVFileName))
            {
                Console.WriteLine($"{Constants.Student.InfoCSVFileName} file exists.");
            }
            else
            {
                Console.WriteLine($"{Constants.Student.InfoCSVFileName} file does not exist.");
            }
            if (FTP.FileExists(Constants.FTP.MyUrl + "/" + Constants.Student.MyImageFileName))
            {
                Console.WriteLine($"{Constants.Student.MyImageFileName} file exists.");
            }
            else
            {
                Console.WriteLine($"{Constants.Student.MyImageFileName} file does not exist.");
            }

            //Question 2.2
            List<string> fileList = FTP.GetFilesinDirectory(Constants.FTP.MyUrl);
            foreach (var file in fileList)
            {
                Console.WriteLine(file);
            }

            //Question2.3
            var myCsvBytes = FTP.DownloadFileBytes(Constants.FTP.MyUrl + "/info.csv");
            string infoMyCsvData = Encoding.UTF8.GetString(myCsvBytes, 0, myCsvBytes.Length);

            string[] myLines = infoMyCsvData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < myLines[0].Split(",", StringSplitOptions.RemoveEmptyEntries).Count(); i++)
            {
                Console.WriteLine($"{myLines[0].Split(",", StringSplitOptions.RemoveEmptyEntries)[i]}: {myLines[1].Split(",", StringSplitOptions.RemoveEmptyEntries)[i]}\n");
            }

            //Question 2.4

            //Question 4
            List<Student> students = new List<Student>();
            foreach (var directory in directories)
            {
                Student student = new Student();
                if (directory == "200471940 Karish Thangarajah")
                {
                    student.MyRecord = true;
                }

                if (FTP.FileExists(Constants.FTP.BaseUrl + "/" + directory + "/info.csv"))
                {
                    var CSVFileBytes = FTP.DownloadFileBytes(Constants.FTP.BaseUrl + "/" + directory + "/info.csv");
                    string infoCsvData = Encoding.UTF8.GetString(CSVFileBytes, 0, CSVFileBytes.Length);

                    string[] lines = infoCsvData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                    if (lines[1] != "" || lines[1] != null || infoCsvData != "")
                    {
                        student.FromCSV(lines[1]);
                        students.Add(student);
                    }
                    else
                    {
                        Console.WriteLine("Missing content");
                    }
                }
                else
                {
                    Console.WriteLine("    info.csv does not exist.");
                }

                if (FTP.FileExists(Constants.FTP.BaseUrl + "/" + directory + "/myimage.jpg"))
                {
                    var ImageFileBytes = FTP.DownloadFileBytes(Constants.FTP.BaseUrl + "/" + directory + "/myimage.jpg");
                    Image image = Converter.ByteArrayToImage(ImageFileBytes);
                    string imagedata = Converter.ImageToBase64(image, ImageFormat.Jpeg);
                    student.ImageData = imagedata;
                }
                else
                {
                    Console.WriteLine("    info.csv does not exist.");
                }



                Console.WriteLine(student.ToString());
                Console.WriteLine(student.ToCSV());
                Console.WriteLine("\n");
            }

            // Question 5.1
            //   a)
            Console.WriteLine(students.Count());
            //   b)
            List<Student> filtered = new List<Student>();
            foreach (var student in students)
            {
                if (student.StudentId.StartsWith("2004"))
                {
                    filtered.Add(student);
                }
            }
            Console.WriteLine(filtered.Count());
            //   c)
            List<Student> filtered2 = new List<Student>();
            foreach (var student in students)
            {
                if (student.DateOfBirthDT.ToShortDateString().Contains("1997"))
                {
                    filtered2.Add(student);
                }
            }

            Console.WriteLine(filtered2.Count());

            Student me = students.Find(x => x.MyRecord == true);
            Console.WriteLine(me.ToString());

            Console.WriteLine($"The average age of the class is { students.Average(x => x.Age)}.");
            Console.WriteLine($"The oldest age of the class is { students.Max(x => x.Age)}.");
            Console.WriteLine($"The youngest age of the class is { students.Min(x => x.Age)}.");

            //csv upload
            CsvFileReaderWriter reader = new CsvFileReaderWriter();
            List<string> ForCSV = new List<string> { "StudentId,FirstName,LastName,DateOfBirth,ImageData" };
            foreach (var student in students)
            {
                ForCSV.Add(student.ToCSV());
            }
            string ForCSVStr = String.Join("\r\n", ForCSV);

            List<string[]> fields = reader.GetEntities(reader.ParseString(ForCSVStr));
            reader.WriteFile(@"C:\Users\karis\OneDrive\Desktop\BDAT1001\Assignment3\students.csv", fields);

            Console.WriteLine(FTP.UploadFile(@"C:\Users\karis\OneDrive\Desktop\BDAT1001\Assignment3\students.csv", Constants.FTP.MyUrl + "/students.csv"));

            //JSON upload
            string stuJSON = JsonConvert.SerializeObject(students);
            System.IO.File.WriteAllText(@"C:\Users\karis\OneDrive\Desktop\BDAT1001\Assignment3\students.json", stuJSON);

            Console.WriteLine(FTP.UploadFile(@"C:\Users\karis\OneDrive\Desktop\BDAT1001\Assignment3\students.json", Constants.FTP.MyUrl + "/students.json"));

            //XML upload
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(students.GetType());
            TextWriter writer = new StreamWriter(@"C:\Users\karis\OneDrive\Desktop\BDAT1001\Assignment3\students.xml");
            x.Serialize(writer, students);

            Console.WriteLine(FTP.UploadFile(@"C:\Users\karis\OneDrive\Desktop\BDAT1001\Assignment3\students.xml", Constants.FTP.MyUrl + "/students.xml"));



        }
    }
}