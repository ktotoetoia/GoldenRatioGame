namespace IM.Visuals
{
    public class PortAlignerOrder : IPortAligner
    {
        private readonly PortAligner _aligner = new();
        public float OrderAdjustmentModifier { get; set; } = -0.01f;
        
        public void AlignPorts(IPortVisualObject portToMove, IPortVisualObject anchorPort)
        {
            _aligner.AlignPorts(portToMove,anchorPort);

            portToMove.OwnerVisualObject.ModuleLocalOrder = anchorPort.OwnerVisualObject.ModuleLocalOrder +
                                                            anchorPort.OutputOrderAdjustment * OrderAdjustmentModifier;
        }
    }
}