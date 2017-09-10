#region

using System.Reflection;
using System.Runtime.InteropServices;
using Advitex.ExceptionAnalizer.Resharper.Highlightings;
using JetBrains.Application.PluginSupport;
using JetBrains.ReSharper.Daemon;

#endregion

[assembly: AssemblyTitle("Advitex plugin for Resharper v7.1")]
[assembly: AssemblyDescription("Advitex plugin for Resharper v7.1")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Advitex")]
[assembly: AssemblyProduct("Advitex.ExceptionAnalizer")]
[assembly: AssemblyCopyright("Copyright © Advitex 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("ad53ffc9-b2ff-4574-9bbb-f63f7cba208d")]
[assembly: AssemblyVersion("0.2.3.29380")]
[assembly: AssemblyFileVersion("0.2.3.29380")]
[assembly: PluginTitle("Advitex Exception Analyzer")]
[assembly: PluginVendor("Advitex")]
[assembly: PluginDescription("Advitex plugin for Resharper v7.1")]


[assembly: RegisterConfigurableSeverity(NotDocumentedThrowHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Thrown exception is not documented",
    "Thrown uncatched exception is not documented",
    Severity.WARNING,
    false)]
[assembly: RegisterConfigurableSeverity(NotDocumentedArgumentNullExceptionHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Thrown exception of type ArgumentNullException is not documented",
    "Exception of type ArgumentNullException is not documented",
    Severity.SUGGESTION,
    false)]
[assembly: RegisterConfigurableSeverity(NotDocumentedExceptionHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Exception is not documented",
    "Exception is not documented",
    Severity.WARNING,
    false)]


[assembly: RegisterConfigurableSeverity(PossibleNotDocumentedThrowHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Perhaps, thrown exception is not documented",
    "Perhaps, thrown exception is not documented",
    Severity.HINT,
    false)]
[assembly: RegisterConfigurableSeverity(PossibleNotDocumentedArgumentNullExceptionHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Perhaps, exception of type ArgumentNullException is not documented",
    "Perhaps, exception of type ArgumentNullException is not documented",
    Severity.HINT,
    false)]
[assembly: RegisterConfigurableSeverity(PossibleNotDocumentedExceptionHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Perhaps, exception is not documented",
    "Perhaps, exception is not documented",
    Severity.HINT,
    false)]

[assembly: RegisterConfigurableSeverity(UncatchedThrowInDestructorHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Thrown uncatched exception in destructor",
    "Thrown uncatched exception in destructor",
    Severity.WARNING,
    false)]
[assembly: RegisterConfigurableSeverity(UncatchedExceptionInDestructorHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Uncatched exception in destructor",
    "Uncatched exception in destructor",
    Severity.WARNING,
    false)]

[assembly: RegisterConfigurableSeverity(ReThrowWithoutInnerExceptionHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Re-thrown exception in not contains full call stack",
    "Re-thrown exception is not contains full call stack",
    Severity.WARNING,
    false)]
[assembly: RegisterConfigurableSeverity(ReThrowWithoutFullCallStackHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Re-thrown exception is not contains an inner exception",
    "Re-thrown exception is not contains an inner exception",
    Severity.WARNING,
    false)]

[assembly: RegisterConfigurableSeverity(ThrownExceptionOfTypeExceptionHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Throwing exception of type Exception is a bad practice",
    "Throwing exception of type Exception is a bad practice",
    Severity.WARNING,
    false)]
[assembly: RegisterConfigurableSeverity(DocumentedExceptionTypeIsNotRecognizedHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Exception type is not recognized",
    "Exception type is not recognized",
    Severity.WARNING,
    false)]
[assembly: RegisterConfigurableSeverity(DocumentedExceptionIsNotThrownHighlighting.SeverityId,
    null,
    HighlightingGroupIds.CodeSmell,
    "[Exception Analyzer] Documented exception is not thrown by type member",
    "Documented exception is not thrown by type member",
    Severity.WARNING,
    false)]