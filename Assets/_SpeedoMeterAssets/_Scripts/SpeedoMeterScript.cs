using UnityEngine;
//[ExecuteInEditMode()] 									//Execute this script in the editor, you dont have to play the game for it to update. (Note that it is best to play the game for best results).
public class SpeedoMeterScript : MonoBehaviour {
    public GUISkin mySkin;                                  //Custom GUI skin with custom settings.

    private int digitOne;                                   //Variable used to store the first digit of the velocity variable
    private int digitTwo;                                   //Variable used to store the second digit of the velocity variable
    private int digitThree;                                 //Variable used to store the third digit of the velocity variable.

    public Texture2D compassTexture = null;                 //Compass Texture.
    private Vector2 compassSize = new Vector2(287, 286);    //The x and y size of the image inserted.

    public Texture2D needleTexture = null;					//Needle Texture.
    public float angle = 0;									//Angle Instantiation. Leave this at 0 to avoid mistakes. Use the startingAngle variable below to adjust its angle if needed.
    private Vector2 needleSize = new Vector2(45, 141);      //The x and y size of the image inserted. 

    private Vector2 pos = new Vector2(0, 0);
                                                            //Leave these values as they are, change the Position of the GameObject this script is attached to in the two variables below.
    private float screenPosX = 0.15f;                       //Change this var to position the compass dynamically to 0.15 * the screen.Width. Because of multiplying it with the screen.Width, it works on each resolution.
    private float screenPosY = 0.90f;                       //Change this var to position the compass dunamically to 0.90 * the screen.Height. 

                                                             //(Just experiment a little with this setting in the Editor to get ahold of it! :) ).
    private Rect compassRect;								//Compass rectangle that is being drawn onscreen.
    private Rect needleRect;								//Needle rectangle that is being drawn onscreen.
    private Vector2 pivot;

    private float halfSize = 0.50f;                         //Just leave this here at 0.50. It is used to calculate the object position relative to the size of the object, and the position on the screen. 
                                                            //(So that the object is centered on its position).

    private float xCenterPos = 0.51f;                       //The center position of the GUI item on the x axis. (0.50 means its in the center of the object).
    private float yCenterPos = 0.83f;                       //The center position of the GUI item on the y axis. (0.85 means its near the bottom of the object, 1 is fully at the bottom).

    private float percentage = 100;                         //This is the percentage being used for a 100 / 100 * percentage calculation to get a 0.0x figure. Just leave this 100.

    private float startingAngle = -120;                        //This is the degree value where the angle will start counting from. 360/0 = Upwards, 180 is at the bottom.
    private float maximumDegrees = 240;                     //This is the MAXIMUM degree value the angle will rotate towards. 240 means, rotate towards a maximum of 240 degrees, starting from your startingAngle. (-120 + 240 = + 120).
                                                            //So in this example, the needle will rotate from -120 degrees, to a maximum of 240 added from the startingAngle. Which is + 120 degrees.
                                                            //The dynamic values inbetween are interpolated, so dont worry about the needle going over the edge.

    private float currentValue = 0;                       //This value changes dynamically, between the minimum and the cap.	(Change this to your own value, like Amount of fuel, Speed, Chickens gathered etc).
    private float capValue = 240;                           //This is the max value. The cap. (Change this to howmany fuel you can hold, the maximum speed you can travel or the maximum amount of chickens you can gather.

    private CarroManager playerScript;

    private void Start() {
        playerScript = FindObjectOfType(typeof(CarroManager)) as CarroManager;

        maximumDegrees *= 0.01f;        //Just leave this here. It changes the maximumDegree angle from a 120 degree digit towards a (120 * 0.01) = 1.2 digit, used to calculate the angle.
        UpdateSettings();				//Update the settings on startup.
    }

    private void UpdateSettings() {

        pos = GUIUtility.ScreenToGUIPoint(new Vector2(transform.localPosition.x, transform.localPosition.y));       //Change the editors position to that of the object in GUI Space in pixels
        pos = new Vector2(Screen.width * screenPosX, Screen.height * screenPosY);                                   //The position of the compass on screen. Dynamically when window is resized.

        needleRect = new Rect(pos.x - needleSize.x * halfSize, Screen.height - (pos.y - needleSize.y * halfSize), needleSize.x, needleSize.y);	//Create the Object at 0,0 position from the botton left. 
        pivot = new Vector2(needleRect.xMin + needleRect.width * xCenterPos, needleRect.yMin + needleRect.height * yCenterPos);                 //Calculation of the pivot point of the object. (Adjust values xCenterPos and yCenterPos).

        compassRect = new Rect(pos.x - compassSize.x * 0.50f, Screen.height - (pos.y - compassSize.y * 0.15f), compassSize.x, compassSize.y);   //Create the Object at 0,0 position from the bottom left. Play alittle with the (0.5 vars) in this example to allign the needle with the compass.
    }

    private void Update() {
        //this.angle = startingAngle + (((currentValue / startingValue) * percentage) * maximumDegrees);			//Calculation of the Angle in realtime. Replace the currentValue and startingValue with anything you like.

        currentValue = playerScript.RpmMotor();
        this.angle = startingAngle + (((currentValue / 12000) * percentage) * maximumDegrees);       //Example function, used in the playerScript. Needle smoothly lerps to its new angle based on the players velocity.	

        //Calculate each individual digit in the oldVelocity variable.
        //The reason I did this, is so that each individual digit can be placed, sized, colored differently when needed.
        //If all the digits were in a single row variable, all the digits will have the same size, color and static placement in a sequence.
        digitOne = (int)Mathf.Floor(playerScript.VelocidadeTotal() / (Mathf.Pow(10, 2)) % 10);
        digitTwo = (int)Mathf.Floor(playerScript.VelocidadeTotal() / (Mathf.Pow(10, 1)) % 10);
        digitThree = (int)Mathf.Floor(playerScript.VelocidadeTotal() / (Mathf.Pow(10, 0)) % 10);
    }

    private void OnGUI() {
        GUI.skin = mySkin;                                      //Custom skin assigned to the GUI.

        UpdateSettings();

        GUI.DrawTexture(compassRect, compassTexture);           //Draw the object texture on screen.

        Matrix4x4 matrixBackup = GUI.matrix;			        //Matrix calculation to put the object from 3D world space to screen space.
        GUIUtility.RotateAroundPivot(angle, pivot);		        //Rotate the object around a pivot with x angles. 
        GUI.DrawTexture(needleRect, needleTexture);		        //Draw the object texture on screen.
        GUI.matrix = matrixBackup;                              //Set the matrix 3D World space position to GUI 2D Position.

        //These beginning areas are used to keep the digital counter in the right place while moving the compass around. It calculates the position by deducting the compassSize off the Position, multiplying it by 0.15 of the screen width.
        //The same goes for height. Note that height is turned around (Screen.height - (pos.y - compass.y)) etc, because the camera projects the GUI field up side down.
        GUILayout.BeginArea(new Rect(pos.x - compassSize.x * 0.15f, Screen.height - (pos.y - compassSize.y * 0.8f), 50, 50));
        GUILayout.Label("" + digitOne.ToString("F0"));          //oldVelocity digit one. F0 means, no decimals behind the first number.
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(pos.x - compassSize.x * 0.05f, Screen.height - (pos.y - compassSize.y * 0.8f), 50, 50));
        GUILayout.Label("" + digitTwo.ToString("F0"));          //oldVelocity digit two.
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(pos.x - compassSize.x * -0.05f, Screen.height - (pos.y - compassSize.y * 0.8f), 50, 50));
        GUILayout.Label("" + digitThree.ToString("F0"));        //oldVelocity digit three.
        GUILayout.EndArea();
    }
}

//===============================================================================================
//This speedometer graphic resource has been created by Mariusz Zawistowicz for PixelMonarchy.com
//===============================================================================================