using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class PlayerMovement
{
    private InputDataSO _inputData;
    private PlayerMovementDataSO _playerMovementData;
    private Animator _animator;
    private PlayerController _playerController;

    private float _blendValue = 0;

    public void Init(
        InputDataSO inputData, PlayerMovementDataSO playerMovementDataSo,
        Animator animator, PlayerController playerController)
    {
        _inputData = inputData;
        _playerMovementData = playerMovementDataSo;
        _animator = animator;
        _playerController = playerController;
    }


    public void Update()
    {
        SetAnim();
        if (!_inputData.onDown) return;

        LookAtDirection(LookDirection());
        MoveForward();
    }

    private void SetAnim()
    {
        if (_blendValue <= 1 && _inputData.onDown)
            _blendValue += Time.deltaTime * _playerMovementData.Acceleration;
        if (_blendValue >= 0 && !_inputData.onDown)
            _blendValue -= Time.deltaTime * _playerMovementData.Acceleration;
        _animator.SetFloat(Consts.PLAYER_BLEND, _blendValue);
    }

    private void MoveForward()
    {
        _playerController.Transform.position +=
            _playerController.Transform.forward * (Time.deltaTime * _playerMovementData.ForwarSpeed);
    }

    private void LookAtDirection(Vector3 lookDirection)
    {
        if (lookDirection == Vector3.zero) return;
        _playerController.Transform.rotation = Quaternion.RotateTowards(_playerController.Transform.rotation,
            Quaternion.LookRotation(lookDirection), Time.time * _playerMovementData.RotateSpeed);
    }

    private Vector3 LookDirection()
    {
        return new Vector3(_inputData.Horizontal, 0, _inputData.Vertical);
    }
}