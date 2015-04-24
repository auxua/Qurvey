using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading;

using Newtonsoft.Json;
using Qurvey.api.DataModel;

namespace Qurvey.api
{
    class AuthenticationManager
    {
        # region general Authentication Managemant

        public enum AuthenticationState
        {
            NONE, // Not Authenticated, no Refresh Token known
            ACTIVE, // Not necessarily authenticated at the moment, but can re-authenticate by getting an access Token using the refresh token or holding valid token at the moment
            WAITING // There is an ongoing Authentication process (e.g. a user needs to confirm authorization in the browser
        }

        private static AuthenticationState State = AuthenticationState.NONE;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static AuthenticationState getState()
        {
            return State;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void setState(AuthenticationState authState)
        {
            State = authState;
        }

        /// <summary>
        /// Forces the Manager to refresh the State and tries to regenerate an accessToken if possible
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void CheckState()
        {
            if (State == AuthenticationState.WAITING)
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
                    GenerateAccessTokenFromRefreshToken();
                    return;
                }
            }

            if (Config.getRefreshToken() == "")
            {
                // No refreshtoken, but holding an Access Token - Check Token
                CheckAccessToken();
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

        /// <summary>
        /// Checks the AccessToken against the tokenInfo REST-Service.
        /// If it fails, the accessToken is removed automatically
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void CheckAccessToken()
        {
            string call = "{ \"client_id:\" \""+Config.ClientID+"\" \"access_token\": \""+Config.getAccessToken()+"\" }";

            var answer = RESTCalls.RestCall<OAuthTokenInfo>(call, Config.OAuthTokenInfoEndPoint, true);
            
            if (answer.state != "valid")
            {
                // Invalid Token - delete it
                Config.setAccessToken("");
            }
        }

        # endregion

        # region Authorization Process

        /// <summary>
        /// Starts the Autehntication Process
        /// </summary>
        /// <returns>returns the verifaication URL for this app or an empty string on fails</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string StartAuthenticationProcess()
        {
            var answer = OAuthInitCall();
            if (answer.status == "ok")
            {
                // call was successfull!
                string url = answer.verification_url + "?q=verify&d=" + answer.user_code;
                Config.setDeviceToken(answer.device_code);
                setState(AuthenticationState.WAITING);
                Thread t = new Thread(new ThreadStart(ExpireThread));
                t.Start();
                return url;
            }
            // Failed
            return "";
        }

        private static int expireTimeWaitingProcess;

        /// <summary>
        /// Checks whether the users has already authenticated the app (Part of the Authentication process!)
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool CheckAuthenticationProgress()
        {
            var answer = TokenCall();
            if (answer.status.StartsWith("Fail:") || answer.status.StartsWith("Error:"))
            {
                // Not working!
                return false;
            }
            // working!
            // Store the tokens
            Config.setAccessToken(answer.access_token);
            Config.setRefreshToken(answer.refresh_token);
            setState(AuthenticationState.ACTIVE);
            return true;
        }

        /// <summary>
        /// Initiates the Authorization process
        /// </summary>
        /// <returns>The answer on the Initial Call to Endpoint or an empty answer if something went wrong! (having a status field Fail: (Error message) )</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static OAuthRequestData OAuthInitCall()
        {
            string parsedContent = "{ \"client_id\": \"" + Config.ClientID + "\", \"scope\": \"l2p.rwth campus.rwth l2p2013.rwth\" }";
            var answer = RESTCalls.RestCall<OAuthRequestData>(parsedContent, Config.OAuthEndPoint, true);
            return answer;
        }

        /// <summary>
        /// Calls the /token Endpoint to check status of authorization process
        /// </summary>
        /// <returns>The answer to the Call or an artificial answer containing an Error Descripstion in the status-field</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static OAuthTokenRequestData TokenCall()
        {
            try
            {
                var req = new OAuthTokenRequestSendData();
                req.client_id = api.Config.ClientID;
                req.code = Config.getDeviceToken();
                req.grant_type = "device";

                string postData = JsonConvert.SerializeObject(req);

                var answer = RESTCalls.RestCall<OAuthTokenRequestData>(postData, Config.OAuthTokenEndPoint, true);
                return answer;
            }
            catch (Exception e)
            {
                var t = e.Message;
                var pseudo = new OAuthTokenRequestData();
                pseudo.status = "Fail: " + t;
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


        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool GenerateAccessTokenFromRefreshToken()
        {
            string callBody = "{ \"client_id\": \"" + Config.ClientID + "\", \"refresh_token\": \"" + Config.getRefreshToken() + "\", \"grant_type\":\"refresh_token\" }";
            var answer = RESTCalls.RestCall<OAuthTokenRequestData>(callBody, Config.OAuthTokenEndPoint, true);
            if ((answer.error == null))
            {
                //Console.WriteLine("Got a new Token!");
                Config.setAccessToken(answer.access_token);
                TokenExpireTime = answer.expires_in;
                //TokenExpireTime = 10;
                Refresher = new Thread(new ThreadStart(TokenRefresherThread));
                Refresher.Start();
                return true;
            }
            // Failed!
            return false;
        }

        private static Thread Refresher = null;
        private static int TokenExpireTime;
        
        private static void TokenRefresherThread()
        {
            //Console.WriteLine("Startet Refresh-Thread");
            Thread.Sleep(TokenExpireTime*1000);
            //Console.WriteLine("Refreshing!");
            GenerateAccessTokenFromRefreshToken();
        }


        

    }
}
