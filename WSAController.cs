using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[System.Serializable]
public class UnityRectangle {

    public float    centerX;
    public float    centerY;
    public float    height;
    public bool     inQE;
    public bool     inQI;
    public float    width;
}

[System.Serializable]
public class UnityRectangleCollection {
    public UnityRectangle[] rects;
}

public class WSAController {
    public static UnityRectangle GenerateFromJSONstring(string jsonString) {
        return JsonUtility.FromJson<UnityRectangle>(jsonString);
    }

    public static UnityRectangleCollection GenerateFromJSONfile(string resourcesPath) {
        TextAsset asset = Resources.Load<TextAsset>(resourcesPath);
        return JsonUtility.FromJson<UnityRectangleCollection>(asset.text);
    }

    public static UnityRectangleCollection TestGenerateFromJSON() {
        TextAsset asset = Resources.Load<TextAsset>("Rectangle Data\\test_rect_data");
        return JsonUtility.FromJson<UnityRectangleCollection>(asset.text);
    }
}
