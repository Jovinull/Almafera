using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Animation Parameters")]
    [SerializeField] private string moveXParam = "moveX";
    [SerializeField] private string moveYParam = "moveY";
    [SerializeField] private string isMovingParam = "isMoving";

    private bool _isMoving;
    private Vector2 _input;

    private PlayerInputActions _actions;
    private Animator _animator;

    // Cache dos hashes dos parâmetros do Animator (mais performático)
    private int _moveXHash;
    private int _moveYHash;
    private int _isMovingHash;

    private void Awake()
    {
        // Mapa de ações do Input System
        _actions = new PlayerInputActions();

        // Referência ao Animator
        _animator = GetComponent<Animator>();

        // Cache dos hashes
        _moveXHash = Animator.StringToHash(moveXParam);
        _moveYHash = Animator.StringToHash(moveYParam);
        _isMovingHash = Animator.StringToHash(isMovingParam);
    }

    private void OnEnable()
    {
        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
    }

    private void Update()
    {
        // Se já está andando, só garante que o Animator sabe disso
        if (_isMoving)
        {
            _animator.SetBool(_isMovingHash, true);
            return;
        }

        // Lê o valor da action Move (Vector2) do Input System
        _input = _actions.Player.Move.ReadValue<Vector2>();

        // Como é grid-based, arredondamos para -1, 0 ou 1
        _input = new Vector2(
            Mathf.RoundToInt(_input.x),
            Mathf.RoundToInt(_input.y)
        );

        // Remove movimento em diagonal (prioriza horizontal)
        if (_input.x != 0f)
            _input.y = 0f;

        // Sem input: para animação de movimento
        if (_input == Vector2.zero)
        {
            _animator.SetBool(_isMovingHash, false);
            return;
        }

        // Atualiza direção no Animator (igual ao professor)
        _animator.SetFloat(_moveXHash, _input.x);
        _animator.SetFloat(_moveYHash, _input.y);

        // Calcula a próxima célula do grid
        var targetPos = transform.position;
        targetPos.x += _input.x;
        targetPos.y += _input.y;

        // Inicia a coroutine de movimento até o próximo tile
        StartCoroutine(Move(targetPos));
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        _isMoving = true;
        _animator.SetBool(_isMovingHash, true);

        // Anda suavemente até chegar no alvo
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Garante que parou exatamente na posição alvo
        transform.position = targetPos;

        _isMoving = false;
        _animator.SetBool(_isMovingHash, false);
    }
}
