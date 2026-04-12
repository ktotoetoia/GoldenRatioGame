using System;

namespace IM.Graphs
{
    public interface IDataModuleGraphCloner<out TTargetGraph, TTargetValue> 
        where TTargetGraph : IDataModuleGraphReadOnly<TTargetValue>
    {
        TTargetGraph Copy<TSourceValue>(
            IDataModuleGraphReadOnly<TSourceValue> source, 
            Func<TSourceValue, TTargetValue> mapper);

        void Apply<TSourceValue>(
            IDataModuleGraphReadOnly<TSourceValue> source, 
            IDataModuleGraph<TTargetValue> target,
            Func<TSourceValue, TTargetValue> valueMapper);
    }
}