using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF
{
    class DataNumber
    {
        public ObservableCollection<int> col = new ObservableCollection<int>();
        Random rand = new Random();

        public void AddItem()
        {
            for (int i = 0; i < 30; i++)
            {
                col.Add(rand.Next(10, 500));
            }
        }

        public async void SortItems()
        {
            int index, temp;
            for (int i = 0; i < col.Count - 1; i++)
            {
                index = i;
                for (int j = i+1; j < col.Count; j++)
                {
                    if (col[index] > col[j]) { index = j; }
                }
                await Task.Delay(10);
                if (index != i)
                {
                    temp = col[i];
                    col[i] = col[index];
                    col[index] = temp;
                }
            }
        }
    }
}
