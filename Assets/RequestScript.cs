using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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
        WebRequest request = WebRequest.Create("https://httpbin.org/anything");
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
            ResponseContentField.GetComponent<Text>().text = ResponseContentField.GetComponent<Text>().text + line + "\n";
        }
        ResponseContentField.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
        response.Close();
        readStream.Close();
    }
}
