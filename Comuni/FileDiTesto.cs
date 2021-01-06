using System;
using System.Text;
using System.IO;
//using Odi; 

namespace Comuni
{
    public static class FileDiTesto
    {
        static Encoding fileEncoding = Encoding.Default;

        /// <summary>
        /// Apre il file passato come parametro.
        /// ATTENZIONE: lo lascia aperto
        /// </summary>
        /// <param name="NomeFile">Path e nome del file da aprire</param>
        /// <param name="appendi">Indica se il file viene aperto in accodamento (append)</param>
        /// <returns>Uno StreamWriter che servirà po per leggere e scrivere nel file</returns>
        private static StreamWriter openFileOut(string NomeFile, bool appendi)
        {
            try
            {
                //prova ad aprire il file
                FileStream fsOut; 
                if (appendi)
                    fsOut = new FileStream(NomeFile, FileMode.Append, FileAccess.Write , FileShare.Read);
                else
                    fsOut = new FileStream(NomeFile, FileMode.Create, FileAccess.Write , FileShare.Read);
                StreamWriter fOut = new StreamWriter(fsOut, fileEncoding);
                return (fOut);
            }
            catch (Exception e) 
            {   // il nome del file è sbagliato o non si riesce ad aprirlo
                string err = "FileDiTesto: non si riesce ad aprire il file " + NomeFile + ".";
                err += "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                Comuni.OggettiEDati.LogDelProgramma.Error(err, e);
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
        public static void CreaFileVuoto(string NomeFile, string stringa)
        {
            FileStream fsOut = new FileStream(NomeFile, FileMode.Create, FileAccess.Write, FileShare.Read);
            StreamWriter fOut = new StreamWriter(fsOut, fileEncoding);

            fOut.WriteLine(stringa);

            fOut.Close();
        }
        /// <summary>
        /// Scrive nel file indicato tutto il contenuto della stringa passata
        /// </summary>
        /// <param name="NomeFile"></param>
        /// <param name="Stringa"></param>
        /// <param name="Appendi"></param>
        /// <returns>Vero se non ci sono stati errori nella </returns>
        public static bool StringaInFile(string NomeFile, string Stringa, bool Appendi)
        {   // scrive una stringa in un file di testo
            StreamWriter fileOut;
            fileOut = openFileOut(NomeFile, Appendi);
            try
            {
                fileOut.Write(Stringa);
                fileOut.Close();
                return true;
            } catch {
                return false; 
            }
        }
        /// <summary>
        /// Legge tutto il contenuto del file indicato e lo mette nella stringa passata
        /// </summary>
        /// <param name="NomeFile"></param>
        /// <returns>Tutto il contenuto del file</returns>
        public static string FileInStringa(string NomeFile)
        {
            // legge riga per riga in un array di stringhe un file di testo
            //int nLine = 0;
            string stringaFile = "";

            try
            {
                // prova ad aprire il file
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
                    string err = "FileDiTesto: il file " + NomeFile + " non è leggibile";
                    err += "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                    OggettiEDati.LogDelProgramma.Error(err, e);
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
        public static bool VettoreInFile(string NomeFile, string[] stringa, bool appendi)
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

            } catch {
                return false ;
            }
        }
        /// <summary>
        /// Legge riga per riga in un array di stringhe un file di testo
        /// </summary>
        /// <param name="NomeFile"></param>
        /// <returns>Vettore di stringhe nel quale è stato letto il conetnuto del file</returns>
        public static string[] FileInVettore(string NomeFile)
        {
            int nLine = 0;
            string[] stringaFile = new string[0];

            Array.Resize(ref stringaFile, 0);

            try
            {
                // prova ad aprire il file
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
                    string err = "FileDiTesto: il file " + NomeFile + " non è leggibile";
                    err += "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                    OggettiEDati.LogDelProgramma.Error(err, e);
                }
                return null;
            }
            return (stringaFile);
        }

        public static byte[] FileInVettoreByte(string NomeFile)
        {
            byte[] buffer; 
            try
            {
                // prova ad aprire il file
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
                    string err = "FileDiTesto: Il file " + NomeFile + " non è leggibile.";
                    err += "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                    OggettiEDati.LogDelProgramma.Error(err, e);
                }
                return null;
            }
            return (buffer);
        }

        /// <summary>
        /// Legge da file di testo csv una matrice di stringhe. 
        /// </summary>
        /// <param name="NomeFile">Nome del file da cui leggere i dati</param>
        /// <param name="Separatore">Carattere che separa un campo dall'altro nella stessa riga del file</param>
        /// <returns>Matrice di stringhe che contiene i dati</returns>
        public static string[,] FileInMatrice(string NomeFile, char Separatore)
        {
            int nLine = 0;
            string[,] MatriceFile = new string[0, 0]; // inizializzazione fittizia, quella buona dopo

            try
            {
                // prova ad aprire il file
                FileStream fsIn = new FileStream(NomeFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fsIn, fileEncoding, true);
                // prima lettura del file per determinare il numero delle righe e delle colonne
                int nRighe = 0;
                int nColonne = 0;
                // lettura nell'array di tutte le righe del file
                for (string Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    string[] campi = Line.Split(Separatore);
                    if (campi.Length > nColonne)
                        nColonne = campi.Length;
                    nRighe++;
                }
                // chiusura dello StreamReader
                sr.Close();

                // creazione della matrice
                MatriceFile = new string[nRighe, nColonne];

                // riapertura del file per il riempimento della matrice
                fsIn = new FileStream(NomeFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                sr = new StreamReader(fsIn, fileEncoding, true);
                nLine = 0;
                // lettura nella matrice di tutte le righe del file
                for (string Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    string[] campi = Line.Split(Separatore);
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
                // prova in sola lettura (se era locked)
                try { 
                    // copia il file perchè è locked
                    System.IO.File.Copy(NomeFile, "temp"); 
                    // ci riprovo con il file copiato
                    string[,] campi = FileInMatrice("temp", Separatore);
                    // cancello il file appena letto
                    System.IO.File.Delete("temp");
                     // restituisco quello che ho letto
                    return campi;
                } 
                catch
                {
                    // non era locked: proprio non c'è od è rotto
                    return null; 
                }
              }

            catch (Exception ex)
            {
                // guarda se non si può leggere perchè c'è un lock
                string err = "FileDiTesto";
                err += "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name +
                OggettiEDati.LogDelProgramma.Error(err, ex);
                {

                }
                // il nome del file è sbagliato o non si riesce a leggerlo
                if (NomeFile != "")
                {
                    err = "FileDiTesto.";
                    err += "Il file " + NomeFile + " non è leggibile.";
                    err += "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    err += "\nInner exception: " + ex.InnerException.Message;
                    Comuni.OggettiEDati.LogDelProgramma.Error(err, ex);
                }
                return (null);
            }
            return (MatriceFile);
        }
        /// <summary>
        /// Legge riga per riga in un array di stringhe un file di testo. La prima riga viene separata dal resto del file e scritta nel vettore primaRiga[].
        /// </summary>
        /// <param name="NomeFile">Nome del file da cui leggere</param>
        /// <param name="Separatore">Carattere che separa i diversi campi della stessa riga</param>
        /// <param name="primaRiga">Tutti i campi trovati nella prima riga del file di testo</param>
        /// <returns></returns>
        public static string[,] FileInMatrice(string NomeFile, char Separatore, out string[] primaRiga)
        {
            int nLine = 0;
            string Line;
            string[,] MatriceFile = new string[0, 0]; // inizializzazione fittizia, quella buona dopo
            try
            {
                // prova ad aprire il file
                FileStream fsIn = new FileStream(NomeFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fsIn, fileEncoding, true);
                // prima lettura del file per determinare il numero delle righe e delle colonne
                int nRighe = 0;
                int nColonne = 0;
                // lettura nell'array di tutte le righe del file
                for (Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    string[] campi = Line.Split(Separatore);
                    if (campi.Length > nColonne)
                        nColonne = campi.Length;
                    nRighe++;
                }
                // chiusura dello StreamReader
                sr.Close();

                // creazione della matrice
                MatriceFile = new string[nRighe -1, nColonne];

                // riapertura del file per il riempimento della matrice
                fsIn = new FileStream(NomeFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                sr = new StreamReader(fsIn, fileEncoding, true);
                nLine = 0;
                // lettura nella stringa primaRiga della prima riga del file
                Line = sr.ReadLine();
                primaRiga = Line.Split(Separatore);

                // lettura nella matrice di tutte le righe del file
                for (Line = sr.ReadLine(); Line != null; Line = sr.ReadLine())
                {
                    string[] campi = Line.Split(Separatore);
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
                if (NomeFile != "")
                {
                    string err = "FileDiTesto: Il file " + NomeFile + " non è leggibile.";
                    err += "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    OggettiEDati.LogDelProgramma.Error(err, e);
                }
                primaRiga = null;
                return (null);
            }
            return (MatriceFile);
        }

        public static string[,] FileInMatrice(string NomeFile, string Separatore)
        {   // per prendere il primo carattere della stringa Separatore
            return FileInMatrice(NomeFile, Separatore[0]);
        }
        /// <summary>
        /// Scrive sul file indicato il contenuto della matrice di stringhe passata. Ogni riga della matrice corrisponde ad una riga del file. I campi sono delimitati dal separatore. 
        /// </summary>
        /// <param name="NomeFile">Nome del file da scrivere</param>
        /// <param name="Matrice">Matrice di stringhe da cui leggere i dati</param>
        /// <param name="Separatore">Carattere che delimita i campi del file della stessa riga</param>
        /// <param name="appendi">Se vero il metodo scrive in fondo al file esistente, se no lo crea daccapo</param>
        /// <returns>Vero se tutto è andato bene</returns>
        public static bool MatriceInFile(string NomeFile, string[,] Matrice, char Separatore, bool appendi)
        {   /// scrive riga per riga un array di stringhe in un file di testo
            StreamWriter fileOut;
            try
            {
                fileOut = openFileOut(NomeFile, appendi);
                for (int i = 0; i < Matrice.GetLength(0); i++)
                {
                    string linea = Matrice[i, 0];
                    for (int j = 1; j < Matrice.GetLength(1); j++)
                    {
                        linea += Separatore + Matrice[i, j];
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
        /// <param name="NomeFile">Nome del file da scrivere</param>
        /// <param name="Matrice">Matrice di stringhe da cui leggere i dati</param>
        /// <param name="Separatore">Carattere che delimita i campi del file della stessa riga</param>
        /// <param name="primaRiga">Array di strignhe che viene scritto nella prima riga del file </param>
        /// <param name="appendi">Se vero il metodo scrive in fondo al file esistente, se no lo crea daccapo</param>
        /// <returns>Vero se tutto è andato bene</returns>
        public static bool MatriceInFile(string NomeFile, string[,] Matrice, char Separatore, string[] primaRiga, bool appendi)
        {   /// scrive riga per riga un array di stringhe in un file di testo
            /// la prima riga viene gestita separatamente. Viene scritta la prima riga, poi il contenuto di tutta la matice. 
            StreamWriter fileOut;
            try
            {
                fileOut = openFileOut(NomeFile, appendi);
                if (fileOut != null)
                {
                    string linea = primaRiga[0];
                    for (int j = 1; j < Matrice.GetLength(1); j++)
                    {
                        linea += Separatore + primaRiga[j];
                    }
                    fileOut.WriteLine(linea);
                    for (int i = 0; i < Matrice.GetLength(0); i++)
                    {
                        linea = Matrice[i, 0];
                        for (int j = 1; j < Matrice.GetLength(1); j++)
                        {
                            linea += Separatore + Matrice[i, j];
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
