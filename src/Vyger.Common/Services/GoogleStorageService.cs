using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Augment.Caching;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Newtonsoft.Json;
using Vyger.Common.Configuration;
using io = System.IO;

namespace Vyger.Common.Services
{
    #region Interface

    public interface IGoogleStorageService
    {
        /// <summary>
        ///
        /// </summary>
        T GetContents<T>(string name);

        /// <summary>
        ///
        /// </summary>
        void StoreContents<T>(string name, T data);
    }

    #endregion

    public partial class GoogleStorageService : IGoogleStorageService
    {
        #region Members

        private ClaimsPrincipal _principal;
        private IVygerConfiguration _config;
        private ICacheManager _cache;

        #endregion

        #region Constructors

        public GoogleStorageService(
            ClaimsPrincipal principal,
            IVygerConfiguration config,
            ICacheManager cache)
        {
            _principal = principal;
            _config = config;
            _cache = cache;
        }

        #endregion

        #region Drive Methods

        public void StoreContents<T>(string name, T data)
        {
            string json = JsonConvert.SerializeObject(data);

            File file = SearchForMetadata(name);

            if (file == null)
            {
                File metadata = CreateMetaData(name, true);

                CreateStorage(metadata, json);
            }
            else
            {
                File metadata = CreateMetaData(name, false);

                UpdateStorage(metadata, file.Id, json);
            }
        }

        public T GetContents<T>(string name)
        {
            File file = SearchForMetadata(name);

            if (file == null)
            {
                return default(T);
            }

            string json = DownloadStorage(file.Id);

            return JsonConvert.DeserializeObject<T>(json);
        }

        //private void DownloadContents()
        //{
        //    IList<File> files = GetStorageFiles();

        //    foreach (File file in files)
        //    {
        //        string path = io.Path.Combine(sa.ProfileFolder, file.Name);

        //        io.FileInfo fileInfo = new io.FileInfo(path);

        //        if (!fileInfo.Exists || file.ModifiedTime > fileInfo.LastWriteTimeUtc)
        //        {
        //            File metadata = GetMetaData(file.Name, false);

        //            //  then we need the latest
        //            DownloadStorage(metadata, file.Id);
        //        }
        //    }
        //}

        //private IList<File> GetStorageFiles()
        //{
        //    DriveService ds = CreateDriveService();

        //    FilesResource.ListRequest request = ds.Files.List();

        //    request.Spaces = "appDataFolder";

        //    request.Fields = "files(id, modifiedTime, name)";

        //    FileList files = request.Execute();

        //    return files.Files;
        //}

        private void CreateStorage(File file, string json)
        {
            using (io.MemoryStream contents = GetMemoryStream(json))
            {
                DriveService ds = CreateDriveService();

                FilesResource.CreateMediaUpload request = ds.Files.Create(file, contents, "application/json");

                request.ProgressChanged += Request_ProgressChanged;

                request.Fields = "id";

                request.Upload();
            }
        }

        private void UpdateStorage(File file, string id, string json)
        {
            using (io.MemoryStream contents = GetMemoryStream(json))
            {
                DriveService ds = CreateDriveService();

                FilesResource.UpdateMediaUpload request = ds.Files.Update(file, id, contents, "application/json");

                request.ProgressChanged += Request_ProgressChanged;

                request.Upload();
            }
        }

        private string DownloadStorage(string id)
        {
            DriveService ds = CreateDriveService();

            FilesResource.GetRequest request = ds.Files.Get(id);

            using (io.MemoryStream contents = new io.MemoryStream())
            {
                request.Download(contents);

                return Encoding.Default.GetString(contents.ToArray());
            }
        }

        private void Request_ProgressChanged(IUploadProgress prg)
        {
            if (prg.Exception != null)
            {
                throw new Exception("Drive Interaction Failed", prg.Exception);
            }
        }

        private io.MemoryStream GetMemoryStream(string json)
        {
            byte[] data = Encoding.UTF8.GetBytes(json);

            return new io.MemoryStream(data);
        }

        private File CreateMetaData(string name, bool creating)
        {
            File file = new File() { Name = name };

            if (creating)
            {
                file.Parents = new List<string> { "appDataFolder" };
            }

            return file;
        }

        private File SearchForMetadata(string name)
        {
            DriveService ds = CreateDriveService();

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

        private DriveService CreateDriveService()
        {
            UserCredential creds = CreateCredentials();

            BaseClientService.Initializer initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = creds,
                ApplicationName = "vyger"
            };

            DriveService ds = new DriveService(initializer);

            return ds;
        }

        private UserCredential CreateCredentials()
        {
            string[] scopes = new[]
            {
                DriveService.Scope.DriveAppdata
            };

            Claim tokenClaim = _principal.FindFirst(Constants.GoogleTokenClaim);
            Claim emailClaim = _principal.FindFirst(ClaimTypes.Email);

            TokenResponse token = new TokenResponse()
            {
                AccessToken = tokenClaim.Value,
                ExpiresInSeconds = 3600,
                IssuedUtc = DateTime.UtcNow
            };

            GoogleAuthorizationCodeFlow.Initializer initializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _config.Identity.GoogleClientId,
                    ClientSecret = _config.Identity.GoogleSecret
                }
            };

            GoogleAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(initializer);

            UserCredential creds = new UserCredential(flow, emailClaim.Value, token);

            return creds;
        }

        #endregion
    }
}