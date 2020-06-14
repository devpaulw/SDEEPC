﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SDEE_Editor
{
    public class PreviewEnvironmentGridCollection : IList<FrameworkElement>, INotifyCollectionChanged
    {
        readonly PreviewEnvironmentGrid m_peGrid;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public PreviewEnvironmentGridCollection(PreviewEnvironmentGrid peGrid)
        {
            m_peGrid = peGrid;
        }

        public FrameworkElement this[int index]
        {
            get => m_peGrid.Children[index] as FrameworkElement;
            set => m_peGrid.Children[index] = value; 
        }

        public int Count => m_peGrid.Children.Count;

        public bool IsReadOnly => false;

        public void Add(FrameworkElement item)
        {
            m_peGrid.Children.Add(item);
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Clear()
        {
            m_peGrid.Children.Clear();

            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(FrameworkElement item)
        {
            return m_peGrid.Children.Contains(item);
        }

        public void CopyTo(FrameworkElement[] array, int arrayIndex)
            => m_peGrid.Children.CopyTo(array, arrayIndex);

        public IEnumerator<FrameworkElement> GetEnumerator()
        {
            //return m_peGrid.Children.Cast<FrameworkElement>().GetEnumerator();

            IEnumerator iterator = m_peGrid.Children.GetEnumerator(); // TODO Try with my cast above
            while (iterator.MoveNext())
                yield return iterator.Current as FrameworkElement;
        }

        public int IndexOf(FrameworkElement item)
        {
            return m_peGrid.Children.IndexOf(item);
        }

        public void Insert(int index, FrameworkElement item)
        {
            m_peGrid.Children.Insert(index, item);
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        public bool Remove(FrameworkElement item)
        {
            if (!m_peGrid.Children.Contains(item))
                return false;

            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            m_peGrid.Children.Remove(item);

            return true;
        }

        public void RemoveAt(int index)
        {
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Remove, 
                m_peGrid.Children.Cast<UIElement>().ElementAt(index), index));

            m_peGrid.Children.RemoveAt(index);
        }

        public void Move(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= m_peGrid.Children.Count)
                throw new ArgumentOutOfRangeException(nameof(oldIndex));
            if (newIndex < 0 || newIndex >= m_peGrid.Children.Count)
                throw new ArgumentOutOfRangeException(nameof(newIndex));

            var item = this[oldIndex];
            m_peGrid.Children.RemoveAt(oldIndex);
            m_peGrid.Children.Insert(newIndex, item);

            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
        }

        IEnumerator IEnumerable.GetEnumerator() => m_peGrid.Children.GetEnumerator();

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs) 
        {
            CollectionChanged?.Invoke(this, notifyCollectionChangedEventArgs);
        }
    }
}