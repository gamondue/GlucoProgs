using System;
using System.IO;
using System.Security.Cryptography;
using System.Reflection;
using SharedData;

namespace SharedFunctions
{
    internal static partial class CommonFunctions
    {
        internal static string CalculateSHA1(string File)
        {
            try
            {
                byte[] buff = null;
                FileStream fs = new FileStream(File, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(File).Length;
                buff = br.ReadBytes((int)numBytes);

                SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
                string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buff)).Replace("-", "");
                buff = null;
                GC.Collect(); // lancia il garbage collector, per liberare subito la memoria usata
                return hash;
            }
            catch (Exception ex)
            {
                //return ErrorLog("Error in SHA1 calculation: " + ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// Writes the given object instance to a binary file.
        /// (taken from https://stackoverflow.com/questions/6115721/how-to-save-restore-serializable-object-to-from-file)
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the binary file.</typeparam>
        /// <param name="FilePathAndName">The file path to write the object instance to.</param>
        /// <param name="ObjectToWrite">The object instance to write to the binary file.</param>
        /// <param name="Append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        internal static void WriteObjectToBinaryFile<T>(string FilePathAndName, T ObjectToWrite, bool Append = false)
        {
            using (Stream stream = File.Open(FilePathAndName, Append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, ObjectToWrite);
            }
        }

        /// <summary>
        /// Reads an object instance from a binary file.
        /// (taken from https://stackoverflow.com/questions/6115721/how-to-save-restore-serializable-object-to-from-file)
        /// </summary>
        /// <typeparam name="T">The type of object to read from the binary file.</typeparam>
        /// <param name="FilePathAndName">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        internal static T ReadObjectFromBinaryFile<T>(string FilePathAndName)
        {
            using (Stream stream = File.Open(FilePathAndName, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
        internal static void MakeFolderAndFileIfDontExist(string FilePathAndName)
        {
            string dir = Path.GetDirectoryName(FilePathAndName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (!File.Exists(FilePathAndName))
            { 
                File.Create(FilePathAndName);
            }
        }
        internal static void MakeFolderIfDontExist(string PathOfFolder)
        {
            // !!!!!!!!!!!! rimettere !!!!!!!!!!!!!!!!
            //////////////string dir = Path.GetDirectoryName(PathOfFolder);
            //////////////if (!Directory.Exists(dir))
            //////////////    Directory.CreateDirectory(dir);
        }

        internal static string ConvertStringToFilename(string SubmittedName, bool SubstituteSpaces)
        {
            string s = SubmittedName;
            s = s.Replace('<', '-');
            s = s.Replace('>', '-');
            s = s.Replace(':', '-');
            s = s.Replace('"', '-');
            s = s.Replace('/', '-');
            s = s.Replace('\\', '-');
            s = s.Replace('|', '-');
            s = s.Replace('?', '-');
            s = s.Replace('*', '-');

            if (SubstituteSpaces)
                s = s.Replace(' ', '-');
            return s;
        }

        internal static DateTime NextWeekSameDay(DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }

        public static object CloneObject(object o)
        {
            // from https://stackoverflow.com/questions/4544657/duplicate-group-box
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();

            Object p = t.InvokeMember("", System.Reflection.
                BindingFlags.CreateInstance, null, o, null);

            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(p, pi.GetValue(o, null), null);
                }
            }
            return p;
        }
    }
}
