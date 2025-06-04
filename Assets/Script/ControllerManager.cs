using System;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    [SerializeField] public Notificacao notificacao;
    [SerializeField] public String nomeCena;

    private bool sintoAfivelado = false;
    public bool ignicaoAcionada = false;


    void Update()
    {


    }

    public void AfivelarSinto()
    {
        sintoAfivelado = true;
        notificacao.MostrarNotificacao("Sinto Afinelado");
    }

    public void LigarCarro()
    {
        ignicaoAcionada = true;
        notificacao.MostrarNotificacao("Ignição Acinada");
    }
}
