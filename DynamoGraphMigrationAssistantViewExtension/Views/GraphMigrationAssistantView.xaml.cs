﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DynamoGraphMigrationAssistant.ViewModels;

namespace DynamoGraphMigrationAssistant.Views
{
    /// <summary>
    /// Interaction logic for GraphMigrationAssistantView.xaml
    /// </summary>
    public partial class GraphMigrationAssistantView : UserControl
    {
        public GraphMigrationAssistantView(GraphMigrationAssistantViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            GraphMigrationAssistantViewModel vm = this.TargetVersionComboBox.DataContext as GraphMigrationAssistantViewModel;

            vm.TargetDynamoVersion = this.TargetVersionComboBox.SelectedItem as TargetDynamoVersion;

            if (vm.Graphs is null) return;

            vm.CheckVersions();
        }


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!IsLoaded) return;

            ComboBox versionCombobox = sender as ComboBox;

            GraphMigrationAssistantViewModel vm = versionCombobox.DataContext as GraphMigrationAssistantViewModel;

            if (versionCombobox.SelectedItem != null)
            {
                vm.TargetDynamoVersion = versionCombobox.SelectedItem as TargetDynamoVersion;


                vm.FixNodeSpacing = vm.TargetDynamoVersion.NodeSpacingSuggested;
                vm.ReplaceIfNodes = vm.TargetDynamoVersion.IfNodeSuggested;
            }


            if (vm.Graphs is null) return;
            
            vm.CheckVersions();
        }

        private void EditGraphSettingsCommand(object sender, MouseButtonEventArgs e)
        {
            if (!IsLoaded) return;
            GraphMigrationAssistantViewModel vm = this.DataContext as GraphMigrationAssistantViewModel;

            vm.EditGraphSettingsCommand.Execute(this);
        }

        private void ExportButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (this.TrustCheckbox.IsEnabled)
            {
                if (!this.TrustCheckbox.IsChecked.Value)
                {
                    Storyboard sb = this.FindResource("WiggleStoryboard") as Storyboard;
                  
                    sb.Begin();
                }
            }

            GraphMigrationAssistantViewModel vm = this.DataContext as GraphMigrationAssistantViewModel;

            this.SourceBorder.BorderBrush = vm.SourcePathViewModel.FolderPath is null ? new SolidColorBrush(Colors.Red) : new SolidColorBrush();

            this.TargetBorder.BorderBrush = vm.TargetPathViewModel.FolderPath is null ? new SolidColorBrush(Colors.Red) : new SolidColorBrush();
        }
    }
}
