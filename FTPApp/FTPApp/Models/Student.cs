using System;
using System.Collections.Generic;
using System.Text;

namespace FTPApp.Models
{
    public class Student
    {
        /// <summary>
        /// Use as a default Header Row output, modify if you need to change the format of the CSV output
        /// </summary>
        public static string HeaderRow = $"{nameof(StudentId)},{nameof(FirstName)},{nameof(LastName)},{nameof(DateOfBirth)},{nameof(ImageData)}";
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private string _DateOfBirth;
        /// <summary>
        /// DateOfBirth stored in local DateTime format (see culture setting i.e. 12/31/2020 is Month/Day/Year)
        /// </summary>
        public string DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {
                _DateOfBirth = value;

                //Convert DateOfBirth to DateTime
                DateTime dtOut;
                DateTime.TryParse(_DateOfBirth, out dtOut);
                DateOfBirthDT = dtOut;
            }
        }

        /// <summary>
        /// Used to convert DateOfBirth to DateTime type
        /// </summary>
        public DateTime DateOfBirthDT { get;  set; }
        /// <summary>
        /// ImageData stored as Base64
        /// </summary>
        public string ImageData { get; set; }

        public bool MyRecord { get; set; }



        /// <summary>
        /// Returns an absolute path to the root FTP folder
        /// </summary>
        public string AbsoluteUrl { get { return Constants.FTP.BaseUrl; } }
        public string Directory { get; set; }
        /// <summary>
        /// Returns an absolute path to the info.csv file
        /// </summary>
        public string InfoCSVPath { get { return (FullPathUrl + "/" + Constants.Student.InfoCSVFileName); } }
        /// <summary>
        /// Returns an absolute path to the myimage.jpg file
        /// </summary>
        public string MyImagePath { get { return (FullPathUrl + "/" + Constants.Student.MyImageFileName); } }
        public string FullPathUrl
        {
            get
            {
                return AbsoluteUrl + "/" + Directory;
            }
        }

        public List<string> Exceptions { get; set; } = new List<string>();

        /// <summary>
        /// Calulates Age in Years based of the DataOfBirth
        /// </summary>
        public virtual int Age
        {
            get
            {
                if (DateOfBirthDT == DateTime.MinValue)
                {
                    return 0;
                }

                DateTime Now = DateTime.Now;
                int Years = new DateTime(DateTime.Now.Subtract(DateOfBirthDT).Ticks).Year - 1;
                DateTime PastYearDate = DateOfBirthDT.AddYears(Years);
                int Months = 0;

                for (int i = 1; i <= 12; i++)
                {
                    if (PastYearDate.AddMonths(i) == Now)
                    {
                        Months = i;
                        break;
                    }
                    else if (PastYearDate.AddMonths(i) >= Now)
                    {
                        Months = i - 1;
                        break;
                    }
                }

                //int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
                //int Hours = Now.Subtract(PastYearDate).Hours;
                //int Minutes = Now.Subtract(PastYearDate).Minutes;
                //int Seconds = Now.Subtract(PastYearDate).Seconds;

                return Years;
            }
        }

        /// <summary>
        /// Converts a CSV formated string to a Student
        /// </summary>
        /// <param name="csvdata">StudentId,FirstName,LastName,DateOfBirth,ImageData</param>
        public void FromCSV(string csvdata)
        {
            string[] data = csvdata.Split(",", StringSplitOptions.None);

            try
            {
                StudentId = data[0];
                FirstName = data[1];
                LastName = data[2];
                DateOfBirth = data[3];
                ImageData = data[4];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Converts a Space Delimited formated string in the form of StudentId FirstName LastName to a Student
        /// </summary>
        /// <param name="directory">StudentId FirstName LastName</param>
        public void FromDirectory(string directory)
        {
            Directory = directory;

            if (String.IsNullOrEmpty(directory.Trim()))
            {
                return;
            }

            string[] data = directory.Trim().Split(" ", StringSplitOptions.None);

            StudentId = data[0];
            FirstName = data[1];
            LastName = data[2];
        }

        /// <summary>
        /// Converts the Student object to a CSV representation
        /// </summary>
        /// <returns>StudentId,FirstName,LastName,DateOfBirth,ImageData</returns>
        public string ToCSV()
        {
            string result = $"{StudentId},{FirstName},{LastName},{DateOfBirthDT.ToShortDateString()},{ImageData}";
            return result;
        }

        /// <summary>
        /// Converts the Student object to a String
        /// </summary>
        /// <returns>StudentId FirstName LastName</returns>
        public override string ToString()
        {
            string result = $"{StudentId} {FirstName} {LastName} {MyRecord}";
            return result;
        }
    }
}
