using System.Collections;

namespace PetHome.Core.Domain.Models;
//Надстройка над list для предотвращения написания в агрегатах details
public class ValueObjectList<T> : IReadOnlyList<T>
{
    public IReadOnlyList<T> Values { get; }
    private ValueObjectList() { }
    public ValueObjectList(IEnumerable<T> list)
    {
        Values = new List<T>(list);
    }

    public T this[int index] => Values[index];
    public int Count => Values.Count;
    public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();
    public static implicit operator ValueObjectList<T>(List<T> list) => new ValueObjectList<T>(list);
    public static implicit operator List<T>(ValueObjectList<T> list) => list.Values.ToList();
}
