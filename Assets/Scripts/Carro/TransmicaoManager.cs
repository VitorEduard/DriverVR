using UnityEngine;

public class TransmicaoManager : MonoBehaviour
{
    private MotorManager motor; 
    private float pedalEmbreagem, pedalEmbreagemInv, pedalAceleracao, rpmRodas;

    public void Acelerar(RodasManager rodas, MotorManager motor, float pedalEmbreagem, float pedalAceleracao, float kph)
    {
        this.pedalEmbreagem = pedalEmbreagem;
        this.pedalAceleracao = pedalAceleracao;
        this.rpmRodas = rodas.RodasRPM();
        this.pedalEmbreagemInv = Mathf.Abs(pedalEmbreagem - 1);
        this.motor = motor;

        float forcaAceleracaoMotor = CalcularForcaAceleracaoMotor();

        if (!motor.EhMarchaNeutra())
        {
            float forcaFreioMotor = CalcularForcaFreioMotor(pedalEmbreagem, pedalAceleracao, motor);
            if (kph < 10)
            {
                forcaFreioMotor = 0;
            }
            float torqueAplicadoNaRoda = forcaAceleracaoMotor - forcaFreioMotor;
            Debug.Log(torqueAplicadoNaRoda);
            rodas.Mover(torqueAplicadoNaRoda);
        }
    }

    private float CalcularForcaFreioMotor(float pedalEmbreagem, float pedalAceleracao, MotorManager motor)
    {
        float forcaFreioMotor = motor.CalcularFreioMotor();
        return pedalEmbreagemInv * forcaFreioMotor;
    }

    private float CalcularForcaAceleracaoMotor()
    {
        if (motor.EhMarchaNeutra())
            return FaseMotorNeutro();
        else
            return FaseMotor();
    }

    private float FaseMotorNeutro()
    {
        float rpmMotorAlvoMotor = motor.RpmAlvoMotorLivre(pedalAceleracao);
        rpmMotorAlvoMotor = rpmMotorAlvoMotor - (300 + motor.RpmMotor() * 15 * Time.deltaTime);
        motor.CalcularRpmMotor(rpmMotorAlvoMotor, 0.05f + 0.4f);
        return 0f;
    }

    private float FaseMotor()
    {
        // Pegar Valor inverso do pedal da embreagem
        // Enquanto mais toca no pedal, menos deve considerar a roda��o da roda
        float rpmMotorAlvoMotor = pedalEmbreagem * motor.RpmAlvoMotorLivre(pedalAceleracao);
        float rpmMotorAlvoRodas = pedalEmbreagemInv * rpmRodas * motor.RelacaoMarcha();
        float rpmMotorAlvo = rpmMotorAlvoMotor + rpmMotorAlvoRodas;

        rpmMotorAlvo -= 30_000 * Time.deltaTime;

        // Se a rota��o alvo for menos que a livre deve matar o carro!!!
        motor.ValidarSeRpmMuitoBaixo(rpmMotorAlvo, pedalEmbreagem);

        // Se a embreagem estiver muito apertada a velocidade com que o motor aumenta as rota��es aumenta tamb�m
        float impactoEmbreagem = (Mathf.Abs(pedalEmbreagem - 1) * 0.4f);
        Debug.Log(motor.CalcularRpmMotor(rpmMotorAlvo, 0.05f + impactoEmbreagem));

        motor.ValidarVariacaoRpmMuitoAlta(rpmMotorAlvo, pedalEmbreagem);

        float torqueTotal = motor.CalcularPotenciaMotor(pedalAceleracao) * pedalEmbreagemInv;

        return torqueTotal;
    }



}
