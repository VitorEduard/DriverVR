using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    [SerializeField] public Notificacao notificacao;

    private bool sintoAfivelado = false;
    private bool ignicaoAcionada = false;


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
