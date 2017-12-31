/*
 * RunAsService, the service to run non-service applications as a service
 *
 * Distributable under Academic Free License Version 1.2
 * See http://www.opensource.org/licenses/academic.php
 */

using System.Configuration;
using System.Xml;
using log4net;

namespace RunAsService
{
	/// <summary>
	/// Processes the config section in the application.config file.
	/// </summary>
	public class ConfigSectionHandler : IConfigurationSectionHandler
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigSectionHandler));
		private readonly ServicesConfig _config = new ServicesConfig();

		/// <summary>
		/// Processes one single custom section in the application configuration file.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="context"></param>
		/// <param name="section">The section in XmlNode format</param>
		/// <returns>The resulting configuration object</returns>
		public object Create(object parent,  object context,  XmlNode section) 
		{
			if (Log.IsDebugEnabled)
			{
				Log.Debug("+Create(" + section + ")");
			}

		    var nodes = section.SelectNodes("service");
		    if (nodes != null)
		    {
		        foreach (XmlNode item in nodes)
		        {
		            _config.Add(item);
		        }
		    }

            if (Log.IsDebugEnabled)
			{
				Log.Debug("-Create");
			}

			return _config;
		}
	}
}
