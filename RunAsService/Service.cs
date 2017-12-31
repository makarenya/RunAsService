/*
 * RunAsService, the service to run non-service applications as a service
 *
 * Distributable under Academic Free License Version 1.2
 * See http://www.opensource.org/licenses/academic.php
 */

using System;
using System.Diagnostics;
using System.IO;
using log4net;

namespace RunAsService
{
	/// <summary>
	/// Describes the properties of an application that needs to be run as a service.
	/// </summary>

	public class Service
	{
        private static readonly ILog Log = LogManager.GetLogger("Service");

		private readonly string _name;
		private readonly string _executable;
		private readonly string _parameterList;
		private Process _process;
        private readonly StreamWriter _stdoutWriter;
        private readonly StreamWriter _stderrWriter;
        private readonly string _home;
        private volatile bool _exitRequested;

	    /// <summary>
	    /// Constructs a Service object.
	    /// </summary>
	    /// <param name="aName">Descriptive name of the service</param>
	    /// <param name="anExecutable">Fullpath to the executable that is run as service</param>
	    /// <param name="aParamList">List of commandline parameters</param>
	    /// <param name="stdoutName">Name of stdout redirection file (or null for no redirection)</param>
	    /// <param name="stderrName">Name of stderr redirection file (or null for no redirection)</param>
	    /// <param name="home">Home directory</param>
	    public Service(string aName, string anExecutable, string aParamList, string stdoutName, string stderrName, string home)
		{
			Log.Debug("+cons");
			_name = aName;
			_executable = anExecutable;
			_parameterList = aParamList;
            if (stdoutName != null)
            {
                _stdoutWriter = new StreamWriter(stdoutName) {AutoFlush = true};
            }
            if (stderrName != null)
            {
                _stderrWriter = new StreamWriter(stderrName) {AutoFlush = true};
            }
            _home = home;
			Log.Debug("-cons");
		}

		/// <summary>
		/// Starts the service without a window.
		/// </summary>
		public void Start()
		{
			Log.Debug("+start:" + _name);

		    var startInfo = new ProcessStartInfo
		    {
		        FileName = _executable,
		        Arguments = _parameterList,
		        WindowStyle = ProcessWindowStyle.Hidden,
		        RedirectStandardError = true,
		        RedirectStandardOutput = true,
		        UseShellExecute = false
		    };
		    if (_home != null)
                startInfo.WorkingDirectory = _home;

		    _process = new Process {StartInfo = startInfo};

		    // Install output redirection handlers
            //
            _process.OutputDataReceived += (sender, args) =>
            {
                _stdoutWriter.WriteLine(args.Data);
            };
            _process.ErrorDataReceived += (sender, args) =>
            {
                _stderrWriter.WriteLine(args.Data);
            };

            // Install an exit handler to let us know if the process exited wihtout us 
            // requesting it.
            //
            _process.Exited += (sender, args) =>
            {
                if (!_exitRequested)
                    Log.Error("Process exited prematurely: " + this);
                else
                    Log.Debug("Process exited successfully on demand: " + this);
            };

			try 
			{
				_process.Start();
                _process.BeginErrorReadLine();
                _process.BeginOutputReadLine();
			} 
			catch (Exception e)
			{
				Log.Error("Unable to start service " + this, e);
				_process = null;
			}

			Log.Debug("-start");
		}

		/// <summary>
		/// Stops the service and destroy's the associated process.
		/// If the process exists its kill method is called to end the process.
		/// </summary>
		public void Stop()
		{
			Log.Debug("+stop");

			if (null != _process)
			{
                try
                {
                    _exitRequested = true;
                    _process.Kill();
                }
                catch (Exception e)
                {
                    Log.Error("Unable to kill process (probably already dead)" + this, e);
                }
				_process.Close();
                try
                {
                    _stderrWriter?.Close();
                    _stdoutWriter?.Close();
                }
                catch (Exception e)
                {
                    Log.Error("Unable to kill stream gobblers (probably already dead)" + this, e);
                }
			}
			else
			{
				Log.Error("Service " + _name + " is not started");
			}

			Log.Debug("-stop");
		}

		/// <summary>
		/// Constructs a concatenation of the properties of the service.
		/// </summary>
		/// <returns>The string concationation of the service's properties</returns>
		public override string ToString()
		{
			return _name + ":" + _executable + " " + _parameterList;
		}
	}
}

