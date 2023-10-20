/*
 * Certain versions of software accessible here may contain branding from Hewlett-Packard Company (now HP Inc.) and Hewlett Packard Enterprise Company.
 * This software was acquired by Micro Focus on September 1, 2017, and is now offered by OpenText.
 * Any reference to the HP and Hewlett Packard Enterprise/HPE marks is historical in nature, and the HP and Hewlett Packard Enterprise/HPE marks are the property of their respective owners.
 * __________________________________________________________________
 * MIT License
 *
 * Copyright 2012-2023 Open Text
 *
 * The only warranties for products and services of Open Text and
 * its affiliates and licensors ("Open Text") are as may be set forth
 * in the express warranty statements accompanying such products and services.
 * Nothing herein should be construed as constituting an additional warranty.
 * Open Text shall not be liable for technical or editorial errors or
 * omissions contained herein. The information contained herein is subject
 * to change without notice.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ___________________________________________________________________
 */


namespace PSModule
{
    public enum RunType
    {
        Alm,
        AlmLabManagement,
        FileSystem,
        LoadRunner
    }

    public enum AlmRunMode
    {
        RUN_NONE,
        RUN_LOCAL,
        RUN_REMOTE,
        RUN_PLANNED_HOST
    }

    public enum RunTestType
    {
        TEST_SUITE,
        BUILD_VERIFICATION_SUITE
    }

    public enum ArtifactType
    {
        onlyReport,
        onlyArchive,
        bothReportArchive,
        None
    }

    public enum RunStatus
    {
        PASSED = 0,
        FAILED = -1,
        UNSTABLE = -2,
        CANCELED = -3,
        UNDEFINED = -9
    }
    public enum LauncherExitCode
    {
        Passed = 0,
        Failed = -1,
        PartialFailed = -2,
        Aborted = -3,
        Unstable = -4,
        AlmNotConnected = -5,
        Closed = -1073741510 // 0xC000013A STATUS_CONTROL_C_EXIT https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-erref/596a1078-e883-4972-9bbc-49e60bebca55
    }

    public enum EnvType
    {
        None,
        Mobile,
        Web
    }
}
