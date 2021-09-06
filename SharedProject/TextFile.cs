using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GlucoMan
{
    internal static class TextFile
    {
        /// <summary>
        /// Apre il file passato come parametro.
        /// ATTENZIONE: lo lascia aperto
        /// </summary>
        /// <param name="NomeFile">Path e nome del file da aprire</param>
        /// <param name="appendi">Indica se il file viene aperto in accodamento (append)</param>
        /// <returns>Uno StreamWriter che servirà po per leggere e scrivere nel file</returns>
        internal static StreamWriter openFileOut(string NomeFile, bool appendi)
        {
            if (!Directory.Exists(Path.GetDirectoryName(NomeFile)))
            {
                Directory.CreateDirectory(Path.GetFullPath(NomeFile));
            }
            //if(!File.Exists(NomeFile))
            //{
            //    File.Create(NomeFile); 
            //}
            Encoding fileEncoding = Encoding.Default;
            try
            {
                //prova ad aprire il file
                FileStream fsOut;
                if (appendi)
                    fsOut = new FileStream(NomeFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                else
                    fsOut = new FileStream(NomeFile, FileMode.Create, FileAccess.Write, FileShare.Read);
                StreamWriter fOut = new StreamWriter(fsOut, fileEncoding);
                return (fOut);
            }
            catch (Exception e)
            {	// il nome del file è sbagliato o non si riesce ad aprirlo
                //Console.WriteLine("Non si riesce ad aprire il file. Provo a crearlo" + NomeFile + "\r\nErrore:" + e.Message);
                Console.WriteLine("Non si riesce ad aprire il file. Provo a crearlo" + NomeFile);
                // lo apro creandolo
                FileStream fsOut = new FileStream(NomeFile, FileMode.Create, FileAccess.Write, FileShare.Read);
                StreamWriter fOut = new StreamWriter(fsOut, fileEncoding);

                return (fOut);
            }
        }
        /// <summary>
        /// Crea il file indicato e che scrive la stringa passata
        /// </summary>
        /// <param name="NomeFile"></param>
        /// <param name="stringa"></param>
        internal static void CreateEmptyFile(string NomeFile, string stringa)
        {
            Encoding fileEncoding = Encoding.Default;
            FileStream fsOut = new FileStream(NomeFile, FileMode.Create, FileAccess.Write, FileShare.Read);
            StreamWriter fOut = new StreamWriter(fsOut, fileEncoding);

            fOut.WriteLine(stringa);

            fOut.Close();
        }
        /// <summary>
        /// Write a string as a text file
        /// </summary>
        /// <param name="NomeFile"></param>
        /// <param name="stringa"></param>
        /// <param name="appendi"></param>
        /// <returns>Vero se non ci sono stati errori nella </returns>
        internal static bool StringToFile(string NomeFile, string stringa, bool appendi)
        {   
            StreamWriter fileOut;
            fileOut = openFileOut(NomeFile, appendi);
            try
            {
                fileOut.Write(stringa);
                fileOut.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Legge tutto il contenuto del file indicato e lo mette nella stringa passata
        /// </summary>
        /// <param name="NomeFile"></param>
        /// <returns>Tutto il contenuto del file</returns>
        internal static string FileToString(string NomeFile)
        {
            // legge riga per riga in un array di stringhe un file di testo
            int nLine = 0;
            string stringaFile = "";

            try
            {
                // prova ad aprire il file
                Encoding fileEncoding = Encoding.Default;
                FileStream fsIn = new FileStream(NomeFile, FileMode.Open, FileAccess.Read, FileShare.Read);
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
                if (NomeFile != "")
                {
                    Console.WriteLine("Il file " + NomeFile + " non è leggibile\r\nErrore:" + e.Message);
                }
                return null;
            }
            return (stringaFile);
        }
        /// <summary>
        /// Scrive nel file indicato tutto il contenuto dell'array di stringhe passatto. Ogni elemento dell'array corrisponde ad una riga nel file. 
        /// </summary>
        /// <param name="NomeFile"></param>
        /// <param name="stringa"></param>
        /// <param name="appendi"></param>
        /// <returns>Vero se tutto è andato bene</returns>
        internal static bool ArrayToFile(string NomeFile, string[] stringa, bool appendi)
        {   // scrive riga per riga un array di stringhe in un file di testo
            StreamWriter fileOut;
            try
            {
                fileOut = openFileOut(NomeFile, appendi);

                foreach (string st in stringa)
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
        /// <param name="NomeFile"></param>
        /// <returns>Vettore di stringhe nel quale è stato letto il conetnuto del file</returns>
        internal static string[] FileToArray(string NomeFile)
        {
            int nLine = 0;
            string[] stringaFile = new string[0];

            Array.Resize(ref stringaFile, 0);

            try
            {
                // prova ad aprire il file
                Encoding fileEncoding = Encoding.Default;
                FileStream fsIn = new FileStream(NomeFile, FileMode.Open, FileAccess.Read, FileShare.Read);
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
                if (NomeFile != "")
                {
                    Console.WriteLine("Il file " + NomeFile + " non è leggibile\r\nErrore:" + e.Message);
                }
                return null;
            }
            return (stringaFile);
        }
        internal static byte[] FileToByteArray(string NomeFile)
        {
            byte[] buffer;
            try
            {
                // prova ad aprire il file
                Encoding fileEncoding = Encoding.Default;
                FileStream fsIn = new FileStream(NomeFile, FileMode.Open, FileAccess.Read, FileShare.Read);
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
                if (NomeFile != "")
                {
                    Console.WriteLine("Il file " + NomeFile + " non è leggibile\r\nErrore:" + e.Message);
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
                // copia il file perchè è locked
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
                // guarda se non si può leggere perchè c'è un lock
                Console.WriteLine(e.GetType().ToString());
                {

                }
                // il nome del file è sbagliato o non si riesce al leggerlo
                if (FileName != "")
                {
                    Console.WriteLine("Il file " + FileName + " non è leggibile\r\nErrore:" + e.Message);
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
                // copia il file perchè è locked
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
                // guarda se non si può leggere perchè c'è un lock
                Console.WriteLine(e.GetType().ToString());
                {

                }
                // il nome del file è sbagliato o non si riesce al leggerlo
                if (FileName != "")
                {
                    Console.WriteLine("Il file " + FileName + " non è leggibile\r\nErrore:" + e.Message);
                }
                return (null);
            }
            return (MatriceFile);
        }
        private enum ParserStatus
        {
            CheckDelimiterFirst,
            UnDelimitedString,
            StartDelimitedString,
            DelimitedString,
            FinalBlanks,
            LineFinished
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
            ParserStatus status = ParserStatus.CheckDelimiterFirst;
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
                    case ParserStatus.CheckDelimiterFirst:
                        {
                            if (currentChar == StringDelimiter)
                            {
                                // delimited string started
                                status = ParserStatus.DelimitedString;
                                currentField = ""; // delimited string starts from here 
                            }
                            if (currentChar == FieldSeparator)
                            {
                                //currentField += currentChar;  // adds the character to the field
                                // found a new field, the other is finished
                                status = ParserStatus.CheckDelimiterFirst; // same status, but saves a new field 
                                // adds the field to the list of fields 
                                lineContent.Add(currentField);
                                currentField = "";
                                break;
                            }
                            if (currentChar != space)
                            {
                                // if it is not a blank, the blank is not the first; string is undelimited 
                                status = ParserStatus.UnDelimitedString;
                                currentField += currentChar;  // adds the character to the field
                                // undelimited string starts from there.. (after the comma)
                            }
                            break;
                        }
                    case ParserStatus.UnDelimitedString:
                        {
                            if (currentChar == FieldSeparator)
                            {
                                // found a new field 
                                status = ParserStatus.CheckDelimiterFirst;
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
                                status = ParserStatus.FinalBlanks;
                                // adds the field to the list of fields 
                                lineContent.Add(currentField);
                                currentField = "";
                            }
                            break;
                        }
                    case ParserStatus.FinalBlanks:
                        {
                            // voids all characters after end of delimited string
                            // doesn't add any character to the field

                            if (currentChar == FieldSeparator)
                            {
                                // found a new field 
                                status = ParserStatus.CheckDelimiterFirst;
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
                        MatriceFile[nLine, j] = campi[j]; // OCCHIO, QUI SE C'è UN CAMPO NULL SI INCHIODA!!!
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
                    Console.WriteLine("Il file " + FileName + " non è leggibile\r\nErrore:" + e.Message);
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
                        MatrixFromFile[nLine, j] = campi[j]; // OCCHIO, QUI SE C'è UN CAMPO NULL SI INCHIODA!!!
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
                    Console.WriteLine("Il file " + FileName + " non è leggibile\r\nErrore:" + e.Message);
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
        //        // copia il file perchè è locked
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
        //        // guarda se non si può leggere perchè c'è un lock
        //        Console.WriteLine(e.GetType().ToString());
        //        {

        //        }
        //        // il nome del file è sbagliato o non si riesce al leggerlo
        //        if (FileName != "")
        //        {
        //            Console.WriteLine("Il file " + FileName + " non è leggibile\r\nErrore:" + e.Message);
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
                // copia il file perchè è locked
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
                // guarda se non si può leggere perchè c'è un lock
                Console.WriteLine(e.GetType().ToString());
                {

                }
                // il nome del file è sbagliato o non si riesce al leggerlo
                if (FileName != "")
                {
                    Console.WriteLine("Il file " + FileName + " non è leggibile\r\nErrore:" + e.Message);
                }
                return (null);
            }
        }
        internal static bool MatrixToFile(string NomeFile, string[,] Matrix, char Separatore, bool appendi)
        {   /// scrive riga per riga un array di stringhe in un file di testo
            StreamWriter fileOut;
            try
            {
                fileOut = openFileOut(NomeFile, appendi);
                for (int i = 0; i < Matrix.GetLength(0); i++)
                {
                    string linea = Matrix[i, 0];
                    for (int j = 1; j < Matrix.GetLength(1); j++)
                    {
                        linea += Separatore + Matrix[i, j];
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
        /// <param name="Separator">Carattere che delimita i campi del file della stessa riga</param>
        /// <param name="FirstRow">Array di strignhe che viene scritto nella prima riga del file </param>
        /// <param name="Append">Se vero il metodo scrive in fondo al file esistente, se no lo crea daccapo</param>
        /// <returns>Vero se tutto è andato bene</returns>     
        internal static bool MatrixToFile(string FileName, string[,] Matrix, char Separator, string[] FirstRow, bool Append)
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
                        linea += Separator + FirstRow[j];
                    }
                    fileOut.WriteLine(linea);
                    for (int i = 0; i < Matrix.GetLength(0); i++)
                    {
                        linea = Matrix[i, 0];
                        for (int j = 1; j < Matrix.GetLength(1); j++)
                        {
                            linea += Separator + Matrix[i, j];
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
