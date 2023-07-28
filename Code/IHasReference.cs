namespace Rovsau.Collections
{
    public interface IHasReference<T>
    {
        public int Count { get; }
        ref T this[int index] { get; }
        public bool Contains(T item);
        public ref T GetReferenceAt(int index);// => ref this[index]; // Default implementation. 
    }
}
