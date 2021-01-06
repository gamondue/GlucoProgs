using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Comuni
{
    class Net
    {
        public static void SendMessageWithAttachment()
        {
            // string serverAddress = "smtps.aruba.it";
            string serverAddress = "smtp.gmail.com";
            // Specify the file to be attached and sent.
            string file = @"D:\Develop\Git\OdiDocIntegration\Grafica\Logo e icone\Logo-corrente_107x107.png";
            
            // Create a message and set up the recipients.
            MailMessage message = new MailMessage();
            // message.From = new MailAddress("gabriele@ingmonti.it");
            message.From = new MailAddress("gamondue@gmail.com");
            message.To.Add("gamon@ingmonti.it"); // RecipientsListCommaSeparated,
            message.Subject = "Prova spedizione email con il Sw del framework"; 
            message.Body = "Questo è il contenuto testuale del file\r\nC'è anche un allegato."; 

            //// Create  the file attachment for this email message.
            //Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
            //// Add time stamp information for the file.
            //ContentDisposition disposition = data.ContentDisposition;
            //disposition.CreationDate = System.IO.File.GetCreationTime(file);
            //disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            //disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
            //// Add the file attachment to this email message.
            //message.Attachments.Add(data);

            //Send the message.
            SmtpClient serverAccess = new SmtpClient(serverAddress);
            //serverAccess.Port = 465; // porta TCP del server smtps di Aruba
            serverAccess.Port = 465; // porta TCP del server smtps di Google
            // Add credentials if the SMTP server requires them.
            //serverAccess.Credentials = new System.Net.NetworkCredential("gabriele@ingmonti.it",
            //    "CmbELOoLk");
            serverAccess.Credentials = new System.Net.NetworkCredential("gamondue", "CmbELOoLk");
            serverAccess.EnableSsl = true;
            serverAccess.Timeout = 10000; 
            try
            {
                serverAccess.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}",
                            ex.ToString());
            }
            // Display the values in the ContentDisposition for the attachment.
            //ContentDisposition cd = data.ContentDisposition;
            //Console.WriteLine("Content disposition");
            //Console.WriteLine(cd.ToString());
            //Console.WriteLine("File {0}", cd.FileName);
            //Console.WriteLine("Size {0}", cd.Size);
            //Console.WriteLine("Creation {0}", cd.CreationDate);
            //Console.WriteLine("Modification {0}", cd.ModificationDate);
            //Console.WriteLine("Read {0}", cd.ReadDate);
            //Console.WriteLine("Inline {0}", cd.Inline);
            //Console.WriteLine("Parameters: {0}", cd.Parameters.Count);
            //foreach (DictionaryEntry d in cd.Parameters)
            //{
            //    Console.WriteLine("{0} = {1}", d.Key, d.Value);
            //}
            //data.Dispose();
        }
    }
}
