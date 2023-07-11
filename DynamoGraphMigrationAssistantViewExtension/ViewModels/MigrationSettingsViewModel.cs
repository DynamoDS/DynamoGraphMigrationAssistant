using ProtoCore.AssociativeGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Core;

namespace DynamoGraphMigrationAssistant.ViewModels
{
    public class MigrationSettingsViewModel : NotificationObject
    {
        private double _scaleFactorX;
        /// <summary>
        /// The x spacing of the nodes during migration
        /// </summary>
        public double ScaleFactorX
        {
            get => _scaleFactorX;
            set
            {
                if (value == _scaleFactorX) return;
                _scaleFactorX = value;
                RaisePropertyChanged(nameof(ScaleFactorX));
            }
        }

        private double _scaleFactorY;
        /// <summary>
        /// The y spacing of the nodes during migration
        /// </summary>
        public double ScaleFactorY
        {
            get => _scaleFactorY;
            set
            {
                if (value == _scaleFactorY) return;
                _scaleFactorY = value;
                RaisePropertyChanged(nameof(ScaleFactorY));
            }
        }

        private bool _inputOrdersAsNumbers;
        /// <summary>
        /// Use numbers for ordering inputs?
        /// </summary>
        public bool InputOrderAsNumbers
        {
            get => _inputOrdersAsNumbers;
            set
            {
                if (value == _inputOrdersAsNumbers) return;
                _inputOrdersAsNumbers = value;
                RaisePropertyChanged(nameof(InputOrderAsNumbers));
            }
        }

        private int _inputOrderStartNumber;
        /// <summary>
        /// What number to start at?
        /// </summary>
        public int InputOrderStartNumber
        {
            get => _inputOrderStartNumber;
            set
            {
                if (value == _inputOrderStartNumber) return;
                _inputOrderStartNumber = value;
                RaisePropertyChanged(nameof(InputOrderStartNumber));
            }
        }
        private string _inputOrderStartLetter;
        /// <summary>
        /// What letter to start at?
        /// </summary>
        public string InputOrderStartLetter
        {
            get => _inputOrderStartLetter;
            set
            {
                if (value == _inputOrderStartLetter) return;
                _inputOrderStartLetter = value;
                RaisePropertyChanged(nameof(InputOrderStartLetter));
            }
        }

        private MigrationSettings _migrationSettings;
        /// <summary>
        /// Our current Settings
        /// </summary>
        public MigrationSettings MigrationSettings
        {
            get => _migrationSettings;
            set
            {
                if (value == _migrationSettings) return;
                _migrationSettings = value;
                RaisePropertyChanged(nameof(MigrationSettings));
            }
        }

        public MigrationSettingsViewModel(MigrationSettings settings)
        {
            MigrationSettings = settings;
            ScaleFactorX = settings.ScaleFactorX;
            ScaleFactorY = settings.ScaleFactorY;
            InputOrderAsNumbers = settings.InputOrderAsNumbers;
            InputOrderStartNumber = settings.InputOrderStartNumber;
            InputOrderStartLetter = settings.InputOrderStartLetter;
        }
    }
}
