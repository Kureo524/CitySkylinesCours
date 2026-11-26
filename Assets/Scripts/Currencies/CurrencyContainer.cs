using System.Collections.Generic;
using UnityEngine;

namespace Currencies {
    public enum ECurrencyType {
        Cash,
        Favorability,
        Population,
    }
    
    public class CurrencyContainer {
        public Dictionary<ECurrencyType, int> Currencies { get; }
        
        public CurrencyContainer((ECurrencyType,int)[] baseCurrencyAmounts) {
            Currencies = new Dictionary<ECurrencyType, int>();
            foreach (var currencyAmount in baseCurrencyAmounts) {
                Currencies.Add(currencyAmount.Item1, currencyAmount.Item2);
            }
        }

        public void AddTo(ECurrencyType type, int amount) {
            if (!Currencies.ContainsKey(type)) return;
            Currencies[type] += amount;
        }
        
        public bool HasAmount(ECurrencyType type, int amount) {
            if (!Currencies.ContainsKey(type)) return false;
            return amount <= Currencies[type];
        }

        public void Use(ECurrencyType type, int amount) {
            if (Currencies.ContainsKey(type)) Currencies[type] -= amount;
        }
    }
}
