using System;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    [SerializeField] public Notificacao notificacao;
    [SerializeField] public String nomeCena;
    [SerializeField] private Transform cameraRig;
    [SerializeField] private Transform pontoNoCarro;

    public bool sintoAfivelado { get; set; } = false;
    public bool ignicaoAcionada = false;


    void LateUpdate()
    {
        Vector3 vector3 = new Vector3(1, 1, 0);
        cameraRig.position = pontoNoCarro.position;
        cameraRig.rotation = pontoNoCarro.rotation;
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
