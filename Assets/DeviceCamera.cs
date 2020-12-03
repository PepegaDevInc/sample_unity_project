using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;
    private WebCamTexture frontCam;
    private WebCamTexture choosedCam;

    public RawImage background;
    public AspectRatioFitter fitter;

    private void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            camAvailable = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
            else
            {
                frontCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
        {
            frontCam.Play();
            background.texture = frontCam;
            choosedCam = frontCam;
        }
        else
        {
            backCam.Play();
            background.texture = backCam;
            choosedCam = backCam;
        }


        camAvailable = true;
    }

    private void Update()
    {
        if (!camAvailable)
            return;
        if (!choosedCam)
            return;
        float ratio = (float)choosedCam.width / (float)choosedCam.height;
        fitter.aspectRatio = ratio;

        float scaleY = choosedCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -choosedCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
}

