using System.Collections.Generic;
using UnityEngine;

namespace IM.SelectionSystem
{
    public interface ISelection<out T>
    {
        T First { get; }
        IEnumerable<T> Selected { get; }
        Vector3 SelectionPosition { get; }
        
        ISelection<TT> OfType<TT>();
    }
}