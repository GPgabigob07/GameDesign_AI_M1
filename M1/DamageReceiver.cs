using System;
using UnityEngine;

namespace M1
{
    public class DamageReceiver: MonoBehaviour
    {
        public bool log;
        public event Action OnTakeDamage; 
        
        public void DealDamage()
        {
            if (log) Debug.Log($"{gameObject.name} damage received");
            OnTakeDamage?.Invoke();
        }
    }
}