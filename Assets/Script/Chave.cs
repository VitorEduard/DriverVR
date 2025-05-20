using UnityEngine;
using Oculus.Interaction;
using System.Linq;

public class Chave : MonoBehaviour
{
    public Transform pivotPoint;
    public Vector3 localAxis = Vector3.forward;
    public float minAngle = 0f;
    public float maxAngle = 90f;

    private Transform interactorTransform;
    private Quaternion initialRotation;
    private GrabInteractable grab;

    void Start()
    {
        initialRotation = transform.localRotation;
        grab = GetComponent<GrabInteractable>();
    }

    void Update()
    {
        if (interactorTransform != null)
        {
            Vector3 dirToHand = interactorTransform.position - pivotPoint.position;

            // Projeta no plano perpendicular ao eixo local
            Vector3 projected = Vector3.ProjectOnPlane(dirToHand, pivotPoint.TransformDirection(localAxis));
            float angle = Vector3.SignedAngle(pivotPoint.forward, projected, pivotPoint.TransformDirection(localAxis));
            angle = Mathf.Clamp(angle, minAngle, maxAngle);

            transform.localRotation = Quaternion.AngleAxis(angle, localAxis);
        }
    }

    void OnGrab()
    {
        // Pegamos o primeiro interactor (mão) que está segurando
        var grabInteractable = GetComponent<GrabInteractable>();
        if (grabInteractable != null && grabInteractable.Interactors.Count > 0)
        {
            interactorTransform = grabInteractable.Interactors.First().transform;
        }
    }

    void OnUngrab()
    {
        interactorTransform = null;
    }
}
