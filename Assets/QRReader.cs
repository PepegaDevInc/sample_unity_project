using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QRReader : MonoBehaviour
{
    public GameObject QRContentField;
    public RawImage background;
    private string QRStarterText;
    // Start is called before the first frame update
    void Start()
    {
        QRStarterText = QRContentField.GetComponent<Text>().text;
    }

    // Update is called once per frame
    void Update()
    {

        IBarcodeReader barcodeReader = new BarcodeReader();
        // decode the current frame
        Texture2D rawImageTexture = TexturetoTexture2D(background.texture);
        Result result = barcodeReader.Decode(rawImageTexture.GetPixels32(), background.texture.width, background.texture.height);
        if (result != null)
        {
            QRContentField.GetComponent<Text>().text = result.Text;
        }
        else
        {
            QRContentField.GetComponent<Text>().text = QRStarterText;
        }
    }

    public Texture2D TexturetoTexture2D(Texture rTex)
    {
        Texture2D dest = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        dest.Apply(false);
        Graphics.CopyTexture(rTex, dest);
        return dest;
    }
}
