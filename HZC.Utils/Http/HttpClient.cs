using System;
using System.IO;
using System.Net;
using System.Text;

public static class HttpClient
{
    #region Post请求
    public static string HttpPost(string Url, string postDataStr)
    {
        CookieContainer cookie = new CookieContainer();
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        //request.ContentType = "text/html;charset=UTF-8";
        request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
        request.CookieContainer = cookie;
        Stream myRequestStream = request.GetRequestStream();
        //StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
        StreamWriter myStreamWriter = new StreamWriter(myRequestStream);
        myStreamWriter.Write(postDataStr);
        myStreamWriter.Close();

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        response.Cookies = cookie.GetCookies(response.ResponseUri);
        Stream myResponseStream = response.GetResponseStream();
        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
        string retString = myStreamReader.ReadToEnd();
        myStreamReader.Close();
        myResponseStream.Close();

        return retString;
    }
    #endregion

    #region Get请求
    public static string HttpGet(string Url, string postDataStr)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
        request.Method = "GET";
        request.ContentType = "text/html;charset=UTF-8";

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream myResponseStream = response.GetResponseStream();
        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
        string retString = myStreamReader.ReadToEnd();
        myStreamReader.Close();
        myResponseStream.Close();

        return retString;
    }
    #endregion
}
