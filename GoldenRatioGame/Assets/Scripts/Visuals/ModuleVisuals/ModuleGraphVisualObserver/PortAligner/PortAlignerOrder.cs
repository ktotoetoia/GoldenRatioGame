namespace IM.Visuals
{
    public class PortAlignerOrder : IPortAligner
    {
        private readonly PortAligner _aligner =  new();
        
        public void AlignPorts(IPortVisualObject portToMove, IPortVisualObject anchorPort)
        {
            _aligner.AlignPorts(portToMove,anchorPort);
            
            portToMove.OwnerVisualObject.Order = anchorPort.OwnerVisualObject.Order + anchorPort.OutputOrderAdjustment;
        }
    }
}