using UnityEngine;

public class PlayerScript : MonoBehaviour {
    private float oldVelocity;
    private float newVelocity;

    private float minVelocity = 0.0f;
    private float maxVelocity = 240.0f;

    private void Update() {
        //Apply force in an acceleration state on the object
        //Save its old velocity at the time of pressing forward. So it can lerp Towards its new velocity on the speedometer.
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            this.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1750, ForceMode.Acceleration);
            oldVelocity = this.GetComponent<Rigidbody>().linearVelocity.magnitude;
        }

        //New velocity, has to update constantly, to lerp from old velocity to fresh newVelocity.
        newVelocity = this.GetComponent<Rigidbody>().linearVelocity.magnitude;

        //Rotate towards the right
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            this.transform.Rotate(Vector3.down * Time.deltaTime * 100);
        }

        //Rotate towards the left.
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            this.transform.Rotate(Vector3.up * Time.deltaTime * 100);
        }

        //Cap its maximum speed to 240, so the speedometer wont glitch out of its position.
        newVelocity = Mathf.Clamp(newVelocity, minVelocity, maxVelocity);

        //Interpolate (smoothly) from oldVelocity towards newVelocity with t = 10.
        oldVelocity = Mathf.Lerp(oldVelocity, newVelocity, Time.deltaTime * 10.0f);
    }

    private void OnGUI() {
        GUI.Label(new Rect(115, 500, 100, 100), "Tap W to shoot forward, Hold A or D to rotate sideways");
    }

    public float OldVelocity { get { return oldVelocity; } }
}
