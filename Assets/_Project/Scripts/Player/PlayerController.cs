using System;
using ScriptableObjects;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICanStack, ICanCollect
{
    [SerializeField] private InputDataSO inputDataSo;
    [SerializeField] private PlayerMovementDataSO playerMovementDataSo;
    [SerializeField] private StackManager stackManager;

    public Transform Transform
    {
        get { return _transform; }
    }

    private Animator _animator;
    private Transform _transform;
    private PlayerMovement _playerMovement;
    private PlayerStackController _playerStackController;

    private void Awake()
    {
        FillVariables();
        Initialize();
    }

    private void FillVariables()
    {
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
    }


    private void Initialize()
    {
        _playerMovement = new();
        _playerStackController = new();

        _playerMovement.Init(inputDataSo, playerMovementDataSo, _animator, this);
        _playerStackController.Init(stackManager);
    }

    private void Update() => _playerMovement.Update();


    public bool CanStack(ElementType type) => _playerStackController.CanStack(type);


    public GameObject Stack() => _playerStackController.Stack();


    public bool CanCollect(ElementType type) => _playerStackController.CanCollect(type);


    public void Collect(GameObject element) => _playerStackController.Collect(element);


    public Transform Target() => _playerStackController.Target();
}