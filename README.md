# DynamicArray<T\>
Capacity limited to the number of items held. (Does not double in size).  

Implements C# Interfaces: 
- `IList`
- `IList<T>`
- `ICollection`
- `ICollection<T>`
- `IReadOnlyList<T>`
- `IReadOnlyCollection<T>`

Implements Custom Interfaces:
- `IHasReference<T>`
- `IRange<T>`

## IHasReference<T\>
Use `GetReferenceAt(int index)` to get a reference to a value type, so it can be modified directly in the collection. 

## IRange<T\> : IList<T\>
Contains methods for adding, inserting, and removing multiple items in single operations. 
Inherits from IList<T\>, to improve flexibility. 

## DynamicArrayTester
Static test class.  
Made while in a Unity Project.  
Convert to pure CSharp by replacing `Debug.Log` and `Debug.LogError` with `Console.WriteLine`.  
Then add an indicator for errors, in `LogMessage()`.
