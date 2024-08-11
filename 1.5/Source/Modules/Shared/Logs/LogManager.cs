using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rjw.Modules.Shared.Logs
{
	public static class LogManager
	{
		private class Logger : ILog
		{
			private readonly string _loggerTypeName;
			private readonly ILogProvider _logProvider;

			public Logger(string typeName)
			{
				_loggerTypeName = typeName;
			}
			public Logger(string typeName, ILogProvider logProvider)
			{
				_loggerTypeName = typeName;

				_logProvider = logProvider;
			}

			public void Debug(string message)
			{
				LogDebug(CreateLogMessage(message));
			}
			public void Debug(string message, Exception exception)
			{
				LogDebug(CreateLogMessage(message, exception));
			}

			public void Message(string message)
			{
				LogMessage(CreateLogMessage(message));
			}
			public void Message(string message, Exception exception)
			{
				LogMessage(CreateLogMessage(message, exception));
			}

			public void Warning(string message)
			{
				LogWarning(CreateLogMessage(message));
			}
			public void Warning(string message, Exception exception)
			{
				LogWarning(CreateLogMessage(message, exception));
			}

			public void Error(string message)
			{
				LogError(CreateLogMessage(message));
			}
			public void Error(string message, Exception exception)
			{
				LogError(CreateLogMessage(message, exception));
			}

			private string CreateLogMessage(string message)
			{
				return $"[{_loggerTypeName}] {message}";
			}
			private string CreateLogMessage(string message, Exception exception)
			{
				return $"{CreateLogMessage(message)}{Environment.NewLine}{exception}";
			}

			private void LogDebug(string message)
			{
				if (_logProvider?.IsActive != false && RJWSettings.DevMode == true)
				{
					ModLog.Message(message);
				}
			}
			private void LogMessage(string message)
			{
				if (_logProvider?.IsActive != false)
				{
					ModLog.Message(message);
				}
			}
			private void LogWarning(string message)
			{
				if (_logProvider?.IsActive != false)
				{
					ModLog.Warning(message);
				}
			}
			private void LogError(string message)
			{
				if (_logProvider?.IsActive != false)
				{
					ModLog.Error(message);
				}
			}
		}

		public static ILog GetLogger<TType, TLogProvider>()
			where TLogProvider : ILogProvider, new()
		{
			return new Logger(typeof(TType).Name, new TLogProvider());
		}
		public static ILog GetLogger<TType>()
		{
			return new Logger(typeof(TType).Name);
		}
		public static ILog GetLogger(string staticTypeName)
		{
			return new Logger(staticTypeName);
		}
	}
}
