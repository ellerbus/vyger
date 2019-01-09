using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.Hosting;
using Augment;
using EnsureThat;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using vyger.Core.Models;
using io = System.IO;

namespace vyger.Core.Repositories
{
    public class GoogleDriveRepository
    {
        #region Members

        private static readonly string GoogleClientId = ConfigurationManager.AppSettings["Google.ClientId"];
        private static readonly string GoogleSecret = ConfigurationManager.AppSettings["Google.Secret"];

        private ISecurityActor _actor;

        #endregion

        #region Constructor

        public GoogleDriveRepository(ISecurityActor actor)
        {
            _actor = actor;
        }

        #endregion
    }
}