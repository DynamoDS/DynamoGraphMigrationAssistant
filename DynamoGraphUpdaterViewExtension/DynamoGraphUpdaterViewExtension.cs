using Dynamo.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace DynamoGraphUpdater
{
    public class DynamoGraphUpdaterViewExtension : ViewExtensionBase, IViewExtension
    {
        /// <summary>
        /// The name of the extension.
        /// </summary>
        public override string Name => Properties.Resources.ExtensionName;
        /// <summary>
        /// The GUID of the extension
        /// </summary>
        public override string UniqueId => "57B476EF-844D-4719-A6A6-B649CB998868";

        public MenuItem DynamoGraphUpdaterMenuItem;
        private ViewLoadedParams _viewLoadedParamsReference;

        internal DynamoGraphUpdaterView View;
        internal DynamoGraphUpdaterViewModel ViewModel;


        public override void Dispose()
        {
            // Cleanup
            ViewModel?.Dispose();
            View = null;
            ViewModel = null;
        }
        public override void Loaded(ViewLoadedParams viewLoadedParams)
        {
            if (viewLoadedParams == null) throw new ArgumentNullException(nameof(viewLoadedParams));

            _viewLoadedParamsReference = viewLoadedParams;

            //TODO: Temp to write files to json, remove later
            //List<TargetDynamoVersion> versions = new List<TargetDynamoVersion>()
            //{
            //    new TargetDynamoVersion("2.5","Revit", "2021",false,false,true),
            //    new TargetDynamoVersion("2.6","Revit", "2021.1",false,false,true),
            //    new TargetDynamoVersion("2.10","Revit", "2022",false,false,true),
            //    new TargetDynamoVersion("2.12","Revit", "2022.1",false,true,true),
            //    new TargetDynamoVersion("2.13","Revit", "2023",true,true,true),
            //    new TargetDynamoVersion("2.14","Revit"," 2024",true,true,true),

            //    new TargetDynamoVersion("2.10","Civil3D", "2022",false,false,true),
            //    new TargetDynamoVersion("2.13","Civil3D", "2023",true,true,true),
            //    new TargetDynamoVersion("2.14","Civil3D"," 2024",true,true,true),
            //};
          

            //File.WriteAllText(@"D:\repos_john\DynamoGraphUpdater\DynamoGraphUpdaterViewExtension\manifest\VersionDictionary.json", JsonConvert.SerializeObject(versions));


            // Add a button to Dynamo View menu to manually show the window
            DynamoGraphUpdaterMenuItem = new MenuItem { Header = Properties.Resources.HeaderText };

            DynamoGraphUpdaterMenuItem.Click += DynamoGraphUpdaterMenuItemOnClick;

            _viewLoadedParamsReference.AddExtensionMenuItem(DynamoGraphUpdaterMenuItem);

            InitializeViewExtension();
        }

        private void DynamoGraphUpdaterMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            View.Owner = _viewLoadedParamsReference.DynamoWindow;

            View.ShowDialog();
        }

        public void Shutdown()
        {
            Dispose();
        }
        private void InitializeViewExtension()
        {
            ViewModel = new DynamoGraphUpdaterViewModel(_viewLoadedParamsReference);
            View = new DynamoGraphUpdaterView(ViewModel);
        }
       
        public override void Closed()
        {
            if (DynamoGraphUpdaterMenuItem != null) DynamoGraphUpdaterMenuItem.IsChecked = false;
        }
    }
}
