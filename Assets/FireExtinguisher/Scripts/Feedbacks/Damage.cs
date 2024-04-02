using FireExtinguisher.Manager;
using UnityEngine;

namespace FireExtinguisher.Feedbacks
{
    public class Damage : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void OnEnable()
        {
            GameManager.OnWarning += TakeDamage;
        }
        private void OnDisable()
        {
            GameManager.OnWarning -= TakeDamage;
        }

        private void TakeDamage()
        {
            _animator.SetTrigger("TakeDamage");
        }
    }
}
