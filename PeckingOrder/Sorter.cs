using System;
using System.Collections.Generic;
using System.Linq;
using PeckingOrder.Collections;

namespace PeckingOrder
{
    public class Sorter<TMode, TModeRank> where TModeRank : IComparable
    {
        public BiDictionary<TMode, TModeRank> Order { get; set; } = new BiDictionary<TMode, TModeRank>();

        public TValue Resolve<TValue>(IEnumerable<(TMode Mode, TValue Value)> settings)
        {
            return ResolveContainer(settings).Value;
        }

        public (TMode Mode, TValue Value) ResolveContainer<TValue>(IEnumerable<(TMode Mode, TValue Value)> settings)
        {
            validate(settings);

            return settings.Aggregate((selected, x) =>
            {
                if (Order[x.Mode].CompareTo(Order[selected.Mode]) < 0)
                {
                    return x;
                }

                return selected;
            });
        }

        private void validate<TValue>(IEnumerable<(TMode Mode, TValue Value)> settings)
        {
            if (Order == null)
            {
                throw new InvalidOperationException("Cannot resolve settings while Order property is null.");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("Settings cannot be resolved because the settings parameter is null.");
            }

            if(!settings.Any())
            {
                throw new ArgumentException("Settings cannot be resolved because the settings collection is empty.");
            }

            foreach (var setting in settings)
            {
                if (!Order.ContainsKey(setting.Mode))
                {
                    throw new ArgumentOutOfRangeException($"Pecking Order cannot evaluate {setting.Mode.ToString()} because it has not been added to the Sorter's Order property.");
                }
            }
        }
    }
}