using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Comuni
{
    public static class Funzioni
    {
        public static string CalculateSHA1(string File)
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
                string err = "ERRORE in calcolo SHA1: ";
                err += "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                OggettiEDati.LogDelProgramma.Error(err, ex);
                return err;
            }
        }

        //public static string LogErrore(string Errore, string LogFile, bool UseMessageBox)
        //{
        //    if (isLogging)
        //    {
        //        try
        //        {
        //            // append dell'errore nel file di logging
        //            using (StreamWriter sw = File.AppendText(LogFile))
        //            {
        //                sw.WriteLine(DateTime.Now + " " + Errore);
        //            }
        //        }
        //        catch
        //        {
        //            Console.WriteLine(DateTime.Now + " Errore nella memorizzazione del file di log");
        //        }
        //    }
        //    //if (UseMessageBox)
        //    //{
        //    //    MessageBox.Show(Errore, "Errore");
        //    //}
        //    Console.WriteLine(Errore);
        //    return Errore;
        //}

        public static double doub(String txt)
        {
            try
            {
                return double.Parse(txt.Replace(".", ","));
            }
            catch
            {
                try
                {
                    return double.Parse(txt.Replace(",", "."));
                }
                catch (Exception e)
                {
                    return double.NaN;
                }
            }
        }

        public static string NomeValidoPerFile(string NomeProposto, bool TogliSpazi)
        {
            NomeProposto = NomeProposto.Trim();
            if (TogliSpazi)
                NomeProposto = NomeProposto.Replace(' ', '-');

            StringBuilder nomeFile = new StringBuilder(NomeProposto);
            for (int i = 0; i < NomeProposto.Length; i++)
            {
                char c = nomeFile[i];
                if (!(c >= 'a' && c <= 'z') && !(c >= 'A' && c <= 'Z') && !(c >= '0' && c <= '9')
                    && !(c == ' ') && !(c == '\\') && !(c == '_') && !(c == ':') && !(c == '.') && !(c == '/'))
                    // se si vogliono tenere anche le lettere Unicode: 
                    //if (!Char.IsLetterOrDigit(c) 
                    //  && !(c == ' ') && !(c == '\\') && !(c == '_') && !(c == ':') && !(c == '.') && !(c == '/'))
                    nomeFile[i] = '-';
            }
            return nomeFile.ToString();
        }

        public static bool isTrue(object value)
        {
            if (value != null)
                return value.Equals((object)true);
            else
                return false;
        }

        public static string FromNullToEmptyString(object s)
        {
            if (s != null)
                return s.ToString();
            else
                return "";
        }

        internal static string IncreaseLastIntegerIntoString(string NumberWithTextParts)
        {
            // cerca l'ultimo numero che trova nella stringa
            if (NumberWithTextParts == null || NumberWithTextParts == "")
                return "";
            // cerca all'indietro l'ultima cifra
            int index = NumberWithTextParts.Length;
            while (index > 0 && !(NumberWithTextParts[index - 1] >= '0' && NumberWithTextParts[index - 1] <= '9'))
                index--;
            int fineNumero = index;
            while (index > 0 && NumberWithTextParts[index - 1] >= '0' && NumberWithTextParts[index - 1] <= '9')
                index--;
            int inizioNumero = index;
            if (inizioNumero == 0 && inizioNumero == fineNumero)
                return NumberWithTextParts;
            int nr = int.Parse(NumberWithTextParts.Substring(inizioNumero, fineNumero - inizioNumero));
            nr++;
            return NumberWithTextParts.Substring(0, inizioNumero) +
                nr.ToString() +
                NumberWithTextParts.Substring(fineNumero);
        }

        internal static string  IncreaseFirstIntegerIntoString(string NumberWithTextParts)
        {
            // cerca il primo numero che trova nella stringa
            if (NumberWithTextParts == null || NumberWithTextParts == "")
                return "";
            // cerca all'avanti la prima cifra
            //int index = NumberWithTextParts.Length;
            int index = 0;
            // esclude tutti gli eventuali caratteri prima dell'inizio del numero 
            while (index < NumberWithTextParts.Length && 
                !(NumberWithTextParts[index] >= '0' && NumberWithTextParts[index] <= '9'))
                index++;
            int inizioNumero = index;
            // cerca la fine del numero 
            while (index < NumberWithTextParts.Length &&
                (NumberWithTextParts[index] >= '0' && NumberWithTextParts[index] <= '9'))
                index++;
            int fineNumero = index;
            if (inizioNumero == 0 && inizioNumero == fineNumero)
                return NumberWithTextParts;
            int nr = int.Parse(NumberWithTextParts.Substring(inizioNumero, fineNumero - inizioNumero));
            nr++;
            return NumberWithTextParts.Substring(0, inizioNumero) +
                nr.ToString() +
                NumberWithTextParts.Substring(fineNumero);
        }

        private static void MescolaArrayStringhe(string[] array)
        {
            Random r = new Random();
            // mescola l'array di stringhe 
            for (int i = 0; i < array.GetLength(0) - 1; i++)
            {
                // riga a caso in cui prendere la riga da scambiare 
                int rigaACaso = r.Next(i, array.GetLength(0));
                // scambio fra le righe
                string dummy = array[i];
                array[i] = array[rigaACaso];
                array[rigaACaso] = dummy;
            }
        }

        internal static Random rng = new Random();
        public static void MescolaLista<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        internal static void MescolaArrayOggetti(object[] array)
        {
            Random r = new Random();
            // mescola l'array di stringhe 
            for (int i = 0; i < array.GetLength(0) - 1; i++)
            {
                // riga a caso in cui prendere la riga da scambiare 
                int rigaACaso = r.Next(i, array.GetLength(0));
                // scambio fra le righe
                object dummy = array[i];
                array[i] = array[rigaACaso];
                array[rigaACaso] = dummy;
            }
        }

        internal static void OrdinaArrayStringhe(string[] array)
        {
            // mescola l'array di stringhe 
            for (int i = 0; i < array.GetLength(0) - 1; i++)
            {
                string min = array[i];
                int minIndex = i, j;
                for (j = i + 1; j < array.GetLength(0) - 1; j++)
                {
                    if (String.Compare(array[j], min) < 0)
                    {
                        min = array[j];
                        minIndex = j;
                    }
                }
                // scambio fra le righe
                string dummy = array[minIndex];
                array[minIndex] = array[i];
                array[i] = dummy;
            }
        }

        internal static bool FileIsOpened(string Filename)
        {
            try
            {
                Stream s = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.None);
                s.Close();
                return false;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return true;
            }
        }

        internal static string FirstCharToUpper(this string s)
        {
            // Check for empty string.  
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.  
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        internal static int? NullableInt(object Valore)
        {
            try
            {
                return Convert.ToInt32(Valore);
            }
            catch
            {
                return null;
            }
        }

        internal static string NullableString(object Value)
        {
            try
            {
                return Convert.ToString(Value);
            }
            catch
            {
                return null;
            }
        }
    }
}
