using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

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
// (El resto de tus clases: CheckDistanceSelector, WaitTask, JumpTask, SimpleSelector se quedan EXACTAMENTE IGUAL que en la versión que funcionaba con el StartWait())

// --- LA NUEVA Y MEJORADA MoveTask ---public class MoveTask : Task
// ... (CheckDistanceSelector, SimpleSelector, etc. no cambian) ...

// MoveTask ahora solo activa el interruptor de persecución en el controlador.// (CheckDistanceSelector, SimpleSelector, etc., no cambian)
// ... (CheckDistanceSelector, SimpleSelector, etc. no cambian) ...

// MoveTask ahora solo activa el interruptor de persecución en el controlador.
public class MoveTask : Task
{
    private AI_Controller _controller;

    public MoveTask(AI_Controller controller)
    {
        _controller = controller;
    }

    public override bool Execute(GameObject agent)
    {
        // Encendemos el interruptor de persecución.
        _controller.SetChaseStatus(true);
        // La tarea siempre tiene éxito para que la Sequence pueda continuar.
        return true;
    }
}

// JumpTask hace el salto Y apaga el interruptor de persecución.
public class JumpTask : Task
{
    private Rigidbody _rigidbody;
    private float _jumpForce;
    private AI_Controller _controller; // Necesita una referencia al controlador

    // Le pasamos el controlador además de los datos del salto
    public JumpTask(Rigidbody rigidbody, float jumpForce, AI_Controller controller)
    {
        _rigidbody = rigidbody;
        _jumpForce = jumpForce;
        _controller = controller;
    }

    public override bool Execute(GameObject agent)
    {
        // Apagamos el interruptor de persecución.
        _controller.SetChaseStatus(false);

        // Hacemos el salto (solo si está en el suelo)
        if (_rigidbody != null && _rigidbody.linearVelocity.y == 0)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            Debug.Log("¡Saltando!");
            return true; // Éxito
        }
        return false; // Falla si ya está en el aire
    }
}

// WaitTask se queda como estaba en la versión que funcionaba con StartWait()
public class WaitTask : Task
{
    private AI_Controller _controller;

    public WaitTask(AI_Controller controller)
    {
        _controller = controller;
    }

    public override bool Execute(GameObject agent)
    {
        _controller.StartWait();
        return true;
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
