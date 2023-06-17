using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "BusinessTitleSO", menuName = "ScriptableObjects/BusinessTitleSO", order = 1)]
    public class BusinessTitlesSO : ScriptableObject
    {
        public BusinessTitle[] BusinessTitles;
    }
}