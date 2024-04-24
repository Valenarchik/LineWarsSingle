using LineWars.Controllers;
using LineWars.Education;
using UnityEngine;

namespace LineWars.Scripts.Education
{
    public class EducationCardWaitingSetActive : EducationCardBase
    {
        [SerializeField] private new EnablingCallback gameObject;
        private void OnEnable()
        {
            gameObject.Enabled.AddListener(Enabled);
        }

        private void Enabled()
        {
            carousel.Next();
        }
        
        private void OnDisable()
        {
            gameObject.Enabled.RemoveListener(Enabled);
        }
    }
}