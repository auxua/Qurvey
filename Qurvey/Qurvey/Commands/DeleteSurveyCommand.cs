using System;
using System.Windows.Input;
using Qurvey.Shared.Models;

namespace Qurvey.Commands
{
	public class DeleteSurveyCommand : ICommand 
	{
		#region ICommand implementation

		public event EventHandler CanExecuteChanged;

		public bool CanExecute (object parameter)
		{
			return parameter is Survey;
		}

		public void Execute (object parameter)
		{
			Survey survey = (Survey)parameter;

		}

		#endregion

		public DeleteSurveyCommand ()
		{
		}
	}
}

