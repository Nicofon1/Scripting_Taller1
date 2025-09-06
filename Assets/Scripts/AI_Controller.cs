using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AI_Controller : MonoBehaviour
{
    // --- Configuración ---
    public Transform targetObject;
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float validDistance = 15f;
    public float waitTime = 2f;

    // --- Estados internos ---
    private Root _behaviorTree;
    private bool _isWaiting = false;
    private float _waitTimer = 0f;
    private bool _isChasing = false; // El interruptor que las tareas controlan
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // --- Construcción del Árbol ---
        // Las tareas ahora necesitan una referencia a 'this' (el controlador) para dar órdenes
        var moveNode = new MoveTask(this);
        var waitNode = new WaitTask(this);
        var jumpNode = new JumpTask(_rb, jumpForce, this); // Le pasamos el controlador

        var checkDistanceNode = new CheckDistanceSelector(
            targetObject,
            validDistance,
            new List<Node> { moveNode }
        );

        var mainSelector = new SimpleSelector(new List<Node>
        {
            checkDistanceNode,
            jumpNode
        });

        var mainSequence = new Sequence(new List<Node>
        {
            mainSelector,
            waitNode
        });

        _behaviorTree = new Root(mainSequence);
    }

    void Update()
    {
        // 1. PRIMERO: Manejar el estado de espera. Si estamos esperando, no hacemos NADA MÁS.
        if (_isWaiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitTime)
            {
                _isWaiting = false;
            }
            return; // Salimos del Update aquí mismo.
        }

        // 2. SEGUNDO: Ejecutar el árbol para que actualice los "interruptores" de estado.
        if (_behaviorTree != null)
        {
            _behaviorTree.Execute(this.gameObject);
        }

        // 3. TERCERO: Actuar según el estado de los interruptores.
        //    Esta parte ya no está dentro de un 'else', por lo que siempre se ejecuta después del árbol.
        if (_isChasing)
        {
            // Mover el personaje continuamente
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetObject.position,
                moveSpeed * Time.deltaTime);
        }
    }

    // --- Métodos públicos para que las Tareas den órdenes ---

    public void StartWait()
    {
        // La espera solo comienza si no estamos en medio de una persecución
        if (!_isChasing)
        {
            _isWaiting = true;
            _waitTimer = 0f;
        }
    }

    public void SetChaseStatus(bool isChasing)
    {
        _isChasing = isChasing;
    }
}