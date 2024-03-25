using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera Camera;

    private void Start()
    {
        Camera = GameObject.Find("PlayerCam").GetComponent<Camera>();
    }
    private void Update()
    {
        transform.LookAt(Camera.transform, Camera.transform.up);
        
        //if(transform.parent.parent.tag == "Enemy")
       
    }
}
