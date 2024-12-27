using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private static readonly int IsDrawing = Animator.StringToHash("isDrawing");

    [Header("References")]
    [SerializeField] private Transform aim;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject trajectoryPointPrefab;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Camera followCamera;
    [Header("Trajectory Settings")]
    [SerializeField]private int trajectoryStep = 20;
    [SerializeField]private float trajectoryTime = 0.1f;
    
    [Header("Pull Settings")]
    [SerializeField] private float maxPullDistance = 5.0f;
    [SerializeField] private float power = 10.0f;
    
    private Animator _animator;
    private List<SpriteRenderer> _trajectoryPoints;
    private AudioSource _audioSource;
    
    private bool _isDrawing = false;
    public Vector3 startPosition;
    public Vector3 pullPosition;

    public Arrow currentArrow;
    public int maxArrowCount = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _trajectoryPoints = new List<SpriteRenderer>();
        _audioSource = this.GetComponent<AudioSource>();
        for (var i = 0; i < trajectoryStep; i++)
        {
            var instance = Instantiate(trajectoryPointPrefab, aim.transform.position, Quaternion.identity, aim);
            _trajectoryPoints.Add(instance.GetComponent<SpriteRenderer>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetGameStatus() != GameStatus.Playing) return;
        DrawTrajectory();
        if (Input.GetMouseButtonDown(0) && !_isDrawing  && !currentArrow)
        {
            StartDrawing();
        }
        else if (Input.GetMouseButton(0) && _isDrawing)
        {
            UpdateDrawing();
        } 
        else if (Input.GetMouseButtonUp(0) && _isDrawing)
        {
            ReleaseArrow();
        }
        ZoomTarget();
    }

    void ZoomTarget()
    {
        
        if (_isDrawing && !this.currentArrow)
        {
            followCamera.transform.position = new Vector3(aim.transform.position.x, aim.transform.position.y,followCamera.transform.position.z);
            followCamera.orthographicSize = 7;
        }
        else if (!_isDrawing && this.currentArrow && !this.currentArrow.IsDestroyed())
        {
            followCamera.transform.position = new Vector3(currentArrow.transform.position.x, currentArrow.transform.position.y,followCamera.transform.position.z);
            followCamera.orthographicSize = 2;
        }
        else
        {
            followCamera.transform.position = new Vector3(0, 0, -1);
            followCamera.orthographicSize = 7;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, followCamera.WorldToScreenPoint(aim.position).z);
        return followCamera.ScreenToWorldPoint(screenPosition);
    }
    
    void StartDrawing()
    {
        _isDrawing = true;
        pullPosition = startPosition = GetMouseWorldPosition();
        _animator.SetBool(IsDrawing, true);
    }

    void UpdateDrawing()
    {
        pullPosition = GetMouseWorldPosition();
        var direction = startPosition - pullPosition;
        if (direction.magnitude > maxPullDistance)
        {
            direction = direction.normalized * maxPullDistance;
            pullPosition = startPosition - direction;
        }
    }

    void ReleaseArrow()
    {
        _isDrawing = false;
        _animator.SetBool(IsDrawing, false);
        _audioSource.Play();
        var direction = startPosition - pullPosition;
        var arrowInstance = Instantiate(arrowPrefab, aim.transform.position, Quaternion.identity);
        currentArrow = arrowInstance.GetComponent<Arrow>();
        currentArrow.ArrowType = gameManager.GetCurrentArrowData().arrowType;
        currentArrow.OnDestroyed += ResetCurrentArrow;
        var velocity = power * new Vector2(direction.normalized.x, direction.normalized.y);
        currentArrow.SetVelocity(velocity);
    }

    void ResetCurrentArrow()
    {
        foreach (var spriteRenderer in _trajectoryPoints)
        {
            spriteRenderer.gameObject.SetActive(false);
        }
        currentArrow = null;
        maxArrowCount--;
        if (maxArrowCount <= 0 && gameManager &&gameManager.GetGameStatus() != GameStatus.Clear)
        {
            gameManager.GameOver();
        }
    }

    private void DrawTrajectory()
    {
        if (_isDrawing)
        {
            foreach (var spriteRenderer in _trajectoryPoints)
            {
                spriteRenderer.gameObject.SetActive(true);
            }
        }
        
        var initialVelocity = power;
        var direction = startPosition - pullPosition;
        var color = Color.white;
        var dot = Vector3.Dot(direction.normalized, aim.transform.right);
        if ( dot < 0)
        {
            color = Color.red;
        }
        
        if (direction == Vector3.zero) return;
        for (var i = 0; i < trajectoryStep; i++)
        {
            if (i == 0)
            {
                _trajectoryPoints[i].gameObject.SetActive(false);
                continue;
            }
            var t = trajectoryTime * i;
            var vx = initialVelocity * direction.normalized.x;
            var vy = initialVelocity * direction.normalized.y;
            var x = aim.transform.position.x + vx*t;
            var y = aim.transform.position.y + vy * t - 0.5f * -Physics.gravity.y *  t * t;
            _trajectoryPoints[i].transform.position = new Vector3(x, y, 0);
            _trajectoryPoints[i].color = color;
            var angle = Mathf.Atan2(y, x);
            _trajectoryPoints[i].transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
