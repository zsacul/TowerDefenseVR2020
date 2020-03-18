using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public Transform cam;
    
    void Update() {
        transform.LookAt(transform.position + cam.forward);
    }
}
