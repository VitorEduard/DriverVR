using Logitech;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private LogitechGSDK.DIJOYSTATE2ENGINES inputsLogi;

    void Start()
    {
        Debug.Log("SteeringInit:" + LogitechGSDK.LogiSteeringInitialize(false));
    }

    void Update()
    {

        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.LogiPlaySpringForce(0, 0, 30, 60);
            inputsLogi = LogitechGSDK.LogiGetStateUnity(0);
            if (inputsLogi.rgbButtons[0] == 128)
            {
                SceneManager.LoadScene("Corrida_Formula_1");
            }
            else if (inputsLogi.rgbButtons[1] == 128)
            {
                SceneManager.LoadScene("Corrida_Nascar");
            }
            else if (inputsLogi.rgbButtons[2] == 128)
            {
                SceneManager.LoadScene("Corrida_Off_Road");
            }
            else if (inputsLogi.rgbButtons[3] == 128)
            {
                SceneManager.LoadScene("Morro");
            }
            else if (inputsLogi.rgdwPOV[0] == 9000)
            {
                SceneManager.LoadScene("Plano");
            }
            else if (inputsLogi.rgdwPOV[0] == 27000)
            {
#if UNITY_EDITOR
                // Fecha o jogo dentro do Editor da Unity
                UnityEditor.EditorApplication.isPlaying = false;
#else
            // Fecha o jogo na build final
            Application.Quit();
#endif
            }
        }
    }
}
