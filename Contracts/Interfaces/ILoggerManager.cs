
namespace Contracts.Interfaces;
//This interface defines special methods for LoggerManager
public interface ILoggerManager
{
	void LogInfo(string message);
	void LogWarn(string message);
	void LogDebug(string message);
	void LogError(string message);
}