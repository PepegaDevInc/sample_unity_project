using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class RequestScript : MonoBehaviour
{
    public GameObject TextField;
    public Button GetResponceButton;

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
        TextField.GetComponent<Text>().text = ((HttpWebResponse)response).StatusDescription;
    }
}
