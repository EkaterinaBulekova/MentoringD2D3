using System;
using TasksInterface;

namespace Task6
{
    public class PrintableCollectionCreator : IStartable
    {
         private readonly int _count;

         public PrintableCollectionCreator(int count)
         {
            _count = count;
         }

         public void Start()
         {
            using (PrintableCollection collection = new PrintableCollection())
            {
                for (int i = 0; i < _count; i++) collection.AddItem("Item " + i);
            }
            Console.WriteLine();
         }
    }
}
