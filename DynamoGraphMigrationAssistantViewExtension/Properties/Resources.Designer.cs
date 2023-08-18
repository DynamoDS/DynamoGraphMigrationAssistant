﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DynamoGraphMigrationAssistant.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DynamoGraphMigrationAssistant.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stop.
        /// </summary>
        public static string CancelButtonText {
            get {
                return ResourceManager.GetString("CancelButtonText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        public static System.Drawing.Bitmap checkmark {
            get {
                object obj = ResourceManager.GetObject("checkmark", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Target Dynamo Version:.
        /// </summary>
        public static string DynamoVersionsMsg {
            get {
                return ResourceManager.GetString("DynamoVersionsMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Graphs in the source folder will be migrated to the selected target version. Graphs already in the target version will be copied with no changes..
        /// </summary>
        public static string DynamoVersionsTooltip {
            get {
                return ResourceManager.GetString("DynamoVersionsTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start.
        /// </summary>
        public static string ExportButtonText {
            get {
                return ResourceManager.GetString("ExportButtonText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dynamo Graph Migration Assistant.
        /// </summary>
        public static string ExtensionName {
            get {
                return ResourceManager.GetString("ExtensionName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} was in target Dynamo version and copied to output folder with no changes..
        /// </summary>
        public static string FileCopiedLogMessage {
            get {
                return ResourceManager.GetString("FileCopiedLogMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully migrated {0} graphs.
        /// </summary>
        public static string FinishMsg {
            get {
                return ResourceManager.GetString("FinishMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Migration finished..
        /// </summary>
        public static string FinishMsgTitle {
            get {
                return ResourceManager.GetString("FinishMsgTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully exported {0} image..
        /// </summary>
        public static string FinishSingleMsg {
            get {
                return ResourceManager.GetString("FinishSingleMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Swap Line Breaks to Pinned Notes.
        /// </summary>
        public static string FixInputLinebreaksCheckboxMsg {
            get {
                return ResourceManager.GetString("FixInputLinebreaksCheckboxMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If a node name includes a line break, this option will separate text before the line break into a pinned note..
        /// </summary>
        public static string FixInputLinebreaksCheckboxTooltip {
            get {
                return ResourceManager.GetString("FixInputLinebreaksCheckboxTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Force input order.
        /// </summary>
        public static string FixInputOrderCheckboxMsg {
            get {
                return ResourceManager.GetString("FixInputOrderCheckboxMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Appends a prefix (001|, 002|…) to the names of nodes marked as input. Ordering begins with nodes closest to the top of the workspace..
        /// </summary>
        public static string FixInputOrderCheckboxTooltip {
            get {
                return ResourceManager.GetString("FixInputOrderCheckboxTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cleanup node layout.
        /// </summary>
        public static string FixNodeSpacingCheckboxMsg {
            get {
                return ResourceManager.GetString("FixNodeSpacingCheckboxMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adjust node spacing to avoid overlapping nodes..
        /// </summary>
        public static string FixNodeSpacingTooltip {
            get {
                return ResourceManager.GetString("FixNodeSpacingTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Graphs in Target Version.
        /// </summary>
        public static string GraphsInTargetVersionMsg {
            get {
                return ResourceManager.GetString("GraphsInTargetVersionMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to graphs selected for update..
        /// </summary>
        public static string GraphsSelectedText {
            get {
                return ResourceManager.GetString("GraphsSelectedText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dynamo Graph Migration Assistant.
        /// </summary>
        public static string HeaderText {
            get {
                return ResourceManager.GetString("HeaderText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} if nodes migrated to refactored if..
        /// </summary>
        public static string IfNodeReplacementLogMessage {
            get {
                return ResourceManager.GetString("IfNodeReplacementLogMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use numbers for prefix?.
        /// </summary>
        public static string InputOrderPrefixAsNumbersLabel {
            get {
                return ResourceManager.GetString("InputOrderPrefixAsNumbersLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start Letter.
        /// </summary>
        public static string InputOrderStartLetterLabel {
            get {
                return ResourceManager.GetString("InputOrderStartLetterLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start Number.
        /// </summary>
        public static string InputOrderStartNumberLabel {
            get {
                return ResourceManager.GetString("InputOrderStartNumberLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Keep folder structure.
        /// </summary>
        public static string KeepFolderStructureCheckboxMsg {
            get {
                return ResourceManager.GetString("KeepFolderStructureCheckboxMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Retains the folder/subfolder structure from the source folder..
        /// </summary>
        public static string KeepFolderStructureTooltip {
            get {
                return ResourceManager.GetString("KeepFolderStructureTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Graphs to Migrate.
        /// </summary>
        public static string MigrateGraphMsg {
            get {
                return ResourceManager.GetString("MigrateGraphMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to View help documentation.
        /// </summary>
        public static string MigrationAssistantHelpTooltip {
            get {
                return ResourceManager.GetString("MigrationAssistantHelpTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Modify graph update settings..
        /// </summary>
        public static string MigrationAssistantSettingsTooltip {
            get {
                return ResourceManager.GetString("MigrationAssistantSettingsTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Graph Spacing.
        /// </summary>
        public static string MigrationSettingsGraphSpacingTab {
            get {
                return ResourceManager.GetString("MigrationSettingsGraphSpacingTab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Input Order.
        /// </summary>
        public static string MigrationSettingsInputOrderTab {
            get {
                return ResourceManager.GetString("MigrationSettingsInputOrderTab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adjust Migration Settings.
        /// </summary>
        public static string MigrationSettingsTitle {
            get {
                return ResourceManager.GetString("MigrationSettingsTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to General Settings.
        /// </summary>
        public static string MiscSettingsMsg {
            get {
                return ResourceManager.GetString("MiscSettingsMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} nodes and notes moved to prevent overlap..
        /// </summary>
        public static string NodesMovedLogMessage {
            get {
                return ResourceManager.GetString("NodesMovedLogMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The source folder contains {0} Dynamo graphs..
        /// </summary>
        public static string NotificationMsg {
            get {
                return ResourceManager.GetString("NotificationMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Target path cannot be the same path as the source path!.
        /// </summary>
        public static string NotificationMsgWarningTargetCannotBeSource {
            get {
                return ResourceManager.GetString("NotificationMsgWarningTargetCannotBeSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Optional.
        /// </summary>
        public static string OptionalFixesMsg {
            get {
                return ResourceManager.GetString("OptionalFixesMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to These migrations tasks are optional as they are more user-specific..
        /// </summary>
        public static string OptionalFixesTooltip {
            get {
                return ResourceManager.GetString("OptionalFixesTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use this tool to migrate Dynamo graphs from pre-2.13 versions to 2.13+ versions..
        /// </summary>
        public static string OverviewMsg {
            get {
                return ResourceManager.GetString("OverviewMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Processed {0} out of {1} graphs..
        /// </summary>
        public static string ProcessMsg {
            get {
                return ResourceManager.GetString("ProcessMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        public static System.Drawing.Bitmap Progress_circle {
            get {
                object obj = ResourceManager.GetObject("Progress_circle", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Update If nodes.
        /// </summary>
        public static string ReplaceIfNodesCheckboxMsg {
            get {
                return ResourceManager.GetString("ReplaceIfNodesCheckboxMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Replaces outdated If nodes with updated If nodes..
        /// </summary>
        public static string ReplaceIfNodesCheckboxTooltip {
            get {
                return ResourceManager.GetString("ReplaceIfNodesCheckboxTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Resume previous session.
        /// </summary>
        public static string ResumeCheckboxMsg {
            get {
                return ResourceManager.GetString("ResumeCheckboxMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attempts to resume progress from a previous run. Progress is stored within the log.txt file in the root target folder..
        /// </summary>
        public static string ResumeTooltip {
            get {
                return ResourceManager.GetString("ResumeTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to X Spacing.
        /// </summary>
        public static string ScaleFactorXLabel {
            get {
                return ResourceManager.GetString("ScaleFactorXLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Y Spacing.
        /// </summary>
        public static string ScaleFactorYLabel {
            get {
                return ResourceManager.GetString("ScaleFactorYLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select folder ...
        /// </summary>
        public static string SelectFolderMsg {
            get {
                return ResourceManager.GetString("SelectFolderMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Export current graph..
        /// </summary>
        public static string SingleExportCheckboxMsg {
            get {
                return ResourceManager.GetString("SingleExportCheckboxMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Allows the migration of the current graph..
        /// </summary>
        public static string SingleExportTooltip {
            get {
                return ResourceManager.GetString("SingleExportTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Source folder.
        /// </summary>
        public static string SourceFolderMsg {
            get {
                return ResourceManager.GetString("SourceFolderMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Recommended.
        /// </summary>
        public static string SuggestedFixesMsg {
            get {
                return ResourceManager.GetString("SuggestedFixesMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to These updates are recommended based on the target Dynamo version to improve functionality and usability of the migrated graph(s)..
        /// </summary>
        public static string SuggestedFixesTooltip {
            get {
                return ResourceManager.GetString("SuggestedFixesTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Target folder.
        /// </summary>
        public static string TargetFolderMsg {
            get {
                return ResourceManager.GetString("TargetFolderMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add source folder to trusted locations.
        /// </summary>
        public static string TrustFileLocationCheckboxMsg {
            get {
                return ResourceManager.GetString("TrustFileLocationCheckboxMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This location is currently not in the trusted locations. In order to run the tool, you need to add it in preferences or check this box..
        /// </summary>
        public static string TrustFileLocationCheckboxTooltip {
            get {
                return ResourceManager.GetString("TrustFileLocationCheckboxTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select folder.
        /// </summary>
        public static string UpdatePathTooltip {
            get {
                return ResourceManager.GetString("UpdatePathTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Graph Updates.
        /// </summary>
        public static string UpdatesMsg {
            get {
                return ResourceManager.GetString("UpdatesMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to View Output.
        /// </summary>
        public static string ViewOutputButtonText {
            get {
                return ResourceManager.GetString("ViewOutputButtonText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} was in an older XML-based Dynamo version. This cannot be upgraded and was simply copied to output folder with no changes..
        /// </summary>
        public static string XmlFileCopiedLogMessage {
            get {
                return ResourceManager.GetString("XmlFileCopiedLogMessage", resourceCulture);
            }
        }
    }
}
