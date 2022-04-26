using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotter : MonoBehaviour
{

    public string prefix;
    public int counter;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            ScreenCapture.CaptureScreenshot("/Users/yodalemu/YodaheCantCode/Unity Projects/Hopper/Screenshots/" + prefix + "_" + counter + ".png");
            print("/Users/yodalemu/YodaheCantCode/Unity Projects/Hoppor/Screenshots/" + prefix + "_" + counter);
            counter++;
        }
    }
}
