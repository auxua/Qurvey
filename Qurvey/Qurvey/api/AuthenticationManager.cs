using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Qurvey.api.DataModel;

using Xamarin.Forms;

namespace Qurvey.api
{
    class AuthenticationManager
    {
        # region general Authentication Managemant

        public enum AuthenticationState
        {
            NONE = 0, // Not Authenticated, no Refresh Token known
            ACTIVE = 1, // Not necessarily authenticated at the moment, but can re-authenticate by getting an access Token using the refresh token or holding valid token at the moment
            WAITING = 2 // There is an ongoing Authentication process (e.g. a user needs to confirm authorization in the browser
        }

        //private static AuthenticationState State = AuthenticationState.NONE;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static AuthenticationState getState()
        {
            // Store the State independent from App Lifecycle
            if (Application.Current.Properties.ContainsKey("AuthState"))
            {
                return (AuthenticationState)Enum.Parse(typeof(AuthenticationState), (string)Application.Current.Properties["AuthState"]);
                //return (AuthenticationState)Application.Current.Properties["AuthState"];
            }
            else
            {
                setState(AuthenticationState.NONE);
                return AuthenticationState.NONE;
            }
            //return State;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void setState(AuthenticationState authState)
        {
            Application.Current.Properties["AuthState"] = authState.ToString("G");
            Application.Current.SavePropertiesAsync();
            //State = authState;
        }

        /// <summary>
        /// Forces the Manager to refresh the State and tries to regenerate an accessToken if possible
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void CheckState()
        {
            if (getState() == AuthenticationState.WAITING)
            {
                // Authentication Process ongoing - do nothing and wait
                return;
            }
            
            if (Config.getAccessToken() == "")
            {
                if (Config.getRefreshToken() == "")
                {
                    // No access or refresh Token available!
                    setState(AuthenticationState.NONE);
                    return;
                }
                else 
                {
                    // No Access Token, but refreshToken
                    GenerateAccessTokenFromRefreshTokenAsync();
                    return;
                }
            }

            if (Config.getRefreshToken() == "")
            {
                // No refreshtoken, but holding an Access Token - Check Token
                CheckAccessTokenAsync();
                if (Config.getAccessToken() == "")
                {
                    // Holding a valid AccesToken now
                    setState(AuthenticationState.ACTIVE);
                    return;
                }
                else
                {
                    // Validation failed - no tokens!
                    setState(AuthenticationState.NONE);
                }
            }

        }


        private static Mutex CheckAccessTokenMutex = new Mutex();
        /// <summary>
        /// Checks the AccessToken against the tokenInfo REST-Service.
        /// If it fails, the accessToken is removed automatically
        /// </summary>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        private async static void CheckAccessTokenAsync()
        {
            // use mutex for sync
            CheckAccessTokenMutex.WaitOne();
            string call = "{ \"client_id:\" \""+Config.ClientID+"\" \"access_token\": \""+Config.getAccessToken()+"\" }";

            var answer = await RESTCalls.RestCallAsync<OAuthTokenInfo>(call, Config.OAuthTokenInfoEndPoint, true);
            
            if (answer.state != "valid")
            {
                // Invalid Token - delete it
                Config.setAccessToken("");
            }
            CheckAccessTokenMutex.ReleaseMutex();
        }

        # endregion

        # region Authorization Process

        private static Mutex StartAuthMutex = new Mutex();

        /// <summary>
        /// Starts the Autehntication Process
        /// </summary>
        /// <returns>returns the verification URL for this app or an empty string on fails</returns>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        public async static Task<string> StartAuthenticationProcessAsync()
        {
            StartAuthMutex.WaitOne();
            var answer = await OAuthInitCallAsync();
            if (answer.status == "ok")
            {
                // call was successfull!
                string url = answer.verification_url + "?q=verify&d=" + answer.user_code;
                Config.setDeviceToken(answer.device_code);
                setState(AuthenticationState.WAITING);
                Thread t = new Thread(new ThreadStart(ExpireThread));
                t.Start();
                StartAuthMutex.ReleaseMutex();
                return url;
            }
            // Failed
            StartAuthMutex.ReleaseMutex();
            return "";
        }

        private static int expireTimeWaitingProcess;

        private static Mutex CheckAuthMutex = new Mutex();

        /// <summary>
        /// Checks whether the users has already authenticated the app (Part of the Authentication process!)
        /// </summary>
        /// <returns></returns>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        public async static Task<bool> CheckAuthenticationProgressAsync()
        {
            CheckAuthMutex.WaitOne();
            var answer = await TokenCallAsync();
            if (answer.status.StartsWith("Fail:") || answer.status.StartsWith("error:"))
            {
                // Not working!
                return false;
            }
            // working!
            // Store the tokens
            Config.setAccessToken(answer.access_token);
            Config.setRefreshToken(answer.refresh_token);
            setState(AuthenticationState.ACTIVE);
            CheckAuthMutex.ReleaseMutex();
            return true;
        }

        private static Mutex InitAuthMutex = new Mutex();

        /// <summary>
        /// Initiates the Authorization process
        /// </summary>
        /// <returns>The answer on the Initial Call to Endpoint or an empty answer if something went wrong! (having a status field Fail: (Error message) )</returns>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        internal async static Task<OAuthRequestData> OAuthInitCallAsync()
        {
            InitAuthMutex.WaitOne();
            string parsedContent = "{ \"client_id\": \"" + Config.ClientID + "\", \"scope\": \"l2p.rwth campus.rwth l2p2013.rwth\" }";
            var answer = await RESTCalls.RestCallAsync<OAuthRequestData>(parsedContent, Config.OAuthEndPoint, true);
            InitAuthMutex.ReleaseMutex();
            return answer;
        }


        private static Mutex TokenCallMutex = new Mutex();

        /// <summary>
        /// Calls the /token Endpoint to check status of authorization process
        /// </summary>
        /// <returns>The answer to the Call or an artificial answer containing an Error Descripstion in the status-field</returns>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        private async static Task<OAuthTokenRequestData> TokenCallAsync()
        {
            TokenCallMutex.WaitOne();
            try
            {
                var req = new OAuthTokenRequestSendData();
                req.client_id = api.Config.ClientID;
                req.code = Config.getDeviceToken();
                req.grant_type = "device";

                string postData = JsonConvert.SerializeObject(req);

                var answer = await RESTCalls.RestCallAsync<OAuthTokenRequestData>(postData, Config.OAuthTokenEndPoint, true);
                TokenCallMutex.ReleaseMutex();
                return answer;
            }
            catch (Exception e)
            {
                var t = e.Message;
                var pseudo = new OAuthTokenRequestData();
                pseudo.status = "Fail: " + t;
                TokenCallMutex.ReleaseMutex();
                return pseudo;
            }
        }


        /// <summary>
        /// A sinple method that will reset the State to NONE after (expireTime) seconds
        /// </summary>
        private static void ExpireThread()
        {
            // Wait for the expire time
            Thread.Sleep(expireTimeWaitingProcess * 1000);
            // wake up and check, whether the process has expired
            setState(AuthenticationState.NONE);
        }

        # endregion


        private static Mutex RefreshhMutex = new Mutex();

        /// <summary>
        /// Uses the current RefreshToken to get an Access Token
        /// </summary>
        /// <returns>true, if the call was successfull</returns>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        public async static Task<bool> GenerateAccessTokenFromRefreshTokenAsync()
        {
            RefreshhMutex.WaitOne();
            string callBody = "{ \"client_id\": \"" + Config.ClientID + "\", \"refresh_token\": \"" + Config.getRefreshToken() + "\", \"grant_type\":\"refresh_token\" }";
            var answer = await RESTCalls.RestCallAsync<OAuthTokenRequestData>(callBody, Config.OAuthTokenEndPoint, true);
            if ((answer.error == null) && (!answer.status.StartsWith("error")) && (answer.expires_in>0))
            {
                //Console.WriteLine("Got a new Token!");
                Config.setAccessToken(answer.access_token);
                TokenExpireTime = answer.expires_in;
                //TokenExpireTime = 10;
                Refresher = new Thread(new ThreadStart(TokenRefresherThread));
                Refresher.Start();
                RefreshhMutex.ReleaseMutex();
                return true;
            }
            // Failed!
            RefreshhMutex.ReleaseMutex();
            return false;
        }

        private static Thread Refresher = null;
        private static int TokenExpireTime;
        
        private static void TokenRefresherThread()
        {
            //Console.WriteLine("Startet Refresh-Thread");
            Thread.Sleep(TokenExpireTime*1000);
            //Console.WriteLine("Refreshing!");
            GenerateAccessTokenFromRefreshTokenAsync();
        }


        

    }
}
