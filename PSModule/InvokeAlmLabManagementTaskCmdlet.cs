using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using PSModule.AlmLabMgmtClient.Result.Model;
using PSModule.AlmLabMgmtClient.SDK;
using PSModule.AlmLabMgmtClient.SDK.Interface;
using PSModule.AlmLabMgmtClient.SDK.Util;

namespace PSModule
{
    using C = Constants;
    using H = Helper;

    [Cmdlet(VerbsLifecycle.Invoke, "AlmLabManagementTask")]
    public class InvokeAlmLabManagementTaskCmdlet : AbstractLauncherTaskCmdlet, ILogger
    {
        private bool _printLineFeed = true;
        private readonly object _sync = new object();

        [Parameter(Position = 0, Mandatory = true)]
        public string ALMServerPath { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string ALMUserName { get; set; }

        [Parameter(Position = 2)]
        public string ALMPassword { get; set; }

        [Parameter(Position = 3, Mandatory = true)]
        public string ALMDomain { get; set; }

        [Parameter(Position = 4, Mandatory = true)]
        public string ALMProject { get; set; }

        [Parameter(Position = 5)]
        public string TestRunType { get; set; }

        [Parameter(Position = 6, Mandatory = true)]
        public string ALMEntityId { get; set; }

        [Parameter(Position = 7)]
        public string Description { get; set; }

        [Parameter(Position = 8, Mandatory = true)]
        public string TimeslotDuration { get; set; }

        [Parameter(Position = 9)]
        public string EnvironmentConfigurationID { get; set; }

        [Parameter(Position = 10)]
        public string ReportName { get; set; }

        [Parameter(Position = 11)]
        public string BuildNumber { get; set; }

        protected override string GetReportFilename()
        {
            return string.IsNullOrEmpty(ReportName) ? base.GetReportFilename() : ReportName;
        }

        public override Dictionary<string, string> GetTaskProperties()
        {
            LauncherParamsBuilder builder = new LauncherParamsBuilder();

            builder.SetRunType(RunType.AlmLabManagement);
            builder.SetAlmServerUrl(ALMServerPath);
            builder.SetAlmUserName(ALMUserName);
            builder.SetAlmPassword(ALMPassword);
            builder.SetAlmDomain(ALMDomain);
            builder.SetAlmProject(ALMProject);
            builder.SetBuildNumber(BuildNumber);

            switch (TestRunType)
            {
                case C.TEST_SET:
                    builder.SetTestRunType(RunTestType.TEST_SUITE);
                    break;
                case C.BVS:
                    builder.SetTestRunType(RunTestType.BUILD_VERIFICATION_SUITE);
                    break;
            }

            if (!string.IsNullOrEmpty(ALMEntityId))
            {
                int i = 1;
                foreach (string testSet in ALMEntityId.Split(C.LINE_FEED))
                {
                    builder.SetTestSet(i++, testSet.Replace(@"\", @"\\"));
                }
            }
            else
            {
                builder.SetAlmTestSet(string.Empty);
            }

            //set ALM mandatory parameters
            builder.SetAlmTimeout(TimeslotDuration);
            builder.SetAlmRunMode(AlmRunMode.RUN_PLANNED_HOST);
            builder.SetAlmRunHost(string.Empty);

            return builder.GetProperties();
        }

        protected override string GetRetCodeFileName()
        {
            return "TestRunReturnCode.txt";
        }

        protected async override void ProcessRecord()
        {
            string paramFileName = string.Empty, resultsFileName;
            try
            {
                Dictionary<string, string> properties;
                try
                {
                    properties = GetTaskProperties();
                    if (properties == null || !properties.Any())
                    {
                        throw new AlmException("Invalid or missing properties!");
                    }
                }
                catch (Exception e)
                {
                    ThrowTerminatingError(new ErrorRecord(e, nameof(GetTaskProperties), ErrorCategory.ParserError, string.Empty));
                    return;
                }

                string ufttfsdir = Environment.GetEnvironmentVariable(UFT_LAUNCHER);
                string propdir = Path.GetFullPath(Path.Combine(ufttfsdir, PROPS));

                if (!Directory.Exists(propdir))
                    Directory.CreateDirectory(propdir);

                string resdir = Path.GetFullPath(Path.Combine(ufttfsdir, $@"res\Report_{properties[BUILD_NUMBER]}"));

                if (!Directory.Exists(resdir))
                    Directory.CreateDirectory(resdir);

                string timeSign = DateTime.Now.ToString(DDMMYYYYHHMMSSSSS);

                paramFileName = Path.Combine(propdir, $"Props{timeSign}.txt");
                resultsFileName = Path.Combine(resdir, $"Results{timeSign}.xml");

                properties.Add(RESULTS_FILENAME, resultsFileName.Replace(@"\", @"\\")); // double backslashes are expected by HpToolsLauncher.exe (JavaProperties.cs, in LoadInternal method)

                if (!SaveProperties(paramFileName, properties))
                {
                    WriteError(new ErrorRecord(new Exception("Cannot save properties"), nameof(SaveProperties), ErrorCategory.WriteError, string.Empty));
                    return;
                }

                //run the build task
                var runStatus = await Run(resultsFileName);

                //collect results
                //bool hasResults = CollateResults(resultsFileName, _launcherConsole.ToString(), resdir);
                
                CollateRetCode(resdir, (int)runStatus);
            }
            catch (IOException ioe)
            {
                WriteError(new ErrorRecord(ioe, nameof(IOException), ErrorCategory.ResourceExists, string.Empty));
            }
            catch (ThreadInterruptedException e)
            {
                WriteError(new ErrorRecord(e, nameof(ThreadInterruptedException), ErrorCategory.OperationStopped, "ThreadInterruptedException target"));
                //TODO stop the processing
            }
        }

        private async Task<RunStatus> Run(string resFilename)
        {
            var res = RunStatus.FAILED;
            Args args = new Args
            {
                ClientType = string.Empty, // TODO we may need an input box control for this, like in Jenkins
                Description = Description,
                Domain = ALMDomain,
                Project = ALMProject,
                Username = ALMUserName,
                Password = ALMPassword,
                ServerUrl = ALMServerPath,
                Duration = TimeslotDuration,
                EntityId = ALMEntityId,
                RunType = TestRunType
            };
            var client = new RestClient(args.ServerUrl, args.Domain, args.Project, args.Username, this);
            var runMgr = new RunManager(client, args);
            TestSuites testsuites = await runMgr.Execute();
            await SaveResults(resFilename, testsuites);
            if (!testsuites.ListOfTestSuites.Any(ts => ts.ListOfTestCases.Any(tc => tc.Status.In(JUnitTestCaseStatus.ERROR, JUnitTestCaseStatus.FAILURE))))
            {
                res = RunStatus.PASSED;
            }
            return res;
        }

        private async Task SaveResults(string filePath, TestSuites testsuites)
        {
            if (testsuites != null)
            {
                string xml;
                try
                {
                    xml = testsuites.ToXML();
                }
                catch (Exception e)
                {
                    LogError(e.Message, ErrorCategory.ParserError);
                    return;
                }
                try
                {
                    using StreamWriter file = new StreamWriter(filePath, true);
                    await file.WriteAsync(xml);
                }
                catch(Exception e)
                {
                    LogError(e.Message, ErrorCategory.WriteError);
                }
            }
        }


        public void LogInfo(string msg)
        {
            lock (_sync)
            {
                if (_printLineFeed)
                {
                    WriteVerbose($"{C.LINE_FEED}");
                    _printLineFeed = false;
                }
                WriteVerbose(msg);
            }
        }

        public void ShowProgress(char @char)
        {
            lock (_sync)
            {
                WriteVerbose($"{@char}");
                if (!_printLineFeed)
                    _printLineFeed = true;
            }
        }

        public void LogError(string err, ErrorCategory categ = ErrorCategory.NotSpecified, bool isCritical = false, [CallerMemberName] string methodName = "")
        {
            lock (_sync)
            {
                if (_printLineFeed)
                {
                    WriteVerbose($"{C.LINE_FEED}");
                    _printLineFeed = false;
                }
                if (isCritical)
                    ThrowTerminatingError(new ErrorRecord(new Exception(err), methodName, categ, string.Empty));
                else
                    WriteError(new ErrorRecord(new Exception(err), methodName, categ, string.Empty));
            }

        }
    }
}