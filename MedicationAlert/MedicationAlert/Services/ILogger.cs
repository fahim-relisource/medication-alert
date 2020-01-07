using System.Runtime.CompilerServices;

namespace MedicationAlert.Services
{
	public interface ILogger
	{
		void D(object obj, [CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
												[CallerLineNumber] int line = 0);

		void E(object obj, [CallerFilePath] string file = "",
			[CallerMemberName] string member = "",
												[CallerLineNumber] int line = 0);
	}
}
