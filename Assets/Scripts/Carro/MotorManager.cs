using Logitech;
using System;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class MotorManager : MonoBehaviour
{
    [SerializeField] private ControllerManager controllerManager;
    [SerializeField] private AnimationCurve eficienciaMotor;
    [SerializeField] private AnimationCurve curvaTorque;
    [SerializeField] private AnimationCurve curvaFreioMotor;
    [SerializeField] private AnimationCurve curvaLimiteRPMMotor;

    private MarchaEnum marchaAtual;
    private float rotacaoLivre = 900f;
    private float rpmMotor = 0f;
    private float rpmMotorAnterior = 800f;
    private float diferencial = 4.1f;
    private float velocidadeSuavizada = 0.0f;
    private float freioMotor = 2_000f;


    public float CalcularFreioMotor()
    {
        return curvaFreioMotor.Evaluate(rpmMotor) * freioMotor * Time.deltaTime;
    }

    public float RelacaoMarcha()
    {
        return marchaAtual.Relacao * diferencial;
    }

    public float RpmAlvoMotorLivre(float pedalAceleracao)
    {
        rpmMotorAnterior = rpmMotor;
        float rpmMotorLivre = Mathf.Max(rotacaoLivre, rpmMotor + (pedalAceleracao * 400_000 * Time.deltaTime));
        float aumentoRpmMotor = rpmMotor - rpmMotorAnterior;
        float resistenciaMotor = MathF.Abs(curvaLimiteRPMMotor.Evaluate(rpmMotor) - 1);
        return rpmMotorAnterior + (aumentoRpmMotor * resistenciaMotor);
    }

    public bool EhMarchaNeutra()
    {
        return MarchaEnum.NEUTRO.Equals(marchaAtual);
    }

    public float CalcularPotenciaMotor(float pedalAceleracao)
    {
        float eficienciaMotor = curvaTorque.Evaluate(rpmMotor);
        // Calcula o torque usando a curva de eficiência (Injeção Eletrônica)
        float aceleracaoAplicada = Mathf.Min(eficienciaMotor, pedalAceleracao);
        return aceleracaoAplicada * marchaAtual.Torque * 600;
    }

    public float CalcularRpmMotor(float rpmMotorAlvo, float impactoEmbreagem)
    {
        rpmMotorAnterior = rpmMotor;
        rpmMotor = Mathf.SmoothDamp(rpmMotor, rpmMotorAlvo, ref velocidadeSuavizada, impactoEmbreagem);
        float aumentoRpmMotor = rpmMotor - rpmMotorAnterior;
        if (aumentoRpmMotor > 0)
        {
            float resistenciaMotor = MathF.Abs(curvaLimiteRPMMotor.Evaluate(rpmMotor) -1);
            rpmMotor = rpmMotorAnterior + (aumentoRpmMotor * resistenciaMotor);
        }
        return rpmMotor;
    }

    public float RpmMotor() { return rpmMotor; }

    public void ValidarSeRpmMuitoBaixo(float rpmMotorAlvo, float pedalEmbreagem)
    {
        if (rpmMotorAlvo < (rotacaoLivre * 0.5) && EmbreagemPressionada(pedalEmbreagem))
        {
            controllerManager.ignicaoAcionada = false;
            controllerManager.notificacao.MostrarNotificacao("Carro Morreu!\nRPM Ficou muito abaixo");
        }
    }

    public void ValidarVariacaoRpmMuitoAlta(float rpmMotorAlvo, float pedalEmbreagem)
    {
        if (Mathf.Abs(rpmMotorAlvo - rpmMotor) > 3000 && EmbreagemPressionada(pedalEmbreagem))
        {
            controllerManager.ignicaoAcionada = false;
            controllerManager.notificacao.MostrarNotificacao("Carro Morreu!\nRPM variou muito");
        }
    }

    private void Start()
    {
        marchaAtual = MarchaEnum.NEUTRO;
    }


    private bool EmbreagemPressionada(float pedalEmbreagem)
    {
        return pedalEmbreagem < 0.8f;
    }

    public void TrocarMarcha(MarchaEnum marcha, float pedalEmbreagem)
    {
        if (pedalEmbreagem < 0.8)
        {
            controllerManager.ignicaoAcionada = false;
            controllerManager.notificacao.MostrarNotificacao("Carro Morreu!\nDeveria ter apertado na embreagem para trocar de Marcha");
        }
        this.marchaAtual = marcha;
    }

}
