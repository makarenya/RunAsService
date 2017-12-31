using log4net;

using System;
using System.Timers;

namespace nl.dulsoft.console.runasservice.net.demo
{
	/// <summary>
	/// This class demonstrates the use of RunAsService to run a .Net console 
	/// application as a windows service.
	/// The class starts up a thread that writes a message to the log4net logfile 
	/// every 1 minute, then sleeps for a minute.
	/// Indeed not a very usefull application in itself, but a proper 
	/// demonstration of what RunAsService can do.
	/// </summary>
	class DotNetDemo
	{
		/// <summary>
		/// Class level debug logger
		/// </summary>
		private static readonly ILog logger = LogManager.GetLogger(typeof(DotNetDemo));

		/// <summary>
		/// Class level timer logger
		/// </summary>
		private static readonly ILog demoLog = LogManager.GetLogger("Timer");

		/// <summary>
		/// Timer used to write a message on a scheduled interval
		/// </summary>
		private Timer clock = null;

		/// <summary>
		/// Timer interval set to 1 minute
		/// </summary>
		private static readonly double interval = 1000 * 60;

		private DotNetDemo()
		{
			if (logger.IsDebugEnabled)
			{
				logger.Debug("+DotNetDemo");
			}

			clock = new Timer();
			clock.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			clock.Interval = interval;

			if (logger.IsDebugEnabled)
			{
				logger.Debug("-DotNetDemo");
			}
		}

		private static string GetTime()
		{
			if (logger.IsDebugEnabled)
			{
				logger.Debug("+GetTime");
			}

			DateTime now = DateTime.Now;

			string result = now.Hour + ":" + now.Minute + ":" + now.Second;

			if (logger.IsDebugEnabled)
			{
				logger.Debug("-GetTime");
			}

			return result;
		}		

		private static void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			if (logger.IsDebugEnabled)
			{
				logger.Debug("+OnTimedEvent");
			}

			demoLog.Info("Timed event occured. Current time is: " + GetTime());

			if (logger.IsDebugEnabled)
			{
				logger.Debug("-OnTimedEvent");
			}
		}

		private void Run()
		{
			if (logger.IsDebugEnabled)
			{
				logger.Debug("+Run");
			}

			clock.Enabled = true;

			Console.WriteLine("Press q to quit");
			while (Console.Read() != 'q');

			if (logger.IsDebugEnabled)
			{
				logger.Debug("-Run");
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// Creates and starts the thread.
		/// Waits until teh thread is stopped.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (logger.IsDebugEnabled)
			{
				logger.Debug("+Main");
			}

			DotNetDemo demo = new DotNetDemo();
			demo.Run();

			//
			// TODO: Add code to start application here
			//
			if (logger.IsDebugEnabled)
			{
				logger.Debug("-Main");
			}
		}
	}
}
