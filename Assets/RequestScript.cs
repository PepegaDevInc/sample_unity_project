using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RequestScript : MonoBehaviour
{
    public GameObject ResponseStatusField;
    public Button GetResponceButton;
    public GameObject ResponseContentField;

    void Start()
    {
        Button btn = GetResponceButton.GetComponent<Button>();
        btn.onClick.AddListener(SendRequest);
    }

    // Performs simple request
    public void SendRequest()
    {
        WebRequest request = WebRequest.Create("http://91.122.34.182/get_system/test_system/");
        request.Credentials = CredentialCache.DefaultCredentials;
        WebResponse response = request.GetResponse();
        //Fetching status
        ResponseStatusField.GetComponent<Text>().text = "Status: " + ((HttpWebResponse)response).StatusDescription;
        //Fetching responce body using Stream
        Stream receiveStream = response.GetResponseStream();
        Encoding encode = Encoding.GetEncoding("utf-8");
        // Pipes the stream to a higher level stream reader with the required encoding format.
        StreamReader readStream = new StreamReader(receiveStream, encode);
        ResponseContentField.GetComponent<Text>().text = "";
        while (readStream.Peek() >= 0)
        {
            string line = readStream.ReadLine();
            ResponseContentField.GetComponent<Text>().text = ResponseContentField.GetComponent<Text>().text + JsonUtil.Beautify(line);
        }
        ResponseContentField.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
        response.Close();
        readStream.Close();
    }

    public static class JsonUtil
    {
        public static string Beautify(string json)
        {
            const int indentWidth = 4;
            const string pattern = "(?>([{\\[][}\\]],?)|([{\\[])|([}\\]],?)|([^{}:]+:)([^{}\\[\\],]*(?>([{\\[])|,)?)|([^{}\\[\\],]+,?))";

            var match = Regex.Match(json, pattern);
            var beautified = new StringBuilder();
            var indent = 0;
            while (match.Success)
            {
                if (match.Groups[3].Length > 0)
                    indent--;

                beautified.AppendLine(
                    new string(' ', indent * indentWidth) +
                    (match.Groups[4].Length > 0
                        ? match.Groups[4].Value + " " + match.Groups[5].Value
                        : (match.Groups[7].Length > 0 ? match.Groups[7].Value : match.Value))
                );

                if (match.Groups[2].Length > 0 || match.Groups[6].Length > 0)
                    indent++;

                match = match.NextMatch();
            }

            return beautified.ToString();
        }
    }
}
