using System;
using System.Collections.Generic;
using System.Text;

namespace Qurvey.api
{
    class Config
    {
        protected internal const string OAuthEndPoint = "https://oauth.campus.rwth-aachen.de/oauth2waitress/oauth2.svc/code";

        protected internal const string ClientID = "";

        protected internal const string L2PEndPoint = "https://www3.elearning.rwth-aachen.de/_vti_bin/l2pservices/api.svc/v1/";
        
        protected internal const string OAuthTokenEndPoint = "https://oauth.campus.rwth-aachen.de/oauth2waitress/oauth2.svc/token";
        
        protected internal const string OAuthTokenInfoEndPoint = "https://oauth.campus.rwth-aachen.de/oauth2waitress/oauth2.svc/tokeninfo";
        
        //protected internal static string accessToken = "";
	
	//protected internal static string device_code = "";

	//protected internal static string refreshToken = "";

	#region Supporting functions for storing tokens

	internal static string getAccessToken()
	{
	    if (!Application.Current.Properties.ContainsKey("accesstoken"))
	    {
		Application.Current.Properties["accesstoken"] = "";
	    }
	    return (string)Application.Current.Properties["accesstoken"];
	}

	internal static void setAccessToken(string token)
	{
	    Application.Current.Properties["accesstoken"] = token;

	}

	internal static string getRefreshToken()
	{
	    if (!Application.Current.Properties.ContainsKey("refreshtoken"))
	    {
		Application.Current.Properties["refreshtoken"] = "";
	    }
	    return (string)Application.Current.Properties["refreshtoken"];
	}

	internal static void setRefreshToken(string token)
	{
	    Application.Current.Properties["refreshtoken"] = token;

	}

	internal static string getDeviceToken()
	{
	    if (!Application.Current.Properties.ContainsKey("devicetoken"))
	    {
		Application.Current.Properties["devicetoken"] = "";
	    }
	    return (string)Application.Current.Properties["devicetoken"];
	}

	internal static void setDeviceToken(string token)
	{
	    Application.Current.Properties["devicetoken"] = token;

        }

        #endregion
    }
}
