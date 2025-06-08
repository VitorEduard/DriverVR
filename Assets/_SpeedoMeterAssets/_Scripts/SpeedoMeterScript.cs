using TMPro;
using UnityEngine;
using UnityEngine.UI;
//[ExecuteInEditMode()] 									//Execute this script in the editor, you dont have to play the game for it to update. (Note that it is best to play the game for best results).
public class SpeedoMeterScript : MonoBehaviour {
    public Image compassImage;
    public Image needleImage;
    public TextMeshProUGUI speedText;

    public float startingAngle = -120f;
    public float capValue = 8000f; // RPM máxima
    public float maximumDegrees = 120;

    private float anguloAnt = 0f;
    private float angulo = 0f;
    private CarroManager playerScript;

    void Start()
    {
        playerScript = FindObjectOfType<CarroManager>();
    }

    void Update()
    {
        float rpm = playerScript.RpmMotor();
        angulo = startingAngle + ((rpm / capValue) * (maximumDegrees + Mathf.Abs(startingAngle)));

        // Rotaciona a agulha
        needleImage.rectTransform.localRotation = Quaternion.Euler(0, 0, -angulo);

        // Atualiza a velocidade digital
        int velocidade = Mathf.RoundToInt(playerScript.VelocidadeTotal());
        speedText.text = velocidade.ToString("000");
    }
}

//===============================================================================================
//This speedometer graphic resource has been created by Mariusz Zawistowicz for PixelMonarchy.com
//===============================================================================================