using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rjw.Modules.Shared.Logs
{
	public interface ILog
	{
		void Debug(string message);
		void Debug(string message, Exception e);
		void Message(string message);
		void Message(string message, Exception e);
		void Warning(string message);
		void Warning(string message, Exception e);
		void Error(string message);
		void Error(string message, Exception e);
	}
}
