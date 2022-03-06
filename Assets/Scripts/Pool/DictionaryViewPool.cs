using System;
using System.Collections.Generic;
using View;

namespace Pool
{
  public class DictionaryViewPool<T> : IDisposable where T : BaseView
  {
    private readonly Dictionary<LibraryCatalogNames, Stack<T>> _dictionary;
    private readonly Func<PoolObjectType, T> _createFunc;
    private readonly Action<T> _actionOnGet;
    private readonly Action<T> _actionOnRelease;
    private readonly Action<T> _actionOnDestroy;
    private readonly int _maxSize;
    private readonly int _defaultCapacity;
    private readonly bool _collectionCheck;

    private int _countInactive(LibraryCatalogNames type) => _dictionary[type].Count;

    public DictionaryViewPool(
      Func<PoolObjectType, T> createFunc,
      Action<T> actionOnGet = null,
      Action<T> actionOnRelease = null,
      Action<T> actionOnDestroy = null,
      bool collectionCheck = true,
      int defaultCapacity = 10,
      int maxSize = 10000)
    {
      if (createFunc == null)
      {
        throw new ArgumentNullException(nameof(createFunc));
      }

      if (maxSize <= 0)
      {
        throw new ArgumentException("Max Size must be greater than 0", nameof(maxSize));
      }

      _defaultCapacity = defaultCapacity;
      _dictionary = new Dictionary<LibraryCatalogNames, Stack<T>>();
      _createFunc = createFunc;
      _maxSize = maxSize;
      _actionOnGet = actionOnGet;
      _actionOnRelease = actionOnRelease;
      _actionOnDestroy = actionOnDestroy;
      _collectionCheck = collectionCheck;
    }

    public T Get(PoolObjectType type)
    {
      T obj;
      if (!_dictionary.ContainsKey(type.ObjectType))
      {
        _dictionary.Add(type.ObjectType, new Stack<T>(_defaultCapacity));
      }
      if (_dictionary[type.ObjectType].Count == 0)
      {
        obj = _createFunc(type);
      }
      else
      {
        obj = _dictionary[type.ObjectType].Pop();
      }

      _actionOnGet?.Invoke(obj);

      return obj;
    }

    public void Release(PoolObjectType type, T element)
    {
      if (_collectionCheck && _dictionary.Count > 0 && _dictionary.ContainsKey(type.ObjectType) &&
          _dictionary[type.ObjectType].Contains(element))
      {
        throw new InvalidOperationException("Trying to release an object that has already been released to the pool.");
      }

      _actionOnRelease?.Invoke(element);
      if (_countInactive(type.ObjectType) < _maxSize)
      {
        _dictionary[type.ObjectType].Push(element);
      }
      else
      {
        _actionOnDestroy?.Invoke(element);
      }
    }

    public void Clear()
    {
      if (_actionOnDestroy != null)
      {
        foreach (var key in _dictionary.Keys)
        {
          foreach (var obj in _dictionary[key])
          {
            _actionOnDestroy(obj);
          }

          _dictionary[key].Clear();
        }
      }

      _dictionary.Clear();
    }

    public void Dispose() => Clear();
  }
}