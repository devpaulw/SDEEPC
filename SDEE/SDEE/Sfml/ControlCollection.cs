using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE.Sfml
{

    class ControlCollection : ICollection<Control>
    {
        readonly List<Control> controls;

        public Control Owner { get; }

        public ControlCollection(Control owner)
        {
            Owner = owner;

            controls = new List<Control>();
        }

        public Control this[int index] {
            get => controls[index];
            set => controls[index] = value;
        }

        public int Count => controls.Count;

        public bool IsReadOnly => false;

        public void Add(Control item)
        {
            item.Parent = Owner; // Set parent of the item just added
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
    }
}
