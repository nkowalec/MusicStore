using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models.Module
{
    public class CurrencySymbols
    {
        public static readonly string PLN = "PLN";
        public static readonly string EUR = "EUR";
        public static readonly string GBP = "GBP";
        public static readonly string USD = "USD";
    }

    public struct Currency
    {
        public decimal Value { get; private set; }
        public string Symbol { get; private set; }

        public Currency(decimal value, string symbol)
        {
            Value = value;
            Symbol = symbol;
        }

        public Currency Default
        {
            get { return new Currency(0, "PLN"); }
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Currency)) return false;
            var curr = (Currency)obj;
            if (curr.Value == this.Value && curr.Symbol.ToUpper() == this.Symbol.ToUpper())
                return true;
            return false;
        }
        public override string ToString()
        {
            return $"{this.Value} {this.Symbol}";
        }

        public static Currency operator +(Currency first, Currency second)
        {
            if(first.Symbol == second.Symbol)
            {
                return new Currency(first.Value + second.Value, first.Symbol);
            }
            throw new InvalidOperationException("Nie można dodawać kwot w różnych walutach");
        }
        public static Currency operator -(Currency first, Currency second)
        {
            if (first.Symbol == second.Symbol)
            {
                return new Currency(first.Value - second.Value, first.Symbol);
            }
            throw new InvalidOperationException("Nie można odejmować kwot w różnych walutach");
        }
        public static Currency operator *(Currency first, Currency second)
        {
            if (first.Symbol == second.Symbol)
            {
                return new Currency(first.Value * second.Value, first.Symbol);
            }
            throw new InvalidOperationException("Nie można mnożyć kwot w różnych walutach");
        }
        public static Currency operator /(Currency first, Currency second)
        {
            if (first.Symbol == second.Symbol)
            {
                return new Currency(first.Value / second.Value, first.Symbol);
            }
            throw new InvalidOperationException("Nie można dzielić kwot w różnych walutach");
        }
    }
}