using Logitech;
using System;
using UnityEngine;

public class VolanteManager : MonoBehaviour
{

    [SerializeField] private float velocidadeGiroVolante = 1.34f;
    private float rotacaoVolanteAnt = 0;
        
    public void RotacionarVolante(float rotacaoVoltanteAbsoluto)
    {
        float qtdRotacaoRelativa = rotacaoVoltanteAbsoluto - rotacaoVolanteAnt;
        if (qtdRotacaoRelativa != 0)
        {
            float valorRelativo = qtdRotacaoRelativa * velocidadeGiroVolante;
            Vector3 rotacao = transform.localEulerAngles;
            //transform.Rotate(Vector3.forward * (valorRelativo / 100), Space.World);
            rotacao.y += (valorRelativo / 100);
            transform.localEulerAngles = rotacao;
            //transform.localRotation = Quaternion.Euler(0, 0, (rotacaoVoltanteAbsoluto / 100));
            rotacaoVolanteAnt = rotacaoVoltanteAbsoluto;
        }
    }
    
}
