using UnityEngine;
using System.Collections;

public class MarketingHand : MonoBehaviour
{
    private Vector3 _defaultPos = new Vector3(5000, -5000, 0);
    private Vector3 _target;
    private float _speed = 10f;
    private Vector3 _defaultScale;
    private float _delayMax = 2f;
    private float _currentDelay = 0;
    private bool _canScale = false;
    private Coroutine _scaler;

    private Vector3 _lastPos;
    private Vector3 _p;

    private void Start() {
        _defaultPos = new Vector3(Screen.width + 200f, -100f, 0);
        transform.position = _defaultPos;
        _target = _defaultPos; 
        _defaultScale = transform.localScale;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            _target = Input.mousePosition;
            // transform.position = _target;    
            _defaultPos.y = _target.y;
            _speed = 30f;
            _currentDelay = 0;
            _scaler = StartCoroutine(ScalerDelay());
            _canScale = false;
        }
        else if (Input.GetMouseButton(0))
        {
            _target = Input.mousePosition;
            _defaultPos.y = Mathf.Lerp(_defaultPos.y, _target.y, Time.unscaledDeltaTime);
            _currentDelay = 0;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _defaultPos = _defaultPos - Vector3.up * Random.Range(-200, 200);
            if (_scaler != null)
            {
                StopCoroutine(_scaler);
            }
            _canScale = false;
        }

        _currentDelay += Time.unscaledDeltaTime;
        if (_currentDelay >= _delayMax)
        {
            _target = _defaultPos;
            _speed = 4f;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, _defaultScale * (_canScale ? 1f : 1.1f), Time.unscaledDeltaTime * 20f);

        transform.position = Vector3.Lerp(transform.position, _target, Time.unscaledDeltaTime * _speed);

        _lastPos = transform.position;

        _p = Vector3.Lerp(_p, new Vector3(_target.x / (float)(Screen.width), _target.y / (float)(Screen.height), 0), Time.unscaledDeltaTime * 10f);

        transform.localEulerAngles = Vector3.Lerp(
            new Vector3(0, 0, 35),
            new Vector3(0, 0, -15),
            (_p.x + _p.y) / 2f
        );

    }

    IEnumerator ScalerDelay()
    {
        _canScale = false;
        yield return new WaitWhile(() => {return Vector3.Distance(transform.position, _lastPos) > 1f;});
        _canScale = true;
    }

}
