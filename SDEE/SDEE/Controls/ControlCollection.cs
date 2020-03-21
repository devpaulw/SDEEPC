using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE
{
    public abstract partial class Control
    {
        public class ControlCollection : ICollection<Control>
        {
            readonly List<Control> controls;

            public Control Owner { get; }

            public ControlCollection(Control owner)
            {
                Owner = owner;

                controls = new List<Control>();
            }

            public virtual Control this[int index] {
                get => controls[index];
                set => controls[index] = value;
            }

            public int Count => controls.Count;

            public bool IsReadOnly => false;

            public virtual void Add(Control item)
            {
                Owner.OnControlAdded(item);
                controls.Add(item);
            }

            public void Clear()
            {
                controls.Clear();
            }

            public bool Contains(Control item)
            {
                return controls.Contains(item);
            }

            public void CopyTo(Control[] array, int arrayIndex)
            {
                controls.CopyTo(array, arrayIndex);
            }

            public IEnumerator<Control> GetEnumerator()
            {
                return controls.GetEnumerator();
            }

            public bool Remove(Control item)
            {
                return controls.Remove(item);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return controls.GetEnumerator();
            }

            //public List<TType> GetEachFiltered<TType>() where TType : Control
            //    => (from control in this
            //       where control is TType
            //       select control as TType).ToList();

            ////public void ForeachFiltered<TType>(Action<TType> action) where TType : Control
            ////{

            ////}

            //public void SetFiltered<TType>(int index, TType item) where TType : Control
            //{
            //    for (int i = 0, filteredI = 0; i < controls.Count; i++)
            //        if (controls[i] is TType)
            //            if (filteredI == index) 
            //                controls[i] = item;
            //            else 
            //                filteredI++;
            //}

            //public void SetFilteredWhere<TType>(Predicate<TType> predicate, TType item) where TType : Control
            //{
            //    for (int i = 0; i < controls.Count; i++)
            //        if (controls[i] is TType control)
            //            if (predicate(control))
            //                controls[i] = item;
            //}
        }
    }
}
