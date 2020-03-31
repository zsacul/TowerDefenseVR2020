using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update() {
        transform.LookAt(cam.transform.position);
    }
}
