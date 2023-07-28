using System;
using System.Collections;
using System.Collections.Generic;

namespace Rovsau.Collections
{
    public class DynamicArray<T> : IList, IList<T>, ICollection, ICollection<T>, IReadOnlyList<T>, IReadOnlyCollection<T>, IHasReference<T>, IRange<T>
    {
        private T[] items;
        public int Count => items.Length;
        public bool IsReadOnly => false;
        bool IList.IsFixedSize => false;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => throw new NotSupportedException();
        object IList.this[int index]
        {
            get
            {
                ValidateIndex(index);
                return items[index];
            }

            set
            {
                ValidateIndex(index);
                if (value is not T) throw new InvalidCastException("Object cannot be cast to the generic collection type " + nameof(T) + ".");
                items[index] = (T)value;
            }
        }
        public T this[int index]
        {
            get
            {
                ValidateIndex(index);
                return items[index];
            }

            set
            {
                ValidateIndex(index);
                items[index] = value;
            }
        }
        public T[] ToArray()
        {
            T[] copy = new T[Count];
            Array.Copy(items, copy, copy.Length);
            return copy;
        }
        ref T IHasReference<T>.this[int index]
        {
            get
            {
                ValidateIndex(index);
                return ref items[index];
            }
        }
        public ref T GetReferenceAt(int index) => ref ((IHasReference<T>)this)[index];
        public DynamicArray()
        {
            items = new T[0];
            //Count = items.Length;
        }
        public DynamicArray(params T[] initialItems)
        {
            items = new T[initialItems.Length];
            Array.Copy(initialItems, items, initialItems.Length);
            //Count = items.Length;
        }
        public void AddRange(T[] range)
        {
            var newArray = new T[items.Length + range.Length];
            Array.Copy(items, 0, newArray, 0, items.Length);
            Array.Copy(range, 0, newArray, items.Length, range.Length);
            items = newArray;
            //Count += range.Length;
        }
        public void InsertRange(T[] range, int index)
        {
            ValidateIndex(index);
            var array = new T[items.Length + range.Length];
            Array.Copy(items, 0, array, 0, index);
            Array.Copy(range, 0, array, index, range.Length);
            Array.Copy(items, index, array, index + range.Length, items.Length - index);
            items = array;
            //Count += range.Length;
        }
        /// <returns>The corresponding indexes which were removed. An index of -1 means the item was not found.</returns>
        public int[] RemoveRange(T[] range)
        {
            var indexes = new int[range.Length];
            for (int i = 0; i < range.Length; i++)
            {
                // Items not found have an index of -1. 
                indexes[i] = IndexOf(range[i]);
            }

            // Uses Default Implementation.
            // Consider implicit implementation to avoid cast. 
            // Achieved by Copy/Pasting the default interface method into this class. 
            (int, int)[] rangesToRemove = ((IRange<T>)this).ExtractRangeIndices(indexes);

            // Reverse loop, to keep indexes valid while removing them.
            for (int i = rangesToRemove.Length - 1; i >= 0; i--)
            {
                RemoveRange(rangesToRemove[i].Item1, rangesToRemove[i].Item2);
            }
            return indexes;
        }
        public void RemoveRange(int firstIndex, int lastIndex)
        {
            ValidateIndex(firstIndex);
            ValidateIndex(lastIndex);
            if (lastIndex < firstIndex)
            {
                throw new ArgumentOutOfRangeException("Last index (" + lastIndex + ") must be greater than or equal to First index(" + firstIndex + ").");
            }
            int lengthToRemove = lastIndex - firstIndex + 1;
            var array = new T[items.Length - lengthToRemove];
            Array.Copy(items, 0, array, 0, firstIndex);
            Array.Copy(items, lastIndex + 1, array, firstIndex, items.Length - lastIndex - 1);
            items = array;
            //Count -= lengthToRemove;
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return items[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(T item)
        {
            Array.Resize(ref items, items.Length + 1);
            items[items.Length - 1] = item;
            //Count++;
        }
        public void Clear()
        {
            items = new T[0];
            //Count = 0;
        }
        public bool Contains(T item)
        {
            return Array.IndexOf(items, item) != -1;
        }
        public void CopyTo(T[] array, int index)
        {
            if (array == null) throw new ArgumentNullException("Destination array cannot be null.");
            if (array.Rank != 1) throw new ArgumentException("Method only works for single-dimension arrays.");
            if (array.GetLowerBound(0) != 0) throw new ArgumentException("Method only supports arrays whose first index is zero.");
            if (index < 0 || index > array.Length) throw new ArgumentOutOfRangeException("Index out of range.");
            if (array.Length - index < Count) throw new ArgumentException("Not enough available space from index to the end of this array.");
            Array.Copy(items, 0, array, index, items.Length);
        }
        public int IndexOf(T item)
        {
            return Array.IndexOf(items, item);
        }
        public void Insert(int index, T item)
        {
            ValidateIndex(index);
            var array = new T[items.Length + 1];
            Array.Copy(items, 0, array, 0, index);
            array[index] = item;
            Array.Copy(items, index, array, index + 1, items.Length - index);
            items = array;
            //Count++;
        }
        public bool Remove(T item)
        {
            int index = Array.IndexOf(items, item);
            if (index == -1)
            {
                return false;
            }
            RemoveAt(index);
            return true;
        }
        public void RemoveAt(int index)
        {
            ValidateIndex(index);
            int newLength = items.Length - 1;
            var newArray = new T[newLength];
            Array.Copy(items, 0, newArray, 0, index);
            Array.Copy(items, index + 1, newArray, index, newLength - index);
            items = newArray;
            //Count--;
        }
        int IList.Add(object value)
        {
            ValidateCast(value);
            Add((T)value);
            return items.Length - 1;
        }
        bool IList.Contains(object value)
        {
            ValidateCast(value);
            return Contains((T)value);
        }
        int IList.IndexOf(object value)
        {
            ValidateCast(value);
            return IndexOf((T)value);
        }
        void IList.Insert(int index, object value)
        {
            ValidateCast(value);
            Insert(index, (T)value);
        }
        void IList.Remove(object value)
        {
            ValidateCast(value);
            Remove((T)value);
        }
        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null) throw new ArgumentNullException("Destination array cannot be null.");
            if (array.Rank != 1) throw new ArgumentException("Method only works for single-dimension arrays.");
            if (array.GetLowerBound(0) != 0) throw new ArgumentException("Method only supports arrays whose first index is zero.");
            if (index < 0 || index > array.Length) throw new ArgumentOutOfRangeException("Index out of range.");
            if (array.Length - index < Count) throw new ArgumentException("Not enough available space from index to the end of this array.");
            Array.Copy(items, 0, array, index, items.Length);
        }
        private void ValidateCast(object value)
        {
            if (value is not T) throw new InvalidCastException("Value cannot be cast to the type of items in the collection.");
        }
        public bool IsValidIndex(int index)
        {
            return index >= 0 || index < Count;
        }
        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException("Index " + index + " does not exist in array of length " + Count + ".");
            }
        }
    }
}
