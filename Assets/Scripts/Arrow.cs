using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject fireEffectPrefab;
    [SerializeField] private GameObject arrowPrefab;
    private Rigidbody2D _arrowRigidbody2D;
    private bool _hasHit = false;
    private bool _hasSplit = false;
    private static int _arrowCount = 0;
    public ArrowType ArrowType { get; set; }
    public Action OnDestroyed;
    private AudioSource _audioSource;
    

    
    // Start is called before the first frame update
    void Awake()
    {
        _arrowRigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _arrowCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasHit)
        {
            SetAngle(_arrowRigidbody2D.velocity);
        }
    }

    public void SetVelocity(Vector2 velocity)
    {
        _arrowRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _arrowRigidbody2D.velocity = velocity;
        Destroy(gameObject, 5.0f);
    }

    private void SetAngle(Vector2 velocity)
    {
        if (velocity.magnitude > 0.1f)
        {
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            if (angle < 0 && ArrowType == ArrowType.Multiple && !_hasSplit)
            {
                ApllyMultipleEffect();
                _hasSplit = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _hasHit = true;
        _audioSource.Play();
        var joint = gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = other.rigidbody;
        if (this.ArrowType == ArrowType.Fire)
        {
            ApplyFireEffect(other.contacts[0].point);
        }
        Destroy(gameObject, 3.0f);
    }
    
    private void ApplyFireEffect(Vector2 position)
    {
        var fire = Instantiate(fireEffectPrefab, position, Quaternion.identity);
        Destroy(fire, 3.0f);
    }

    private void ApllyMultipleEffect()
    {
        var numberOfArrows = 3;
        float spreadAngle = 45f;
        float angleStep = spreadAngle / (numberOfArrows - 1);
        float startAngle = 45f; // 중앙에서부터 양쪽으로 발사
    
        for (int i = 0; i < numberOfArrows; i++)
        {
            float angle = startAngle + angleStep * i; // 각 화살의 각도를 계산
            Vector2 direction = GetDirectionFromAngle(angle); // 각도를 벡터로 변환

            // 새로운 화살 생성
            GameObject newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            Arrow newArrowScript = newArrow.GetComponent<Arrow>();
            newArrowScript.ArrowType = ArrowType.Default;
            newArrowScript.SetVelocity(direction * _arrowRigidbody2D.velocity.magnitude); // 기존 속도를 반영
        }
    }
    
    private Vector2 GetDirectionFromAngle(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
