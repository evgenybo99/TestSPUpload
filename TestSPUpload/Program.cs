using System;
using System.Net;
using System.Security;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;

namespace TestSPUpload // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static int Main(string[] args)
        {
            string filePath;
            string webUri;
            string userid;
            string password;
            string domain;
            string targetPath;
            
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter file path.");
                return 1;
            }
            else
            {
                filePath = args[0];
                webUri = args[1];
                userid = args[2];
                password = args[3];
                domain = args[4];
                targetPath = args[5];
            }

            using (var ctx = new ClientContext(webUri))
            {
                // SecureString secPassword = new SecureString();

                ctx.Credentials = new NetworkCredential(userid, password, domain); // credentials;

                ctx.AuthenticationMode = ClientAuthenticationMode.Default;

                UploadFile(ctx, targetPath, filePath); // LibName/FolderName/Sub Folder Name/Sub Sub Folder Name/Sub Sub Sub Folder Name"
            }

            return 0;
        }

        private static void UploadFile(ClientContext context, string uploadFolderUrl, string uploadFilePath)
        {
            var fileCreationInfo = new FileCreationInformation
            {
                Content = System.IO.File.ReadAllBytes(uploadFilePath),
                Overwrite = true,
                Url = Path.GetFileName(uploadFilePath)
            };
            var targetFolder = context.Web.GetFolderByServerRelativeUrl(uploadFolderUrl);
            var uploadFile = targetFolder.Files.Add(fileCreationInfo);
            context.Load(uploadFile);
            context.ExecuteQuery();
        }
    }
}