// Guids.cs
// MUST match guids.h
using System;

namespace Adage.VSGrunt
{
    static class GuidList
    {
        public const string guidParfait_ToolMenuPkgString = "7b17ef52-ad00-4aec-8a95-b4cdd5cb41e3";
        public const string guidParfait_ToolMenuCmdSetString = "5ff48643-9646-4831-af38-df0548f03256";
        public const string guidParfait_ToolMenuOutputPaneString = "5ff48643-9646-4831-af38-df0548f03257";

        public const string UICONTEXT_SolutionExistsString = "f1536ef8-92ec-443c-9ed7-fdadf150da82";

        public static readonly Guid guidParfait_ToolMenuCmdSet = new Guid(guidParfait_ToolMenuCmdSetString);
        public static readonly Guid guidParfait_ToolMenuOutputPane = new Guid(guidParfait_ToolMenuOutputPaneString);

    };
}