using System.Collections.Generic;
using UnityEngine;

public class CheckDistanceSelector : Selector
{
    private Transform _target;
    private float _validDistance;

    public CheckDistanceSelector(Transform target, float validDistance, List<Node> children) : base(children)
    {
        _target = target;
        _validDistance = validDistance;
    }

    // Aquí implementamos la lógica de la condición
    public override bool Check(GameObject agent)
    {
        float distance = Vector3.Distance(agent.transform.position, _target.position);
        Debug.Log($"Distancia al objetivo: {distance}");
        return distance <= _validDistance;
    }
}

// Punto 2.b: Una Tarea que mueve al agente hacia el objetivo.
public class MoveTask : Task
{
    private Transform _target;
    private float _speed;

    public MoveTask(Transform target, float speed)
    {
        _target = target;
        _speed = speed;
    }

    public override bool Execute(GameObject agent)
    {
        // Mover el agente
        agent.transform.position = Vector3.MoveTowards(
            agent.transform.position,
            _target.position,
            _speed * Time.deltaTime);

        Debug.Log("Moviéndome hacia el objetivo...");

        // Esta tarea siempre se considera "exitosa" mientras se ejecuta
        return true;
    }
}

// Punto 2.c: Una Tarea que espera un tiempo determinado.
public class WaitTask : Task
{
  // Una referencia al controlador para poder decirle que empiece a esperar.
  private AI_Controller _controller;

  public WaitTask(AI_Controller controller)
  {
      _controller = controller;
  }

  public override bool Execute(GameObject agent)
  {
      // La única misión de esta tarea es activar el modo de espera en el controlador.
      _controller.StartWait();
      Debug.Log("Esperando-parte1");
      // La tarea en sí misma siempre se completa instantáneamente con éxito.
      return true;
  }

}

// (Aquí irían las clases CheckDistanceSelector, MoveTask y WaitTask que ya hicimos)

// NUEVA TAREA: Una Tarea que hace que el agente salte.
public class JumpTask : Task
{
    private Rigidbody _rigidbody;
    private float _jumpForce;
    private bool _isGrounded => _rigidbody.linearVelocity.y == 0; // Una forma simple de saber si está en el suelo

    public JumpTask(Rigidbody rigidbody, float jumpForce)
    {
        _rigidbody = rigidbody;
        _jumpForce = jumpForce;
    }

    public override bool Execute(GameObject agent)
    {
        // Solo saltamos si el Rigidbody existe y si está en el suelo
        if (_rigidbody != null && _isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            Debug.Log("¡Saltando!");
            return true; // El salto fue exitoso
        }

        // Si ya está en el aire o no tiene Rigidbody, la acción falla para que el árbol no se quede atascado.
        return false;
    }
}

// NECESITAREMOS ESTO: Un Selector simple que no tiene condición propia.
// Su único trabajo es ejecutar a sus hijos hasta que uno tenga éxito (pura lógica OR).
public class SimpleSelector : Selector
{
    public SimpleSelector(List<Node> children) : base(children) { }

    // Su condición siempre es verdadera para que pueda intentar ejecutar a sus hijos.
    public override bool Check(GameObject agent)
    {
        return true;
    }
}
