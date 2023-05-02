using UnityEngine;
using Cinemachine;
 
/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's co-ordinates
/// </summary>
[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
public class CinemachineCameraAxisLock : CinemachineExtension
{
    [Tooltip("Lock the camera's axes")]
    [SerializeField] private bool m_LockX;
    [SerializeField] private bool m_LockY;
    [SerializeField] private bool m_LockZ;
    [SerializeField] private float m_XPosition = 10;
    [SerializeField] private float m_YPosition = 10;
    [SerializeField] private float m_ZPosition = 10;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            if (m_LockX)
            {
                pos.x = m_XPosition;

            }
            if (m_LockY)
            {
                pos.y = m_YPosition;

            }
            if (m_LockZ)
            {
                pos.z = m_ZPosition;

            }
            state.RawPosition = pos;
        }
    }
}
