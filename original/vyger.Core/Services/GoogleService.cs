using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Newtonsoft.Json;
using vyger.Core.Models;
using io = System.IO;

namespace vyger.Core.Services
{
    #region Interface

    public interface IGoogleService
    {
        /// <summary>
        /// Gets the login URL for google
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        string LoginUrl(string redirectUrl, string scope);

        /// <summary>
        /// Authenticates against Google and creates a SecurityActor
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        ISecurityActor Authenticate(string redirectUrl, string code);
        string[] GetScopesRequired();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sa"></param>
        /// <param name="name"></param>
        /// <param name="modified"></param>
        void DownloadContents(ISecurityActor sa);
    }

    #endregion

    public partial class GoogleService : IGoogleService
    {
        #region Members

        private static readonly string ClientId = ConfigurationManager.AppSettings["Google.ClientId"];
        private static readonly string ClientSecret = ConfigurationManager.AppSettings["Google.Secret"];

        #endregion

        #region Auth Methods

        public string LoginUrl(string redirectUrl, string scope)
        {
            string url = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={ClientId}&response_type=code&scope={scope}&redirect_uri={redirectUrl}&state=abcdef";

            return url;
        }

        public ISecurityActor Authenticate(string redirectUrl, string code)
        {
            GoogleToken token = GetGoogleToken(redirectUrl, code);

            GoogleProfile profile = GetGoogleProfile(token);

            SecurityActor sa = new SecurityActor(profile.Email, true, token.AccessToken);

            return sa;
        }

        private GoogleToken GetGoogleToken(string redirectUrl, string code)
        {
            using (WebClient web = new WebClient())
            {
                string url = $"https://www.googleapis.com/oauth2/v4/token";

                NameValueCollection data = new NameValueCollection();

                data["code"] = code;
                data["client_id"] = ClientId;
                data["client_secret"] = ClientSecret;
                data["redirect_uri"] = redirectUrl;
                data["grant_type"] = "authorization_code";

                byte[] results = web.UploadValues(url, data);

                string json = Encoding.UTF8.GetString(results);

                return JsonConvert.DeserializeObject<GoogleToken>(json);
            }
        }

        private GoogleProfile GetGoogleProfile(GoogleToken token)
        {
            using (WebClient web = new WebClient())
            {
                string url = $"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={token.IdToken}";

                string json = web.DownloadString(url);

                return JsonConvert.DeserializeObject<GoogleProfile>(json);
            }
        }

        #endregion

        #region Drive Methods

        public void UploadContents(ISecurityActor sa, string name, string xml)
        {
            File file = GetStorageMetadata(sa, name);

            if (file == null)
            {
                File metadata = GetMetaData(name, true);

                CreateStorage(sa, metadata, name, xml);
            }
            else
            {
                File metadata = GetMetaData(name, false);

                UpdateStorage(sa, metadata, file.Id, xml);
            }
        }

        public void DownloadContents(ISecurityActor sa)
        {
            if (!io.Directory.Exists(sa.ProfileFolder))
            {
                io.Directory.CreateDirectory(sa.ProfileFolder);
            }

            IList<File> files = GetStorageFiles(sa);

            foreach (File file in files)
            {
                string path = io.Path.Combine(sa.ProfileFolder, file.Name);

                io.FileInfo fileInfo = new io.FileInfo(path);

                if (!fileInfo.Exists || file.ModifiedTime > fileInfo.LastWriteTimeUtc)
                {
                    File metadata = GetMetaData(file.Name, false);

                    //  then we need the latest
                    DownloadStorage(sa, metadata, file.Id);
                }
            }
        }

        private File GetStorageMetadata(ISecurityActor sa, string name)
        {
            DriveService ds = CreateDriveService(sa);

            FilesResource.ListRequest request = ds.Files.List();

            request.Spaces = "appDataFolder";

            request.Fields = "files(id, modifiedTime)";

            request.Q = $"name = '{name}'";

            FileList files = request.Execute();

            if (files.Files.Count == 1)
            {
                return files.Files[0];
            }

            return null;
        }

        private IList<File> GetStorageFiles(ISecurityActor sa)
        {
            DriveService ds = CreateDriveService(sa);

            FilesResource.ListRequest request = ds.Files.List();

            request.Spaces = "appDataFolder";

            request.Fields = "files(id, modifiedTime, name)";

            FileList files = request.Execute();

            return files.Files;
        }

        private void CreateStorage(ISecurityActor sa, File metadata, string name, string xml)
        {
            using (var stream = GetMemoryStream(xml))
            {
                DriveService ds = CreateDriveService(sa);

                FilesResource.CreateMediaUpload request = ds.Files.Create(metadata, stream, "application/xml");

                request.ProgressChanged += Request_ProgressChanged;

                request.Fields = "id";

                request.Upload();
            }
        }

        private void UpdateStorage(ISecurityActor sa, File metadata, string id, string xml)
        {
            using (var stream = GetMemoryStream(xml))
            {
                DriveService ds = CreateDriveService(sa);

                FilesResource.UpdateMediaUpload request = ds.Files.Update(metadata, id, stream, "application/xml");

                request.ProgressChanged += Request_ProgressChanged;

                request.Upload();
            }
        }

        private void DownloadStorage(ISecurityActor sa, File metadata, string id)
        {
            DriveService ds = CreateDriveService(sa);

            FilesResource.GetRequest request = ds.Files.Get(id);

            using (io.MemoryStream stream = new io.MemoryStream())
            {
                request.Download(stream);

                string path = io.Path.Combine(sa.ProfileFolder, metadata.Name);

                io.File.WriteAllBytes(path, stream.ToArray());
            }
        }

        private void Request_ProgressChanged(IUploadProgress prg)
        {
            if (prg.Exception != null)
            {
                throw new Exception("Drive Interaction Failed", prg.Exception);
            }
        }

        private io.MemoryStream GetMemoryStream(string xml)
        {
            byte[] data = Encoding.UTF8.GetBytes(xml);

            return new io.MemoryStream(data);
        }

        private File GetMetaData(string name, bool creating)
        {
            File file = new File() { Name = name };

            if (creating)
            {
                file.Parents = new List<string> { "appDataFolder" };
            }

            return file;
        }

        private DriveService CreateDriveService(ISecurityActor sa)
        {
            UserCredential creds = CreateCredentials(sa);

            BaseClientService.Initializer initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = creds,
                ApplicationName = "vyger"
            };

            DriveService ds = new DriveService(initializer);

            return ds;
        }

        private UserCredential CreateCredentials(ISecurityActor sa)
        {
            string[] scopes = new[]
            {
                DriveService.Scope.DriveAppdata
            };

            TokenResponse token = new TokenResponse()
            {
                AccessToken = sa.AccessToken,
                ExpiresInSeconds = 3600,
                IssuedUtc = DateTime.UtcNow
            };

            GoogleAuthorizationCodeFlow.Initializer initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = ClientId,
                    ClientSecret = ClientSecret
                }
            };

            GoogleAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(initializer);

            UserCredential creds = new UserCredential(flow, sa.Email, token);

            return creds;
        }

        public string[] GetScopesRequired()
        {
            return new[]
            {
                "email",
                "https://www.googleapis.com/auth/plus.me",
                //"https://www.googleapis.com/auth/drive.metadata.readonly",
                "https://www.googleapis.com/auth/drive.appdata"
            };
        }

        #endregion
    }
}
