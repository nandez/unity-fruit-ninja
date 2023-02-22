using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    [SerializeField] protected GameObject normalState;
    [SerializeField] protected GameObject slicedState;

    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var swordCtrl = other.GetComponent<SwordController>();

            var direction = swordCtrl.Direction;
            var cutPosition = swordCtrl.transform.position;
            var cutForce = swordCtrl.CutForce;

            // Cambiamos el estado de la fruta activando y desactivando los objetos.
            normalState.SetActive(false);
            slicedState.SetActive(true);

            // Deshabilitamos el collider de la fruta para que no se pueda cortar dos veces.
            col.enabled = false;

            // Calculamos el ángulo de rotación de la fruta utilizando la dirección del corte y rotamos el objeto.
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            slicedState.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Obtenemos los componentes Rigidbody de los objetos hijos del objeto slicedState,
            // que representan las 2 mitades de la fruta.
            var slicedParts = slicedState.GetComponentsInChildren<Rigidbody>();

            // Añadimos una fuerza a cada una de las mitades de la fruta.
            foreach (var part in slicedParts)
            {
                // Mantenemos la velocidad de la fruta antes de cortarla.
                part.velocity = rb.velocity;

                // Calculamos la dirección de la fuerza a aplicar a la fruta.
                part.AddForceAtPosition(direction * cutForce, cutPosition, ForceMode.Impulse);
            }
        }
    }
}
