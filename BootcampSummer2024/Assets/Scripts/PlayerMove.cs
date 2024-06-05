using UnityEngine;

public class PlayerMove : MonoBehaviour {
    [SerializeField] private float _speed = 8f;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _maxYPosition = 5.5f;

    private float _yPosition;
    private float _oldYPosition;

    void Update() {
        transform.position += new Vector3(_speed * Time.deltaTime, 0, 0);

        if (Input.GetMouseButton(0))
            _yPosition += _speed * Time.deltaTime;
        else
            _yPosition -= _speed * Time.deltaTime;

        _yPosition = Mathf.Clamp(_yPosition, -_maxYPosition, _maxYPosition);
        
        _playerTransform.localPosition = new Vector3(0, _yPosition, 0);
    }
}

