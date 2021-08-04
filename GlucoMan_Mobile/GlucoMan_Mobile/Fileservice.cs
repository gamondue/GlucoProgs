using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using SharedFunctions;
using System.IO;

//[assembly: DefaultDependency(typeof(FileService))]

namespace GlucoMan_Mobile
{
    //public interface IFileService
    //{
    //    void Save(byte[] data, string name, string location);
    //}
    //public class FileService : IFileService
    //{
    //    public void Save(string name, Stream data, string location )
    //    {
    //        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    //        documentsPath = Path.Combine(documentsPath, "Orders", location);
    //        Directory.CreateDirectory(documentsPath);

    //        string filePath = Path.Combine(documentsPath, name);

    //        byte[] bArray = new byte[data.Length];
    //        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
    //        {
    //            using (data)
    //            {
    //                data.Read(bArray, 0, (int)data.Length);
    //            }
    //            int length = bArray.Length;
    //            fs.Write(bArray, 0, length);
    //        }
    //    }
    //}
}
