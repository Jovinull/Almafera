using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float cellSize = 1f;

    private bool _isMoving;
    private Vector2Int _gridInput;

    private PlayerInputActions _actions;
    private Transform _transform;
    private Coroutine _moveCoroutine;

    private void Awake()
    {
        _transform = transform;
        _actions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _actions.Enable();

        // Inscreve nos eventos da action Move (Vector2)
        _actions.Player.Move.performed += OnMovePerformed;
        _actions.Player.Move.canceled  += OnMoveCanceled;
    }

    private void OnDisable()
    {
        // Desinscreve para evitar leaks / múltiplos handlers
        _actions.Player.Move.performed -= OnMovePerformed;
        _actions.Player.Move.canceled  -= OnMoveCanceled;

        _actions.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        var rawInput = ctx.ReadValue<Vector2>();

        // Arredonda para -1, 0 ou 1
        _gridInput = new Vector2Int(
            Mathf.RoundToInt(rawInput.x),
            Mathf.RoundToInt(rawInput.y)
        );

        // Remove diagonal (prioriza horizontal)
        if (_gridInput.x != 0)
            _gridInput.y = 0;

        // Se ainda não está se movendo, inicia a rotina de movimento
        if (!_isMoving && _gridInput != Vector2Int.zero)
        {
            _moveCoroutine = StartCoroutine(MoveRoutine());
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        // Quando solta o direcional, zera input.
        // A coroutine atual termina o tile em andamento e depois para.
        _gridInput = Vector2Int.zero;
    }

    private IEnumerator MoveRoutine()
    {
        _isMoving = true;

        // Enquanto houver input (tecla/direção pressionada), continua andando de tile em tile
        while (_gridInput != Vector2Int.zero)
        {
            // Calcula o próximo alvo no grid
            Vector3 startPos = _transform.position;
            Vector3 targetPos = startPos + new Vector3(_gridInput.x, _gridInput.y) * cellSize;

            // Anda suavemente até o target
            while ((_transform.position - targetPos).sqrMagnitude > 0.0001f)
            {
                _transform.position = Vector3.MoveTowards(
                    _transform.position,
                    targetPos,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }

            // Garante que parou exatamente na célula
            _transform.position = targetPos;
        }

        _isMoving = false;
        _moveCoroutine = null;
    }
}
