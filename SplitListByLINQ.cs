using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DotNet.Utilities
{
    /// <summary>
    /// 将一个集合List<T>分割成N个子集合
    /// </summary>
    public static class SplitListByLINQ
    {
        /// <summary>
        /// 使用LINQ实现的分割集合List<T>的泛型扩展方法
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
        {
            int i = 0;
            var splits = from item in list
                         group item by i++ % parts into part
                         select part.AsEnumerable();
            return splits;
        }

        /// <summary>
        /// 未使用LINQ实现的返回只读的子集合的分割方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source,int size)
        {
            T[] array = null;
            int count = 0;
            foreach (T item in source)
            {
                if (array == null)
                {
                    array = new T[size];
                }
                array[count] = item;
                count++;
                if (count == size)
                {
                    yield return new ReadOnlyCollection<T>(array);
                    array = null;
                    count = 0;
                }
            }
            if (array != null)
            {
                Array.Resize(ref array, count);
                yield return new ReadOnlyCollection<T>(array);
            }
        }

        /// <summary>
        /// 使用LINQ的lambda表达式实现的方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Split2<T>(this IEnumerable<T> list, int parts)
        {
            return list.Select((item, index) => new { index, item })
                       .GroupBy(x => x.index % parts)
                       .Select(x => x.Select(y => y.item));
        }
    }

    #region 多功能的集合分割扩展类
    class PartitioningEnumerator<T> : IEnumerator<IEnumerable<T>>
    {
        IEnumerator<T> _enumerator;
        int _partitionSize;
        public PartitioningEnumerator(IEnumerator<T> enumerator, int partitionSize)
        {
            _enumerator = enumerator;
            _partitionSize = partitionSize;
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        IEnumerable<T> _current;
        public IEnumerable<T> Current
        {
            get { return _current; }
        }
        object IEnumerator.Current
        {
            get { return _current; }
        }

        public void Reset()
        {
            _current = null;
            _enumerator.Reset();
        }

        public bool MoveNext()
        {
            bool result;

            if (_enumerator.MoveNext())
            {
                _current = new PartitionEnumerable<T>(_enumerator, _partitionSize);
                result = true;
            }
            else
            {
                _current = null;
                result = false;
            }

            return result;
        }

    }

    class PartitionEnumerable<T> : IEnumerable<T>
    {
        IEnumerator<T> _enumerator;
        int _partitionSize;
        public PartitionEnumerable(IEnumerator<T> enumerator, int partitionSize)
        {
            _enumerator = enumerator;
            _partitionSize = partitionSize;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new PartitionEnumerator<T>(_enumerator, _partitionSize);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class PartitionEnumerator<T> : IEnumerator<T>
    {
        IEnumerator<T> _enumerator;
        int _partitionSize;
        int _count;
        public PartitionEnumerator(IEnumerator<T> enumerator, int partitionSize)
        {
            _enumerator = enumerator;
            _partitionSize = partitionSize;
        }

        public void Dispose()
        {
        }

        public T Current
        {
            get { return _enumerator.Current; }
        }
        object IEnumerator.Current
        {
            get { return _enumerator.Current; }
        }
        public void Reset()
        {
            if (_count > 0) throw new InvalidOperationException();
        }

        public bool MoveNext()
        {
            bool result;

            if (_count < _partitionSize)
            {
                if (_count > 0)
                {
                    result = _enumerator.MoveNext();
                }
                else
                {
                    result = true;
                }
                _count++;
            }
            else
            {
                result = false;
            }

            return result;
        }

    }
    #endregion

}

