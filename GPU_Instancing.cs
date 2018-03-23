using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPU_Instancing : MonoBehaviour {

    [SerializeField]
    private bool cameraDepthByWidth = true;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Material WSA_external,
                     WSA_internal,
                     WSA_outofrange;

    [SerializeField]
    private GameObject rectObjectPrefab;

    private GameObject[] instancedRects;

    private float leftMostPoint = 0f,
                  rightMostPoint = 0f;
    private float topMostPoint = 0f;

    void Start() {
        UnityRectangleCollection rects
            = WSAController.GenerateFromJSON("Rectangle Data\\rectangle_data_d=0.001");
        instancedRects = new GameObject[rects.rects.Length];

        for (int i = 0; i < rects.rects.Length; i++) {
            UnityRectangle curRect = rects.rects[i];
            GameObject rectangle = Instantiate(rectObjectPrefab, this.transform);
            Vector4 pos = new Vector2(curRect.centerX, curRect.centerY);
            Vector2 scale = new Vector2(curRect.width, curRect.height);

            // leftmost & rightmost points calculation 
            // with assumption that given prefab is exactly 1x1
            float rectLeftMostPoint = pos.x - scale.x / 2f;
            float rectRightMostPoint = pos.x + scale.x / 2f;
            if(rectLeftMostPoint < leftMostPoint) {
                leftMostPoint = rectLeftMostPoint;
            }
            // works only with number of rectangles 2 and more
            else if(rectRightMostPoint > rightMostPoint) {
                rightMostPoint = rectRightMostPoint;
            }

            // topmost point calculation 
            // with assumption that given prefab is exactly 1x1
            float rectTopMostPoint = pos.y + scale.y / 2f;
            if(rectTopMostPoint > topMostPoint) {
                topMostPoint = rectTopMostPoint;
            }

            Material mat;
            if(curRect.inQE && curRect.inQI) {
                // internal
                mat = WSA_internal;
            }
            else if(curRect.inQE && !curRect.inQI) {
                // external
                mat = WSA_external;
            }
            else {
                // out of range
                mat = WSA_outofrange;
            }

            rectangle.transform.localPosition = pos;
            rectangle.transform.localScale = scale;
            rectangle.GetComponent<MeshRenderer>().material = mat;
        }

        SetCameraPosition();
	}

    private void SetCameraPosition() {
        // getting center of the plot
        Bounds xBounds = new Bounds();
        xBounds.Encapsulate(new Vector3(leftMostPoint, 0, 0));
        xBounds.Encapsulate(new Vector3(rightMostPoint, 0, 0));

        float cameraDepth =
            cameraDepthByWidth ? 
            -CalculateCamDepthByWidth(xBounds.size.x) : 
            -CalculateCamDepthByHeight(topMostPoint);

        mainCamera.transform.position =
            new Vector3(
                xBounds.center.x,
                topMostPoint / 2,
                cameraDepth
            );;
    }

    private float CalculateCamDepthByHeight(float desiredHeight) {
        float thisZDepth = this.transform.position.z + Mathf.Abs(mainCamera.transform.position.z);
        float vFOV = mainCamera.fieldOfView * Mathf.PI / 180;

        return (desiredHeight / (2 * Mathf.Tan(vFOV / 2))) - this.transform.position.z;
    }

    private float CalculateCamDepthByWidth(float desiredWidth) {
        return CalculateCamDepthByHeight(desiredWidth / mainCamera.aspect);
    }

    private float VisibleHeight() {
        float thisZDepth = this.transform.position.z + Mathf.Abs(mainCamera.transform.position.z);
        float vFOV = mainCamera.fieldOfView * Mathf.PI / 180;

        return 2 * Mathf.Tan(vFOV / 2) * thisZDepth;
    }

    private float VisibleWidth() {
        float height = VisibleHeight();
        return height * mainCamera.aspect;
    } 
}
