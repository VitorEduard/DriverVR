using Oculus.Interaction.HandGrab;
using UnityEngine;

public class ControladorIgnicao : MonoBehaviour
{
    [SerializeReference] private ControllerManager controllerManager;
    [SerializeReference] private Transform pontoAncora;
    private bool chaveNaIgnicao = false;
    [SerializeReference] private GameObject chaveIginicao;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("ChaveCarro"))
        {
            if (!chaveNaIgnicao)
            {
                var handGrabInteractable = collider.GetComponentInChildren<HandGrabInteractable>();
                if (handGrabInteractable != null)
                {
                    // Solta da mão (todas as interações)
                    foreach (var interactor in FindObjectsOfType<HandGrabInteractor>())
                    {
                        if (interactor.HasSelectedInteractable && interactor.SelectedInteractable == handGrabInteractable)
                        {
                            interactor.Unselect();
                        }
                    }

                    handGrabInteractable.enabled = false; // opcional: impede de pegar de novo
                }

                collider.transform.position = pontoAncora.position;
                collider.transform.rotation = pontoAncora.rotation;
                collider.transform.SetParent(pontoAncora);
                collider.gameObject.SetActive(false);
                
                chaveIginicao.SetActive(true);
                chaveNaIgnicao = true;
            }

            controllerManager.LigarCarro();
        }

        
    }
}
