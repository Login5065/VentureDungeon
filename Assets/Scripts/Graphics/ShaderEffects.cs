using UnityEngine;

namespace Dungeon.Graphics
{
    public class ShaderOperation
    {
        public float shadervalue;
        public string shader;
        public float operationSpeed;
        public float operationEnd;
        public float start;
        public ShaderOperation(float shadervalue, string shader, float operationSpeed, float operationEnd)
        {
            this.shadervalue = shadervalue;
            this.shader = shader;
            this.operationSpeed = operationSpeed;
            this.operationEnd = operationEnd;
            start = shadervalue;
        }
    }
    public class ShaderEffects : MonoBehaviour
    {
        [SerializeField] public Material material;
        private ShaderOperation operation;

        public void Awake() => material = gameObject.GetComponent<Renderer>().material;

        public void Update()
        {
            if (operation == null) return;
            operation.shadervalue = Mathf.MoveTowards(operation.shadervalue, operation.operationEnd, operation.operationSpeed * Time.deltaTime);
            material.SetFloat(operation.shader, operation.shadervalue);
            if (operation.shadervalue == operation.operationEnd)
            {
                operation = null;
            }
        }

        public void AddOperation(float shadervalue, string shader, float operationSpeed, float operationEnd) => operation = new ShaderOperation(shadervalue, shader, operationSpeed, operationEnd);
    }
}