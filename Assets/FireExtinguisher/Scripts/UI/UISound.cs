using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FireExtinguisher.Feedbacks
{
    public class UISound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler,IPointerExitHandler
    {
        private AudioSource _audioSource;
        [SerializeField] private AudioClip _clickClip;
        [SerializeField] private AudioClip _hoverClip;
        [SerializeField] private AudioClip _unhoverClip;


        private void Awake()
        {
            if (_audioSource == null)
            {
                _audioSource = GameObject.FindWithTag("UiSound").GetComponent<AudioSource>();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_audioSource != null && _hoverClip != null)
            {
                if (GetComponent<Button>().interactable)
                    _audioSource.PlayOneShot(_hoverClip);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_audioSource != null && _clickClip != null)
            {
                if (GetComponent<Button>().interactable)
                    _audioSource.PlayOneShot(_clickClip);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_audioSource != null && _unhoverClip != null)
            {
                if (GetComponent<Button>().interactable)
                    _audioSource.PlayOneShot(_unhoverClip);
            }
        }
    }
}