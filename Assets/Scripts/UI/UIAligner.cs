using UnityEngine;

public class UIAligner : MonoBehaviour
{
    [SerializeField] private float _movementLagSpeed;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _uiTransform;

    private Quaternion _toRotation;
    private Quaternion _currentRotation;

    void Update()
    {
        _toRotation = _cameraTransform.rotation;

        _currentRotation = Quaternion.Slerp(_currentRotation, _toRotation, _movementLagSpeed * Time.deltaTime);
        _currentRotation.x = 0;
        _currentRotation.z = 0;

        _uiTransform.rotation = _currentRotation;
    }
}
