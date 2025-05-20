using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class ColisaoSinto : MonoBehaviour
{
    [SerializeReference] private ControllerManager controllerManager;
    [SerializeReference] private Transform ancoraPontaSinto;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("PontaSinto"))
        {
            controllerManager.AfivelarSinto();

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

            // Posiciona e rotaciona o objeto no ponto
            collider.transform.position = ancoraPontaSinto.position;
            collider.transform.rotation = ancoraPontaSinto.rotation;

            // Torna filho da ancora (opcional, se quiser manter fixo mesmo se B mover)
            collider.transform.SetParent(ancoraPontaSinto);
        }
    }

}
