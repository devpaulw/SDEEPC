using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEE_Editor.Miscellaneous
{
    static class ObservableCollectionExtension // Can be used for every collection later
    {
        public static void TryMove<T>(this ObservableCollection<T> obsCollection, int oldIndex, int newIndex)
        {
            if (oldIndex >= 0 
                && oldIndex < obsCollection.Count
                && newIndex >= 0 
                && newIndex < obsCollection.Count) // Out of range, clean catch way
            {
                obsCollection.Move(oldIndex, newIndex);
            }
        }

        public static void TryMoveElementBy<T>(this ObservableCollection<T> obsCollection, T element, int count)
        {
            int index = obsCollection.IndexOf(element);
            // Try Move, don't worry, it handles two issues naturally
            obsCollection.TryMove(index, index + count);
        }
    }
}
