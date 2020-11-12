using System;

namespace FTPApp.Models
{
    public class Constants
    {
        public class FTP
        {
            public const string UserName = @"bdat100119f\bdat1001";
            public const string Password = "bdat1001";

            public const string BaseUrl = "ftp://waws-prod-dm1-127.ftp.azurewebsites.windows.net/bdat1001-10983";
            public const string MyUrl = BaseUrl + "/200471940 Karish Thangarajah";
            public const string localUrl = @"C:\Users\karis\OneDrive\Desktop\BDAT1001\midterm\";

            public const int OperationPauseTime = 10000;
        }

        public class Student
        {
            public const string InfoCSVFileName = "info.csv";
            public const string MyImageFileName = "myimage.jpg";
        }
    }
}

