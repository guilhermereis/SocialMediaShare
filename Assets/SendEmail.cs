using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SendEmail : MonoBehaviour
{
    public void DoSendEmail()
    {
        string FilePath = "";
        string AttachmentName = "small.mp4";
        string FileName = "";


#if UNITY_EDITOR
        FilePath = string.Format(@"Assets/StreamingAssets/{0}", AttachmentName);
#else
             FilePath = Application.persistentDataPath + "/" + AttachmentName;
             if(!File.Exists(FilePath)) {
                 WWW loadImage = new WWW("jar:file://" + Application.dataPath + "!/assets/" + AttachmentName);
                 while(!loadImage.isDone) {}
                 File.WriteAllBytes(FilePath, loadImage.bytes);
             }
#endif

        FileName = FilePath;

        System.Net.Mail.MailMessage mail = new MailMessage();
        //src = cid:Image1
        mail.From = new MailAddress("agoravaiguilherme@gmail.com");
        mail.To.Add("grrbm2@gmail.com");
        mail.Subject = "eMail Subject";
        mail.IsBodyHtml = true;
        string str;
        str = "<h1>This is a test</h1>";
        str += @"<img src=""https://www.mail-signatures.com/wp-content/themes/emailsignatures/images/facebook-35x35.gif"" alt=""Facebook Icon"" />";
        str += @"<img src=""https://www.mail-signatures.com/wp-content/themes/emailsignatures/images/twitter-35x35.gif"" alt=""Twitter Icon"" />";
        str += @"<img src=""https://www.mail-signatures.com/wp-content/themes/emailsignatures/images/facebook-35x35.gif"" alt=""Instagram Icon"" />";

        Debug.Log(str);
        mail.Body = str;

        System.Net.Mail.Attachment data = new Attachment(FileName, System.Net.Mime.MediaTypeNames.Application.Octet);
        // Add time stamp information for the file.
        System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
        disposition.CreationDate = System.IO.File.GetCreationTime(FileName);
        disposition.ModificationDate = System.IO.File.GetLastWriteTime(FileName);
        disposition.ReadDate = System.IO.File.GetLastAccessTime(FileName);

        mail.Attachments.Add(data);

        System.Net.Mail.SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("agoravaiguilherme", "holysmoke");
        smtpServer.EnableSsl = true;
        System.Net.ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
                return true;
            };
        try
        {
            smtpServer.Send(mail);
            Debug.Log("Email sent.");
        }
        catch (Exception e)
        {
            Debug.Log(e.GetBaseException());
        }

    }
}
