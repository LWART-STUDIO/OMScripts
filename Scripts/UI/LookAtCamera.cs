using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        transform.LookAt(transform.position + mainCamera.transform.rotation * -Vector3.back, mainCamera.transform.rotation * -Vector3.down);
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * -Vector3.back, mainCamera.transform.rotation * -Vector3.down);
        //transform.LookAt(mainCamera.transform.position, mainCamera.transform.rotation * -Vector3.down);
    }
}
