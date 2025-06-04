using Logitech;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarroManager : MonoBehaviour
{

    private LogitechGSDK.DIJOYSTATE2ENGINES inputsLogi;
    private int _INDEX_LOGI = 0;
    private float _MAXIMO_INPUT_VOLANTE = 32768f;
    private int _DOWN_FORCE = 50;

    [SerializeField] private ControllerManager controllerManager;
    [SerializeField] private VolanteManager volante;
    [SerializeField] private RodasManager rodas;
    [SerializeField] private MotorManager motor;
    [SerializeField] private TransmicaoManager transmicao;
    [SerializeField] private bool offRoad = false;
    [SerializeField] private bool pistaMolhada = false;
    private Rigidbody carroRigidBody;
    private GameObject centroDeMassa;
    public bool motorLigado = false;


    // Variveis do Carro
    private float embreagem = 0;
    private float freio = 0;
    private float acelerador = 0;
    private float rotacaoVolanteAbsoluta = 0;
    private float rotacaoVolanteAbsolutaNormalizado = 0;
    private float kph;
    private MarchaEnum marcha;
    private MarchaEnum marchaNova;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("SteeringInit:" + LogitechGSDK.LogiSteeringInitialize(false));
        carroRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        AtualizarVariaveis();

        if (!motorLigado) return;
        if (marchaNova != this.marcha)
        {
            marcha = marchaNova;
            motor.TrocarMarcha(marcha, embreagem);
        }
        addDownForce();

        addEffects();

        volante.RotacionarVolante(rotacaoVolanteAbsoluta);
        rodas.GirarRodas(rotacaoVolanteAbsolutaNormalizado);
        transmicao.Acelerar(rodas, motor, embreagem, acelerador, kph);
        rodas.FreiarMao(freio);
        rodas.FreiarPedal(freio);
    }

    void addEffects()
    {
        //LogitechGSDK.LogiPlayBumpyRoadEffect(0, 20);
        //int forcaEfeito = (int) Mathf.Min(5 + kph / 5f, 15);
        //LogitechGSDK.LogiPlayBumpyRoadEffect(0, forcaEfeito);
        if (offRoad)
        {
            int forcaEfeito = (int) Mathf.Min(10 + kph / 1.5f, 75);
            LogitechGSDK.LogiPlayDirtRoadEffect(0, forcaEfeito);
        } 
        else if (pistaMolhada)
        {
            int forcaEfeito = (int) Mathf.Min(10 + kph / 1.2f, 65);
            LogitechGSDK.LogiPlaySlipperyRoadEffect(0, forcaEfeito);
        }

    
    }

    void AtualizarVariaveis()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(_INDEX_LOGI))
        {
            LogitechGSDK.LogiPlaySpringForce(_INDEX_LOGI, 0, 30, 60);
            inputsLogi = LogitechGSDK.LogiGetStateUnity(_INDEX_LOGI);

            embreagem = 0;
            freio = 0;
            acelerador = 0;
            if (!ValidarBugInicio(inputsLogi))
            {
                embreagem = NormalizarDadoPedal(inputsLogi.rglSlider[0]);
                freio = NormalizarDadoPedal(inputsLogi.lRz);
                acelerador = Mathf.Max(0.1f, NormalizarDadoPedal(inputsLogi.lY));
            }

            rotacaoVolanteAbsoluta = inputsLogi.lX;
            rotacaoVolanteAbsolutaNormalizado = rotacaoVolanteAbsoluta / _MAXIMO_INPUT_VOLANTE;

            marchaNova = ObterMarchaAtual();
            if (inputsLogi.rgbButtons[24] == 128)
                SceneManager.LoadScene("Menu");

            if (inputsLogi.rgbButtons[6] == 128 || inputsLogi.rgbButtons[7] == 128)
                SceneManager.LoadScene(controllerManager.nomeCena);
        }
        else
        {
            embreagem = 0;
            freio = Input.GetKey(KeyCode.S) ? 1f : 0f;
            acelerador = Input.GetKey(KeyCode.W) ? 1f : 0.1f;
            if (Input.GetKey(KeyCode.A)) rotacaoVolanteAbsolutaNormalizado = -1;
            else if (Input.GetKey(KeyCode.D)) rotacaoVolanteAbsolutaNormalizado = 1;
            else rotacaoVolanteAbsolutaNormalizado = 0;

            rotacaoVolanteAbsoluta += Mathf.Min(_MAXIMO_INPUT_VOLANTE, rotacaoVolanteAbsolutaNormalizado * 12000 * Time.deltaTime);
        }

        kph = carroRigidBody.linearVelocity.magnitude * 3.6f;

        centroDeMassa = GameObject.Find("Massa");
        carroRigidBody.centerOfMass = centroDeMassa.transform.localPosition;
    }

    public float VelocidadeTotal()
    {
        return kph;
    }

    public float RpmMotor()
    {
        return motor.RpmMotor(); ;
    }

    private MarchaEnum ObterMarchaAtual()
    {
        MarchaEnum marchaAtual = MarchaEnum.NEUTRO;
        if (inputsLogi.rgbButtons[12] == 128)
        {
            marchaAtual = MarchaEnum.PRIMEIRA;
        } 
        else if (inputsLogi.rgbButtons[13] == 128)
        {
            marchaAtual = MarchaEnum.SEGUNDA;
        }
        else if (inputsLogi.rgbButtons[14] == 128)
        {
            marchaAtual = MarchaEnum.TERCEIRA;
        }
        else if (inputsLogi.rgbButtons[15] == 128)
        {
            marchaAtual = MarchaEnum.QUARTA;
        }
        else if (inputsLogi.rgbButtons[16] == 128)
        {
            marchaAtual = MarchaEnum.QUINTA;
        }
        else if (inputsLogi.rgbButtons[17] == 128)
        {
            marchaAtual = MarchaEnum.SEXTA;
        }
        else if (inputsLogi.rgbButtons[18] == 128)
        {
            marchaAtual = MarchaEnum.RE;
        }

        return marchaAtual;
    }

    void OnApplicationQuit()
    {
        Debug.Log("SteeringShutdown:" + LogitechGSDK.LogiSteeringShutdown());
    }

    float NormalizarDadoPedal(float dadoBruto)
    {
        return 1 - (dadoBruto + 32768) / 65535;
    }

    // Quando o jogo inicia antes de apertar em qualquer pedal o input deles é marcado como todos em 0 ao mesmo tempo
    bool ValidarBugInicio(LogitechGSDK.DIJOYSTATE2ENGINES rec)
    {
        return rec.rglSlider[0] == 0f && rec.lRz == 0f && rec.lY == 0f;
    }

    void addDownForce()
    {
        carroRigidBody.AddForce(-transform.up * _DOWN_FORCE * carroRigidBody.linearVelocity.magnitude);
    }

}
