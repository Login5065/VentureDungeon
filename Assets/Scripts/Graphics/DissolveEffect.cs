using UnityEngine;

namespace Dungeon.Graphics
{
    public class DissolveEffect : MonoBehaviour
    {
        [SerializeField] private Material material;

        private float dissolveAmount = 0f;
        private float dissolveSpeed;
        private bool isDissolving;

        public void Start()
        {
            material = gameObject.GetComponent<Renderer>().material;
        }

        public void Update()
        {
            if (isDissolving)
            {
                dissolveAmount = Mathf.Clamp01(dissolveAmount + dissolveSpeed * Time.deltaTime);
            }
            else
            {
                dissolveAmount = Mathf.Clamp01(dissolveAmount - dissolveSpeed * Time.deltaTime);
            }
            material.SetFloat("_DissolveAmount", dissolveAmount);
        }

        public void StartDissolve(float dissolveSpeed)
        {
            isDissolving = true;
            //material.SetColor("_Dissolvecolor", dissolveColor);
            this.dissolveSpeed = dissolveSpeed;
        }

        public void StopDissolve(float dissolveSpeed)
        {
            isDissolving = false;
            //material.SetColor("_Dissolvecolor", dissolveColor);
            this.dissolveSpeed = dissolveSpeed;
        }
    }
}