using System;
using System.Collections.ObjectModel;
using AssemblyBrowserGUI.ViewModel;
using System.Windows.Input;
using AssemblyBrowserGUI.Model;
using AssemblyBrowserLib;
using Infrastructure.WPF;

namespace AssemblyBrowser.WPF {

   public class RootViewModel : ViewModel {

      public RootViewModel()
      {
         var assemblyViewModel = new AssemblyViewModel();
         var commandViewModel = new CommandViewModel();

         commandViewModel.Subscribe(assemblyViewModel);

         Collection.Add(assemblyViewModel);
         Collection.Add(commandViewModel);
         Content = nameof(RootViewModel);
      }
   }


   public class AssemblyViewModel : ViewModel, IObserver<AssemblyDO> {
      public void OnCompleted() {
         throw new NotImplementedException();
      }

      public void OnError(Exception error) {
         throw new NotImplementedException();
      }

      public void OnNext(AssemblyDO value) {
         Content = value;
      }
   }

   public class CommandViewModel : ViewModel, IObservable<AssemblyDO> {
      private readonly Collection<IObserver<AssemblyDO>> collection = new();

      public CommandViewModel() {

         Command = new RelayCommand<object>(_ => Load());
         Content = "Load";

         void Load() {
            if (dialogService.OpenFileDialog()) {
               var assembly = Model.LoadAssembly(dialogService.FilePath);
               foreach (var observer in collection) {
                  observer.OnNext(assembly);
               }
            }
         }
      }

      public ICommand Command { get; }

      public IDisposable Subscribe(IObserver<AssemblyDO> observer) {
         return new Disposable<AssemblyDO>(collection, observer);
      }
   }
}


