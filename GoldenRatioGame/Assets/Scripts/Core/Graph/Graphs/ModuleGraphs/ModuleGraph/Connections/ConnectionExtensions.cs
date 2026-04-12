namespace IM.Graphs
{
    public static class ConnectionExtensions
    {
        public static IPort GetOtherPort(this IConnection connection, IPort port)
        {
            if(connection.Port1== port) return connection.Port2;
            if(connection.Port2 == port) return connection.Port1;

            return null;
        }
        
        public static IDataPort<T> GetOtherPort<T>(this IDataConnection<T> connection, IDataPort<T> port)
        {
            if(connection.DataPort1== port) return connection.DataPort2;
            if(connection.DataPort2 == port) return connection.DataPort1;

            return null;
        }
    }
}