
using System;
using UnityEngine;

public class RodasManager : MonoBehaviour
{
    [SerializeField] private WheelCollider[] pneus = new WheelCollider[4];
    [SerializeField] private GameObject[] meshPneus = new GameObject[4];
    [SerializeField] private TracaoEnum tracao = TracaoEnum.DIANTEIRA;
    [SerializeField] private int poderDeFreioPedal = 50_000_000;
    [SerializeField] private int poderDeFreioMao = 20_000_000;
    private int raio = 6;


    public float RodasRPM()
    {
        float sum = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += pneus[i].rpm;
        }
        return Mathf.Abs(sum / 4);
    }

    public void Mover(float forcaTotalMotor)
    {
        float torquePorRoda = forcaTotalMotor / tracao.qtdRodas;

        for (int i = tracao.inicioRodas; i < tracao.fimRodas; i++)
        {
            pneus[i].motorTorque = torquePorRoda * Time.deltaTime;
        }
     
    }

    public void FreiarMao(float freioNormalizado)
    {
        float valorFreio = (freioNormalizado * poderDeFreioMao * Time.deltaTime);
        pneus[2].brakeTorque = valorFreio;
        pneus[3].brakeTorque = valorFreio;
    }

    public void FreiarPedal(float freioNormalizado)
    {
        float valorFreio = (freioNormalizado * poderDeFreioPedal * Time.deltaTime);
        pneus[0].brakeTorque = valorFreio * 0.7f;
        pneus[1].brakeTorque = valorFreio * 0.7f;
        pneus[2].brakeTorque = valorFreio;
        pneus[3].brakeTorque = valorFreio;
    }

    public void GirarRodas(float anguloAbsoluto)
    {
        // acerman steering formula
        if (anguloAbsoluto > 0)
        {
            pneus[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (raio + 0.75f)) * anguloAbsoluto;
            pneus[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (raio - 0.75f)) * anguloAbsoluto;
        }
        else if (anguloAbsoluto < 0)
        {
            pneus[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (raio - 0.75f)) * anguloAbsoluto;
            pneus[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (raio + 0.75f)) * anguloAbsoluto;
        }
        else
        {
            pneus[0].steerAngle = 0;
            pneus[1].steerAngle = 0;
        }


        AnimarPneus();
    }

    void AnimarPneus()
    {
        Vector3 posicaoPneus = Vector3.zero;
        Quaternion rotacaoPneus = Quaternion.identity;

        // Corrige a rotação com base na inclinação do seu modelo
        Quaternion correcao = Quaternion.Euler(new Vector3(90f, 0f, 0f));

        for (int i = 0; i < pneus.Length; i++)
        {
            pneus[i].GetWorldPose(out posicaoPneus, out rotacaoPneus);
            meshPneus[i].transform.position = posicaoPneus;
            meshPneus[i].transform.rotation = rotacaoPneus * correcao;
        }
    }

}