using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Collision")]
    [SerializeField] private LayerMask solidObjectsLayer;
    [SerializeField] private float collisionRadius = 0.3f;

    [Header("Encounter")]
    [SerializeField] private LayerMask grassLayer;
    [SerializeField, Range(0f, 1f)]
    [Tooltip("Chance de encontrar batalha ao pisar na grama (0–1).")]
    private float encounterChance = 0.1f;
    [SerializeField]
    [Tooltip("Raio usado para detectar grama sob o jogador.")]
    private float encounterCheckRadius = 0.2f;

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

        // Só anda se a posição for caminhável (sem colisão)
        if (IsWalkable(targetPos))
        {
            StartCoroutine(Move(targetPos));
        }
        else
        {
            // Se bateu em algo, garante que a animação de movimento pare
            _animator.SetBool(_isMovingHash, false);
        }
    }

    /// <summary>
    /// Verifica se a próxima posição é caminhável usando um OverlapCircle 2D.
    /// Retorna false se houver qualquer collider na layer de objetos sólidos.
    /// </summary>
    private bool IsWalkable(Vector3 targetPos)
    {
        var hit = Physics2D.OverlapCircle(targetPos, collisionRadius, solidObjectsLayer);
        return hit == null;
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

        // Após terminar o movimento, verifica se houve encontro na grama
        CheckForEncounters();
    }

    /// <summary>
    /// Verifica se o player está sobre uma tile de grama e,
    /// com base em uma chance, dispara um encontro aleatório.
    /// </summary>
    private void CheckForEncounters()
    {
        // Primeiro checa se há grama sob o jogador
        var grassHit = Physics2D.OverlapCircle(transform.position, encounterCheckRadius, grassLayer);
        if (grassHit == null)
            return;

        // Usa Random.value (0–1) para comparar com a chance configurada
        if (Random.value <= encounterChance)
        {
            // Aqui você pluga o sistema de batalha / cena, etc.
            Debug.Log("Encountered a wild pokemon");

            // Exemplo para futuro:
            // OnEncounter?.Invoke();
        }
    }
}
