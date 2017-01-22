using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class CameraFollow : MonoBehaviour
    {
		public float zoom = 10;
        public Transform target;            // The position that that camera will be following.
        public float smoothing = 5f;        // The speed with which the camera will be following.
		public float camHeigth = 10;

        Vector3 offset;                     // The initial offset from the target.


        void Start ()
        {
            // Calculate the initial offset.
//            offset = transform.position - target.position;
			//offset = target.position.normalized * zoom ;
        }


        void FixedUpdate ()
        {
			offset = target.position.normalized * zoom;
            // Create a postion the camera is aiming for based on the offset from the target.
			Vector3 targetCamPos = target.position + offset + new Vector3(0,camHeigth,0);

            // Smoothly interpolate between the camera's current position and it's target position.
            transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
        }

		void Update()
		{
			zoom -= Input.mouseScrollDelta.y;
			camHeigth -= Input.mouseScrollDelta.y / 3f;
			//Vector3 camStartRot = transform.rotation.eulerAngles;
//			Quaternion.LookRotation(transform.forward, transform.up);
//
//			Vector3 rot = Quaternion.FromToRotation(transform.position, Vector3.zero).eulerAngles;
//
//			transform.rotation = 
			Quaternion lastLook = transform.rotation;
			transform.LookAt(target.position);

			transform.rotation = Quaternion.Lerp (lastLook, transform.rotation, smoothing * Time.deltaTime);
			//transform.localRotation = Quaternion.Euler(new Vector3(camStartRot.x, transform.rotation.y, camStartRot.z)); 
		}
    }
}