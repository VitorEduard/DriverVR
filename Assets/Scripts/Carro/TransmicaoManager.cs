using UnityEngine;

public class TransmicaoManager : MonoBehaviour
{
    private MotorManager motor; 
    private float pedalEmbreagem, pedalEmbreagemInv, pedalAceleracao, rpmRodas;

    public void Acelerar(RodasManager rodas, MotorManager motor, float pedalEmbreagem, float pedalAceleracao, float kph, bool deveAcelerar)
    {
        this.pedalEmbreagem = pedalEmbreagem;
        this.pedalAceleracao = pedalAceleracao;
        this.rpmRodas = rodas.RodasRPM();
        this.pedalEmbreagemInv = Mathf.Abs(pedalEmbreagem - 1);
        this.motor = motor;

        float forcaAceleracaoMotor = CalcularForcaAceleracaoMotor(deveAcelerar);

        if (deveAcelerar)
        {
            float forcaFreioMotor = CalcularForcaFreioMotor(pedalEmbreagem, pedalAceleracao, motor, kph);
            float torqueAplicadoNaRoda = forcaAceleracaoMotor - forcaFreioMotor;
            rodas.Mover(torqueAplicadoNaRoda);
        }
        else
        {
            rodas.Mover(0f);
        }
    }

    private float CalcularForcaFreioMotor(float pedalEmbreagem, float pedalAceleracao, MotorManager motor, float kph)
    {
        // IF para o carro não andar pra trás
        if (kph < 1)
        {
            return 0f;
        }
        // Só age quando o pedal de aceleração não for apertado
        // 0,05 tem uma margem para quando não detecta corretamente o pedal livre
        if (pedalAceleracao < 0.05f)
        {
            float forcaFreioMotor = motor.CalcularFreioMotor();
            return pedalEmbreagemInv * forcaFreioMotor;
        }
        return 0;
    }

    private float CalcularForcaAceleracaoMotor(bool deveAcelerar)
    {
        if (deveAcelerar)
            return FaseMotor();
        else
            return FaseMotorNeutro();
    }

    private float FaseMotorNeutro()
    {
        float rpmMotorAlvoMotor = motor.RpmAlvoMotorLivre(pedalAceleracao);
        rpmMotorAlvoMotor -= 30_000 * Time.deltaTime;
        rpmMotorAlvoMotor = rpmMotorAlvoMotor - (300 + motor.RpmMotor() * 15 * Time.deltaTime);
        motor.CalcularRpmMotor(rpmMotorAlvoMotor, 0.05f + 0.4f);
        return 0f;
    }

    private float FaseMotor()
    {
        // Pegar Valor inverso do pedal da embreagem
        // Enquanto mais toca no pedal, menos deve considerar a rodação da roda
        float rpmMotorAlvoMotor = pedalEmbreagem * motor.RpmAlvoMotorLivre(pedalAceleracao);
        float rpmMotorAlvoRodas = pedalEmbreagemInv * rpmRodas * motor.RelacaoMarcha();
        float rpmMotorAlvo = rpmMotorAlvoMotor + rpmMotorAlvoRodas;

        rpmMotorAlvo -= 30_000 * Time.deltaTime;

        // Se a rotação alvo for menos que a livre deve matar o carro!!!
        motor.ValidarSeRpmMuitoBaixo(rpmMotorAlvo, pedalEmbreagem);

        // Se a embreagem estiver muito apertada a velocidade com que o motor aumenta as rotações aumenta também
        float impactoEmbreagem = (Mathf.Abs(pedalEmbreagem - 1) * 0.4f);
        float v = motor.CalcularRpmMotor(rpmMotorAlvo, 0.05f + impactoEmbreagem);

        //motor.ValidarVariacaoRpmMuitoAlta(rpmMotorAlvo, pedalEmbreagem);

        float torqueTotal = motor.CalcularPotenciaMotor(Mathf.Max(0.1f, pedalAceleracao)) * pedalEmbreagemInv;

        return torqueTotal;
    }



}
