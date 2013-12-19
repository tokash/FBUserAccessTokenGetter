using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Facebook;
using FBUserAccessTokenGetter.DataClasses;
using System.IO;
using System.Collections.Specialized;
using System.Configuration;

namespace FBUserAccessTokenGetter.Controllers
{
    public class AccountController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);

                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");

                return uriBuilder.Uri;
            }
        }

        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "630904423618987",
                client_secret = "485456384f450bde62bc9eacbfc2c316",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();

            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "630904423618987",
                client_secret = "485456384f450bde62bc9eacbfc2c316",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            DateTime expiryTime = DateTime.Now.AddSeconds(result.expires);

            FaceBookToken facebookToken = new FaceBookToken() { access_token = result.access_token, expires = expiryTime };

            var configurationValues = (NameValueCollection)ConfigurationManager.GetSection("Data");
            string accessTokenFilename = configurationValues["AccessTokenFileName"];

            string path = Path.GetTempPath() + accessTokenFilename;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                file.WriteLine(facebookToken.access_token);
                file.WriteLine(facebookToken.expires);
            }

            //var accessToken = result.access_token;

            //// Store the access token in the session
            //Session["AccessToken"] = accessToken;

            //// update the facebook client with the access token so 
            //// we can make requests on behalf of the user
            //fb.AccessToken = accessToken;


            
            return RedirectToAction("AfterLogin", "Home");
        }

        public ActionResult Login()
        {
            return View();
        }

    }
}
