using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollowExtension : CinemachineExtension
{
    public float smoothSpeed = 0.125f;
    public float mouseMoveSpeed = 0.1f;
    public bool enableParallax = true;
    public float parallaxAmount = 0.2f;

    private Vector3 offset;
    private Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        // Establecer el offset inicial entre la cámara y el objetivo
        if (VirtualCamera.Follow != null)
        {
            offset = VirtualCamera.transform.position - VirtualCamera.Follow.position;
        }
    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            // Obtén la posición deseada en base a la lógica de tu script
            Vector3 targetPosition = state.RawPosition;
            Vector2 mousePosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);

            // Si el mouse está cerca de los bordes de la pantalla, ajusta la posición de la cámara
            if (mousePosition.x < 0.1f || mousePosition.x > 0.9f || mousePosition.y < 0.1f || mousePosition.y > 0.9f)
            {
                Vector3 mouseDirection = new Vector3(mousePosition.x - 0.5f, mousePosition.y - 0.5f, 0f);
                targetPosition += mouseDirection * mouseMoveSpeed;
            }

            // Aplica la interpolación suave
            Vector3 smoothedPosition = Vector3.Lerp(state.RawPosition, targetPosition, smoothSpeed);
            state.RawPosition = smoothedPosition;

            // Aplica el efecto de parallax si está habilitado
            if (enableParallax)
            {
                Vector3 parallaxOffset = new Vector3(mousePosition.x - 0.5f, mousePosition.y - 0.5f, 0f) * parallaxAmount;
                state.RawPosition += parallaxOffset;
            }
        }
    }
}
