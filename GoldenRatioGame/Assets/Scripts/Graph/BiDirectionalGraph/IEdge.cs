﻿namespace IM.Graphs
{
    public interface IEdge
    {
        INode From { get; }
        INode To { get; }
    }
}