using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.WPF {
   public class Disposable<T> : IDisposable {
      private readonly ICollection<IObserver<T>> collection;
      private readonly IObserver<T> item;

      public Disposable(ICollection<IObserver<T>> collection, IObserver<T> item)
      {
         this.collection = collection;
         this.item = item;
         collection.Add(item);
      }

      public void Dispose()
      {
         collection.Remove(item);
      }
   }
}
