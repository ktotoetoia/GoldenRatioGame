namespace IM.GoldenRatio
{
    public static class Fibonacci
    {
        public static int[] GetSequence(int count)
        {
            int[] sequence = new int[count];

            int prev = 0;
            int curr = 1;
            
            for (int i = 0; i < count; i++)
            {
                sequence[i] = curr;
                
                int next = prev + curr;
                prev = curr;
                curr = next;
            }
            
            return sequence;
        }
    }
}