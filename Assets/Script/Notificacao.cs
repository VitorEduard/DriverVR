using System.Collections;
using TMPro;
using UnityEngine;

public class Notificacao : MonoBehaviour
{
    public float duracaoSegundos = 3f;               // tempo que a notificação fica visível
    private Coroutine rotinaNotificacao;
    [SerializeField] private GameObject painelNotificacao;     // objeto pai do texto (ex: NotificacaoObject)

    [SerializeField] public TextMeshProUGUI textoNotificacao;
    [SerializeField] private Transform cameraTransform;      // Normalmente: Camera.main.transform
    [SerializeField] private float distanciaDaCamera = 0f;
    [SerializeField] private float alturaNaCamera = 0f;
    private Vector3 offset; // Posição em relação à câmera
    private float suavizacao = 8f;          // Suavização no movimento

    void Start()
    {
        offset = new Vector3(0, alturaNaCamera, distanciaDaCamera);
    }

    void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        // Calcula a posição à frente da câmera
        Vector3 destino = cameraTransform.position + cameraTransform.forward * offset.z
                        + cameraTransform.up * offset.y
                        + cameraTransform.right * offset.x;

        // Move suavemente até a posição desejada
        transform.position = Vector3.Lerp(transform.position, destino, Time.deltaTime * suavizacao);

        // Rotaciona para olhar para a câmera
        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(transform.position - cameraTransform.position), Time.deltaTime * suavizacao);
    }

    public void MostrarNotificacao(string mensagem)
    {
        if (rotinaNotificacao != null)
            StopCoroutine(rotinaNotificacao);

        rotinaNotificacao = StartCoroutine(ExibirNotificacaoTemporaria(mensagem));
    }

    private IEnumerator ExibirNotificacaoTemporaria(string mensagem)
    {
        textoNotificacao.text = mensagem;
        painelNotificacao.SetActive(true);

        yield return new WaitForSeconds(duracaoSegundos);

        painelNotificacao.SetActive(false);
    }

}
