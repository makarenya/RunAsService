/*
 * RunAsService, the service to run non-service applications as a service
 *
 * Distributable under Academic Free License Version 1.2
 * See http://www.opensource.org/licenses/academic.php
 */

using System;
using System.Collections;
using System.Configuration;
using System.Xml;
using log4net;

namespace RunAsService
{
	/// <summary>
	/// Contains the configuration information of each application that needs to 
	/// be run as service.
	/// </summary>
	public class ServicesConfig : IEnumerable
	{

		private const string ServiceSetting = "service.settings";
		private const string NameSection = "name";
		private const string ExecSection = "executable";
		private const string ParamSection = "parameters";
        private const string StdoutSection = "stdout";
        private const string StderrSection = "stderr";
        private const string HomeSection = "home";

        private static readonly ILog Log = LogManager.GetLogger("ServicesConfig");
        
		private readonly ArrayList _serviceList = new ArrayList();
        
		internal ServicesConfig()
		{
			Log.Debug("cons");
		}

		internal void Add(XmlNode section) 
		{
			Log.Debug("+Add");
			Service service = null;

			try
			{
				var name = GetSectionText(section, NameSection);
				var executable = section[ExecSection]?.InnerText;
				var parameterList = GetSectionText(section, ParamSection);
                var stdout = GetSectionTextOrNull(section, StdoutSection);
                var stderr = GetSectionTextOrNull(section, StderrSection);
                var home = GetSectionTextOrNull(section, HomeSection);

				service = new Service(name, executable, parameterList, stdout, stderr, home);
				_serviceList.Add(service);
			}
			catch (NullReferenceException e)
			{
				Log.Error("Error in configuration. Is mandatory element <" + ExecSection + "> set?", e);
			}

			Log.Debug("-Add(" + service + ")");
		}



		private string GetSectionText(XmlNode section, string name)
		{
			Log.Debug("+getSectionText(" + section + ", " + name + ")");
			var elm = section[name];
		    var result = elm?.InnerText ?? "";
			Log.Debug("-getSectionText:" + result);
			return result;
		}

        private static string GetSectionTextOrNull(XmlNode section, string name)
        {
            var elm = section[name];
            return elm?.InnerText;
        }


		/// <summary>
		/// Retrieves the configuration settings as described in the application 
		/// configuration file.
		/// </summary>
		/// <returns>The configuration settings</returns>
		public static ServicesConfig GetConfig() 
		{
			Log.Debug("+getConfig");
			var config = (ServicesConfig)ConfigurationManager.GetSection(ServiceSetting);
			Log.Debug("-getConfig");
			return config;
		}

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator capable of enumrating over the configuration settings
		/// </summary>
		/// <returns>The enumerator as supplied by the serives List.</returns>
		public IEnumerator GetEnumerator()
		{
			return _serviceList.GetEnumerator();
		}

		#endregion

		/// <summary>
		/// Get's the total number of services in the configuration.
		/// </summary>
		public int Count => _serviceList.Count;
	}

}

