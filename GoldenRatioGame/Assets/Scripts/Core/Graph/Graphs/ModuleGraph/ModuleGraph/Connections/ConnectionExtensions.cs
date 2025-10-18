using System;

namespace IM.Graphs
{
    public static class ConnectionExtensions
    {
        public static IPort GetOther(this IConnection connection, IPort port)
        {
            if (connection.Input == port) return connection.Output;
            if (connection.Output == port) return connection.Input;

            throw new ArgumentException(nameof(port));
        }
    }
}