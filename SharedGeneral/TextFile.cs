using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace gamon
{
    internal static class TextFile
    {
        /// <summary>
        /// Apre il file passato come parametro.
        /// ATTENZIONE: lo lascia aperto
        /// </summary>
        /// <param name="FileName">Path and filename of file to open </param>
        /// <param name="Append">Tells if the file has to be opened in append mode</param>
        /// <returns>Returns StreamWriter thah can be useful to read or write to the file</returns>
        internal static StreamWriter openFileOut(string FileName, bool Append)
        {
            if (!Directory.Exists(Path.GetDirectoryName(FileName)))
            {
                Directory.CreateDirectory(Path.GetFullPath(FileName));
            }
            //if(!File.Exists(FileName))
            //{
            //    File.Create(FileName); 
            //}
            Encoding fileEncoding = Encoding.Default;
            try
            {
                //prova ad aprire il file
                FileStream fsOut;
                if (Append)
                    fsOut = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                else
                    fsOut = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                StreamWriter fOut = new StreamWriter(fsOut, fileEncoding);
                return (fOut);
            }
            catch (Exception e)
            {	// il nome del file è sbagliato o non si riesce ad aprirlo
                //Console.Out.WriteLine("Non si riesce ad aprire il file. Provo a crearlo" + FileName + "\r\nErrore:" + e.Message);
                Console.Out.WriteLine("Non si riesce ad aprire il file. Provo a crearlo" + FileName);
                // lo apro creandolo
                FileStream fsOut = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                StreamWriter fOut = new StreamWriter(fsOut, fileEncoding);

                return (fOut);
            }
        }
        /// <summary>
        /// Crea il file indicato e che scrive la stringa passata
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="FileContent"></param>
        internal static void CreateEmptyFile(string FileName, string FileContent)
        {
            Encoding fileEncoding = Encoding.Default;
            FileStream fsOut = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            StreamWriter fOut = new StreamWriter(fsOut, fileEncoding);

            fOut.WriteLine(FileContent);

            fOut.Close();
        }
        /// <summary>
        /// Write a string as a text file
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="FileContent"></param>
        /// <param name="AppendToFile"></param>
        /// <returns>Vero se non ci sono stati errori nella </returns>
        internal static bool StringToFile(string FileName, string FileContent, bool AppendToFile)
        {   
            StreamWriter fileOut;
            fileOut = openFileOut(FileName, AppendToFile);
            try
            {
                fileOut.Write(FileContent);
                fileOut.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        internal static async Task StringToFileAsync(string FileName, string FileContent)
        {
            try
            {
                if (!File.Exists(FileName))
                {
                    File.CreateText(FileName);
                }
                using (var writer = File.CreateText(FileName))
                {
                    await writer.WriteAsync(FileContent);
                }
            }
            catch (Exception e)
            {
                General.LogOfProgram.Error("TextFile", e); 
            }
        }
        /// <summary>
        /// Legge tutto il contenuto del file indicato e lo mette nella stringa passata
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns>Tutto il contenuto del file</returns>
        internal static string FileToString(string FileName)
        {
            // legge riga per riga in un array di stringhe un file di testo
            int nLine = 0;
            string stringaFile = "";

            try
            {
                // prova ad aprire il file
                Encoding fileEncoding = Encoding.Default;
                FileStream fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fsIn, fileEncoding, true);

                stringaFile = sr.ReadToEnd();
                //nLine = 0;
                //// lettura nella stringa di tutte le righe del file
                //for (string Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                //{
                //    stringaFile += Line + "\r\n";
                //    nLine++;
                //}
                //// toglie l'ultima andata a capo che era sata aggiunta
                //stringaFile = stringaFile.Substring(0, stringaFile.Length - 2);

                // chiusura dello StreamReader
                sr.Close();
            }
            catch (Exception e)
            {	// il nome del file è sbagliato o non si riesce al leggerlo
                if (FileName != "")
                {
                    Console.Out.WriteLine("Il file " + FileName + " non � leggibile\r\nErrore:" + e.Message);
                }
                return null;
            }
            return (stringaFile);
        }
        internal static async Task<string> FileToStringAsync(string FileName)
        {
            if (FileName == null || !File.Exists(FileName))
            {
                return null;
            }
            try
            {
                string line;
                using (var reader = new StreamReader(FileName, true))
                {
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                    }
                }
                return line;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Scrive nel file indicato tutto il contenuto dell'array di stringhe passatto. Ogni elemento dell'array corrisponde ad una riga nel file. 
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="FileContent"></param>
        /// <param name="Append"></param>
        /// <returns>Vero se tutto � andato bene</returns>
        internal static bool ArrayToFile(string FileName, string[] FileContent, bool Append)
        {   // scrive riga per riga un array di stringhe in un file di testo
            StreamWriter fileOut;
            try
            {
                fileOut = openFileOut(FileName, Append);

                foreach (string st in FileContent)
                {
                    fileOut.WriteLine(st);
                }
                fileOut.Close();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// Legge riga per riga in un array di stringhe un file di testo
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns>Vettore di stringhe nel quale � stato letto il conetnuto del file</returns>
        internal static string[] FileToArray(string FileName)
        {
            int nLine = 0;
            string[] stringaFile = new string[0];

            Array.Resize(ref stringaFile, 0);

            try
            {
                // prova ad aprire il file
                Encoding fileEncoding = Encoding.Default;
                FileStream fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fsIn, fileEncoding, true);

                nLine = 0;
                // lettura nell'array di tutte le righe del file
                for (string Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    Array.Resize(ref stringaFile, stringaFile.Length + 1);
                    stringaFile[nLine] = Line;
                    nLine++;
                }
                // chiusura dello StreamReader
                sr.Close();
            }
            catch (Exception e)
            {	// il nome del file è sbagliato o non si riesce al leggerlo
                if (FileName != "")
                {
                    Console.Out.WriteLine("Il file " + FileName + " non � leggibile\r\nErrore:" + e.Message);
                }
                return null;
            }
            return (stringaFile);
        }
        internal static byte[] FileToByteArray(string FileName)
        {
            byte[] buffer;
            try
            {
                // prova ad aprire il file
                Encoding fileEncoding = Encoding.Default;
                FileStream fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fsIn, fileEncoding, true);

                string stringaFile = sr.ReadToEnd();
                //ASCIIEncoding ascii = new ASCIIEncoding();
                //buffer = ascii.GetBytes(stringaFile);

                buffer = fileEncoding.GetBytes(stringaFile);

                // chiusura dello StreamReader
                sr.Close();
            }
            catch (Exception e)
            {	// il nome del file è sbagliato o non si riesce al leggerlo
                if (FileName != "")
                {
                    Console.Out.WriteLine("Il file " + FileName + " non � leggibile\r\nErrore:" + e.Message);
                }
                return null;
            }
            return (buffer);
        }
        /// <summary>
        /// Reads from a text file a string matrix. 
        /// If some of the strings are delimited by a specific character, it shoud be give as the optional parameter
        /// The optional string delimiter must be the first non blank character after a separator character
        /// </summary>
        /// <param name="FileName">Nome del file da cui leggere i dati</param>
        /// <param name="FieldSeparator">Carattere che separa un campo dall'altro nella stessa riga del file</param b
        /// 
        /// <returns>Matrix of strings that contains the data</returns>
        internal static string[,] FileToMatrix(string FileName, char FieldSeparator)
        {
            int nLine = 0;
            string[,] MatriceFile = new string[0, 0]; // inizializzazione fittizia, quella buona dopo
            try
            {
                // prova ad aprire il file
                Encoding fileEncoding = Encoding.Default;
                FileStream fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fsIn, fileEncoding, true);
                // prima lettura del file per determinare il numero delle righe e delle colonne
                int nRows = 0;
                int nColumns = 0;
                // lettura nell'array di tutte le righe del file
                for (string Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    string[] campi = Line.Split(FieldSeparator);
                    if (campi.Length > nColumns)
                        nColumns = campi.Length;
                    nRows++;
                }
                // chiusura dello StreamReader
                sr.Close();

                // creazione della matrice
                MatriceFile = new string[nRows, nColumns];

                // riapertura del file per il riempimento della matrice
                fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                sr = new StreamReader(fsIn, fileEncoding, true);
                nLine = 0;
                // lettura nella matrice di tutte le righe del file
                for (string Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    string[] campi = Line.Split(FieldSeparator);
                    for (int j = 0; j < campi.Length; j++)
                    {
                        MatriceFile[nLine, j] = campi[j];
                    }
                    nLine++;
                }
                // chiusura dello StreamReader
                sr.Close();
            }

            catch (IOException e)
            {
                // copia il file perch� � locked
                System.IO.File.Copy(FileName, "temp");
                // ci riprovo con il file copiato
                string[,] campi = FileToMatrix("temp", FieldSeparator);
                // cancello il file appena letto
                System.IO.File.Delete("temp");
                // restituisco quello che ho letto
                return campi;
            }

            catch (Exception e)
            {
                // guarda se non si pu� leggere perch� c'� un lock
                Console.Out.WriteLine(e.GetType().ToString());
                {

                }
                // il nome del file è sbagliato o non si riesce al leggerlo
                if (FileName != "")
                {
                    Console.Out.WriteLine("Il file " + FileName + " non � leggibile\r\nErrore:" + e.Message);
                }
                return (null);
            }
            return (MatriceFile);
        }
        /// <summary>
        /// Legge riga per riga in un array di stringhe un file di testo. La prima riga viene separata dal resto del file e scritta nel vettore primaRiga[].
        /// </summary>
        /// <param name="FileName">Nome del file da cui leggere</param>
        /// <param name="FieldSeparator">Carattere che separa i diversi campi della stessa riga</param>
        /// <param name="StringDelimiter">Character indicating the beginning or end of a string. 
        /// <returns>A list of arrays of strings that contains </returns>
        internal static string[,] FileToMatrix(string FileName, char FieldSeparator, char StringDelimiter)
        {
            int nLine = 0;
            string[,] MatriceFile = new string[0, 0]; // inizializzazione fittizia, quella buona dopo
            try
            {
                // prova ad aprire il file
                Encoding fileEncoding = Encoding.Default;
                FileStream fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fsIn, fileEncoding, true);
                // prima lettura del file per determinare il numero delle righe e delle colonne
                int nRows = 0;
                int nColumns = 0;
                // lettura nell'array di tutte le righe del file
                for (string Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    List<string> lineContent = ParseLine(Line, FieldSeparator, StringDelimiter);
                    if (lineContent.Count > nColumns)
                        nColumns = lineContent.Count;
                    nRows++;
                }
                // chiusura dello StreamReader
                sr.Close();

                // creazione della matrice
                MatriceFile = new string[nRows, nColumns];

                // riapertura del file per il riempimento della matrice
                fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                sr = new StreamReader(fsIn, fileEncoding, true);
                nLine = 0;
                // lettura nella matrice di tutte le righe del file
                for (string Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    List<string> lineContent = ParseLine(Line, FieldSeparator, StringDelimiter);
                    for (int j = 0; j < lineContent.Count; j++)
                    {
                        MatriceFile[nLine, j] = lineContent[j];
                    }
                    nLine++;
                }
                // chiusura dello StreamReader
                sr.Close();
            }

            catch (IOException e)
            {
                // copia il file perch� � locked
                System.IO.File.Copy(FileName, "temp");
                // ci riprovo con il file copiato
                string[,] campi = FileToMatrix("temp", FieldSeparator);
                // cancello il file appena letto
                System.IO.File.Delete("temp");
                // restituisco quello che ho letto
                return campi;
            }

            catch (Exception e)
            {
                // guarda se non si pu� leggere perch� c'� un lock
                Console.Out.WriteLine(e.GetType().ToString());
                {

                }
                // il nome del file è sbagliato o non si riesce al leggerlo
                if (FileName != "")
                {
                    Console.Out.WriteLine("Il file " + FileName + " non � leggibile\r\nErrore:" + e.Message);
                }
                return (null);
            }
            return (MatriceFile);
        }
        private enum ParserStatus
        {
            StratDelimitedString,
            CheckIfDelimited,
            UnDelimitedString,
            DelimitedString,
            FindNext, 
            LineFinished,
            CheckIfDelimiterIsDouble
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LineToParse"></param>
        /// <param name="FieldSeparator"></param>
        /// <param name="StringDelimiter">Character indicating the beginning or end of a string. 
        /// <returns></returns>
        private static List<string> ParseLine(string LineToParse, char FieldSeparator, char StringDelimiter)
        {
            ParserStatus status = ParserStatus.CheckIfDelimited;
            List<string> lineContent = new List<string>();
            int currentPositionInString = 0;
            char currentChar = LineToParse[currentPositionInString];
            char space = ' ';
            char lineFeed = '\n';
            string currentField = "";
            while (currentChar != lineFeed)
            {
                // finite states automa 
                switch (status)
                {
                    case ParserStatus.CheckIfDelimited:
                        {
                            if (currentChar == StringDelimiter)
                            {
                                // delimited string started
                                status = ParserStatus.DelimitedString;
                                currentField = ""; // delimited string starts from here 
                            }
                            if (currentChar == FieldSeparator)
                            {
                                // found a new field, the other is finished
                                status = ParserStatus.FindNext; // same status, but saves a new field 
                                // adds the field to the list of fields 
                                lineContent.Add(currentField);
                                currentField = "";
                                break;
                            }
                            currentField += currentChar;  // adds the character to the field
                            break;
                        }
                    case ParserStatus.UnDelimitedString:
                        {
                            if (currentChar == FieldSeparator)
                            {
                                // found a new field 
                                status = ParserStatus.FindNext;
                                // adds the field to the list of fields 
                                lineContent.Add(currentField);
                                currentField = "";
                                break;
                            }
                            currentField += currentChar;  // adds the character to the field
                            break;
                        }
                    case ParserStatus.DelimitedString:
                        {
                            currentField += currentChar; // adds the character to the field
                            if (currentChar == StringDelimiter)
                            {
                                // delimited string finished
                                status = ParserStatus.FindNext;
                                // adds the field to the list of fields 
                                lineContent.Add(currentField);
                                currentField = "";
                            }
                            break;
                        }
                    case ParserStatus.FindNext:
                        {
                            // voids all characters after end of delimited string
                            // doesn't add any character to the field
                            if (currentChar == FieldSeparator)
                            {
                                // found a new field 
                                status = ParserStatus.CheckIfDelimited;
                                // adds the field to the list of fields 
                                lineContent.Add(currentField);
                                currentField = "";
                            }
                            break;
                        }
                }
                // every character is checked if it is line feed 
                if (currentChar == lineFeed)
                {
                    // line finished: do nothing 
                    // adds the last field to the list of fields 
                    lineContent.Add(currentField);
                    currentField = "";
                }
                currentPositionInString++;
                if (currentPositionInString == LineToParse.Length)
                {
                    lineContent.Add(currentField);
                    return lineContent;
                }
                currentChar = LineToParse[currentPositionInString];
            }
            return lineContent;
        }
        /// <summary>
        /// Legge riga per riga in un array di stringhe un file di testo. La prima riga viene separata dal resto del file e scritta nel vettore primaRiga[].
        /// </summary>
        /// <param name="FileName">Nome del file da cui leggere</param>
        /// <param name="FieldSeparator">Carattere che separa i diversi campi della stessa riga</param>
        /// <returns>A list of arrays of strings that contains </returns>
        internal static string[,] FileToMatrix(string FileName, char FieldSeparator, out string[] FirstRow)
        {
            int nLine = 0;
            string Line;
            string[,] MatriceFile = new string[0, 0]; // inizializzazione fittizia, quella buona dopo
            try
            {
                // prova ad aprire il file
                Encoding fileEncoding = Encoding.Default;
                FileStream fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fsIn, fileEncoding, true);
                // prima lettura del file per determinare il numero delle righe e delle colonne
                int nRows = 0;
                int nColumns = 0;
                // lettura nell'array di tutte le righe del file
                for (Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    string[] campi = Line.Split(FieldSeparator);
                    if (campi.Length > nColumns)
                        nColumns = campi.Length;
                    nRows++;
                }
                // chiusura dello StreamReader
                sr.Close();

                // creazione della matrice
                MatriceFile = new string[nRows - 1, nColumns];

                // riapertura del file per il riempimento della matrice
                fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                sr = new StreamReader(fsIn, fileEncoding, true);
                nLine = 0;
                // lettura nella stringa primaRiga della prima riga del file
                Line = sr.ReadLine();
                FirstRow = Line.Split(FieldSeparator);

                // lettura nella matrice di tutte le righe del file
                for (Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    string[] campi = Line.Split(FieldSeparator);
                    for (int j = 0; j < campi.Length; j++)
                    {
                        MatriceFile[nLine, j] = campi[j]; // OCCHIO, QUI SE C'� UN CAMPO NULL SI INCHIODA!!!
                    }
                    nLine++;
                }
                // chiusura dello StreamReader
                sr.Close();
            }
            catch (Exception e)
            {	// il nome del file è sbagliato o non si riesce al leggerlo
                if (FileName != "")
                {
                    Console.Out.WriteLine("Il file " + FileName + " non � leggibile\r\nErrore:" + e.Message);
                }
                FirstRow = null;
                return (null);
            }
            return (MatriceFile);
        }
        /// <summary>
        /// Legge riga per riga in un array di stringhe un file di testo. La prima riga viene separata dal resto del file e scritta nel vettore primaRiga[].
        /// </summary>
        /// <param name="FileName">Nome del file da cui leggere</param>
        /// <param name="FieldSeparator">Carattere che separa i diversi campi della stessa riga</param>
        /// <param name="FirstRow">Tutti i campi trovati nella prima riga del file di testo</param>
        /// <param name="SecondRow">Tutti i campi trovati nella seconda riga del file di testo</param>
        /// <returns></returns>
        internal static string[,] FileToMatrix(string FileName, char FieldSeparator, out string[] FirstRow, out string[] SecondRow)
        {
            int nLine = 0;
            string Line;
            string[,] MatrixFromFile = new string[0, 0]; // inizializzazione fittizia, quella buona dopo
            try
            {
                // prova ad aprire il file
                Encoding fileEncoding = Encoding.Default;
                FileStream fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fsIn, fileEncoding, true);
                // prima lettura del file per determinare il numero delle righe e delle colonne
                int nRows = 0;
                int nColumns = 0;
                // lettura nell'array di tutte le righe del file
                for (Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    string[] campi = Line.Split(FieldSeparator);
                    if (campi.Length > nColumns)
                        nColumns = campi.Length;
                    nRows++;
                }
                // chiusura dello StreamReader
                sr.Close();

                // creazione della matrice
                MatrixFromFile = new string[nRows - 1, nColumns];

                // riapertura del file per il riempimento della matrice
                fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                sr = new StreamReader(fsIn, fileEncoding, true);
                nLine = 0;
                // lettura nella stringa primaRiga della prima riga del file
                Line = sr.ReadLine();
                FirstRow = Line.Split(FieldSeparator);

                // lettura nella stringa secondaRiga della seconda riga del file
                Line = sr.ReadLine();
                SecondRow = Line.Split(FieldSeparator);

                // lettura nella matrice di tutte le righe del file
                for (Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    string[] campi = Line.Split(FieldSeparator);
                    for (int j = 0; j < campi.Length; j++)
                    {
                        MatrixFromFile[nLine, j] = campi[j]; // OCCHIO, QUI SE C'� UN CAMPO NULL SI INCHIODA!!!
                    }
                    nLine++;
                }
                // chiusura dello StreamReader
                sr.Close();
            }
            catch (Exception e)
            {	// il nome del file è sbagliato o non si riesce al leggerlo
                if (FileName != "")
                {
                    Console.Out.WriteLine("Il file " + FileName + " non � leggibile\r\nErrore:" + e.Message);
                }
                FirstRow = null;
                SecondRow = null;
                return (null);
            }
            return (MatrixFromFile);
        }
        internal static string[,] FileToMatrix(string FileName, string FieldSeparator)
        {   // per prendere il primo carattere della stringa Separatore
            return FileToMatrix(FileName, FieldSeparator[0]);
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="FileName">Name of the file from which to read data</param>
        ///// <param name="FieldSeparator">separator char that separates the fields in the same row</param>
        ///// <param name="StringDelimiter">char that indicates the beginning and the and of some strings</param>
        ///// <returns>List of the list of fields read in the file. Every list isa a row of the file. 
        ///// The rows can have different number of columns</returns>
        //internal static List<List<string>> FileToListOfLists(string FileName, char FieldSeparator, char StringDelimiter)
        //{
        //    List<List<string>> rows = new List<List<string>>();
        //    try
        //    {
        //        // prova ad aprire il file
        //        Encoding fileEncoding = Encoding.Default;
        //        FileStream fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        //        StreamReader sr = new StreamReader(fsIn, fileEncoding, true);

        //        int nRows = 0;
        //        int nColumns = 0;
        //        int beginSubString, endSubString;
        //        // loop in the lines of the file 
        //        for (string line = sr.ReadLine(); line != null; line = sr.ReadLine())
        //        {
        //            beginSubString = 0; 
        //            endSubString = 0;
        //            // parse the line character by character and put the fields in the following List
        //            char status = 's'; // start status
        //            List<string> row = new List<string>();
        //            int thisRowColumns = 0;
        //            string field; 
        //            for (int i = 0; i < line.Length; i++)
        //            {
        //                char c = line[i];
        //                switch (status)
        //                {
        //                    case ('s'): // start status
        //                        if (c != ' ') // if c = 0, stay in the same state, looking for delimiter or other character
        //                        {
        //                            if (c == StringDelimiter)
        //                            {
        //                                status = 'f'; // first delimiter string detected
        //                                beginSubString = i + 1;
        //                            }
        //                            else if (c == FieldSeparator)
        //                            {
        //                                // produce field without changing the state
        //                                endSubString = i;
        //                                field = line.Substring(beginSubString, endSubString - beginSubString);
        //                                row.Add(field);
        //                                thisRowColumns++;
        //                                beginSubString = i + 1;
        //                            }
        //                            else
        //                            {
        //                                status = 'u'; // scan into undelimited string 
        //                            }
        //                        }
        //                        break;
        //                    case ('f'): // first occurrence of a delimiter 
        //                        if (c == StringDelimiter)
        //                        {   // two consecutive delimeters
        //                            // back to start, we don't know if it is a delimeter or a character
        //                            status = 's';
        //                        }
        //                        else
        //                        {   // a delimeter + a normal character: scan looking for a delimeter
        //                            status = 'd'; 
        //                        }
        //                        break;
        //                    case ('d'): // inside delimited string status
        //                        // waiting for final delimiter 
        //                        if (c == StringDelimiter)
        //                        {
        //                            status = 'c'; // now check for double StringDelimiter character 
        //                        }
        //                        break;
        //                    case ('c'): // delimited string status: waiting for final delimiter 
        //                        if (c == StringDelimiter)
        //                        {
        //                            // two consecutive delimiters are a normal character 
        //                            // back to the "inside delimited string" status 
        //                            status = 'd'; 
        //                        }
        //                        else if(c == FieldSeparator)
        //                        {   // separator already found 
        //                            status = 's';   // go back to start
        //                            endSubString = i - 1;
        //                            field = line.Substring(beginSubString, endSubString - beginSubString);
        //                            field = field.Replace(StringDelimiter.ToString() + StringDelimiter.ToString(), StringDelimiter.ToString()); 
        //                            row.Add(field);
        //                            thisRowColumns++;
        //                            // prepare for the next field 
        //                            beginSubString = i + 1;
        //                        }
        //                        else
        //                        {
        //                            status = 'w'; // now waits the field separator
        //                        }
        //                        break;
        //                    case ('w'): // wait for the separator after a delimited string
        //                        if (c == FieldSeparator)
        //                        {
        //                            status = 's'; // restart recognition
        //                            field = line.Substring(beginSubString, endSubString - beginSubString);
        //                            row.Add(field);
        //                            thisRowColumns++;
        //                            beginSubString = i + 1;
        //                        }
        //                        break; 
        //                    case ('u'):
        //                        if (c == FieldSeparator)
        //                        {
        //                            status = 's';   // go back to start
        //                            endSubString = i;
        //                            field = line.Substring(beginSubString, endSubString - beginSubString);
        //                            row.Add(field);
        //                            thisRowColumns++;
        //                            // prepare for the next field 
        //                            beginSubString = i + 1;
        //                        }
        //                        break;
        //                }
        //            }
        //            // last field, not terminated with a separator
        //            field = line.Substring(beginSubString, line.Length - beginSubString);
        //            row.Add(field);
        //            thisRowColumns++;
        //            if (thisRowColumns > nColumns)
        //                nColumns = thisRowColumns;
        //            rows.Add(row);
        //            nRows++;
        //        }
        //        // chiusura dello StreamReader
        //        sr.Close();
        //    }

        //    catch (IOException e)
        //    {
        //        // copia il file perch� � locked
        //        System.IO.File.Copy(FileName, "temp");
        //        // ci riprovo con il file copiato
        //        List<List<string>> campi = FileToListOfLists("temp", FieldSeparator, StringDelimiter);
        //        // cancello il file appena letto
        //        System.IO.File.Delete("temp");
        //        // restituisco quello che ho letto
        //        return campi;
        //    }

        //    catch (Exception e)
        //    {
        //        // guarda se non si pu� leggere perch� c'� un lock
        //        Console.Out.WriteLine(e.GetType().ToString());
        //        {

        //        }
        //        // il nome del file è sbagliato o non si riesce al leggerlo
        //        if (FileName != "")
        //        {
        //            Console.Out.WriteLine("Il file " + FileName + " non � leggibile\r\nErrore:" + e.Message);
        //        }
        //        return (null);
        //    }
        //    return (rows);
        //}
        internal static List<List<string>> FileToListOfLists(string FileName, char FieldSeparator, char StringDelimiter)
        {
            int nLine = 0;
            List<List<string>> ListOfLists = new List<List<string>>();
            try
            {
                // prova ad aprire il file
                Encoding fileEncoding = Encoding.Default;
                FileStream fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fsIn, fileEncoding, true);

                // creazione della matrice
                ListOfLists = new List<List<string>>();

                // riapertura del file per il riempimento della matrice
                fsIn = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                sr = new StreamReader(fsIn, fileEncoding, true);
                nLine = 0;
                // lettura nella lista di tutte le righe del file
                for (string Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    List<string> lineContent = ParseLine(Line, FieldSeparator, StringDelimiter);
                    //for (int j = 0; j < lineContent.Count; j++)
                    //{
                        ListOfLists.Add(lineContent);
                    //}
                    nLine++;
                }
                // chiusura dello StreamReader
                sr.Close();

                return ListOfLists;
            }
            catch (IOException e)
            {
                // copy the file (if it was locked..) 
                System.IO.File.Copy(FileName, "temp");
                // ci riprovo con il file copiato
                List<List<string>> campi = FileToListOfLists(".\\temp", FieldSeparator, StringDelimiter);
                // cancello il file appena letto
                System.IO.File.Delete("temp");
                // restituisco quello che ho letto
                return ListOfLists;
            }
            catch (Exception e)
            {
                // guarda se non si pu� leggere perch� c'� un lock
                Console.Out.WriteLine(e.GetType().ToString());
                {

                }
                // il nome del file è sbagliato o non si riesce al leggerlo
                if (FileName != "")
                {
                    Console.Out.WriteLine("Il file " + FileName + " non � leggibile\r\nErrore:" + e.Message);
                }
                return (null);
            }
        }
        internal static List<List<string>> FileToListOfLists_GlobalParse(string FileName, char FieldSeparator, 
            char StringDelimiter, char NewLine)
        {
            int nLine = 0;
            List<List<string>> ListOfLists = new List<List<string>>();
            try
            {
                // read all the file in one string 
                string allFile = File.ReadAllText(FileName);

                // creazione della matrice
                ListOfLists = ParseString(allFile, FieldSeparator, StringDelimiter, NewLine);

                return ListOfLists;
            }
            catch (IOException e)
            {
                // copy the file (if it was locked..) 
                System.IO.File.Copy(FileName, "temp");
                // ci riprovo con il file copiato
                List<List<string>> campi = FileToListOfLists(".\\temp", FieldSeparator, StringDelimiter);
                // cancello il file appena letto
                System.IO.File.Delete("temp");
                // restituisco quello che ho letto
                return ListOfLists;
            }
            catch (Exception e)
            {
                // guarda se non si pu� leggere perch� c'� un lock
                Console.Out.WriteLine(e.GetType().ToString());
                {

                }
                // il nome del file è sbagliato o non si riesce al leggerlo
                if (FileName != "")
                {
                    Console.Out.WriteLine("Il file " + FileName + " non � leggibile\r\nErrore:" + e.Message);
                }
                return (null);
            }
        }
        private static List<List<string>> ParseString(string FileToParse, char FieldSeparator, char StringDelimiter, char NewLine)
        {
            List<string> lineContent = new List<string>();
            List <List<string>> matrix = new List<List<string>>();

            int currentPositionInString = 0;
            char space = ' ';
            string currentField = "";
            ParserStatus status = ParserStatus.CheckIfDelimited;
            for (int i = 0; i < FileToParse.Length; i++)
            {
                char currentChar = FileToParse[i]; 
                // finite states automa 
                switch (status)
                {
                    // we looki for a string delimiter, while adding characters to the field 
                    // (if we don't find any delimiter, then the string was undelimented, and
                    // we have already recovered it)  
                    case ParserStatus.CheckIfDelimited:
                        {
                            if (currentChar == StringDelimiter)
                            {
                                // delimited string started
                                status = ParserStatus.DelimitedString;
                                // delimited string starts from the character after the delimiter
                                currentField = ""; 
                                break;
                            }
                            if (currentChar == FieldSeparator)
                            {
                                // found a new field, the other was undelimited and is finished
                                // adds the field to the list of fields 
                                lineContent.Add(currentField);
                                // prepare for the next field 
                                currentField = "";
                                // the state remains the same, we are still looking for a 
                                // string delimiter, adding characters to the field 
                                break;
                            }
                            if (currentChar == NewLine)
                            {
                                // the line is finished, we put the last string
                                // into the row and the new row into the list 
                                lineContent.Add(currentField);
                                currentField = "";
                                // add the line to the matrix 
                                matrix.Add(lineContent);
                                // prepare for the new line
                                lineContent = new List<string>();
                                break; 
                            }
                            // if this is not a special character, we add it to the field
                            currentField += currentChar;
                            break;
                        }
                    case ParserStatus.DelimitedString:
                        {
                            if (currentChar == StringDelimiter)
                            {
                                // delimited string is finishing? 
                                status = ParserStatus.CheckIfDelimiterIsDouble;
                            } else
                                currentField += currentChar; // adds the character to the field
                            break;
                        }
                    case ParserStatus.CheckIfDelimiterIsDouble:
                        {
                            if (currentChar == StringDelimiter)
                            {
                                // double delimiter means that I simply want to put a delimiter character
                                // into the field. So we continue to look for the finishing delimeter
                                status = ParserStatus.DelimitedString;
                                // adds the character (delimiter) to the field
                                currentField += currentChar; 
                            }
                            else
                            {
                                // 
                                // the end of the delimiters means that the string that we accumulated
                                // is the currentField 
                                lineContent.Add(currentField);
                                currentField = "";
                                // drop all the characters that could be after the
                                // delimited string is finished 
                                status = ParserStatus.FindNext;
                                // we went one step beoynd to check if the quote
                                // was double, but it is another character that 
                                // we have to parse. We go back to parse it
                                i--;
                            }
                            break;
                        }
                    case ParserStatus.FindNext:
                        {
                            // voids all characters after the end of delimited string
                            // doesn't add any character to the field
                            if (currentChar == FieldSeparator)
                            {
                                // found the beginning a new field 
                                status = ParserStatus.CheckIfDelimited;
                                break; 
                            }
                            // if a new line comes here, it is the "real" new line
                            // it is NOT part of a string! 
                            if (currentChar == NewLine)
                            {
                                lineContent.Add(currentField);
                                currentField = "";
                                matrix.Add(lineContent);
                                lineContent = new List<string>();
                                status = ParserStatus.CheckIfDelimited;
                                currentField = "";
                            }
                            break;
                        }
                }
            }
            // end of the file, add the last field to the row 
            lineContent.Add(currentField);
            // add the row to the matrix 
            matrix.Add(lineContent);
            return matrix;
        }
        internal static bool MatrixToFile(string FileName, string[,] Matrix, char FieldSeparator, bool Append)
        {   /// scrive riga per riga un array di stringhe in un file di testo
            StreamWriter fileOut;
            try
            {
                fileOut = openFileOut(FileName, Append);
                for (int i = 0; i < Matrix.GetLength(0); i++)
                {
                    string linea = Matrix[i, 0];
                    for (int j = 1; j < Matrix.GetLength(1); j++)
                    {
                        linea += FieldSeparator + Matrix[i, j];
                    }
                    fileOut.WriteLine(linea);
                }
                fileOut.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Scrive sul file indicato il contenuto della matrice di stringhe passata. 
        /// Ogni riga della matrice corrisponde ad una riga del file. 
        /// I campi sono delimitati dal separatore. 
        /// Versione con scrittura anche della prima riga, diversa dal resto. 
        /// </summary>
        /// <param name="FileName">Nome del file da scrivere</param>
        /// <param name="Matrix">Matrix di stringhe da cui leggere i dati</param>
        /// <param name="FieldSeparator">Carattere che delimita i campi del file della stessa riga</param>
        /// <param name="FirstRow">Array di strignhe che viene scritto nella prima riga del file </param>
        /// <param name="Append">Se vero il metodo scrive in fondo al file esistente, se no lo crea daccapo</param>
        /// <returns>Vero se tutto � andato bene</returns>     
        internal static bool MatrixToFile(string FileName, string[,] Matrix, char FieldSeparator, string[] FirstRow, bool Append)
        {   /// scrive riga per riga un array di stringhe in un file di testo
            /// la prima riga viene gestita separatamente. Viene scritta la prima riga, poi il contenuto di tutta la matice. 
            StreamWriter fileOut;
            try
            {
                fileOut = openFileOut(FileName, Append);
                if (fileOut != null)
                {
                    string linea = FirstRow[0];
                    for (int j = 1; j < Matrix.GetLength(1); j++)
                    {
                        linea += FieldSeparator + FirstRow[j];
                    }
                    fileOut.WriteLine(linea);
                    for (int i = 0; i < Matrix.GetLength(0); i++)
                    {
                        linea = Matrix[i, 0];
                        for (int j = 1; j < Matrix.GetLength(1); j++)
                        {
                            linea += FieldSeparator + Matrix[i, j];
                        }
                        fileOut.WriteLine(linea);
                    }
                    fileOut.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
