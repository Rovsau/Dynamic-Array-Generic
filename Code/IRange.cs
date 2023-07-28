namespace Rovsau.Collections
{
    public interface IRange<T> : IList<T>
    {
        // Provided by IList<T>.
        //public int Count { get; }
        //public T this[int index] { get; }
        //public bool Contains(T item);
        public T[] ToArray();
        public void AddRange(T[] range);
        public void InsertRange(T[] range, int index);
        public int[] RemoveRange(T[] range);
        public void RemoveRange(int firstIndex, int lastIndex);

        // Default Implementation. 
        public (int, int)[] ExtractRangeIndices(int[] indexes)
        {
            var ranges = new List<(int, int)>();
            if (indexes == null || indexes.Length == 0)
                return ranges.ToArray();
            Array.Sort(indexes);
            int? start = null;
            int end = 0;
            foreach (int index in indexes)
            {
                if (index == -1) continue;
                if (start.HasValue == false)
                {
                    start = index;
                    end = index;
                }
                else if (index == end + 1)
                {
                    end = index;
                }
                else
                {
                    ranges.Add((start.Value, end));
                    start = index;
                    end = index;
                }
            }
            if (start.HasValue)
                ranges.Add((start.Value, end));
            return ranges.ToArray();
        }
    }
}
