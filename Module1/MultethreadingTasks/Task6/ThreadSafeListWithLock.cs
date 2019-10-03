using System.Collections.Generic;
using System.Threading;

namespace Task6
{
    /// <summary>
    /// Reprents thread-safe generic List with lock synchronization constructions
    /// </summary>
    /// <typeparam name="T">type of List's items</typeparam>
    public class ThreadSafeListWithLock<T> : IList<T>
    {
        private List<T> _internalList;

        private readonly object lockList = new object();

        /// <summary>
        /// Create new instance of ThreadSafeListWithLock
        /// </summary>
        public ThreadSafeListWithLock()
        {
            _internalList = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Clone().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Clone().GetEnumerator();
        }

        /// <summary>
        /// Copies the entire list to a new list.
        /// </summary>
        /// <returns>new list</returns>
        public List<T> Clone()
        {
            ThreadLocal<List<T>> threadClonedList = new ThreadLocal<List<T>>();

            lock (lockList)
            {
                _internalList.ForEach(element => { threadClonedList.Value.Add(element); });
            }

            return (threadClonedList.Value);
        }

        /// <summary>
        /// Adds item thread-safe
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            lock (lockList)
            {
                _internalList.Add(item);
            }
        }

        /// <summary>
        /// Removes item thread-safe
        /// </summary>
        /// <param name="item"></param>
        public bool Remove(T item)
        {
            bool isRemoved;

            lock (lockList)
            {
                isRemoved = _internalList.Remove(item);
            }

            return (isRemoved);
        }

        /// <summary>
        /// Clears all items thread-safe 
        /// </summary>
        public void Clear()
        {
            lock (lockList)
            {
                _internalList.Clear();
            }
        }

        /// <summary>
        /// Check if list contains the item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if contains otherwise false</returns>
        public bool Contains(T item)
        {
            bool containsItem;

            lock (lockList)
            {
                containsItem = _internalList.Contains(item);
            }

            return (containsItem);
        }

        /// <summary>
        /// Copies the entire list to a compatible one-dimensional
        /// array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">one-dimensional target array</param>
        /// <param name="arrayIndex">starting index</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (lockList)
            {
                _internalList.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the list
        /// </summary>
        public int Count
        {
            get
            {
                int count;

                lock ((lockList))
                {
                    count = _internalList.Count;
                }

                return (count);
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Looks for index of item in list
        /// </summary>
        /// <param name="item">item value</param>
        /// <returns>The zero-based index of the first occurrence of item within the list,
        /// if found; otherwise, –1</returns>
        public int IndexOf(T item)
        {
            int itemIndex;

            lock ((lockList))
            {
                itemIndex = _internalList.IndexOf(item);
            }

            return (itemIndex);
        }

        /// <summary>
        /// Insert new item to index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            lock ((lockList))
            {
                _internalList.Insert(index, item);
            }
        }

        /// <summary>
        /// Remove item from index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            lock ((lockList))
            {
                _internalList.RemoveAt(index);
            }
        }

        /// <summary>
        /// Indexer for easily taking items in list
        /// </summary>
        /// <param name="index">index of item</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                lock ((lockList))
                {
                    return _internalList[index];
                }
            }
            set
            {
                lock ((lockList))
                {
                    _internalList[index] = value;
                }
            }
        }
    }

}
