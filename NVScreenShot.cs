// Copyright (c) 2016 Unity Technologies. MIT license - license_unity.txt
// #NVJOB ScreenShot. MIT license - license_nvjob.txt
// #NVJOB Nicholas Veselov - https://nvjob.github.io
// #NVJOB ScreenShot v1.1.3 - https://nvjob.github.io/unity/screenshot


using UnityEngine;

[HelpURL("https://nvjob.github.io/unity/screenshot")]
[AddComponentMenu("#NVJOB/Tools/ScreenShot")]


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


public class NVScreenShot : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    // 4k = 3840 × 2160
    // 2k = 2560 × 1440

    [Header("Settings")]
    public Vector2Int resolution = new Vector2Int(2560, 1440);
    public string nameScreenshot = "ScreenShot";
    public int timeRepit = 1;

    [Header("Information")] // These variables are only information.
    public string Controls = "T - one screenshot, Y - repit screenshot, U - +1 second, J - -1 second";
    public string HelpURL = "nvjob.github.io/unity/screenshot";
    public string ReportAProblem = "nvjob.github.io/support";
    public string Patrons = "nvjob.github.io/patrons";

    //--------------

    Camera thisCamera;
    bool repit;
    float dellay0;
    static string nameScreenshotSt;
    static int numberShot;


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void Awake()
    {
        //--------------

        string filePath  = System.IO.Directory.GetCurrentDirectory() + "/screenshot";
        if (!System.IO.Directory.Exists(filePath)) System.IO.Directory.CreateDirectory(filePath);

        //--------------

        thisCamera = GetComponent<Camera>();
        nameScreenshotSt = nameScreenshot;
        numberShot = 0;

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void Update()
    {
        //--------------

        if (Input.GetKeyDown("t") == true) Screnn(thisCamera, resolution);

        //--------------

        if (Input.GetKeyDown("u") == true) timeRepit++;
        else if (Input.GetKeyDown("j") == true && timeRepit > 1) timeRepit--;

        //--------------

        if (Input.GetKeyDown("y") == true) repit = !repit;

        if (repit == true)
        {
            if (dellay0 >= timeRepit)
            {
                dellay0 = 0;
                Screnn(thisCamera, resolution);
            }
            else dellay0 += Time.deltaTime;
        }

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    static void Screnn(Camera thisCamera, Vector2Int resolution)
    {        
        //--------------

        RenderTexture shot = new RenderTexture(resolution.x, resolution.y, 24);
        thisCamera.targetTexture = shot;
        Texture2D screenShot = new Texture2D(resolution.x, resolution.y, TextureFormat.RGB24, false);
        thisCamera.Render();
        RenderTexture.active = shot;
        screenShot.ReadPixels(new Rect(0, 0, resolution.x, resolution.y), 0, 0);
        RenderTexture.active = thisCamera.targetTexture = null;
        Destroy(shot);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = string.Format("{0}/screenshot/{1}_{2}.png", System.IO.Directory.GetCurrentDirectory(), nameScreenshotSt, numberShot++);
        System.IO.File.WriteAllBytes(filename, bytes);

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
