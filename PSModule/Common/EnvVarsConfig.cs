﻿/*
 * MIT License https://github.com/MicroFocus/ADM-TFS-Extension/blob/master/LICENSE
 *
 * Copyright 2016-2024 Open Text
 *
 * The only warranties for products and services of Open Text and its affiliates and licensors ("Open Text") are as may be set forth in the express warranty statements accompanying such products and services.
 * Nothing herein should be construed as constituting an additional warranty.
 * Open Text shall not be liable for technical or editorial errors or omissions contained herein. 
 * The information contained herein is subject to change without notice.
 */

namespace PSModule.Common
{
    public class EnvVarsConfig(string storageAccount, string container, string leaveUftOpenIfVisible = "") : IConfig
    {
        private readonly string _storageAccount = storageAccount;
        private readonly string _container = container;
        private readonly string _leaveUftOpenIfVisible = leaveUftOpenIfVisible;

        public string StorageAccount => _storageAccount;
        public string Container => _container;
        public string LeaveUftOpenIfVisible => _leaveUftOpenIfVisible;
    }
}