/*
 * RunAsService, the service to run non-service applications as a service
 *
 * Distributable under Academic Free License Version 1.2
 * See http://www.opensource.org/licenses/academic.php
 */

using System.ComponentModel;
using System.Configuration;
using System.ServiceProcess;
using log4net;

namespace RunAsService
{
	/// <summary>
	/// Main class for running multiple applications as a windows service.
	/// </summary>
	public class RunAsService : ServiceBase
	{
		/// <summary>
		/// class level Log4Net logger
		/// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(RunAsService));

	    /// <summary> 
	    /// Required designer variable.
	    /// </summary>
	    private readonly Container _components = null;

	    private readonly Service _service;


        /// <summary>
        /// Creates an array of services to run, and runs each service in the array.
        /// </summary>
        public static void Main()
		{
            // Read logging config from default application config file.
            //
            //LoggingConfiguration config = new LoggingConfiguration();
			Log.Debug("+Main");

			ServicesConfig config;
			try
			{
				config = ServicesConfig.GetConfig();
			}
			catch (ConfigurationException e)
			{
				Log.Fatal("Error in configuration. Exiting...", e);
				return;
			}

			if (null == config) 
			{
				Log.Warn("No services defined. Exiting...");
				return;
			}

			var servicesToRun = new ServiceBase[config.Count];
			var index = 0;

			foreach (Service service in config)
			{
				servicesToRun[index] = new RunAsService(service);
				++index;
			}
			Run(servicesToRun);

			Log.Debug("-Main");
		}

		/// <summary>
		/// Construct the event log if it does not exist
		/// </summary>
		/// <param name="aService">
		/// Properties of the application that is run as a service.
		/// </param>
		public RunAsService(Service aService)
		{
			Log.Debug("+cons");

			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			// Initialize the service info
			_service = aService;

			Log.Debug("-cons");
		}



		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// RunAsService
			// 
			ServiceName = "RunAsService";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
			    _components?.Dispose();
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Starts the application that needs to be run as service.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			Log.Debug("+OnStart");
			_service.Start();
            Log.Info("Started " + _service);
			Log.Debug("-OnStart");
		}

		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			Log.Debug("+OnStop");
			_service.Stop();
			Log.Info("Stopped " + _service);
			Log.Debug("-OnStop");
		}
	}
}

