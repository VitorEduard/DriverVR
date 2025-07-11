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
            rotacao.y += (valorRelativo / 100);
            transform.localEulerAngles = rotacao;
            rotacaoVolanteAnt = rotacaoVoltanteAbsoluto;
        }
    }
    
}
