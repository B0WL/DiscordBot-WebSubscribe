using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrawlBot.ViewModel.Command
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action Action;

        public Command(Action action)
        {
            Action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Action.Invoke();
        }
    }
}
