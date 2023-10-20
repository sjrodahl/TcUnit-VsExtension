﻿using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using TcUnit.Options;
using Task = System.Threading.Tasks.Task;

namespace TcUnit.VisualStudio
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionHasMultipleProjects_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionHasSingleProject_string, PackageAutoLoadFlags.BackgroundLoad)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] 
    [Guid(PackageGuids.guidTcUnitPackageString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(GeneralOptionsPage), "TwinCAT", "TcUnit\\General", 0, 0, true)]
    public sealed class TcUnitPackage : AsyncPackage
    {
        public TcUnitPackage()
        {
            NotificationProvider.ServiceProvider = this;
            Package = this;
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await Commands.AddUnitTestSuiteCommand.InitializeAsync(this);
            await Commands.AddUnitTestCaseCommand.InitializeAsync(this);
        }

        public static Package Package;

        public static GeneralOptionsPage GetTestCaseTemplate()
        {
            return (GeneralOptionsPage)Package.GetDialogPage(typeof(GeneralOptionsPage));
        }
    }
}
