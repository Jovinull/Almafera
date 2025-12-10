using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private bool _isMoving;
    private Vector2 _input;

    private PlayerInputActions _actions;

    private void Awake()
    {
        // Cria a instância do mapa de ações
        _actions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // Ativa todas as actions
        _actions.Enable();
    }

    private void OnDisable()
    {
        // Desativa quando o objeto for desabilitado
        _actions.Disable();
    }

    private void Update()
    {
        // Se já está andando, não lê novo input
        if (_isMoving) return;

        // Lê o valor da action Move (Vector2)
        _input = _actions.Player.Move.ReadValue<Vector2>();

        // Como é grid-based, arredondamos para -1, 0 ou 1
        _input = new Vector2(
            Mathf.RoundToInt(_input.x),
            Mathf.RoundToInt(_input.y)
        );

        // Remove movimento em diagonal (prioriza horizontal)
        if (_input.x != 0f)
            _input.y = 0f;

        // Se não tem input, não faz nada
        if (_input == Vector2.zero) return;

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
    }
}
