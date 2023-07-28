using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rovsau.Collections.Testing
{
    internal static class DynamicArrayTester
    {
        //[InitializeOnLoadMethod]
        public static void RunTest()
        {
            _IList.RunTest();
            _IList_Generic.RunTest();
            _IRange_Generic.RunTest();
        }
        private static void CompareResults(IList outputResults, IList expectedResults)
        {
            string message = "Output Array Length (" + outputResults.Count + ") === Expected: (" + expectedResults.Count + ").";
            bool equalLength = outputResults.Count == expectedResults.Count;
            LogMessage(equalLength, message);

            int maxIterations = Math.Min(outputResults.Count, expectedResults.Count);
            for (int i = 0; i < maxIterations; i++)
            {
                var range = outputResults[i];
                var expected = expectedResults[i];
                bool isEqual = range.Equals(expected);
                message = "Output: " + range + " === Expected: " + expected;
                LogMessage(isEqual, message);
            }
        }
        private static void CompareResults<T>(IList<T> outputResults, IList<T> expectedResults) where T : IComparable<T>
        {
            string message = "Output Array Length (" + outputResults.Count + ") === Expected: (" + expectedResults.Count + ").";
            bool equalLength = outputResults.Count == expectedResults.Count;
            LogMessage(equalLength, message);

            int maxIterations = Math.Min(outputResults.Count, expectedResults.Count);
            for (int i = 0; i < maxIterations; i++)
            {
                var range = outputResults[i];
                var expected = expectedResults[i];
                bool isEqual = range.Equals(expected);
                message = "Output: " + range + " === Expected: " + expected;
                LogMessage(isEqual, message);
            }
        }
        private static void LogMessage(bool assertion, string message)
        {
            if (assertion)
                Debug.Log(message);
            else
                Debug.LogError(message);
        }

        private static class _IList
        {
            public static void RunTest()
            {
                Debug.Log("# IList RunTest()");
                Add();
                Insert();
                Remove();
                RemoveAt();
            }
            private static void Add()
            {
                IList outputResult = new DynamicArray<object> { 1, 2 };
                object obj = 3;
                object[] expectedResults = { 1, 2, 3, };

                outputResult.Add(obj);
                CompareResults(outputResult, expectedResults);
            }
            private static void Insert()
            {
                IList outputResult = new DynamicArray<object> { 1, 3 };
                object obj = 2;
                int insertIndex = 1;
                object[] expectedResults = { 1, 2, 3, };

                outputResult.Insert(insertIndex, obj);
                CompareResults(outputResult, expectedResults);
            }
            private static void Remove()
            {
                IList outputResult = new DynamicArray<object> { 1, 2, 3 };
                object obj = 2;
                object[] expectedResults = { 1, 3, };

                outputResult.Remove(obj);
                CompareResults(outputResult, expectedResults);
            }
            private static void RemoveAt()
            {
                IList outputResult = new DynamicArray<object> { 1, 2, 3 };
                int removeIndex = 1;
                object[] expectedResults = { 1, 3, };

                outputResult.RemoveAt(removeIndex);
                CompareResults(outputResult, expectedResults);
            }
        }

        private static class _IList_Generic
        {
            public static void RunTest()
            {
                Debug.Log("# IList<T> RunTest()");
                Add();
                Insert();
                Remove();
                RemoveAt();
            }
            private static void Add()
            {
                IList<string> outputResult = new DynamicArray<string>("a", "b");
                string add = "c";
                string[] expectedResult = { "a", "b", "c" };

                outputResult.Add(add);
                CompareResults(outputResult, expectedResult);
            }
            private static void Insert()
            {
                IList<string> outputResult = new DynamicArray<string>("a", "c");
                string insert = "b";
                int insertIndex = 1;
                string[] expectedResult = { "a", "b", "c" };

                outputResult.Insert(insertIndex, insert);
                CompareResults(outputResult, expectedResult);
            }
            private static void Remove()
            {
                IList<string> outputResult = new DynamicArray<string>("a", "b", "c");
                string remove = "b";
                string[] expectedResult = { "a", "c" };

                outputResult.Remove(remove);
                CompareResults(outputResult, expectedResult);
            }
            private static void RemoveAt()
            {
                IList<string> outputResult = new DynamicArray<string>("a", "b", "c");
                int removeIndex = 1;
                string[] expectedResult = { "a", "c" };

                outputResult.RemoveAt(removeIndex);
                CompareResults(outputResult, expectedResult);
            }
        }

        private static class _IRange_Generic
        {
            public static void RunTest()
            {
                Debug.Log("# IRange<T> RunTest()");
                AddRange();
                InsertRange();
                RemoveRange();
                RemoveRangeByIndices();
                ExtractRangeIndices();
            }
            // Chain from extensions to enable builder-pattern testing. 
            private static IRange<int> AddRange(IRange<int> irange = null, int[] addRange = null, int[] expectedResult = null)
            {
                Debug.Log("## AddRange()");
                if (irange == null) irange = new DynamicArray<int>(new int[] { 1, 2, 3 });
                if (addRange == null) addRange = new int[] { 4, 5, 6 };
                if (expectedResult == null) expectedResult = new int[] { 1, 2, 3, 4, 5, 6 };

                irange.AddRange(addRange);
                CompareResults(irange, expectedResult);
                return irange;
            }
            private static IRange<int> InsertRange(IRange<int> irange = null, int[] insertRange = null, int index = int.MinValue, int[] expectedResult = null)
            {
                Debug.Log("## InsertRange()");
                if (irange == null) irange = new DynamicArray<int>(new int[] { 1, 2, 3, 4, 5, 9, 10 });
                if (insertRange == null) insertRange = new int[] { 6, 7, 8 };
                if (index == int.MinValue) index = 5;
                if (expectedResult == null) expectedResult = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

                irange.InsertRange(insertRange, index);
                CompareResults(irange, expectedResult);
                return irange;
            }
            private static IRange<int> RemoveRange(IRange<int> irange = null, int[] removeRange = null, int[] expectedResult = null)
            {
                Debug.Log("## RemoveRange()");
                if (irange == null) irange = new DynamicArray<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
                if (removeRange == null) removeRange = new int[] { 4, 5, 6 };
                if (expectedResult == null) expectedResult = new int[] { 1, 2, 3, 7, 8, 9, 10 };

                irange.RemoveRange(removeRange);
                CompareResults(irange, expectedResult);
                return irange;
            }
            private static IRange<int> RemoveRangeByIndices(IRange<int> irange = null, int firstIndex = int.MinValue, int lastIndex = int.MinValue, int[] expectedResult = null)
            {
                Debug.Log("## RemoveRangeByIndices()");
                if (irange == null) irange = new DynamicArray<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
                if (firstIndex == int.MinValue) firstIndex = 3;
                if (lastIndex == int.MinValue) lastIndex = 7;
                if (expectedResult == null) expectedResult = new int[] { 1, 2, 3, 9, 10 };

                irange.RemoveRange(firstIndex, lastIndex);
                CompareResults(irange, expectedResult);
                return irange;
            }
            private static IRange<int> ExtractRangeIndices(IRange<int> irange = null, int[] extractIndices = null, (int, int)[] expectedResult = null)
            {
                Debug.Log("## ExtractRangeIndices()");
                if (irange == null) irange = new DynamicArray<int>();
                if (extractIndices == null) extractIndices = new int[] { 1, 2, 3, 4, 8, 9, 10 };
                if (expectedResult == null) expectedResult = new (int, int)[] { (1, 4), (8, 10) };

                Array.Sort(extractIndices);
                Array.Sort(expectedResult);

                var outputRanges = irange.ExtractRangeIndices(extractIndices);
                CompareResults<(int, int)>(outputRanges, expectedResult);
                return irange;
            }
        }
    }
}
