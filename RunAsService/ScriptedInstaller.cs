using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using log4net;
using Microsoft.Win32;

namespace RunAsService
{
	/// <summary>
	/// This is a custom service installer.
	/// Makes it possible to install the service using the .Net SDK supplied 
	/// installutil application.
	/// </summary>
	[RunInstaller(true)]
	public class ScriptedInstaller : Installer
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(ScriptedInstaller));
		private readonly ServiceInstaller _serviceInstaller;
		private readonly ServiceProcessInstaller _processInstaller;

		/// <summary>
		/// Initializes the installer
		/// </summary>
		public ScriptedInstaller()
		{
			if (Log.IsDebugEnabled) 
			{
				Log.Debug("+cons");
			}

			_processInstaller = new ServiceProcessInstaller();
			_serviceInstaller = new ServiceInstaller();

			_processInstaller.Account = ServiceAccount.User;
			_serviceInstaller.StartType = ServiceStartMode.Automatic;
			_serviceInstaller.ServiceName = "RunAsService";

			Installers.Add(_serviceInstaller);
			Installers.Add(_processInstaller);

			if (Log.IsDebugEnabled) 
			{
				Log.Debug("-cons");
			}
		}

		#region Access parameters
		/// <summary>
		/// Returns the value of th eparameter indicated by key.
		/// </summary>
		/// <param name="key">Context parameter key</param>
		/// <returns>Context parameter specified by key</returns>
		public string GetContextParameter(string key)
		{
			if (Log.IsDebugEnabled)
			{
				Log.Debug("+GetContextParameter(" + key + ")");
			}

			var result = "";
			try
			{
				result = Context.Parameters[key];
			}
			catch
			{
				if (Log.IsWarnEnabled)
				{
					Log.Warn("#Context parameter not found:" + key);
				}
			}

			if (Log.IsDebugEnabled)
			{
				Log.Debug("-GetContextParameter:" + result);
			}

			return result;
		}
		#endregion

		/// <summary>
		/// This method is run before the install process.
		/// It is overriden to set the following parameters:
		/// - service name taken from the /name switch
		/// - account type, taken frm the /account switch
		/// - user name, for a user account, taken from the /user switch
		/// - password, for a user account, taken from the /password switch
		/// 
		/// If the account type is set to user, but the user name and/or 
		/// password are not set, the user is prompted for the credentials 
		/// to use.
		/// </summary>
		/// <param name="savedState"></param>
		protected override void OnBeforeInstall(IDictionary savedState)
		{
			if (Log.IsDebugEnabled)
			{
				Log.Debug("+OnBeforeInstall(" + savedState + ")");
			}

			base.OnBeforeInstall(savedState);
			var isUserAccount = false;

			var name = GetContextParameter("name");
			_serviceInstaller.ServiceName = name;

			var account = GetContextParameter("account");
			switch (account)
			{
				case "user":
					_processInstaller.Account = ServiceAccount.User;
					isUserAccount = true;
					break;
				case "localservice":
					_processInstaller.Account = ServiceAccount.LocalService;
					break;
				case "localsystem":
					_processInstaller.Account = ServiceAccount.LocalSystem;
					break;
				case "networkservice":
					_processInstaller.Account = ServiceAccount.NetworkService;
					break;
				default:
					_processInstaller.Account = ServiceAccount.User;
					isUserAccount = true;
					break;
			}

			if (isUserAccount)
			{
				var userName = GetContextParameter("user");
				_processInstaller.Username = userName;

				var password = GetContextParameter("password");
				_processInstaller.Password = password;
			}

			if (Log.IsDebugEnabled)
			{
				Log.Debug("-OnBeforeInstall");
			}
		}


		/// <summary>
		/// Modify the registry to install the new service.
		/// </summary>
		/// <param name="stateServer"></param>
		public override void Install(IDictionary stateServer)
		{
			if (Log.IsDebugEnabled)
			{
				Log.Debug("+install(" + stateServer + ")");
			}

			base.Install(stateServer);

		    using (var system = Registry.LocalMachine.OpenSubKey("System"))
		    {
		        using (var currentControlSet = system?.OpenSubKey("CurrentControlSet"))
		        {
		            using (var services = currentControlSet?.OpenSubKey("Services"))
		            {
		                using (var service = services?.OpenSubKey(_serviceInstaller.ServiceName, true))
		                {
		                    service?.SetValue("Description", "Runs console applications as windows service");

		                    var imagePath = (string) service?.GetValue("ImagePath");
		                    Log.Info("ImagePath: " + imagePath);

		                    imagePath += " -Name " + _serviceInstaller.ServiceName;
		                    service?.SetValue("ImagePath", imagePath);
		                    using (service?.CreateSubKey("Parameters"))
		                    {
		                    }
		                }
		            }
		        }
		    }

		    if (Log.IsDebugEnabled)
			{
				Log.Debug("-install");
			}
		}

		/// <summary>
		/// Uninstall based on the services name.
		/// </summary>
		/// <param name="stateServer"></param>
		protected override void OnBeforeUninstall(IDictionary stateServer)
		{
			if (Log.IsDebugEnabled)
			{
				Log.Debug("+OnBeforeUninstall(" + stateServer + ")");
			}

			base.OnBeforeUninstall(stateServer);
			_serviceInstaller.ServiceName = GetContextParameter("name");

			if (Log.IsDebugEnabled)
			{
				Log.Debug("-OnBeforeUninstall");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stateServer"></param>
		public override void Uninstall(IDictionary stateServer)
		{
			if (Log.IsDebugEnabled)
			{
				Log.Debug("+Uninstall(" + stateServer + ")");
			}

			base.Uninstall(stateServer);

		    using (var system = Registry.LocalMachine.OpenSubKey("System"))
		    {
		        using (var currentControlSet = system?.OpenSubKey("CurrentControlSet"))
		        {
		            using (var services = currentControlSet?.OpenSubKey("Services"))
		            {
		                using (var service = services?.OpenSubKey(_serviceInstaller.ServiceName, true))
		                {
		                    service?.DeleteSubKeyTree("Parameters");
		                }
                    }
		        }
		    }

		    if (Log.IsDebugEnabled)
			{
				Log.Debug("-Uninstall");
			}
		}
	}
}
