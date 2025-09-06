using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    // --- Configuración visible en el Inspector de Unity ---
    public Transform targetObject;
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float validDistance = 15f;
    public float waitTime = 2f;

    private Root _behaviorTree;
    private bool _isWaiting = false;
    private float _waitTimer = 0f;
    private Rigidbody _rb;

    void Start()
    {
         _rb = GetComponent<Rigidbody>();
        // --- Construir el Árbol de Comportamiento ---
        // Aquí creamos las instancias de nuestros nodos específicos.

        // 1. La tarea de moverse (hijo del selector)
        var moveNode = new MoveTask(targetObject, moveSpeed);
        var waitNode = new WaitTask(this);
        var jumpNode = new JumpTask(_rb, jumpForce);
        // 2. El selector que comprueba la distancia (padre de la tarea de moverse)
        var checkDistanceNode = new CheckDistanceSelector(
            targetObject,
            validDistance,
            new List<Node> { moveNode }
        );

        // 3. La tarea de esperar

        var mainSelector = new SimpleSelector(new List<Node>
        {
            checkDistanceNode,
            jumpNode
        });

        // 4. La secuencia principal que une todo (según el punto 2.c del PDF)
        var mainSequence = new Sequence(new List<Node>
        {
            mainSelector,
            waitNode
        });

        // 5. El nodo raíz que inicia todo
        _behaviorTree = new Root(mainSequence);
    }

    void Update()
    {
      // Si estamos en modo de espera, solo manejamos el temporizador.
      if (_isWaiting)
      {
          _waitTimer += Time.deltaTime;
          if (_waitTimer >= waitTime)
          {

              _isWaiting = false; // Se acabó la espera
          }
      }
      // Si NO estamos esperando, ejecutamos el árbol de comportamiento.
      else
      {
          if (_behaviorTree != null)
          {
              _behaviorTree.Execute(this.gameObject);
          }
      }
      //Debug.Log(_waitTimer);
    }
    public void StartWait()
    {
        if (!_isWaiting)
        {
            Debug.Log("Iniciando espera...");
            _isWaiting = true;
            _waitTimer = 0f;
        }
    }
}
