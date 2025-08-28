namespace IM.SelectionSystem
{
    public interface IHaveSelection
    {
        ISelection<T> GetSelection<T>()where T : class;
    }
}